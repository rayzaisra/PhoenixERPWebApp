using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Phoenix.Data.Models.Finance;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models.Enum;
using Phoenix.Data.Models.Finance.Transaction.GoodReturn;
using Phoenix.Data.Models.Finance.Transaction.FixedAssetOutMovement;

namespace Phoenix.Data.Services.Finance.Transaction
{
    public interface IFixedAssetMovementService : IDataService<FixedAssetMovement>
    {
        Task<List<FixedAssetMovement>> GetByFixedAsset(string faId);
        Task<int> AddWithDetailAsync(FixedAssetMovement Header, List<FixedAssetMovementDetail> Details);
        Task<int> EditWithDetailAsync(FixedAssetMovement Header, List<FixedAssetMovementDetail> Details);
        //Task<List<FixedAssetMovement>> GetData();
        Task<FixedAssetMovement> GetData(string Id);
        Task<List<FixedAssetMovementDetail>> GetListDetail(string headerId);
        Task<int> Reject(FixedAssetMovement entity);
        Task<FixedAssetMovementDetail> GetDetail(string Id);
        Task<FixedAssetMovementDto> ApproveCustom(FixedAssetMovementDto dto);
        Task<List<FixedAssetOutMovementDetail>> GetListFixedAssetOutDetail(string headerId);
        Task<List<FixedAssetMovementDetail>> GetListFixedAssetmovementDetail(string id);
    }
    public class FixedAssetMovementService : IFixedAssetMovementService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        string idTemplate = "";
        readonly StatusLogService log;
        readonly Services.Um.IManageMenuService menuService;

        public FixedAssetMovementService(DataContext context, GlobalFunctionApproval globalFcApproval, StatusLogService log, Services.Um.IManageMenuService menuService)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.log = log;
            this.menuService = menuService;
        }

        public async Task<List<FixedAssetMovementDetail>> GetListFixedAssetmovementDetail(string id)
        {
            var query = await context.FixedAssetMovementDetail.Where(x => x.FixedAssetMovementNumber == id && x.IsDeleted.Equals(false)).ToListAsync();

            foreach (var item in query)
            {
                var asset = context.FixedAsset.Where(x => x.Id == item.FixedAssetNumber).FirstOrDefault();
                item.LocationName = context.MasterLocationProcurement.Where(x => x.Id == asset.LocationId).Select(x => x.LocationStore).FirstOrDefault();
                item.ConditionName = context.InventoryDetailStatus.Where(x => x.getForm == asset.Condition).Select(x => x.statusName).FirstOrDefault();
                item.LegalEntityName = context.LegalEntity.Where(x => x.Id == asset.LegalEntityId).Select(x => x.LegalEntityName).FirstOrDefault();
                item.UnitCount = asset.UnitCount;
            }
            return query;
        }
        public async Task<List<FixedAssetOutMovementDetail>> GetListFixedAssetOutDetail(string headerId)
        {
            var urllink = "[Link]/finance/FixedAsset/Detail?Id=";
            var query = await (from f in context.FixedassetOutMovementDetail
                               where f.FixedAssetOutId == headerId
                               select new FixedAssetOutMovementDetail()
                               {
                                   Id = f.Id,
                                   LocationId = f.LocationId,
                                   LegalEntityId = f.LegalEntityId,
                                   AccountId = f.AccountId,
                                   Brand = f.Brand,
                                   Condition = f.Condition,
                                   ConditionName = (from x in context.InventoryDetailStatus where x.getForm == Convert.ToInt32(f.Condition) select x.statusName).FirstOrDefault(),
                                   LegalEntityName = (from x in context.LegalEntity where x.Id == f.LegalEntityId select x.LegalEntityName).FirstOrDefault(),
                                   LocationName = (from x in context.MasterLocationProcurement where x.Id == f.LocationId select x.LocationStore).FirstOrDefault(),
                                   FixedAssetId = f.FixedAssetId,
                                   ItemCategoryId = f.ItemCategoryId,
                                   Qty = f.Qty,
                                   QtyMax = f.QtyMax,
                                   UnitName = f.UnitName,
                                   UrlLink = urllink + f.FixedAssetId,
                                   RegistrationDate = f.RegistrationDate
                               }).ToListAsync();
            return query;
        }

        public async Task<int> AddWithDetailAsync(FixedAssetMovement Header, List<FixedAssetMovementDetail> Details)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (Details.Count > 0)
                    {
                        foreach (var item in Details)
                        {
                            if (item.Qty > item.UnitCount)
                            {
                                throw new Exception("Your quantity request exceeds the quantity we have. Please check again.");
                            }
                        }
                    }

                    Header.Id = await TransID.GetTransId(context, "FAMN", DateTime.Today);
                    Header.FaaNumber = Header.Id;

                    if (Details.Count > 0)
                    {
                        foreach (var item in Details)
                        {
                            item.Id = Guid.NewGuid().ToString();
                            item.FixedAssetMovementNumber = Header.Id;
                            context.PhoenixAddAsync(item);
                        }
                    }

                    await context.PhoenixAddAsync(Header);
                    await context.SaveChangesAsync();
                    var isApprove = true;

                    if (!(Header.Status == GoodRequestStatus.Open))
                    {
                        await log.AddAsync(new StatusLog() { TransactionId = Header.Id, Status = GoodRequestStatus.StatusName(Header.Status), Description = Header.Remark });
                        var approval = await context.TrTemplateApprovals.AsNoTracking().Where(x => x.RefId == Header.Id).ToListAsync();
                        if (approval.Count > 0)
                        {
                            var userApproval = await context.TrUserApprovals.AsNoTracking().Where(x => approval.Any(y => y.Id == x.TrTempApprovalId)).ToListAsync();
                            var conditionUserApproval = await context.TrConditionUserApprovals.AsNoTracking().Where(x => userApproval.Any(y => y.Id == x.TrUserApprovalId)).ToListAsync();

                            context.TrConditionUserApprovals.RemoveRange(conditionUserApproval);
                            context.TrUserApprovals.RemoveRange(userApproval);
                            context.TrTemplateApprovals.RemoveRange(approval);
                        }


                        var dataMenu = await menuService.GetByUniqueName(MenuUnique.FixedAssetMovement).ConfigureAwait(false);
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var dataTempate = context.TmTemplateApprovals.Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            IdTemplate = dataTempate.Id,
                            MenuId = dataMenu.Id,
                            RefId = Header.Id,
                            DetailLink = $"{ApprovalLink.FixedAssetMovement}?Id={Header.Id}",
                            NotificationType = Phoenix.Data.Models.NotificationTypeCode.FixedAssetMovement,
                            Module = Phoenix.Data.Models.NotificationModuleType.ModuleFinance
                        };
                        var subGroupId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(context.HttpContext);
                        var divisionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(context.HttpContext);
                        var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
                        isApprove = await globalFcApproval.Save(vm, subGroupId, divisionId, employeeId);
                        if (isApprove)
                        {
                            var save = await context.SaveChangesAsync();
                            transaction.Commit();
                            return save;
                        }
                        else
                        {
                            transaction.Rollback();
                            throw new Exception("please check the approval template that will be processed");
                        }
                    }
                    else
                    {
                        var save = await context.SaveChangesAsync();
                        transaction.Commit();
                        return save;
                    }

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<int> EditWithDetailAsync(FixedAssetMovement Header, List<FixedAssetMovementDetail> Details)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (Details.Count > 0)
                    {
                        foreach (var item in Details)
                        {
                            if (item.Qty > item.UnitCount)
                            {
                                throw new Exception("Your quantity request exceeds the quantity we have. Please check again.");
                            }
                        }
                    }

                    context.PhoenixEdit(Header);

                    var Tmpdetail = context.FixedAssetMovementDetail.AsNoTracking().Where(x => x.FixedAssetMovementNumber == Header.Id).ToList();
                    if (Tmpdetail.Count > 0)
                    {
                        context.FixedAssetMovementDetail.RemoveRange(Tmpdetail);
                    }

                    if (Details.Count > 0)
                    {
                        foreach (var item in Details)
                        {
                            item.Id = Guid.NewGuid().ToString();
                            item.FixedAssetMovementNumber = Header.Id;
                            context.PhoenixAddAsync(item);
                        }
                    }

                    var isApprove = true;
                    if (!(Header.Status == GoodRequestStatus.Open))
                    {
                        await log.AddAsync(new StatusLog() { TransactionId = Header.Id, Status = GoodRequestStatus.StatusName(Header.Status), Description = Header.Remark });
                        var approval = await context.TrTemplateApprovals.AsNoTracking().Where(x => x.RefId == Header.Id).ToListAsync();
                        if (approval.Count > 0)
                        {
                            var userApproval = await context.TrUserApprovals.AsNoTracking().Where(x => approval.Any(y => y.Id == x.TrTempApprovalId)).ToListAsync();
                            var conditionUserApproval = await context.TrConditionUserApprovals.AsNoTracking().Where(x => userApproval.Any(y => y.Id == x.TrUserApprovalId)).ToListAsync();

                            context.TrConditionUserApprovals.RemoveRange(conditionUserApproval);
                            context.TrUserApprovals.RemoveRange(userApproval);
                            context.TrTemplateApprovals.RemoveRange(approval);
                        }


                        var dataMenu = await menuService.GetByUniqueName(MenuUnique.FixedAssetMovement).ConfigureAwait(false);
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var dataTempate = context.TmTemplateApprovals.Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            IdTemplate = dataTempate.Id,
                            MenuId = dataMenu.Id,
                            RefId = Header.Id,
                            DetailLink = $"{ApprovalLink.FixedAssetMovement}?Id={Header.Id}",
                            NotificationType = Phoenix.Data.Models.NotificationTypeCode.FixedAssetMovement,
                            Module = Phoenix.Data.Models.NotificationModuleType.ModuleFinance
                        };
                        var subGroupId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(context.HttpContext);
                        var divisionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(context.HttpContext);
                        var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
                        isApprove = await globalFcApproval.Save(vm, subGroupId, divisionId, employeeId);
                        if (isApprove)
                        {
                            var save = await context.SaveChangesAsync();
                            transaction.Commit();
                            return save;
                        }
                        else
                        {
                            transaction.Rollback();
                            throw new Exception("please check the approval template that will be processed");
                        }
                    }
                    else
                    {
                        var save = await context.SaveChangesAsync();
                        transaction.Commit();
                        return save;
                    }

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }

        }

        public async Task<FixedAssetMovementDto> ApproveCustom(FixedAssetMovementDto dto)
        {

            try
            {
                var model = await context.FixedAssetMovement.Where(x => x.Id == dto.Header.Id).FirstOrDefaultAsync();

                if (dto.Details.Count > 0)
                {
                    foreach (var item in dto.Details)
                    {
                        var fa = await context.FixedAsset.Where(x => x.Id == item.FixedAssetNumber).FirstOrDefaultAsync();
                        fa.LocationId = model.ToLocationID;
                        context.PhoenixEdit(fa);
                    }
                }
                model.Status = GoodRequestStatus.Approved;
                context.PhoenixEdit(model);

                var save = await context.SaveChangesAsync();

                await globalFcApproval.ProcessApproval<FixedAssetMovement>(model.Id, model.Status, model.Remark, model);
                await log.AddAsync(new StatusLog() { TransactionId = model.Id, Status = GoodRequestStatus.StatusName(model.Status), Description = model.Remark });

                var modeltrapp = await context.TrTemplateApprovals.Where(x => x.RefId == model.Id).FirstOrDefaultAsync();
                if (modeltrapp != null)
                {
                    if (modeltrapp.StatusApprovedDescription == GoodRequestStatus.StatusName(GoodRequestStatus.Approved))
                    {
                        model.Status = GoodRequestStatus.Approved;
                    }
                    else
                    {
                        model.Status = GoodRequestStatus.WaitingApproval;
                    }
                }
                else
                {
                    model.Status = GoodRequestStatus.WaitingApproval;
                }


                return dto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task<int> Reject(FixedAssetMovement entity)
        {
            try
            {
                var save = await globalFcApproval.ProcessApproval(entity.Id, GoodRequestStatus.Rejected, entity.Remark, entity);

                entity.Status = GoodRequestStatus.Rejected;
                entity.ModifiedOn = DateTime.Now;

                await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = GoodRequestStatus.StatusName(GoodRequestStatus.Rejected), Description = entity.Remark });

                context.FixedAssetMovement.Update(entity);

                save = await context.SaveChangesAsync();
                return save;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task<int> AddAsync(FixedAssetMovement entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            entity.FaaNumber = await TransID.GetTransId(context, "FAA", DateTime.Today);
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params FixedAssetMovement[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(FixedAssetMovement entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params FixedAssetMovement[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteSoftAsync(FixedAssetMovement entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteSoftRangeAsync(params FixedAssetMovement[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> EditAsync(FixedAssetMovement entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> EditRangeAsync(FixedAssetMovement entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<FixedAssetMovement>> Get()
        {
            var query = await (from x in context.FixedAssetMovement
                               where x.IsDeleted == false
                               select new FixedAssetMovement()
                               {
                                   Id = x.Id,
                                   FaaNumber = x.FaaNumber,
                                   FromLocationID = x.FromLocationID,
                                   ToLocationID = x.ToLocationID,
                                   CreatedBy = x.CreatedBy,
                                   ExecutionDate = x.ExecutionDate,
                                   FaaDateTime = x.FaaDateTime,
                                   FromLocationName = (from y in context.MasterLocationProcurement where y.Id == x.FromLocationID select y.LocationStore).FirstOrDefault(),
                                   ToLocationName = (from y in context.MasterLocationProcurement where y.Id == x.ToLocationID select y.LocationStore).FirstOrDefault(),
                                   EmployeeName = (from y in context.EmployeeBasicInfos where y.Id == x.CreatedBy select y.NameEmployee).FirstOrDefault(),
                                   Status = x.Status
                               }).ToListAsync();
            return query;
        }



        //public async Task<List<FixedAssetMovement>> GetData()
        //{
        //    var query = await (from gr in context.GoodRequest
        //                       join grd in (from g in context.GoodRequestDetail orderby g.ModifiedOn descending select g).Take(1) on gr.Id equals grd.GoodRequestId into left
        //                       from grd in left.DefaultIfEmpty()
        //                       where gr.Type == "FIXED ASSET"
        //                       select new FixedAssetMovement
        //                       {
        //                           Id = gr.Id,
        //                           GoodRequestNumber = gr.GoodRequestNumber,
        //                           GoodRequestDatetime = gr.GoodRequestDatetime,
        //                           Type = gr.Type,
        //                           DUeDate = gr.DUeDate,
        //                           InvFaId = gr.InvFaId,
        //                           InvFaName = gr.InvFaName,
        //                           JobId = gr.JobId,
        //                           JobName = gr.JobName,
        //                           BusinessUnitId = gr.BusinessUnitId,
        //                           BusinessUnitName = gr.BusinessUnitName,
        //                           LegalEntityId = gr.LegalEntityId,
        //                           LegalEntityName = gr.LegalEntityName,
        //                           EmployeeName = gr.EmployeeName,
        //                           Status = gr.Status,
        //                           LocationTransfer = grd.WarehouseIdTo,
        //                           Purpose = grd.Purpose,
        //                           ReturnDate = gr.ReturnDate,
        //                           Move_Loc = gr.Move_Loc,
        //                           Inv_Out_Type = gr.Inv_Out_Type,
        //                           inv_status = gr.inv_status,
        //                           OnBehalfId = gr.OnBehalfId,
        //                           behalf_div = gr.behalf_div,
        //                           behalf_jobtitle = gr.behalf_jobtitle,
        //                           division_id = gr.division_id,
        //                           division_name = gr.division_name,
        //                           TotalQty = gr.TotalQty,
        //                           inv_status_name = (from x in context.Inventory_GetForms
        //                                              where x.Id == gr.inv_status
        //                                              select x.getform_name).FirstOrDefault()
        //                       }
        //                       ).ToListAsync();
        //    return query;
        //}

        public async Task<FixedAssetMovement> GetData(string Id)
        {
            return await context.FixedAssetMovement.Where(x => x.IsDeleted.Equals(false) && x.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<FixedAssetMovementDetail> GetDetail(string Id)
        {
            return await context.FixedAssetMovementDetail.Where(x => x.IsDeleted.Equals(false) && x.Id == Id).FirstOrDefaultAsync();

        }

        public async Task<List<FixedAssetMovementDetail>> GetListDetail(string headerId)
        {
            var query = await context.FixedAssetMovementDetail.Where(x => x.IsDeleted.Equals(false) && x.FixedAssetMovementNumber == headerId).ToListAsync();

            foreach (var item in query)
            {
                var inv = await context.FixedAsset.Where(x => x.Id == item.FixedAssetNumber).FirstOrDefaultAsync();
                item.Qty = inv.UnitCount ?? 0;

            }

            return query;

        }

        public Task<List<FixedAssetMovement>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<FixedAssetMovement> Get(string id)
        {
            var query = await (from fam in context.FixedAssetMovement
                               join emp in context.EmployeeBasicInfos on fam.Employee equals emp.Id
                               join loc in context.MasterLocationProcurement on fam.FromLocationID equals loc.Id
                               join con in context.MasterConditionProcurement on fam.Condition equals con.Id
                               join bu in context.BusinessUnits on fam.BusinessUnitID equals bu.Id
                               join le in context.LegalEntity on fam.LegalEntityID equals le.Id
                               join aff in context.Affiliations on fam.AffiliationID equals aff.Id
                               join status in context.FixedAssetMovementStatus on fam.FixedAssetMovementStatusID equals status.Id
                               where fam.IsDeleted.Equals(false) && fam.Id == id
                               select new FixedAssetMovement
                               {
                                   Id = fam.Id,
                                   FaaDateTime = fam.FaaDateTime,
                                   FromLocationID = loc.Id,
                                   ToLocationID = loc.Id,
                                   LocationName = loc.LocationStore,
                                   Condition = con.Id,
                                   ConditionName = con.ConditionName,
                                   Employee = emp.Id,
                                   EmployeeName = emp.NameEmployee,
                                   BusinessUnitID = bu.Id,
                                   Division = bu.UnitName,
                                   FromBusinessUnitID = bu.Id,
                                   ToBusinessUnitID = bu.Id,
                                   LegalEntityID = le.Id,
                                   LegalEntityName = le.LegalEntityName,
                                   AffiliationID = aff.Id,
                                   AffiliationName = aff.AffiliationName,
                                   Remark = fam.Remark,
                                   FixedAssetMovementStatusID = status.Id,
                                   StatusName = status.StatusName
                               }).FirstOrDefaultAsync();
            //var query = await context.FixedAssetMovement.Where(x=>x.IsDeleted.Equals(false)&&x.Id==id).FirstOrDefaultAsync();
            return query;
        }

        public async Task<List<FixedAssetMovement>> GetByFixedAsset(string faId)
        {
            var query = await (from fam in context.FixedAssetMovement
                               join emp in context.EmployeeBasicInfos on fam.Employee equals emp.Id
                               join loc in context.MasterLocationProcurement on fam.FromLocationID equals loc.Id
                               join con in context.MasterConditionProcurement on fam.Condition equals con.Id
                               join bu in context.BusinessUnits on fam.BusinessUnitID equals bu.Id
                               join le in context.LegalEntity on fam.LegalEntityID equals le.Id
                               join aff in context.Affiliations on fam.AffiliationID equals aff.Id
                               join status in context.FixedAssetMovementStatus on fam.FixedAssetMovementStatusID equals status.Id
                               where fam.IsDeleted.Equals(false) && fam.FixedAssetId == faId
                               select new FixedAssetMovement
                               {
                                   Id = fam.Id,
                                   FaaDateTime = fam.FaaDateTime,
                                   FromLocationID = loc.Id,
                                   ToLocationID = loc.Id,
                                   LocationName = loc.LocationStore,
                                   Condition = con.Id,
                                   ConditionName = con.ConditionName,
                                   Employee = emp.Id,
                                   EmployeeName = emp.NameEmployee,
                                   BusinessUnitID = bu.Id,
                                   Division = bu.UnitName,
                                   FromBusinessUnitID = bu.Id,
                                   ToBusinessUnitID = bu.Id,
                                   LegalEntityID = le.Id,
                                   LegalEntityName = le.LegalEntityName,
                                   AffiliationID = aff.Id,
                                   AffiliationName = aff.AffiliationName,
                                   Remark = fam.Remark,
                                   FixedAssetMovementStatusID = status.Id,
                                   StatusName = status.StatusName
                               }).ToListAsync();
            //var query = await context.FixedAssetMovement.Where(x => x.IsDeleted.Equals(false) && x.FixedAssetId == faId).ToListAsync();
            return query;
        }
    }
}
