using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models.Finance;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Phoenix.Data.Models.Finance.Transaction.GoodReturn;

namespace Phoenix.Data.Services.Finance.Transaction
{
    public interface IGoodRequestService : IDataService<GoodRequest>
    {
        Task<int> AddWithDetailAsync(GoodRequest Header, List<GoodRequestDetail> Details);
        Task<int> EditWithDetailAsync(GoodRequest Header, List<GoodRequestDetail> Details);
        Task<GoodRequestDetail> GetDetail(string Id);
        Task<List<GoodRequestDetail>> GetListDetail(string headerId);
        Task<GoodRequestDTO> Approve(GoodRequestDTO model);
        //    Task<GoodRequest> Complete(GoodRequest model);
        Task<GoodRequest> GetByNumber(string number);
        Task<ParamValueGR> GetBehalf(string id);
        Task<ParamValueGR> GetDivLogin(string basic);
        Task<int> Reject(GoodRequest model);
        Task<dynamic> GetMasterConditionProcurement();
    }

    public class GoodRequestService : IGoodRequestService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        string idTemplate = "";
        readonly StatusLogService log;
        readonly Services.Um.IManageMenuService menuService;

        public GoodRequestService(DataContext context, GlobalFunctionApproval globalFcApproval, StatusLogService log, Services.Um.IManageMenuService menuService)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.log = log;
            this.menuService = menuService;
        }

        public Task<int> AddAsync(GoodRequest entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddRangeAsync(params GoodRequest[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> AddWithDetailAsync(GoodRequest Header, List<GoodRequestDetail> Details)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (Details.Count > 0)
                    {
                        foreach (var item in Details)
                        {
                            if (item.qty > item.qtyMax)
                            {
                                throw new Exception("Your quantity request exceeds the quantity we have. Please check again.");
                            }
                        }
                    }

                    Header.Id = await TransID.GetTransId(context, "GREQ", DateTime.Today);
                    Header.GoodRequestNumber = Header.Id;
                    var InventoryGoodsReturnId = await TransID.GetTransId(context, "GRET", DateTime.Today);
                    if (Header.inv_status == GoodRequestStatus.GoodRequestReturn)
                    {
                        InventoryGoodsReturn modelGoodReturn = new InventoryGoodsReturn();
                        modelGoodReturn.Id = InventoryGoodsReturnId;
                        modelGoodReturn.GoodRequestNumber = Header.GoodRequestNumber;
                        modelGoodReturn.InventoryGoodsRequestId = Header.Id;
                        modelGoodReturn.GoodsReturnDateTime = Header.GoodRequestDatetime;
                        modelGoodReturn.ReturnDate = Header.DUeDate;
                        modelGoodReturn.Status = "0";
                        modelGoodReturn.TotalQty = Header.TotalQty;
                        modelGoodReturn.Type = Header.Type;
                        modelGoodReturn.RequestedBy = Header.CreatedBy;
                        modelGoodReturn.RequestedOn = Header.CreatedOn;
                        modelGoodReturn.OnBehalfId = Header.OnBehalfId;
                        modelGoodReturn.DueDate = Header.DUeDate;
                        modelGoodReturn.OrderReceipt = Header.OrderReceipt;
                        modelGoodReturn.Purpose = Header.Purpose;
                        modelGoodReturn.JobId = Header.JobId;
                        modelGoodReturn.JobName = Header.JobName;
                        modelGoodReturn.InvFaId = Header.InvFaId;
                        modelGoodReturn.InvFaName = Header.InvFaName;
                        modelGoodReturn.InvFaNumber = Header.InvFaId;
                        modelGoodReturn.IsActive = true;
                        modelGoodReturn.IsDeleted = false;
                        modelGoodReturn.GoodsReturnNumber = InventoryGoodsReturnId;
                        await context.PhoenixAddAsync(modelGoodReturn);
                    }

                    if (Details.Count > 0)
                    {
                        foreach (var item in Details)
                        {
                            item.Id = Guid.NewGuid().ToString();
                            item.GoodRequestId = Header.Id;
                            await context.PhoenixAddAsync(item);
                        }

                        if (Header.inv_status == GoodRequestStatus.GoodRequestReturn)
                        {
                            foreach (var item in Details)
                            {
                                InventoryGoodsReturnDetail Detail = new InventoryGoodsReturnDetail();
                                Detail.Id = Guid.NewGuid().ToString();
                                Detail.WareHouseIdTo = item.WarehouseIdTo;
                                Detail.WareHouseIdFrom = item.WarehouseIdFrom;
                                Detail.UomId = item.UomId;
                                Detail.UnitName = item.UomName;
                                Detail.TaskId = item.TaskId;
                                Detail.TaskDetail = item.TaskDetail;
                                Detail.SubBrand = item.Subbrand;
                                Detail.Status = "0";
                                Detail.Remark = item.Remark;
                                Detail.Qty = item.qty;
                                Detail.Purpose = item.Purpose;
                                Detail.OwnerId = item.OwnerId;
                                Detail.LocationName = item.LocationName;
                                Detail.Location = item.Location;
                                Detail.ItemName = item.ItemName;
                                Detail.ItemId = item.ItemId;
                                Detail.ItemCategoryName = item.ItemCategoryName;
                                Detail.ItemCategoryId = item.ItemCategoryId;
                                Detail.IsActive = true;
                                Detail.IsDeleted = false;
                                Detail.InvFaDetailId = item.InvFaDetailId;
                                Detail.InventoryGoodsReturnId = InventoryGoodsReturnId;
                                Detail.Brand = item.Brand;
                                Detail.FileAttachment = item.FileAttachment;

                                await context.PhoenixAddAsync(Detail);
                            }
                        }

                    }
                    await context.PhoenixAddAsync(Header);
                    await context.SaveChangesAsync();
                    var isApprove = true;

                    if (!(Header.Status == GoodRequestStatus.Open.ToString()))
                    {
                        await log.AddAsync(new StatusLog() { TransactionId = Header.Id, Status = GoodRequestStatus.StatusName(int.Parse(Header.Status)), Description = Header.LogDescription });
                        var approval = await context.TrTemplateApprovals.AsNoTracking().Where(x => x.RefId == Header.Id).ToListAsync();
                        if (approval.Count > 0)
                        {
                            var userApproval = await context.TrUserApprovals.AsNoTracking().Where(x => approval.Any(y => y.Id == x.TrTempApprovalId)).ToListAsync();
                            var conditionUserApproval = await context.TrConditionUserApprovals.AsNoTracking().Where(x => userApproval.Any(y => y.Id == x.TrUserApprovalId)).ToListAsync();

                            context.TrConditionUserApprovals.RemoveRange(conditionUserApproval);
                            context.TrUserApprovals.RemoveRange(userApproval);
                            context.TrTemplateApprovals.RemoveRange(approval);
                        }


                        var dataMenu = await menuService.GetByUniqueName(MenuUnique.GoodRequest).ConfigureAwait(false);
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var dataTempate = context.TmTemplateApprovals.Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            IdTemplate = dataTempate.Id,
                            MenuId = dataMenu.Id,
                            RefId = Header.Id,
                            DetailLink = $"{ApprovalLink.GoodRequest}?Id={Header.Id}",
                            NotificationType = Phoenix.Data.Models.NotificationTypeCode.GoodRequest,
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

            ///
            //Header.Id = Guid.NewGuid().ToString();
            //Header.GoodRequestNumber = await TransID.GetTransId(context, "GREQ", DateTime.Today);

            //await globalFcApproval.UnsafeSubmitApproval<GoodRequest>(false, Header.Id, int.Parse(Header.Status), $"{ApprovalLink.GoodRequest}?Id={Header.Id}&isApprove=true", idTemplate, MenuUnique.GoodRequest, Header);
            //if (Details.Count > 0)
            //    {
            //        List<GoodRequestDetail> igrd = new List<GoodRequestDetail>();
            //        foreach (GoodRequestDetail data in Details)
            //        {
            //            data.Id = Guid.NewGuid().ToString();
            //            data.GoodRequestId = Header.Id;
            //            igrd.Add(data);
            //        }

            //        GoodRequestDetail[] igrdArray = igrd.ToArray();
            //        await context.PhoenixAddRangeAsync(igrdArray);
            //    }

            //    return await context.SaveChangesAsync();
            //}
        }

        public Task<int> DeleteAsync(GoodRequest entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params GoodRequest[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteSoftAsync(GoodRequest entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteSoftRangeAsync(params GoodRequest[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> EditAsync(GoodRequest entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> EditRangeAsync(GoodRequest entity)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditWithDetailAsync(GoodRequest Header, List<GoodRequestDetail> Details)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (Details.Count > 0)
                    {
                        foreach (var item in Details)
                        {
                            if (item.qty > item.qtyMax)
                            {
                                throw new Exception("Your quantity request exceeds the quantity we have. Please check again.");
                            }
                        }
                    }

                    context.PhoenixEdit(Header);
                    if (Header.inv_status == GoodRequestStatus.GoodRequestReturn)
                    {
                        var modelGoodReturn = await context.InventoryGoodsReturn.Where(x => x.GoodRequestNumber == Header.GoodRequestNumber).FirstOrDefaultAsync();
                        modelGoodReturn.GoodRequestNumber = Header.GoodRequestNumber;
                        modelGoodReturn.InventoryGoodsRequestId = Header.Id;
                        modelGoodReturn.GoodsReturnDateTime = Header.GoodRequestDatetime;
                        modelGoodReturn.ReturnDate = Header.DUeDate;
                        modelGoodReturn.Status = "0";
                        modelGoodReturn.TotalQty = Header.TotalQty;
                        modelGoodReturn.Type = Header.Type;
                        modelGoodReturn.RequestedBy = Header.CreatedBy;
                        modelGoodReturn.RequestedOn = Header.CreatedOn;
                        modelGoodReturn.OnBehalfId = Header.OnBehalfId;
                        modelGoodReturn.DueDate = Header.DUeDate;
                        modelGoodReturn.OrderReceipt = Header.OrderReceipt;
                        modelGoodReturn.Purpose = Header.Purpose;
                        modelGoodReturn.JobId = Header.JobId;
                        modelGoodReturn.JobName = Header.JobName;
                        modelGoodReturn.InvFaId = Header.InvFaId;
                        modelGoodReturn.InvFaName = Header.InvFaName;
                        modelGoodReturn.InvFaNumber = Header.InvFaId;
                        modelGoodReturn.IsActive = true;
                        modelGoodReturn.IsDeleted = false;

                        context.PhoenixEdit(modelGoodReturn);

                    }


                    var Tmpdetail = context.GoodRequestDetail.AsNoTracking().Where(x => x.GoodRequestId == Header.Id).ToList();
                    if (Tmpdetail.Count > 0)
                    {
                        context.GoodRequestDetail.RemoveRange(Tmpdetail);
                    }

                    if (Details.Count > 0)
                    {
                        foreach (var item in Details)
                        {
                            item.Id = Guid.NewGuid().ToString();
                            item.GoodRequestId = Header.Id;
                            await context.PhoenixAddAsync(item);
                        }

                        if (Header.inv_status == GoodRequestStatus.GoodRequestReturn)
                        {
                            var modelGoodReturn = await context.InventoryGoodsReturn.Where(x => x.GoodRequestNumber == Header.GoodRequestNumber).FirstOrDefaultAsync();
                            var Tmpdetail2 = context.InventoryGoodsReturnDetail.AsNoTracking().Where(x => x.InventoryGoodsReturnId == modelGoodReturn.Id).ToList();
                            if (Tmpdetail2.Count > 0)
                            {
                                context.InventoryGoodsReturnDetail.RemoveRange(Tmpdetail2);
                            }
                            foreach (var item in Details)
                            {

                                InventoryGoodsReturnDetail Detail = new InventoryGoodsReturnDetail();
                                Detail.Id = Guid.NewGuid().ToString();
                                Detail.WareHouseIdTo = item.WarehouseIdTo;
                                Detail.WareHouseIdFrom = item.WarehouseIdFrom;
                                Detail.UomId = item.UomId;
                                Detail.UnitName = item.UomName;
                                Detail.TaskId = item.TaskId;
                                Detail.TaskDetail = item.TaskDetail;
                                Detail.SubBrand = item.Subbrand;
                                Detail.Status = "0";
                                Detail.Remark = item.Remark;
                                Detail.Qty = item.qty;
                                Detail.Purpose = item.Purpose;
                                Detail.OwnerId = item.OwnerId;
                                Detail.LocationName = item.LocationName;
                                Detail.Location = item.Location;
                                Detail.ItemName = item.ItemName;
                                Detail.ItemId = item.ItemId;
                                Detail.ItemCategoryName = item.ItemCategoryName;
                                Detail.ItemCategoryId = item.ItemCategoryId;
                                Detail.IsActive = true;
                                Detail.IsDeleted = false;
                                Detail.InvFaDetailId = item.InvFaDetailId;
                                Detail.InventoryGoodsReturnId = modelGoodReturn.Id;
                                Detail.Brand = item.Brand;
                                Detail.FileAttachment = item.FileAttachment;

                                await context.PhoenixAddAsync(Detail);
                            }
                        }
                    }

                    var isApprove = true;
                    if (!(Header.Status == GoodRequestStatus.Open.ToString()))
                    {
                        await log.AddAsync(new StatusLog() { TransactionId = Header.Id, Status = GoodRequestStatus.StatusName(int.Parse(Header.Status)), Description = Header.LogDescription });
                        var approval = await context.TrTemplateApprovals.AsNoTracking().Where(x => x.RefId == Header.Id).ToListAsync();
                        if (approval.Count > 0)
                        {
                            var userApproval = await context.TrUserApprovals.AsNoTracking().Where(x => approval.Any(y => y.Id == x.TrTempApprovalId)).ToListAsync();
                            var conditionUserApproval = await context.TrConditionUserApprovals.AsNoTracking().Where(x => userApproval.Any(y => y.Id == x.TrUserApprovalId)).ToListAsync();

                            context.TrConditionUserApprovals.RemoveRange(conditionUserApproval);
                            context.TrUserApprovals.RemoveRange(userApproval);
                            context.TrTemplateApprovals.RemoveRange(approval);
                        }


                        var dataMenu = await menuService.GetByUniqueName(MenuUnique.GoodRequest).ConfigureAwait(false);
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var dataTempate = context.TmTemplateApprovals.Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            IdTemplate = dataTempate.Id,
                            MenuId = dataMenu.Id,
                            RefId = Header.Id,
                            DetailLink = $"{ApprovalLink.GoodRequest}?Id={Header.Id}",
                            NotificationType = Phoenix.Data.Models.NotificationTypeCode.GoodRequest,
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



            //context.PhoenixEdit(Header);

            //if (Details.Count > 0)
            //{
            //    List<GoodRequestDetail> igrd = new List<GoodRequestDetail>();
            //    foreach (GoodRequestDetail data in Details)
            //    {
            //        //data.Id = Guid.NewGuid().ToString();
            //        data.GoodRequestId = Header.Id;
            //        igrd.Add(data);
            //    }

            //    GoodRequestDetail[] igrdArray = igrd.ToArray();
            //    context.PhoenixEditRange(igrdArray);
            //}
            ////else
            ////{
            ////    List<GoodRequestDetail> srdtemp = new List<GoodRequestDetail>();
            ////    var checkAvailable = await context.GoodRequestDetail.Where(x => x.GoodRequestId == Header.Id && x.IsDeleted.Equals(false)).ToListAsync();
            ////    foreach (GoodRequestDetail data in checkAvailable)
            ////    {
            ////        srdtemp.Add(data);
            ////    }

            ////    GoodRequestDetail[] srdArraytemp = srdtemp.ToArray();
            ////    context.PhoenixDeleteRange(srdArraytemp);
            ////}

            //return await context.SaveChangesAsync();
        }

        public async Task<List<GoodRequest>> Get()
        {
            var query = await (from gr in context.GoodRequest
                               join grd in (from g in context.GoodRequestDetail orderby g.ModifiedOn descending select g).Take(1) on gr.Id equals grd.GoodRequestId into left
                               from grd in left.DefaultIfEmpty()
                               select new GoodRequest
                               {
                                   Id = gr.Id,
                                   GoodRequestNumber = gr.GoodRequestNumber,
                                   GoodRequestDatetime = gr.GoodRequestDatetime,
                                   Type = gr.Type,
                                   DUeDate = gr.DUeDate,
                                   InvFaId = gr.InvFaId,
                                   InvFaName = gr.InvFaName,
                                   JobId = gr.JobId,
                                   JobName = gr.JobName,
                                   BusinessUnitId = gr.BusinessUnitId,
                                   BusinessUnitName = gr.BusinessUnitName,
                                   LegalEntityId = gr.LegalEntityId,
                                   LegalEntityName = gr.LegalEntityName,
                                   EmployeeName = gr.EmployeeName,
                                   Status = gr.Status,
                                   LocationTransfer = grd.WarehouseIdTo,
                                   Purpose = grd.Purpose,
                                   ReturnDate = gr.ReturnDate,
                                   Move_Loc = gr.Move_Loc,
                                   Inv_Out_Type = gr.Inv_Out_Type,
                                   inv_status = gr.inv_status,
                                   OnBehalfId = gr.OnBehalfId,
                                   behalf_div = gr.behalf_div,
                                   behalf_jobtitle = gr.behalf_jobtitle,
                                   division_id = gr.division_id,
                                   division_name = gr.division_name,
                                   inv_status_name = (from q in context.Inventory_GetForms
                                                      where q.Id == gr.inv_status
                                                      select q.getform_name).FirstOrDefault()
                               }
                               ).ToListAsync();
            return query;
            //return await context.GoodRequest.Where(x => x.IsDeleted.Equals(false)).ToListAsync();
        }

        public Task<List<GoodRequest>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<GoodRequest> Get(string Id)
        {
            return await context.GoodRequest.Where(x => x.IsDeleted.Equals(false) && x.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<List<GoodRequestDetail>> GetListDetail(string headerId)
        {
            var query = await context.GoodRequestDetail.Where(x => x.IsDeleted.Equals(false) && x.GoodRequestId == headerId).ToListAsync();

            foreach (var item in query)
            {
                var inv = await context.InventoryDetail.Where(x => x.Id == item.InvFaDetailId).FirstOrDefaultAsync();
                item.qtyMax = inv.Qty;
            }

            return query;

        }

        public async Task<GoodRequestDetail> GetDetail(string Id)
        {
            return await context.GoodRequestDetail.Where(x => x.IsDeleted.Equals(false) && x.Id == Id).FirstOrDefaultAsync();

        }

        public async Task<GoodRequestDTO> Approve(GoodRequestDTO dto)
        {

            try
            {
                var model = await context.GoodRequest.Where(x => x.Id == dto.Header.Id).FirstOrDefaultAsync();
                model.Id = dto.Header.Id;
                model.InvFaName = dto.Header.InvFaName;
                model.JobId = dto.Header.JobId;
                model.Move_Loc = dto.Header.Move_Loc;
                model.Remark = dto.Header.Remark;
                //model.TotalQty = dto.Header.TotalQty;
                model.division_id = dto.Header.division_id;
                model.LogDescription = dto.Header.LogDescription;

                var inventory = await context.Inventory.Where(x => x.Id == model.InvFaId).FirstOrDefaultAsync();
                //if (model.inv_status == GoodRequestStatus.GoodRequestReturn)
                //{
                //    inventory.QtyOnHand = inventory.QtyOnHand - model.TotalQty;
                //    inventory.QtyOutReturn = model.TotalQty;
                //    context.PhoenixEdit(inventory);
                //}


                if (dto.Details.Count > 0)
                {
                    //for (int i = 0; i < dto.Details.Count(); i++)
                    //{
                    //    var inv = await context.InventoryDetail.Where(x => x.Id == dto.Details[i].Id).FirstOrDefaultAsync();
                    //    var totalQtyRequest = model.TotalQty ?? 0;
                    //    if (inv.Qty > totalQtyRequest)
                    //    {
                    //        inv.Qty = inv.Qty - totalQtyRequest;
                    //    }
                    //    else
                    //    {
                    //        throw new Exception("The total quantity request entered is greater than the quantity available");
                    //    }
                    //    inv.Status = "2";
                    //    context.PhoenixEdit(inv);
                    //}
                    //var modelGoodReturn = await context.InventoryGoodsReturn.Where(x => x.InventoryGoodsRequestId == model.Id).FirstOrDefaultAsync();
                    foreach (var item in dto.Details)
                    {
                        var inv = await context.InventoryDetail.Where(x => x.Id == item.InvFaDetailId).FirstOrDefaultAsync();
                        var qtyGoodRequest = item.qty ?? 0;
                        //inv.Qty = inv.Qty - qtyGoodRequest;
                        if (model.inv_status == GoodRequestStatus.GoodRequestReturn)
                        {
                            inv.Status = "2";
                        }
                        else if (model.inv_status == GoodRequestStatus.GoodRequestNoReturn)
                        {
                            inv.Qty = inv.Qty - qtyGoodRequest;
                            inv.Status = "6";
                            //if (inv.Qty < 0)
                            //{
                            //    inv.Status = "6";
                            //}
                            //else
                            //{
                            //    inv.Status = "3";
                            //}

                        }
                        else if (model.inv_status == GoodRequestStatus.GoodRequestMoving)
                        {
                            inv.Location = dto.Header.Move_Loc;
                            //inv.Condition = "5";
                            inv.Status = "3";
                        }

                        context.PhoenixEdit(inv);

                        //var retDetail = await context.InventoryGoodsReturnDetail.Where(x => x.InventoryGoodsReturnId == modelGoodReturn.Id && x.InvFaDetailId == item.InvFaDetailId).FirstOrDefaultAsync();
                        //retDetail.Status = "3";
                        //context.PhoenixEdit(retDetail);

                    }
                }
                model.Status = GoodRequestStatus.Approved.ToString();
                context.PhoenixEdit(model);
                var save = await context.SaveChangesAsync();

                await globalFcApproval.ProcessApproval<GoodRequest>(model.Id, Int32.Parse(model.Status), model.LogDescription, model);
                await log.AddAsync(new StatusLog() { TransactionId = model.Id, Status = GoodRequestStatus.StatusName(int.Parse(model.Status)), Description = model.LogDescription });

                var modeltrapp = await context.TrTemplateApprovals.Where(x => x.RefId == model.Id).FirstOrDefaultAsync();
                if (modeltrapp != null)
                {
                    if (modeltrapp.StatusApprovedDescription == GoodRequestStatus.StatusName(GoodRequestStatus.Approved))
                    {
                        model.Status = GoodRequestStatus.Completed.ToString();
                    }
                    else
                    {
                        model.Status = GoodRequestStatus.WaitingApproval.ToString();
                    }
                }
                else
                {
                    model.Status = GoodRequestStatus.WaitingApproval.ToString();
                }


                return dto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task<int> Reject(GoodRequest entity)
        {
            try
            {
                var save = await globalFcApproval.ProcessApproval(entity.Id, GoodRequestStatus.Rejected, entity.LogDescription, entity);

                entity.Status = GoodRequestStatus.Rejected.ToString();
                entity.ModifiedOn = DateTime.Now;

                await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = GoodRequestStatus.StatusName(GoodRequestStatus.Rejected), Description = entity.LogDescription });

                context.GoodRequest.Update(entity);

                save = await context.SaveChangesAsync();
                return save;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        //public async Task<GoodRequest> Complete(GoodRequest model)
        //{
        //    try
        //    {
        //        model.Status = GoodRequestStatus.Completed.ToString();
        //        context.PhoenixEdit(model);
        //        var save = await context.SaveChangesAsync();
        //        return model;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message.ToString());
        //    }
        //}

        public async Task<GoodRequest> GetByNumber(string number)
        {
            return await context.GoodRequest.Where(x => x.IsDeleted.Equals(false) && x.GoodRequestNumber == number).FirstOrDefaultAsync();
        }
        public async Task<ParamValueGR> GetBehalf(string id)
        {
            List<ParamValueGR> list = new List<ParamValueGR>();
            try
            {
                var sql = string.Format(@"select DivisionId, DivisiName, JobTitleName from dbo.VwMappingEmployeeDivisiAndDeptAndJobTitle where EmployeeId = '{0}'", id);
                list = await context.ExecuteByQuery<ParamValueGR>(sql);

            }
            catch (Exception e)
            {
                throw new Exception("Error !, " + e.Message);
            }
            return list.FirstOrDefault();
        }
        public async Task<ParamValueGR> GetDivLogin(string basic)
        {
            List<ParamValueGR> list = new List<ParamValueGR>();
            try
            {
                var sql = string.Format(@"select DivisionId, DivisiName, JobTitleName from dbo.VwMappingEmployeeDivisiAndDeptAndJobTitle where Email = '{0}'", basic);
                list = await context.ExecuteByQuery<ParamValueGR>(sql);
            }
            catch (Exception e)
            {
                throw new Exception("Error !, " + e.Message);
            }
            return list.FirstOrDefault();

        }

        public async Task<dynamic> GetMasterConditionProcurement()
        {
            return await context.MasterConditionProcurement.Where(x => x.IsDeleted.Equals(false)).Select(x => new { Text = x.ConditionName, Value = x.Id }).ToListAsync();
        }

    }
}
