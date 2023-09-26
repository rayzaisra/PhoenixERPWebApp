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
    public interface IFixedAssetOutMovementService : IDataService<FixedAssetOutMovement>
    {
        Task<List<FixedAssetOutMovementDetail>> GetListFixedAssetOutDetail(string headerId);
        Task<int> AddWithDetailAsync(FixedAssetOutMovement Header, List<FixedAssetOutMovementDetail> Details);
        Task<int> EditWithDetailAsync(FixedAssetOutMovement Header, List<FixedAssetOutMovementDetail> Details);
        Task<int> Reject(FixedAssetOutMovement entity);
        Task<FixedAssetOutMovementDto> ApproveCustom(FixedAssetOutMovementDto dto);
        Task<List<FixedAssetOutMovementDetail>> GetLogOutMovement(string id);

    }
    public class FixedAssetOutMovementService : IFixedAssetOutMovementService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        string idTemplate = "";
        readonly StatusLogService log;
        readonly Services.Um.IManageMenuService menuService;

        public FixedAssetOutMovementService(DataContext context, GlobalFunctionApproval globalFcApproval, StatusLogService log, Services.Um.IManageMenuService menuService)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.log = log;
            this.menuService = menuService;
        }

        public async Task<List<FixedAssetOutMovementDetail>> GetLogOutMovement(string id)
        {
            var query = await (from x in context.FixedassetOutMovementDetail
                               where x.FixedAssetId == id
                               select new FixedAssetOutMovementDetail()
                               {
                                   FixedAssetOutId = x.FixedAssetOutId,
                                   UrlLink = x.UrlLink,
                                   RequestDate = x.RequestDate,
                                   RequestStatus = x.RequestStatus,
                                   RequestStatusName = (from y in context.Inventory_GetForms where y.Id == x.RequestStatus select y.getform_name).FirstOrDefault()

                               }).ToListAsync();
            return query;
        }

        public async Task<int> AddWithDetailAsync(FixedAssetOutMovement Header, List<FixedAssetOutMovementDetail> Details)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (Details.Count > 0)
                    {
                        foreach (var item in Details)
                        {
                            if (item.Qty > item.QtyMax)
                            {
                                throw new Exception("Your quantity request exceeds the quantity we have. Please check again.");
                            }
                        }
                    }
                    Header.Id = await TransID.GetTransId(context, "FAOM", DateTime.Today);
                    Header.FaNumber = Header.Id;

                    if (Details.Count > 0)
                    {
                        foreach (var item in Details)
                        {
                            item.Id = Guid.NewGuid().ToString();
                            item.FixedAssetOutId = Header.Id;
                            //item.RequestStatus =  context.Inventory_GetForms.Where(x => x.getform_name == item.RequestStatus).Select(x => x.Id).FirstOrDefault();
                            await context.PhoenixAddAsync(item);
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


                        var dataMenu = await menuService.GetByUniqueName(MenuUnique.FixedAssetOutMovement).ConfigureAwait(false);
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var dataTempate = context.TmTemplateApprovals.Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            IdTemplate = dataTempate.Id,
                            MenuId = dataMenu.Id,
                            RefId = Header.Id,
                            DetailLink = $"{ApprovalLink.FixedAssetOutMovement}?Id={Header.Id}",
                            NotificationType = Phoenix.Data.Models.NotificationTypeCode.FixedAssetOutMovement,
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

        public async Task<int> EditWithDetailAsync(FixedAssetOutMovement Header, List<FixedAssetOutMovementDetail> Details)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (Details.Count > 0)
                    {
                        foreach (var item in Details)
                        {
                            if (item.Qty > item.QtyMax)
                            {
                                throw new Exception("Your quantity request exceeds the quantity we have. Please check again.");
                            }
                        }
                    }

                    context.PhoenixEdit(Header);

                    var Tmpdetail = context.FixedassetOutMovementDetail.AsNoTracking().Where(x => x.FixedAssetOutId == Header.Id).ToList();
                    if (Tmpdetail.Count > 0)
                    {
                        context.FixedassetOutMovementDetail.RemoveRange(Tmpdetail);
                    }

                    if (Details.Count > 0)
                    {
                        foreach (var item in Details)
                        {
                            item.Id = Guid.NewGuid().ToString();
                            item.FixedAssetOutId = Header.Id;
                            //item.RequestStatus = context.Inventory_GetForms.Where(x => x.Id == item.RequestStatus).Select(x => x.Id).FirstOrDefault();
                            await context.PhoenixAddAsync(item);
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


                        var dataMenu = await menuService.GetByUniqueName(MenuUnique.FixedAssetOutMovement).ConfigureAwait(false);
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var dataTempate = context.TmTemplateApprovals.Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            IdTemplate = dataTempate.Id,
                            MenuId = dataMenu.Id,
                            RefId = Header.Id,
                            DetailLink = $"{ApprovalLink.FixedAssetOutMovement}?Id={Header.Id}",
                            NotificationType = Phoenix.Data.Models.NotificationTypeCode.FixedAssetOutMovement,
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

        public async Task<int> Reject(FixedAssetOutMovement entity)
        {
            try
            {
                var save = await globalFcApproval.ProcessApproval(entity.Id, GoodRequestStatus.Rejected, entity.Remark, entity);

                entity.Status = GoodRequestStatus.Rejected;
                entity.ModifiedOn = DateTime.Now;

                await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = GoodRequestStatus.StatusName(GoodRequestStatus.Rejected), Description = entity.Remark });

                context.FixedAssetOutMovement.Update(entity);

                save = await context.SaveChangesAsync();
                return save;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task<FixedAssetOutMovementDto> ApproveCustom(FixedAssetOutMovementDto dto)
        {
            try
            {
                var model = await context.FixedAssetOutMovement.Where(x => x.Id == dto.Header.Id).FirstOrDefaultAsync();

                if (dto.Details.Count > 0)
                {
                    foreach (var item in dto.Details)
                    {
                        var fa = await context.FixedAsset.Where(x => x.Id == item.FixedAssetId).FirstOrDefaultAsync();
                        fa.UnitCount = fa.UnitCount - item.Qty;
                        fa.Condition = 5;
                        context.PhoenixEdit(fa);
                    }
                }
                model.Status = GoodRequestStatus.Approved;
                context.PhoenixEdit(model);

                var save = await context.SaveChangesAsync();

                await globalFcApproval.ProcessApproval<FixedAssetOutMovement>(model.Id, model.Status, model.Remark, model);
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
                                   QtyMax = (from x in context.FixedAsset where x.Id == f.FixedAssetId select x.UnitCount).FirstOrDefault() ?? 0,
                                   UnitName = f.UnitName,
                                   UrlLink = urllink + f.FixedAssetId,
                                   RequestDate = f.RequestDate,
                                   RequestStatus = f.RequestStatus,
                                   RequestStatusName = (from x in context.Inventory_GetForms where x.Id == f.RequestStatus select x.getform_name).FirstOrDefault(),
                                   Remark = f.Remark
                               }).ToListAsync();
            return query;
        }

        public Task<int> AddAsync(FixedAssetOutMovement entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddRangeAsync(params FixedAssetOutMovement[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(FixedAssetOutMovement entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params FixedAssetOutMovement[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteSoftAsync(FixedAssetOutMovement entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteSoftRangeAsync(params FixedAssetOutMovement[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> EditAsync(FixedAssetOutMovement entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> EditRangeAsync(FixedAssetOutMovement entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<FixedAssetOutMovement>> Get()
        {
            var query = await (from q in context.FixedAssetOutMovement
                               where q.IsDeleted == false
                               select new FixedAssetOutMovement()
                               {
                                   Id = q.Id,
                                   FaNumber = q.FaNumber,
                                   FaDatetime = q.FaDatetime,
                                   LocationId = q.LocationId,
                                   LocationName = (from x in context.MasterLocationProcurement where x.Id == q.LocationId select x.LocationStore).FirstOrDefault(),
                                   RequestorId = q.RequestorId,
                                   RequestorName = (from x in context.EmployeeBasicInfos where x.Id == q.RequestorId select x.NameEmployee).FirstOrDefault(),
                                   Status = q.Status
                               }
                               ).ToListAsync();
            return query;
        }

        public Task<List<FixedAssetOutMovement>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<FixedAssetOutMovement> Get(string id)
        {
            return await context.FixedAssetOutMovement.Where(x => x.IsDeleted.Equals(false) && x.Id == id).FirstOrDefaultAsync();
        }
    }
}
