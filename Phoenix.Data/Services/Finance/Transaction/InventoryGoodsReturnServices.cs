using Phoenix.Data;
using Phoenix.Data.Models.Finance.Transaction.GoodReturn;
using Phoenix.Data.Services.Finance.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models.Enum;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;

namespace Phoenix.Data.Services.Finance.Transaction
{
    public interface IGoodsReturnServices : IDataService<InventoryGoodsReturn>
    {
        Task<int> AddWithDetailAsync(InventoryGoodsReturn Header, List<InventoryGoodsReturnDetail> Details);
        Task<int> EditWithDetailAsync(InventoryGoodsReturn Header);
        Task<InventoryGoodsReturnDetail> GetDetail(string Id);
        Task<List<InventoryGoodsReturnDetail>> GetListDetail(string headerId);
        Task<InventoryGoodsReturn> Approve(InventoryGoodsReturn model);
        Task<InventoryGoodsReturn> Complete(InventoryGoodsReturn model);
        Task<InventoryGoodsReturn> GetByNumber(string number);
        Task<InventoryGoodsReturn> GetByOpenStatus(string Id);
        Task<int> AddCustomAsync(InventoryGoodsReturn model);


    }
}
public class InventoryGoodsReturnServices : IGoodsReturnServices
{
    readonly DataContext context;
    readonly GlobalFunctionApproval globalFcApproval;
    readonly Phoenix.Data.Services.Um.IManageMenuService menuService;
    readonly StatusLogService log;
    readonly FileService uploadFile;

    public InventoryGoodsReturnServices(DataContext context, GlobalFunctionApproval globalFcApproval, Phoenix.Data.Services.Um.IManageMenuService menuService, StatusLogService log, FileService uploadFile)
    {
        this.context = context;
        this.globalFcApproval = globalFcApproval;
        this.menuService = menuService;
        this.log = log;
        this.uploadFile = uploadFile;
    }
    public Task<int> AddAsync(InventoryGoodsReturn entity)
    {
        throw new NotImplementedException();
    }
    public Task<int> AddCustomAsync(InventoryGoodsReturn model)
    {
        throw new NotImplementedException();
    }

    public Task<int> AddRangeAsync(params InventoryGoodsReturn[] entities)
    {
        throw new NotImplementedException();
    }

    public Task<int> AddWithDetailAsync(InventoryGoodsReturn Header, List<InventoryGoodsReturnDetail> Details)
    {
        throw new NotImplementedException();
    }

    public async Task<InventoryGoodsReturn> Approve(InventoryGoodsReturn model)
    {
        InventoryGoodsReturn DataMyDamApproval = new InventoryGoodsReturn();

        var entity = await context.InventoryGoodsReturn.AsNoTracking().Where(x => x.Id == model.Id).FirstOrDefaultAsync();
        entity.Status = model.Status.ToString();
       
        var dataInventory = await context.Inventory.AsNoTracking().Where(x => x.Id == entity.InvFaId).FirstOrDefaultAsync();
        var dataInventoryDetail = await context.InventoryDetail.AsNoTracking().Where(x => x.InventoryId == dataInventory.Id).ToListAsync();
        var goodReturnDetail = await context.InventoryGoodsReturnDetail.AsNoTracking().Where(x => x.InventoryGoodsReturnId == entity.Id).ToListAsync();
        
        if (entity.InvFaId == GoodRequestStatus.GoodRequestReturn)
        {
            //dataInventory.QtyOnHand = dataInventory.QtyOnHand + entity.TotalQty;
            //dataInventory.QtyOutReturn = dataInventory.QtyOutReturn - entity.TotalQty;
            foreach (var item in dataInventoryDetail)
            {
                //var detailReturn = goodReturnDetail.Where(x => x.InvFaDetailId == item.Id).FirstOrDefault();
                //detailReturn.Qty = 0;
                item.Status = "3";
                context.PhoenixEdit(dataInventoryDetail);
                //context.PhoenixEdit(goodReturnDetail);
            }
        }

        context.PhoenixEdit(dataInventory);
        context.PhoenixEdit(entity);
        var save = await context.SaveChangesAsync();

        await globalFcApproval.ProcessApproval<InventoryGoodsReturn>(model.Id, Convert.ToInt32(model.Status), model.LogDescription, entity);
        await log.AddAsync(new StatusLog() { TransactionId = model.Id, Status = Phoenix.Data.Models.StatusTransaction.StatusName(Convert.ToInt32(model.Status)), Description = model.LogDescription });

        var modeltrapp = await context.TrTemplateApprovals.Where(x => x.RefId == model.Id).FirstOrDefaultAsync();
        if (modeltrapp != null)
        {
            if (modeltrapp.StatusApprovedDescription == Phoenix.Data.Models.StatusTransaction.StatusName(3))
            {
                model.Status = "3";
            }
            else
            {
                model.Status = "2";
            }
        }
        else
        {
            model.Status = "2";
        }
        return model;
    }

    public Task<InventoryGoodsReturn> Complete(InventoryGoodsReturn model)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteAsync(InventoryGoodsReturn entity)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteRageAsync(params InventoryGoodsReturn[] entities)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteSoftAsync(InventoryGoodsReturn entity)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteSoftRangeAsync(params InventoryGoodsReturn[] entities)
    {
        throw new NotImplementedException();
    }

    public Task<int> EditAsync(InventoryGoodsReturn entity)
    {
        throw new NotImplementedException();
    }

    public Task<int> EditRangeAsync(InventoryGoodsReturn entity)
    {
        throw new NotImplementedException();
    }

    public async Task<int> EditWithDetailAsync(InventoryGoodsReturn Header)
    {
        var isApprove = false;
        var result = 0;
        using (var transaction = await context.Database.BeginTransactionAsync())
        {
            var approval = await context.TrTemplateApprovals.AsNoTracking().Where(x => x.RefId == Header.Id).ToListAsync();
            var entity = await context.InventoryGoodsReturn.AsNoTracking().Where(x => x.Id == Header.Id).FirstOrDefaultAsync();
            entity.GoodsReturnDateTime = Header.GoodsReturnDateTime;
            entity.Status = Header.Status;
            context.PhoenixEdit(entity);
            try
            {
                if (approval.Count > 0)
                {
                    var userApproval = await context.TrUserApprovals.AsNoTracking().Where(x => approval.Any(y => y.Id == x.TrTempApprovalId)).ToListAsync();
                    var conditionUserApproval = await context.TrConditionUserApprovals.AsNoTracking().Where(x => userApproval.Any(y => y.Id == x.TrUserApprovalId)).ToListAsync();

                    context.TrConditionUserApprovals.RemoveRange(conditionUserApproval);
                    context.TrUserApprovals.RemoveRange(userApproval);
                    context.TrTemplateApprovals.RemoveRange(approval);
                }

                var dataMenu = await menuService.GetByUniqueName(Phoenix.Data.Models.MenuUnique.InventoryGoodReturn).ConfigureAwait(false);
                var dataTempate = context.TmTemplateApprovals.Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
                var vm = new Phoenix.Data.Models.ViewModel.TransApprovalHrisVm()
                {
                    IdTemplate = dataTempate.Id,
                    MenuId = dataMenu.Id,
                    RefId = entity.Id,
                    DetailLink = $"{Phoenix.Data.Models.ApprovalLink.InventoryGoodRequest}?Id={entity.Id}",
                    NotificationType = Phoenix.Data.Models.NotificationTypeCode.InventoryGoodReturn,
                    Module = Phoenix.Data.Models.NotificationModuleType.ModuleFinance
                };

                var subGroupId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(context.HttpContext);
                var divisionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(context.HttpContext);
                var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);

                isApprove = await globalFcApproval.Save(vm, subGroupId, divisionId, employeeId);
                await log.AddAsync(new Phoenix.Data.Models.StatusLog()
                {
                    TransactionId = entity.Id,
                    Status = Phoenix.Data.Models.StatusTransaction.StatusName(Phoenix.Data.Models.StatusTransaction.WaitingApproval),
                    Description = Header.LogDescription
                });

                if (isApprove)
                {
                    var save = await context.SaveChangesAsync();
                    transaction.Commit();
                    result = save;
                }
                else
                {
                    transaction.Rollback();
                    throw new Exception("please check the approval template that will be processed");
                }
                return result;

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }
    }


    public async Task<List<InventoryGoodsReturn>> Get()
    {
        List<InventoryGoodsReturn> List = new List<InventoryGoodsReturn>();

        //List = await (
        //    from gr in context.GoodRequest.Where(x => x.IsDeleted != null && x.Status.Equals("2"))
        //    join grd in context.GoodRequestDetail.Where(x => x.IsDeleted != true) on gr.Id equals grd.GoodRequestId into lj1
        //    from lgrd in lj1.DefaultIfEmpty()
        //    join igr in context.InventoryGoodsReturn.Where(x => x.IsDeleted != null) on gr.GoodRequestNumber equals igr.InventoryGoodsRequestId into aab
        //    from bnull in aab.DefaultIfEmpty()
        //    select new InventoryGoodsReturn()
        //    {
        //        Id = bnull.Id,
        //        GoodRequestNumber = gr.GoodRequestNumber,
        //         GoodsReturnNumber = bnull.Id,
        //        InvFaId = lgrd.InvFaDetailId,
        //        ReturnDate = bnull.ReturnDate,
        //        Type = context.GoodRequest.Where(x => x.Id == lgrd.GoodRequestId).SingleOrDefault().Type,
        //        InvFaNumber = context.GoodRequest.Where(x => x.Id == lgrd.GoodRequestId).SingleOrDefault().Type == "FIXED ASSET" ? context.FixedAsset.Where(x => x.Id.Equals(lgrd.InvFaDetailId)).SingleOrDefault().FixedAssetNumber : context.Inventory.Where(x => x.Id.Equals(lgrd.InvFaDetailId)).SingleOrDefault().InventoryInNumber,
        //        InvFaName = context.GoodRequest.Where(x => x.Id == lgrd.GoodRequestId).SingleOrDefault().Type == "FIXED ASSET" ? context.FixedAsset.Where(x => x.Id.Equals(lgrd.InvFaDetailId)).SingleOrDefault().FixedAssetName : context.Inventory.Where(x => x.Id.Equals(lgrd.InvFaDetailId)).SingleOrDefault().InventoryName,
        //        RequestedBy = context.EmployeeBasicInfos.Where(x => x.Id.Equals(lgrd.CreatedBy)).SingleOrDefault().NameEmployee,
        //        Status = bnull.Status == null ? context.GoodRequest.Where(x => x.Id == lgrd.GoodRequestId).SingleOrDefault().Status : bnull.Status
        //    }
        //    ).ToListAsync();

        List = await (from q in context.InventoryGoodsReturn
                      where q.IsDeleted != true 
                      select new InventoryGoodsReturn()
                      {
                          Id = q.Id,
                          GoodsReturnNumber = q.GoodsReturnNumber,
                          GoodRequestNumber = q.InventoryGoodsRequestId,
                          DueDate = q.DueDate,
                          ReturnDate = q.ReturnDate,
                          InvFaId = q.InvFaId,
                          InvFaName = q.InvFaName,
                          InvFaNumber = q.InvFaNumber,
                          RequestedBy = context.EmployeeBasicInfos.Where(x => x.Id == (q.RequestedBy ?? q.CreatedBy)).Select(x => x.NameEmployee).FirstOrDefault(),
                          DivisionId = context.BusinessUnits.Where(x => x.Id == q.DivisionId).Select(x => x.UnitName).FirstOrDefault(),
                          Status = GoodRequestStatus.StatusName(Convert.ToInt32(q.Status))
                      }).ToListAsync();

        return List;
    }

    public Task<List<InventoryGoodsReturn>> Get(int skip, int take)
    {
        throw new NotImplementedException();
    }

    public async Task<InventoryGoodsReturn> Get(string id)
    {
        var data = await (from q in context.InventoryGoodsReturn
                          where q.IsDeleted != true && q.Id == id
                          select new InventoryGoodsReturn()
                          {
                              Id = q.Id,
                              GoodsReturnNumber = q.GoodsReturnNumber,
                              GoodRequestNumber = q.InventoryGoodsRequestId,
                              DueDate = q.DueDate,
                              ReturnDate = q.ReturnDate,
                              InvFaId = q.InvFaId,
                              InvFaName = q.InvFaName,
                              InvFaNumber = q.InvFaNumber,
                              RequestedBy = context.EmployeeBasicInfos.Where(x => x.Id == (q.RequestedBy ?? q.CreatedBy)).Select(x => x.NameEmployee).FirstOrDefault(),
                              DivisionId = context.BusinessUnits.Where(x => x.Id == q.DivisionId).Select(x => x.UnitName).FirstOrDefault(),
                              Status = GoodRequestStatus.StatusName(Convert.ToInt32(q.Status)),
                              GoodReceiptDatetime = q.GoodsReturnDateTime,
                              InventoryGoodsRequestId = q.InventoryGoodsRequestId,
                              GoodsReturnDateTime = q.DueDate,
                              Type = q.Type,
                              OnBehalfId = q.OnBehalfId,
                              BehalfJobTitle = context.GoodRequest.Where(x => x.Id == q.InventoryGoodsRequestId).Select(x => x.behalf_jobtitle).FirstOrDefault(),
                              DivisionName = context.GoodRequest.Where(x => x.Id == q.InventoryGoodsRequestId).Select(x => x.division_name).FirstOrDefault(),
                              OnBehalfName = context.EmployeeBasicInfos.Where(x => x.Id == q.OnBehalfId).Select(x => x.NameEmployee).FirstOrDefault(),

                          }).FirstOrDefaultAsync();
        return data;
    }

    public Task<InventoryGoodsReturn> GetByNumber(string number)
    {
        throw new NotImplementedException();
    }

    public async Task<InventoryGoodsReturn> GetByOpenStatus(string Id)
    {
        var query = await (
            from gr in context.GoodRequest
            join grd in context.GoodRequestDetail on gr.Id equals grd.GoodRequestId
            where grd.Id.Equals(Id)
            select new InventoryGoodsReturn()
            {
                GoodRequestNumber = gr.GoodRequestNumber,
                Type = gr.Type,
                RequestedBy = gr.RequestedBy,
                RequestedOn = gr.RequestedOn,
                InvFaName = gr.Type == "FIXED ASSET" ? context.FixedAsset.Where(x => x.Id.Equals(grd.InvFaDetailId)).SingleOrDefault().FixedAssetName : context.Inventory.Where(x => x.Id.Equals(grd.InvFaDetailId)).SingleOrDefault().InventoryName,
                Purpose = grd.Purpose,
                OnBehalfId = gr.OnBehalfId

            }
                     ).SingleOrDefaultAsync();
        return query;
    }

    public Task<InventoryGoodsReturnDetail> GetDetail(string Id)
    {
        throw new NotImplementedException();
    }

    public Task<List<InventoryGoodsReturnDetail>> GetListDetail(string headerId)
    {
        throw new NotImplementedException();
    }
}
