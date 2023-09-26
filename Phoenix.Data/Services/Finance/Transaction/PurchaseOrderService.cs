using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Phoenix.Data.Models.Finance.Transaction;
using Phoenix.Data.Models;
using Phoenix.Data.Attributes;

namespace Phoenix.Data.Services.Finance.Transaction
{
    public interface IPurchaseOrderService
    {
        Task<int> ApproveAsync(PurchaseOrder entity);
        Task<int> AddAsync(PurchaseOrder entity);
        Task<int> EditAsync(PurchaseOrder entity);
        Task<List<PurchaseOrder>> GetPOList(string status);
        Task<PurchaseOrder> Get(string id);
        Task<List<PurchaseOrderDetail>> GetDetailPOList(string id);
        Task<List<PurchaseOrder>> GetByVendor(string vendorId);
        Task<EmployeeBasicInfo> GetDataEmployee(string Id);
        Task<dynamic> Print(string Id);

        //Task<bool> UpdateBudget(string purchaseOrderId, string businessUnitId);
    }

    public class PurchaseOrderService : IPurchaseOrderService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly Services.Um.IManageMenuService menuService;
        readonly IPurchaseRequestService prService;
        readonly StatusLogService log;

        public PurchaseOrderService(DataContext context, GlobalFunctionApproval globalFcApproval, Services.Um.IManageMenuService menuService, IPurchaseRequestService prService, StatusLogService log)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.menuService = menuService;
            this.prService = prService;
            this.log = log;
        }

        public async Task<int> AddAsync(PurchaseOrder entity)
        {
            return await AddEdit(entity, false);
        }

        public async Task<int> EditAsync(PurchaseOrder entity)
        {
            return await AddEdit(entity, true);
        }

        private async Task<int> AddEdit(PurchaseOrder entity, bool isEdit)
        {
            var task = entity.PurchaseOrderDetail.Where(x => x.ParentTaskId == null).Select(x => new { x.TaskId, x.TotalExchangeAmount }).ToList();
            var parent = entity.PurchaseOrderDetail.Where(x => x.ParentTaskId != null).GroupBy(x => x.ParentTaskId).Select(x => new { TaskId = x.Key, TotalExchangeAmount = x.Sum(y => y.TotalExchangeAmount) }).ToList();

            var join = (from a in task
                        join b in parent on a.TaskId equals b.TaskId
                        where a.TotalExchangeAmount != b.TotalExchangeAmount
                        select new { a.TaskId, a.TotalExchangeAmount }).ToList();

            if (parent.Count() > 0)
            {
                if (join.Count() > 0)
                {
                    throw new Exception("the value of the task is not the same as the value of the sub task");
                }
            }

            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (entity.PurchaseOrderDetail != null)
                    {
                        if (entity.PurchaseOrderDetail.Where(x => string.IsNullOrEmpty(x.ItemTypeId) && x.ParentTaskId == null).Count() > 0)
                        {
                            throw new Exception("for the 'type' column in the purchase order the details cannot be empty. Please check the purchase order detail data again.");
                        }
                    }

                    if (string.IsNullOrEmpty(entity.Status)) { entity.Status = PurchaseOrderStatus.Draft; }

                    if (entity.Status == PurchaseOrderStatus.Draft ||
                        entity.Status == PurchaseOrderStatus.WaitingApproval)
                    {
                        entity.CreatedBy = context.GetEmployeeId();
                        entity.CreatedOn = DateTime.Now;
                    }

                    if (isEdit)
                    {
                        context.PhoenixEdit(entity);
                    }
                    else
                    {
                        entity.Id = await TransID.GetTransId(context, Code.PCA, DateTime.Today);
                        entity.PurchaseOrderNumber = entity.Id;
                        await context.PhoenixAddAsync(entity);
                        await context.SaveChangesAsync();
                    }

                    if (!string.IsNullOrEmpty(entity.Id))
                    {
                        var detail = context.PurchaseOrderDetails.AsNoTracking().Where(x => x.PurchaseOrderId == entity.Id).ToList();
                        if (detail.Count > 0)
                        {
                            context.PurchaseOrderDetails.RemoveRange(detail);
                        }
                    }

                    var currency = entity.PurchaseOrderDetail.Where(x => x.CurrencyId != null && x.ParentTaskId == null).FirstOrDefault();
                    if (entity.PurchaseOrderDetail != null)
                    {
                        foreach (var item in entity.PurchaseOrderDetail)
                        {
                            item.Id = Guid.NewGuid().ToString();
                            item.PurchaseOrderId = entity.Id;
                            item.ExchangeRate = entity.ExchangeRate;

                            if (item.ParentTaskId != null)
                            {
                                item.CurrencyId = currency.CurrencyId;
                                item.ExchangeRate = currency.ExchangeRate;
                                item.Operator = currency.Operator;
                            }
                            await context.PhoenixAddAsync(item);
                        };
                    }

                    if (entity.Status == PurchaseOrderStatus.Cancelled)
                    {
                        if (context.PurchaseOrders.AsNoTracking().Where(x => x.Id == entity.Id && x.Status == PurchaseOrderStatus.Approved).Count() > 0)
                        {
                            if (!string.IsNullOrEmpty(entity.PurchaseRequestId))
                            {
                                await CancelBudget(entity.Id, entity.PurchaseRequestId);
                            }

                            //Update budget PCA - mengembalikan nilai pca di karenakan PO nya di cancel
                            if (!string.IsNullOrEmpty(entity.JobPaId))
                            {
                                //var poDetail = await context.PurchaseOrderDetails.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.PurchaseOrderId == entity.Id).GroupBy(x => x.TaskId).Select(x => new { x.First().TaskId, total = x.Sum(y => y.Amount) }).ToListAsync();
                                var poDetail = await context.PurchaseOrderDetails.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.PurchaseOrderId == entity.Id).ToListAsync();
                                var pcaTask = await context.PcaTasks.AsNoTracking().Where(x => poDetail.Any(z => z.TaskId == x.Id)).ToListAsync();

                                //update budget PCA - mengembalikan nilai pca task di karenakan PO nya di cancel
                                foreach (var item in pcaTask)
                                {
                                    var get = poDetail.FirstOrDefault(x => x.TaskId == item.Id);
                                    if (get != null)
                                    {
                                        //item.TotalPriceUsed -= (get.total ?? 0);
                                        //item.TotalPriceBalance = item.TotalPrice - item.TotalPriceUsed;

                                        item.TotalPriceUsed = (item.TotalPriceUsed - get.TotalExchangeAmount);
                                        item.TotalPriceBalance = item.TotalPrice - item.TotalPriceUsed;

                                        if (item.TotalPriceBalance == null)
                                        {
                                            throw new Exception("Remaining Budget Cannot be Empty.");
                                        }

                                        item.Status = PCAStatus.Open;
                                        context.PhoenixEdit(item);
                                    }
                                }

                                await context.SaveChangesAsync();
                                var pca = await context.Pcas.AsNoTracking().Where(x => x.Id == entity.JobPaId).FirstOrDefaultAsync();
                                if (pca != null)
                                {
                                    pca.TotalTaskPriceUsed = context.PcaTasks.Where(x => !(x.IsDeleted ?? false) && x.PcaId == pca.Id).Sum(x => (x.TotalPriceUsed ?? 0));
                                    pca.TotalTaskPriceBalance = context.PcaTasks.Where(x => !(x.IsDeleted ?? false) && x.PcaId == pca.Id).Sum(x => (x.TotalPriceBalance ?? 0));
                                    context.Pcas.Update(pca);
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(entity.JobPaId))
                            {
                                //var poDetail = await context.PurchaseOrderDetails.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.PurchaseOrderId == entity.Id).GroupBy(x => x.TaskId).Select(x => new { x.First().TaskId, total = x.Sum(y => y.Amount) }).ToListAsync();
                                var poDetail = await context.PurchaseOrderDetails.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.PurchaseOrderId == entity.Id).ToListAsync();
                                var pcaTask = await context.PcaTasks.AsNoTracking().Where(x => poDetail.Any(z => z.TaskId == x.Id)).ToListAsync();

                                foreach (var item in pcaTask)
                                {
                                    var get = poDetail.FirstOrDefault(x => x.TaskId == item.Id);
                                    if (get != null)
                                    {
                                        item.TotalPriceUsed = (item.TotalPriceUsed - get.TotalExchangeRequest);
                                        item.TotalPriceBalance = item.TotalPrice - item.TotalPriceUsed;

                                        item.Status = PCAStatus.Open;
                                        context.PhoenixEdit(item);
                                    }
                                };

                                await context.SaveChangesAsync();
                                var pca = await context.Pcas.AsNoTracking().Where(x => x.Id == entity.JobPaId).FirstOrDefaultAsync();
                                if (pca != null)
                                {
                                    pca.TotalTaskPriceUsed = context.PcaTasks.Where(x => !(x.IsDeleted ?? false) && x.PcaId == pca.Id).Sum(x => (x.TotalPriceUsed ?? 0));
                                    pca.TotalTaskPriceBalance = context.PcaTasks.Where(x => !(x.IsDeleted ?? false) && x.PcaId == pca.Id).Sum(x => (x.TotalPriceBalance ?? 0));
                                    context.Pcas.Update(pca);
                                }
                            }
                        }
                    }

                    if (entity.Status == PurchaseOrderStatus.Closed)
                    {
                        if (!string.IsNullOrEmpty(entity.JobPaId))
                        {
                            var poDetail = await context.PurchaseOrderDetails.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.PurchaseOrderId == entity.Id).GroupBy(x => x.TaskId).Select(x => new { x.First().TaskId, total = x.Sum(y => y.Amount) }).ToListAsync();
                            var pcaTask = await context.PcaTasks.AsNoTracking().Where(x => poDetail.Any(z => z.TaskId == x.Id)).ToListAsync();

                            foreach (var item in pcaTask)
                            {
                                item.Status = PCAStatus.Close;
                                context.PhoenixEdit(item);
                            }
                        }
                    }

                    if (entity.Status == PurchaseOrderStatus.Closed || entity.Status == PurchaseOrderStatus.Cancelled)
                    {
                        if (context.InvoiceReceivables.AsNoTracking().Where(x => x.PoId == entity.Id && x.InvoiceStatusId != InvoiceReceivedStatus.Posted).Count() > 0)
                        {
                            var inv = await context.InvoiceReceivables.AsNoTracking().Where(x => x.PoId == entity.Id && x.InvoiceStatusId != InvoiceReceivedStatus.Posted).ToListAsync();
                            foreach (var item in inv)
                            {
                                item.InvoiceStatusId = InvoiceReceivedStatus.Cancelled;
                                context.PhoenixEdit(item);
                            }
                        }
                    }

                    var isApprove = true;
                    if (!(entity.Status == PurchaseOrderStatus.Draft || entity.Status == PurchaseOrderStatus.Cancelled || entity.Status == PurchaseOrderStatus.Closed))
                    {
                        var dataMenu = await menuService.GetByUniqueName(MenuUnique.PO).ConfigureAwait(false);
                        var dataTempate = context.TmTemplateApprovals.Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            IdTemplate = dataTempate.Id,
                            MenuId = dataMenu.Id,
                            RefId = entity.Id,
                            DetailLink = $"{ApprovalLink.PO}?Id={entity.Id}",
                            NotificationType = NotificationTypeCode.PONotification,
                            Module = NotificationModuleType.ModuleFinance
                        };
                        var subGroupId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(context.HttpContext);
                        var divisionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(context.HttpContext);
                        var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
                        isApprove = await globalFcApproval.Save(vm, subGroupId, divisionId, employeeId);
                        if (isApprove)
                        {
                            await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = entity.Status, Description = entity.RemarksProcess });
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
                        context.PurchaseOrders.Update(entity);
                        await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = entity.Status, Description = entity.RemarksProcess });
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

        public async Task<int> ApproveAsync(PurchaseOrder entity)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var save = await globalFcApproval.UnsafeProcessApproval(entity.Id, entity.StatusApproval, entity.RemarksProcess, entity);
                    var approval = context.TrTemplateApprovals.AsNoTracking().Where(x => x.RefId == entity.Id && !(x.IsDeleted ?? false)).FirstOrDefault();
                    if (approval != null)
                    {
                        if (approval.StatusApproved == Models.Um.StatusApproved.Approved)
                        {
                            entity.Status = PurchaseOrderStatus.Approved;

                            if (!string.IsNullOrEmpty(entity.JobPaId))
                            {
                                foreach (var detail in entity.PurchaseOrderDetail)
                                {
                                    if (detail.ParentTaskId == null)
                                    {
                                        var pcaTask = await context.PcaTasks.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.PcaId == entity.JobPaId && x.Id == detail.TaskId).FirstOrDefaultAsync();
                                        if (pcaTask == null) throw new Exception("PCA Task cannot be empty");
                                        pcaTask.TotalPriceUsed = (pcaTask.TotalPriceUsed - detail.TotalExchangeRequest) + detail.TotalExchangeAmount;
                                        pcaTask.TotalPriceBalance = pcaTask.TotalPrice - pcaTask.TotalPriceUsed;

                                        //PCA task update Closenya ketika proses job closing
                                        if (pcaTask.TotalPriceBalance > 0) pcaTask.Status = PCAStatus.Open;
                                        else pcaTask.Status = PCAStatus.Close;

                                        context.PhoenixEdit(pcaTask);
                                    }
                                }

                                await context.SaveChangesAsync();
                                var pca = await context.Pcas.AsNoTracking().Where(x => x.Id == entity.JobPaId).FirstOrDefaultAsync();
                                if (pca != null)
                                {
                                    pca.TotalTaskPriceUsed = context.PcaTasks.Where(x => !(x.IsDeleted ?? false) && x.PcaId == pca.Id).Sum(x => (x.TotalPriceUsed ?? 0));
                                    pca.TotalTaskPriceBalance = context.PcaTasks.Where(x => !(x.IsDeleted ?? false) && x.PcaId == pca.Id).Sum(x => (x.TotalPriceBalance ?? 0));
                                    context.Pcas.Update(pca);
                                }
                            }

                            //proses update budget master
                            if (!string.IsNullOrEmpty(entity.PurchaseRequestId))
                            {
                                foreach (var detail in entity.PurchaseOrderDetail)
                                {
                                    await prService.UpdateAllBudget(detail.ItemId, true, (detail.TotalExchangeRequest ?? 0), (detail.TotalExchangeAmount ?? 0));
                                }

                                await UpdateBudget(entity.Id, true);
                            }
                        }
                        else if (approval.StatusApproved == Models.Um.StatusApproved.Rejected)
                        {
                            entity.Status = PurchaseOrderStatus.Rejected;

                            if (!string.IsNullOrEmpty(entity.JobPaId))
                            {
                                //update Status PCA Task
                                var poDetail = await context.PurchaseOrderDetails.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.PurchaseOrderId == entity.Id).ToListAsync();
                                var pcaTask = await context.PcaTasks.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && poDetail.Any(z => z.TaskId == x.Id)).ToListAsync();
                                if (pcaTask == null) throw new Exception("PCA Task cannot be empty");
                                foreach (var item in pcaTask)
                                {
                                    var get = poDetail.FirstOrDefault(x => x.TaskId == item.Id);
                                    if (get != null)
                                    {
                                        item.Status = PCAStatus.Open;
                                        item.TotalPriceUsed = (item.TotalPriceUsed - get.TotalExchangeRequest);
                                        item.TotalPriceBalance = item.TotalPrice - item.TotalPriceUsed;

                                        if (item.TotalPriceBalance < 0)
                                        {
                                            throw new Exception("the total exchange requests must be less than or equal to the remaining budget");
                                        }

                                        context.PhoenixEdit(item);
                                    }
                                }

                                await context.SaveChangesAsync();
                                var pca = await context.Pcas.AsNoTracking().Where(x => x.Id == entity.JobPaId).FirstOrDefaultAsync();
                                if (pca != null)
                                {
                                    pca.TotalTaskPriceUsed = context.PcaTasks.Where(x => !(x.IsDeleted ?? false) && x.PcaId == pca.Id).Sum(x => (x.TotalPriceUsed ?? 0));
                                    pca.TotalTaskPriceBalance = context.PcaTasks.Where(x => !(x.IsDeleted ?? false) && x.PcaId == pca.Id).Sum(x => (x.TotalPriceBalance ?? 0));
                                    context.Pcas.Update(pca);
                                }

                                ////Upate status RFQ menjadi Rejected
                                //var rfq = await context.RequestForQuotation.AsNoTracking().Where(x => x.Id == entity.RequestForQuotationId).FirstOrDefaultAsync();
                                //if (rfq != null)
                                //{
                                //    rfq.Status = entity.Status;
                                //    context.RequestForQuotation.Update(rfq);
                                //}
                            }

                            //Update budget purchase request
                            if (!string.IsNullOrEmpty(entity.PurchaseRequestId))
                            {
                                foreach (var detail in entity.PurchaseOrderDetail)
                                {
                                    await prService.UpdateAllBudget(detail.ItemId, false, (detail.TotalExchangeRequest ?? 0), (detail.TotalExchangeAmount ?? 0));
                                }

                                await prService.UpdateBudget(entity.PurchaseRequestId, false);
                            }
                        }

                        context.PurchaseOrders.Update(entity);
                        await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = entity.Status, Description = entity.RemarksProcess });
                    }

                    save = await context.SaveChangesAsync();
                    transaction.Commit();
                    return save;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<List<PurchaseOrder>> GetPOList(string status)
        {
            //yang bisa akses ke pca adalah pm team, creator dan approval
            var userId = context.GetUserId();
            var employeeId = context.GetEmployeeId();

            var team = await (from a in context.PmTeams
                              join b in context.PmTeamMembers on a.Id equals b.TeamId
                              where !(a.IsDeleted ?? false) && !(b.IsDeleted ?? false) && b.ApplicationUserId == userId
                              select new { a.JobId }).ToListAsync();

            var approval = await (from a in context.TrTemplateApprovals
                                  join b in context.TrUserApprovals on a.Id equals b.TrTempApprovalId
                                  where b.EmployeeBasicInfoId == employeeId
                                  select new { a.RefId }).ToListAsync();

            var result = await (from a in context.PurchaseOrders
                                join b in context.EmployeeBasicInfos on a.CreatedBy equals b.Id
                                join c in context.LegalEntity on a.LegalEntityId equals c.Id
                                where !(a.IsDeleted ?? false) &&
                                (string.IsNullOrEmpty(status) ? true : (a.Status ?? PurchaseOrderStatus.Draft) == status)
                                select new PurchaseOrder
                                {
                                    Id = a.Id,
                                    RequestForQuotationId = a.RequestForQuotationId,
                                    RequestForQuotationNumber = a.RequestForQuotationNumber,
                                    PurchaseOrderNumber = a.PurchaseOrderNumber,
                                    PurchaseRequestId = a.PurchaseRequestId,
                                    CashAdvanceId = a.CashAdvanceId,
                                    PaymentRequestId = a.PaymentRequestId,
                                    CashAdvanceSettlementId = a.CashAdvanceSettlementId,
                                    JobId = a.JobId,
                                    RequestorName = b.NameEmployee,
                                    CompanyId = a.CompanyId,
                                    CompanyName = a.CompanyName,
                                    GrandTotal = a.GrandTotal,
                                    Status = a.Status,
                                    ModifiedOn = a.ModifiedOn,
                                    CurrencySymbol = context.Currencies.Where(x => x.Id == a.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault(),
                                    LegalEntityName = c.LegalEntityName
                                }).ToListAsync();
            return result.OrderByDescending(x => x.ModifiedOn).ToList();
        }


        public async Task<PurchaseOrder> Get(string id)
        {
            var result = await context.PurchaseOrders.Where(x => !(x.IsDeleted ?? false) && x.Id == id).FirstOrDefaultAsync();
            if (result != null)
            {
                result.CurrencySymbol = context.Currencies.Where(x => x.Id == result.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault();
                result.CurrencySymbolExchange = context.Currencies.Where(x => context.RequestForQuotation.Any(y => y.CurrencyId == x.Id && y.Id == result.RequestForQuotationId)).Select(x => x.CurrencySymbol).FirstOrDefault();

                if (!string.IsNullOrEmpty(result.RequestForQuotationId))
                {
                    var rfq = await context.RequestForQuotation.Where(x => x.Id == result.RequestForQuotationId).FirstOrDefaultAsync();
                    if (rfq != null)
                    {
                        result.JobPaId = rfq.JobPaId;
                        result.JobPeId = rfq.JobPeId;

                        var pca = await context.Pcas.Where(x => x.Id == rfq.JobPaId).FirstOrDefaultAsync();
                        if (pca != null) result.ShareServicesId = pca.ShareservicesId;
                    }
                }

                if (!string.IsNullOrEmpty(result.JobId))
                {
                    var job = await context.JobDetail.Where(x => x.Id == result.JobId).FirstOrDefaultAsync();
                    if (job != null) result.JobName = job.JobName;
                }

                if (!string.IsNullOrEmpty(result.RequestForQuotationId))
                {
                    var rfqVendor = await context.RequestForQuotationVendor.Where(x => x.RequestForQuotationId == result.RequestForQuotationId && x.VendorId == result.CompanyId).FirstOrDefaultAsync();
                    if (rfqVendor != null)
                    {
                        result.VatPercentRFQ = rfqVendor.VatPercent;
                        result.VatRFQ = rfqVendor.Vat;
                        result.SubTotalRFQ = rfqVendor.SubTotalVendor;
                        result.GrandTotalRFQ = rfqVendor.TotalAmountVendor;
                    }
                }

                if (!string.IsNullOrEmpty(result.CashAdvanceSettlementId))
                {
                    var cas = await context.CASettlement.Where(x => x.Id == result.CashAdvanceSettlementId).FirstOrDefaultAsync();
                    if (cas != null)
                    {
                        result.VatPercentRFQ = Convert.ToDecimal(cas.VatPercent ?? 0);
                        result.VatRFQ = cas.Vat;
                        result.SubTotalRFQ = cas.Amount;
                        result.GrandTotalRFQ = cas.TotalAmount;

                        var pce = await context.Pces.Where(x => x.JobId == result.JobId).FirstOrDefaultAsync();
                        if (pce != null)
                        {
                            result.JobPaId = pce.PcaId;
                            result.JobPeId = pce.Id;
                            result.ShareServicesId = pce.ShareservicesId;
                        }
                    }
                }

                result.IsCancel = context.InvoiceReceivables.Where(x => x.PoId == id && x.InvoiceStatusId != InvoiceReceivedStatus.Posted).Count() >= 0;
                result.IsClose = context.InvoiceReceivables.Where(x => x.PoId == id && x.InvoiceStatusId == InvoiceReceivedStatus.Posted).Count() > 0;
            }
            return result;
        }

        public async Task<List<PurchaseOrderDetail>> GetDetailPOList(string id)
        {
            var result = await (from a in context.PurchaseOrderDetails
                                    //join b in context.Uom on a.UomId equals b.Id
                                join d in context.PurchaseOrders on a.PurchaseOrderId equals d.Id
                                //join c in context.ItemType on a.ItemTypeId equals c.Id
                                where a.PurchaseOrderId == id && !(a.IsDeleted ?? false)
                                select new PurchaseOrderDetail
                                {
                                    Id = a.Id,
                                    PurchaseOrderId = a.PurchaseOrderId,
                                    TaskId = a.TaskId,
                                    Task = a.Task,
                                    ItemTypeId = a.ItemTypeId,
                                    ItemId = a.ItemId,
                                    ItemCode = a.ItemCode,
                                    ItemName = a.ItemName,
                                    UomId = a.UomId,
                                    Qty = a.Qty,
                                    UnitPrice = a.UnitPrice,
                                    Amount = a.Amount,
                                    ItemTypeName = context.ItemType.Where(x => x.Id == a.ItemTypeId).Select(x => x.TypeName).FirstOrDefault(),//c.TypeName,
                                    UomName = context.Uom.Where(x => x.Id == a.UomId).Select(x => x.unit_symbol).FirstOrDefault(),//b.unit_symbol,
                                    Description = a.Description,
                                    CurrencyId = a.CurrencyId,
                                    ExchangeRate = a.ExchangeRate,
                                    Operator = a.Operator,
                                    TotalExchangeAmount = a.TotalExchangeAmount,
                                    TotalRequest = a.TotalRequest,
                                    TotalExchangeRequest = a.TotalExchangeRequest,
                                    CurrencySymbol = string.IsNullOrEmpty(d.CashAdvanceSettlementId) ?
                                    context.Currencies.Where(x => context.RequestForQuotation.Any(y => y.CurrencyId == x.Id && y.Id == d.RequestForQuotationId)).Select(x => x.CurrencySymbol).FirstOrDefault() :
                                    (from cas in context.CASettlement
                                     join pca in context.Pcas on cas.PcaId equals pca.Id
                                     join curr in context.Currencies on pca.CurrencyId equals curr.Id
                                     where d.CashAdvanceSettlementId == cas.Id
                                     select curr.CurrencySymbol).FirstOrDefault(),
                                    CurrencySymbolRequest = context.Currencies.Where(x => x.Id == a.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault(),
                                    ParentTaskId = a.ParentTaskId,
                                    CreatedBy = a.CreatedBy,
                                    CreatedOn = a.CreatedOn,
                                    ModifiedBy = a.ModifiedBy,
                                    ModifiedOn = a.ModifiedOn
                                }).ToListAsync();
            return result;
        }

        public async Task<List<PurchaseOrder>> GetByVendor(string vendorId)
        {
            return await context.PurchaseOrders.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.CompanyId == vendorId).ToListAsync();
        }

        public async Task<EmployeeBasicInfo> GetDataEmployee(string Id)
        {
            var data = await (from x in context.EmployeeBasicInfos.Where(c => c.Id == Id)
                              select new EmployeeBasicInfo
                              {
                                  Id = x.Id,
                                  NameEmployee = x.NameEmployee,
                                  Email = x.Email,
                                  Gender = x.Gender,
                                  IsDeleted = x.IsDeleted
                              }).Where(x => x.IsDeleted == false).FirstOrDefaultAsync();
            return data;
        }

        public async Task<bool> UpdateBudget(string purchaseOrderId, bool isAddBudget)
        {
            var poDetail = await context.PurchaseOrderDetails.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.PurchaseOrderId == purchaseOrderId).GroupBy(x => x.ItemId).Select(x => new { x.First().ItemId, total = x.Sum(y => y.TotalExchangeAmount) }).ToListAsync();
            var budget = await context.BudgetMasters.AsNoTracking().Where(x => poDetail.Any(z => z.ItemId == x.Id)).ToListAsync();
            var budgetDetail = await context.BudgetMasterDetails.AsNoTracking().Where(x => budget.Any(z => z.Id == x.BudgetMasterId)).ToListAsync();

            //update budget Capex or Opex
            foreach (var item in budget)
            {
                var get = poDetail.FirstOrDefault(x => x.ItemId == item.Id);
                if (get != null)
                {
                    if (isAddBudget)
                    {
                        item.TotalRealBudgetAcr += (get.total ?? 0);
                    }
                    else
                    {
                        item.TotalRealBudgetAcr -= (get.total ?? 0);
                    }

                    item.TotalBudgetBalanceReal = item.TotalBudget - item.TotalRealBudgetAcr;
                    //if (item.TotalRealBudgetAcr > item.TotalBudget)
                    //{
                    //    throw new Exception("the total budget for this year has exceeded the specified capacity. please check that the input budget cannot exceed the total budget that was set.");
                    //}
                }
                context.PhoenixEdit(item);
            }

            //update budget Capex or Opex detail
            //var message = "the total budget for this month has already exceeded the specified capacity. please double check the input budget must not exceed the total budget that has been set.";
            foreach (var item in budgetDetail)
            {
                var get = poDetail.FirstOrDefault(x => x.ItemId == item.BudgetMasterId);
                if (get != null)
                {
                    var month = DateTime.Now.Month;
                    if (isAddBudget)
                    {
                        if (month == 1) { item.RealBudgetAcr01 += (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.RealBudgetAcr01 > item.Budget01) throw new Exception(message); }
                        if (month == 2) { item.RealBudgetAcr02 += (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.RealBudgetAcr02 > item.Budget02) throw new Exception(message); }
                        if (month == 3) { item.RealBudgetAcr03 += (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.RealBudgetAcr03 > item.Budget03) throw new Exception(message); }
                        if (month == 4) { item.RealBudgetAcr04 += (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.RealBudgetAcr04 > item.Budget04) throw new Exception(message); }
                        if (month == 5) { item.RealBudgetAcr05 += (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.RealBudgetAcr05 > item.Budget05) throw new Exception(message); }
                        if (month == 6) { item.RealBudgetAcr06 += (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.RealBudgetAcr06 > item.Budget06) throw new Exception(message); }
                        if (month == 7) { item.RealBudgetAcr07 += (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.RealBudgetAcr07 > item.Budget07) throw new Exception(message); }
                        if (month == 8) { item.RealBudgetAcr08 += (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.RealBudgetAcr08 > item.Budget08) throw new Exception(message); }
                        if (month == 9) { item.RealBudgetAcr09 += (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.RealBudgetAcr09 > item.Budget09) throw new Exception(message); }
                        if (month == 10) { item.RealBudgetAcr10 += (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.RealBudgetAcr10 > item.Budget10) throw new Exception(message); }
                        if (month == 11) { item.RealBudgetAcr11 += (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.RealBudgetAcr11 > item.Budget11) throw new Exception(message); }
                        if (month == 12) { item.RealBudgetAcr12 += (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.RealBudgetAcr12 > item.Budget12) throw new Exception(message); }
                    }
                    else
                    {
                        if (month == 1) { item.RealBudgetAcr01 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.RealBudgetAcr01 > item.Budget01) throw new Exception(message); }
                        if (month == 2) { item.RealBudgetAcr02 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.RealBudgetAcr02 > item.Budget02) throw new Exception(message); }
                        if (month == 3) { item.RealBudgetAcr03 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.RealBudgetAcr03 > item.Budget03) throw new Exception(message); }
                        if (month == 4) { item.RealBudgetAcr04 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.RealBudgetAcr04 > item.Budget04) throw new Exception(message); }
                        if (month == 5) { item.RealBudgetAcr05 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.RealBudgetAcr05 > item.Budget05) throw new Exception(message); }
                        if (month == 6) { item.RealBudgetAcr06 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.RealBudgetAcr06 > item.Budget06) throw new Exception(message); }
                        if (month == 7) { item.RealBudgetAcr07 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.RealBudgetAcr07 > item.Budget07) throw new Exception(message); }
                        if (month == 8) { item.RealBudgetAcr08 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.RealBudgetAcr08 > item.Budget08) throw new Exception(message); }
                        if (month == 9) { item.RealBudgetAcr09 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.RealBudgetAcr09 > item.Budget09) throw new Exception(message); }
                        if (month == 10) { item.RealBudgetAcr10 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.RealBudgetAcr10 > item.Budget10) throw new Exception(message); }
                        if (month == 11) { item.RealBudgetAcr11 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.RealBudgetAcr11 > item.Budget11) throw new Exception(message); }
                        if (month == 12) { item.RealBudgetAcr12 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.RealBudgetAcr12 > item.Budget12) throw new Exception(message); }
                    }
                    context.PhoenixEdit(item);
                }
            }
            return true;
        }

        public async Task<bool> CancelBudget(string purchaseOrderId, string purchaseRequestId)
        {
            //Update budget purchase request - mengembalikan nilai master budget - PR di karenakan PO nya di cancel
            //proses update budget master - mengembalikan nilai master budget - PO di karenakan PO nya di cancel

            var poDetail = await context.PurchaseOrderDetails.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.PurchaseOrderId == purchaseOrderId).GroupBy(x => x.ItemId).Select(x => new { x.First().ItemId, total = x.Sum(y => y.TotalExchangeAmount) }).ToListAsync();
            var prDetail = await context.PurchaseRequestDetail.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.PurchaseRequestId == purchaseRequestId).GroupBy(x => x.ItemId).Select(x => new { x.First().ItemId, total = x.Sum(y => y.AmountExchange) }).ToListAsync();

            //var budget = await context.BudgetMasters.AsNoTracking().Where(x => prDetail.Any(z => z.ItemId == x.Id)).ToListAsync();
            //var budgetDetail = await context.BudgetMasterDetails.AsNoTracking().Where(x => budget.Any(z => z.Id == x.BudgetMasterId)).ToListAsync();

            var budget = await context.BudgetMasters.AsNoTracking().Where(x => poDetail.Any(z => z.ItemId == x.Id)).ToListAsync();
            var budgetDetail = await context.BudgetMasterDetails.AsNoTracking().Where(x => budget.Any(z => z.Id == x.BudgetMasterId)).ToListAsync();

            //update budget Capex or Opex
            foreach (var item in budget)
            {
                var get = poDetail.FirstOrDefault(x => x.ItemId == item.Id);
                if (get == null)
                {
                    throw new Exception("budget cannot be found");
                }

                if (get != null)
                {
                    item.TotalRealBudgetAcr -= (get.total ?? 0);
                    item.TotalBudgetBalanceReal = item.TotalBudget - item.TotalRealBudgetAcr;
                    //if (item.TotalRealBudgetAcr > item.TotalBudget)
                    //{
                    //    throw new Exception("the total budget for this year has exceeded the specified capacity. please check that the input budget cannot exceed the total budget that was set.");
                    //}
                }

                var getPR = prDetail.FirstOrDefault(x => x.ItemId == item.Id);
                if (getPR == null)
                {
                    throw new Exception("budget cannot be found");
                }

                if (getPR != null)
                {
                    item.TotalAcrBudget -= (get.total ?? 0);
                    item.TotalBudgetBalanceAcr = item.TotalBudget - item.TotalAcrBudget;
                    //if (item.TotalAcrBudget > item.TotalBudget)
                    //{
                    //    throw new Exception("the total budget for this year has exceeded the specified capacity. please check that the input budget cannot exceed the total budget that was set.");
                    //}
                }
                context.PhoenixEdit(item);
            }

            //update budget Capex or Opex detail
            //var message = "the total budget for this month has already exceeded the specified capacity. please double check the input budget must not exceed the total budget that has been set.";
            foreach (var item in budgetDetail)
            {
                var get = poDetail.FirstOrDefault(x => x.ItemId == item.BudgetMasterId);
                if (get == null)
                {
                    throw new Exception("budget cannot be found");
                }

                if (get != null)
                {
                    var month = DateTime.Now.Month;
                    if (month == 1) { item.RealBudgetAcr01 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.RealBudgetAcr01 > item.Budget01) throw new Exception(message); }
                    if (month == 2) { item.RealBudgetAcr02 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.RealBudgetAcr02 > item.Budget02) throw new Exception(message); }
                    if (month == 3) { item.RealBudgetAcr03 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.RealBudgetAcr03 > item.Budget03) throw new Exception(message); }
                    if (month == 4) { item.RealBudgetAcr04 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.RealBudgetAcr04 > item.Budget04) throw new Exception(message); }
                    if (month == 5) { item.RealBudgetAcr05 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.RealBudgetAcr05 > item.Budget05) throw new Exception(message); }
                    if (month == 6) { item.RealBudgetAcr06 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.RealBudgetAcr06 > item.Budget06) throw new Exception(message); }
                    if (month == 7) { item.RealBudgetAcr07 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.RealBudgetAcr07 > item.Budget07) throw new Exception(message); }
                    if (month == 8) { item.RealBudgetAcr08 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.RealBudgetAcr08 > item.Budget08) throw new Exception(message); }
                    if (month == 9) { item.RealBudgetAcr09 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.RealBudgetAcr09 > item.Budget09) throw new Exception(message); }
                    if (month == 10) { item.RealBudgetAcr10 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.RealBudgetAcr10 > item.Budget10) throw new Exception(message); }
                    if (month == 11) { item.RealBudgetAcr11 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.RealBudgetAcr11 > item.Budget11) throw new Exception(message); }
                    if (month == 12) { item.RealBudgetAcr12 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.RealBudgetAcr12 > item.Budget12) throw new Exception(message); } 
                    context.PhoenixEdit(item);
                }

                var getPR = prDetail.FirstOrDefault(x => x.ItemId == item.BudgetMasterId);
                if (getPR == null)
                {
                    throw new Exception("budget cannot be found");
                }

                if (getPR != null)
                {
                    var month = DateTime.Now.Month;
                    if (month == 1) { item.AcrBudget01 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.AcrBudget01 > item.Budget01) throw new Exception(message); }
                    if (month == 2) { item.AcrBudget02 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.AcrBudget02 > item.Budget02) throw new Exception(message); }
                    if (month == 3) { item.AcrBudget03 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.AcrBudget03 > item.Budget03) throw new Exception(message); }
                    if (month == 4) { item.AcrBudget04 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.AcrBudget04 > item.Budget04) throw new Exception(message); }
                    if (month == 5) { item.AcrBudget05 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.AcrBudget05 > item.Budget05) throw new Exception(message); }
                    if (month == 6) { item.AcrBudget06 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.AcrBudget06 > item.Budget06) throw new Exception(message); }
                    if (month == 7) { item.AcrBudget07 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.AcrBudget07 > item.Budget07) throw new Exception(message); }
                    if (month == 8) { item.AcrBudget08 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.AcrBudget08 > item.Budget08) throw new Exception(message); }
                    if (month == 9) { item.AcrBudget09 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.AcrBudget09 > item.Budget09) throw new Exception(message); }
                    if (month == 10) { item.AcrBudget10 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.AcrBudget10 > item.Budget10) throw new Exception(message); }
                    if (month == 11) { item.AcrBudget11 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.AcrBudget11 > item.Budget11) throw new Exception(message); }
                    if (month == 12) { item.AcrBudget12 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.AcrBudget12 > item.Budget12) throw new Exception(message); }

                    context.PhoenixEdit(item);
                }
            }

            return true;
        }

        public async Task<dynamic> Print(string Id)
        {
            var result = await (from a in context.PurchaseOrders
                                join b in context.JobDetail on a.JobId equals b.Id
                                join d in context.CompanyBrands on b.BrandId equals d.Id
                                join e in context.Companys on b.CompanyId equals e.Id
                                //join f in context.Pcas on a.PcaId equals f.Id
                                join g in context.LegalEntity on b.LegalEntityId equals g.Id
                                join h in context.Currencies on a.CurrencyId equals h.Id
                                where !(a.IsDeleted ?? false) && a.Id == Id
                                select new
                                {
                                    Client = a.CompanyName.ToUpper(),
                                    ClientAddress = a.CompanyReference,//(e.BillingAddress ?? "") != "" ? e.BillingAddress : e.OfficeAddress,
                                    ClientTelp = e.Phone,
                                    ClientFax = e.Fax,
                                    ClientAttn = "",
                                    ClientEmail = e.Email,
                                    LegalEntity = g.LegalEntityName.ToUpper(),
                                    LegalEntityAddress = (g.BillingAddress ?? "") != "" ? g.BillingAddress : g.OfficeAddress,
                                    LegalEntityTelp = g.Phone,
                                    LegalEntityFax = g.Fax,
                                    LegalEntityEmail = g.Email,
                                    NPWP = a.NumberNpwp,
                                    PoNo = a.PurchaseOrderNumber,
                                    Date = a.PurchaseOrderDate,
                                    JobNo = b.JobNumber,
                                    RefNo = "",
                                    Brand = d.BrandName,
                                    //BudgetNo = f.Code,
                                    //JobTitle = b.JobName,
                                    //b.JobDescription,
                                    h.CurrencyName,
                                    h.CurrencySymbol,
                                    Total = a.SubTotal,
                                    a.Vat,
                                    a.VatPercent,
                                    TotalAmount = a.GrandTotal,
                                    Memo = "",
                                    PrintPODetail = context.PurchaseOrderDetails.Where(x => x.PurchaseOrderId == a.Id).Select(x => new
                                    {
                                        x.Id,
                                        Desription = x.Task,
                                        Qty = x.Qty,
                                        UnitPrice = x.UnitPrice ?? 0,
                                        Cost = x.Amount,
                                        ParentTaskId = x.ParentTaskId,
                                        TaskId = x.TaskId,
                                        ItemTypeId = x.ItemTypeId
                                    })
                                }).FirstOrDefaultAsync();
            return result;
        }
    }
}
