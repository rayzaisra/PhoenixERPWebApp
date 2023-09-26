using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Phoenix.Data.Models;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models.Enum;

namespace Phoenix.Data.Services.Finance.Transaction
{
    public interface IRequestForQuotationService
    {
        Task<RequestForQuotation> Get(string id);
        Task<RequestForQuotationVendor> GetVendorById(string id);
        Task<List<RequestForQuotation>> GetRFQList(string MainserviceCategoryId, string status);
        Task<List<RequestForQuotationTask>> GetTaskList(string RfqId);
        Task<List<RequestForQuotationVendor>> GetVendorList(string RfqId);
        Task<List<RequestForQuotationDetail>> GetDetailRFQList(string RfqId, string VendorId);
        Task<int> AddVendorAsync(RequestForQuotation entity);
        Task<int> EditAsync(RequestForQuotation entity);
        Task<int> TaskEditAsync(RequestForQuotationTask model);
        Task<int> ApproveAsync(RequestForQuotation entity);
        Task<int> DeleteVendorSoftAsync(RequestForQuotationVendor entity);
        Task<dynamic> GetItem(string itemTypeId, string ItemId);
    }
    public class RequestForQuotationService : IRequestForQuotationService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly Services.Um.IManageMenuService menuService;
        readonly FileService uploadFile;
        readonly IPurchaseRequestService prService;
        readonly StatusLogService log;

        /// <summary>
        /// And endpoint to manage RequestForQuotation
        /// </summary>
        /// <param name="context">Database context</param>
        public RequestForQuotationService(DataContext context, GlobalFunctionApproval globalFcApproval, Services.Um.IManageMenuService menuService, FileService uploadFile, IPurchaseRequestService prService, StatusLogService log)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.menuService = menuService;
            this.uploadFile = uploadFile;
            this.prService = prService;
            this.log = log;
        }

        private async Task<RequestForQuotation> ProcessUpload(RequestForQuotation entity)
        {
            var file1 = await uploadFile.Upload(entity.file, null);
            if (!string.IsNullOrEmpty(file1))
            {
                if (entity.RequestForQuotationVendor != null)
                {
                    entity.RequestForQuotationVendor.FilemasterId = file1;
                }
            }

            return entity;
        }

        public async Task<int> AddVendorAsync(RequestForQuotation model)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var entity = await ProcessUpload(model);
                    entity.Status = StatusTransaction.StatusName(entity.StatusApproval);
                    context.PhoenixEdit(entity);

                    if (entity.RequestForQuotationVendor != null)
                    {
                        var item = entity.RequestForQuotationVendor;
                        item.RequestForQuotationId = entity.Id;

                        if (context.RequestForQuotationVendor.Where(x => x.Id != item.Id && x.RequestForQuotationId == entity.Id && x.VendorId == item.VendorId && !(x.IsDeleted ?? false)).Count() > 0)
                        {
                            throw new Exception($"Vendor {item.VendorName} already exists.");
                        }

                        var detail = context.RequestForQuotationDetail.AsNoTracking().Where(x => x.RequestForQuotationId == entity.Id && x.VendorId == item.VendorId).ToList();
                        if (detail.Count > 0)
                        {
                            context.RequestForQuotationDetail.RemoveRange(detail);
                        }

                        if (context.RequestForQuotationVendor.AsNoTracking().Where(x => x.Id == item.Id).Count() == 0)
                        {
                            var vendor = context.RequestForQuotationVendor.AsNoTracking().Where(x => x.RequestForQuotationId == entity.Id && x.VendorId == item.VendorId && (x.IsDeleted ?? false)).ToList();
                            if (vendor.Count > 0)
                            {
                                context.RequestForQuotationVendor.RemoveRange(vendor);
                            }

                            item.Id = Guid.NewGuid().ToString();
                            await context.PhoenixAddAsync(item);
                        }
                        else
                        {
                            context.PhoenixEdit(item);
                        }
                    }

                    if (entity.RequestForQuotationDetails != null)
                    {
                        var task = entity.RequestForQuotationDetails.Where(x => x.ParentTaskId == null).Select(x => new { x.TaskId, x.TotalExchangeAmount }).ToList();
                        var parent = entity.RequestForQuotationDetails.Where(x => x.ParentTaskId != null).GroupBy(x => x.ParentTaskId).Select(x => new { TaskId = x.Key, TotalExchangeAmount = x.Sum(y => y.TotalExchangeAmount) }).ToList();

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
                        var currency = entity.RequestForQuotationDetails.Where(x => x.CurrencyId != null && x.ParentTaskId == null).FirstOrDefault();
                        foreach (var item in entity.RequestForQuotationDetails)
                        {
                            //var message = string.Empty;
                            //var remaining = context.PcaTasks.Where(x => x.PcaId == model.JobPaId && x.Id == item.TaskId).Select(x => x.TotalPriceBalance).FirstOrDefault();
                            //if (item.Amount > remaining)
                            //{
                            //    if (!string.IsNullOrEmpty(message)) message += "<br/>";
                            //    message += $"for task name {item.Task} with a total price of {(item.Amount ?? 0).ToString("#,#.##")} must not exceed the remaining budget value of {(remaining ?? 0).ToString("#,#.##")}";
                            //}

                            //if (!string.IsNullOrEmpty(message))
                            //{
                            //    throw new Exception(message);
                            //}

                            if (item.TotalExchangeAmount > item.TotalExchangeRequest)
                            {
                                throw new Exception("The total price exchange value should not exceed the value of the total request exchange");
                            }

                            item.Id = Guid.NewGuid().ToString();
                            item.RequestForQuotationId = entity.Id;
                            item.VendorId = entity.RequestForQuotationVendor?.VendorId;

                            if (item.ParentTaskId != null)
                            {
                                item.CurrencyId = currency.CurrencyId;
                                item.ExchangeRate = currency.ExchangeRate;
                                item.Operator = currency.Operator;
                            }

                            await context.PhoenixAddAsync(item);
                        };
                    }

                    if (entity.RequestForQuotationTasks != null)
                    {
                        context.PhoenixEditRange(entity.RequestForQuotationTasks.ToArray());
                    }

                    entity.Status = StatusTransactionName.OnProcess;
                    context.RequestForQuotation.Update(entity);
                    var save = await context.SaveChangesAsync();
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

        public async Task<int> EditAsync(RequestForQuotation entity)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var file = await uploadFile.Upload(entity.file, entity.DetailSpecFile);
                    if (!string.IsNullOrEmpty(file))
                    {
                        entity.DetailSpecFile = file;
                    }

                    entity.Status = StatusTransaction.StatusName(entity.StatusApproval);
                    if (entity.Status == StatusTransactionName.Draft ||
                        entity.Status == StatusTransactionName.OnProcess ||
                        entity.Status == StatusTransactionName.WaitingApproval)
                    {
                        entity.CreatedBy = context.GetEmployeeId();
                        entity.CreatedOn = DateTime.Now;
                    }

                    context.PhoenixEdit(entity);

                    var vendor = entity.RequestForQuotationVendors;
                    if (vendor != null)
                    {
                        foreach (var item in entity.RequestForQuotationVendors)
                        {
                            item.IsRequestPurchaseOrder = true;
                            context.PhoenixEdit(item);
                        }
                    }

                    var task = entity.RequestForQuotationTasks;
                    if (task != null)
                    {
                        context.PhoenixEditRange(task.ToArray());
                    }

                    var isApprove = true;
                    if (!(entity.Status == StatusTransactionName.Draft || entity.Status == StatusTransactionName.OnProcess))
                    {
                        var dataMenu = await menuService.GetByUniqueName(MenuUnique.RFQ).ConfigureAwait(false);
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            MenuId = dataMenu.Id,
                            RefId = entity.Id,
                            DetailLink = $"{ApprovalLink.RFQ}?Id={entity.Id}",
                            NotificationType = NotificationTypeCode.RFQNotification,
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
                        entity.Status = StatusTransactionName.OnProcess;
                        context.RequestForQuotation.Update(entity);
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

        public async Task<int> TaskEditAsync(RequestForQuotationTask model)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var item = await context.RequestForQuotationDetail.AsNoTracking().FirstOrDefaultAsync(x => x.RequestForQuotationId == model.RequestForQuotationId && x.TaskId == model.TaskId);
                    if (item != null)
                    {
                        item.ItemTypeId = model.ItemTypeId;
                        //item.ItemTypeName = model.ItemTypeName;
                        item.ItemId = model.ItemId;
                        item.ItemCode = model.ItemCode;
                        item.ItemName = model.ItemName;
                        context.RequestForQuotationDetail.Update(item);
                    }

                    context.RequestForQuotationTask.Update(model);
                    var save = await context.SaveChangesAsync();
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

        public async Task<int> ApproveAsync(RequestForQuotation entity)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var save = await globalFcApproval.UnsafeProcessApproval(entity.Id, entity.StatusApproval, entity.RemarksProcess, entity);
                    var approval = context.TrTemplateApprovals.AsNoTracking().Where(x => x.RefId == entity.Id && !(x.IsDeleted ?? false)).FirstOrDefault();
                    if (approval != null)
                    {
                        entity.Status = StatusTransaction.StatusName(approval.StatusApproved.GetHashCode());
                        if (approval.StatusApproved == Models.Um.StatusApproved.Approved)
                        {
                            if (context.PurchaseOrders.AsNoTracking().Where(x => x.RequestForQuotationId == entity.Id).Count() == 0)
                            {
                                var jobNumber = await context.JobDetail.AsNoTracking().Where(x => x.Id == entity.JobId).Select(x => x.JobNumber).FirstOrDefaultAsync();
                                var purchaseNumber = await context.PurchaseRequest.AsNoTracking().Where(x => x.Id == entity.PurchaseRequestId).Select(x => x.RequestNumber).FirstOrDefaultAsync();

                                var vendor = await context.RequestForQuotationVendor.AsNoTracking().Where(x => x.RequestForQuotationId == entity.Id && (x.IsRequestPurchaseOrder ?? false)).ToListAsync();
                                foreach (RequestForQuotationVendor data in vendor)
                                {
                                    var poId = await TransID.GetTransId(context, Code.PO, DateTime.Today); //PO NUmber ke generate ketika approve terakhir
                                    var headerPo = new PurchaseOrder();
                                    headerPo.Id = poId;
                                    headerPo.PurchaseOrderNumber = poId;
                                    headerPo.PurchaseOrderDate = DateTime.Now;
                                    headerPo.DeliveryDate = entity.DeliveryDate;
                                    headerPo.BusinessUnitId = entity.BusinessUnitId;
                                    headerPo.LegalEntityId = entity.LegalEntityId;
                                    headerPo.AffiliationId = entity.AffiliationId;
                                    headerPo.PurchaseRequestId = entity.PurchaseRequestId;
                                    headerPo.PurchaseRequestNumber = purchaseNumber;
                                    headerPo.JobId = entity.JobId;
                                    headerPo.JobNumber = jobNumber;
                                    headerPo.CompanyId = data.VendorId;
                                    headerPo.CompanyName = data.VendorName;
                                    headerPo.Description = data.Description;
                                    //headerPo.CurrencyId = entity.CurrencyId;
                                    //headerPo.ExchangeRate = 1;
                                    headerPo.DiscountPercent = 0;
                                    headerPo.DiscountAmount = 0;
                                    //headerPo.Amount = data.TotalAmountVendor;
                                    headerPo.PercentPaymentTerm1 = 100;
                                    headerPo.PercentPaymentTerm2 = 0;
                                    headerPo.PercentPaymentTerm3 = 0;
                                    headerPo.PaymentTerm1 = data.TotalAmountVendor;
                                    headerPo.PaymentTerm2 = 0;
                                    headerPo.PaymentTerm3 = 0;
                                    //headerPo.InvoicingTypeId = null;
                                    //headerPo.PaymentMethodId = null;
                                    headerPo.SubTotal = data.SubTotalVendor;
                                    headerPo.VatPercent = data.VatPercent;
                                    headerPo.Vat = data.Vat;
                                    headerPo.GrandTotal = data.TotalAmountVendor;
                                    headerPo.InvoicingType = "SINGLE";
                                    //headerPo.SpecialRequestId = null;
                                    //headerPo.CompanyBrandId = null;
                                    headerPo.MainserviceCategoryId = entity.MainserviceCategoryId;
                                    headerPo.RequestForQuotationId = entity.Id;
                                    headerPo.RequestForQuotationNumber = entity.RequestForQuotationNumber;
                                    headerPo.TypeOfExpenseId = entity.TypeOfExpenseId;
                                    headerPo.Status = StatusTransactionName.Draft;

                                    var Companys = await context.Companys.AsNoTracking().Where(x => x.Id == headerPo.CompanyId && x.IsClient.Equals(false) && x.IsVendor.Equals(true)).FirstOrDefaultAsync();
                                    if (Companys != null)
                                    {
                                        if (string.IsNullOrEmpty(headerPo.CompanyName))
                                        {
                                            headerPo.CompanyName = Companys.CompanyName;
                                        }
                                        headerPo.NumberNpwp = Companys.TaxRegistrationNumber;
                                        headerPo.CompanyReference = Companys.OfficeAddress;
                                        headerPo.VendorPicName = Companys.ContactName;
                                    }

                                    var detailVendor = await context.RequestForQuotationDetail.AsNoTracking().Where(x => x.RequestForQuotationId == entity.Id && x.VendorId == data.VendorId).ToListAsync();

                                    var firstDetail = detailVendor.FirstOrDefault();
                                    if (firstDetail != null)
                                    {
                                        headerPo.CurrencyId = firstDetail.CurrencyId;
                                        headerPo.ExchangeRate = firstDetail.ExchangeRate;
                                    }
                                    await context.PhoenixAddAsync(headerPo);

                                    foreach (RequestForQuotationDetail detail in detailVendor)
                                    {
                                        var modelDetailPo = new PurchaseOrderDetail();
                                        modelDetailPo.Id = Guid.NewGuid().ToString();
                                        modelDetailPo.PurchaseOrderId = headerPo.Id;
                                        modelDetailPo.TaskId = detail.TaskId;
                                        modelDetailPo.Task = detail.Task;
                                        modelDetailPo.ItemId = detail.ItemId;
                                        modelDetailPo.ItemCode = detail.ItemCode;
                                        modelDetailPo.ItemName = detail.ItemName;
                                        //modelDetailPo.ItemDescription = null;
                                        modelDetailPo.Qty = detail.Qty;
                                        modelDetailPo.UnitPrice = detail.UnitPrice;
                                        modelDetailPo.Amount = detail.Amount;
                                        modelDetailPo.UomId = detail.UomId;
                                        //modelDetailPo.DiscountPercent = 0;
                                        //modelDetailPo.DiscountAmount = 0;
                                        modelDetailPo.ItemTypeId = detail.ItemTypeId;
                                        //modelDetailPo.ItemBrand = null;
                                        //modelDetailPo.ItemSubBrand = null;
                                        //modelDetailPo.AmountNet = detail.Amount;
                                        //modelDetailPo.IsDelivered = null;
                                        //modelDetailPo.BudgetId = null;

                                        //new
                                        modelDetailPo.CurrencyId = detail.CurrencyId;
                                        modelDetailPo.ExchangeRate = detail.ExchangeRate;
                                        modelDetailPo.Operator = detail.Operator;
                                        modelDetailPo.TotalRequest = detail.Amount;
                                        modelDetailPo.TotalExchangeRequest = detail.TotalExchangeAmount;
                                        modelDetailPo.TotalExchangeAmount = detail.TotalExchangeAmount;
                                        modelDetailPo.ParentTaskId = detail.ParentTaskId;

                                        await context.PhoenixAddRangeAsync(modelDetailPo);

                                        //update Status PCA Task
                                        if (detail.ParentTaskId == null)
                                        {
                                            if (!string.IsNullOrEmpty(entity.JobPaId))
                                            {
                                                var pcaTask = await context.PcaTasks.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == detail.TaskId).FirstOrDefaultAsync();
                                                pcaTask.Status = PCAStatus.PO;
                                                pcaTask.TotalPriceUsed = (pcaTask.TotalPriceUsed - detail.TotalExchangeRequest) + detail.TotalExchangeAmount;
                                                pcaTask.TotalPriceBalance = pcaTask.TotalPrice - pcaTask.TotalPriceUsed;

                                                if (pcaTask.TotalPriceBalance == null)
                                                {
                                                    throw new Exception("Remaining Budget Cannot be Empty.");
                                                }

                                                if (pcaTask.TotalPriceBalance < 0)
                                                {
                                                    throw new Exception("the total exchange requests must be less than or equal to the remaining budget");
                                                }

                                                context.PhoenixEdit(pcaTask);
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(entity.PurchaseRequestId))
                                        {
                                            await prService.UpdateAllBudget(detail.ItemId, true, (detail.TotalExchangeRequest ?? 0), (detail.TotalExchangeAmount ?? 0));
                                        }
                                    }
                                }

                                if (!string.IsNullOrEmpty(entity.JobPaId))
                                {
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
                        else if (approval.StatusApproved == Models.Um.StatusApproved.Rejected)
                        {
                            //Update budget purchase request
                            if (!string.IsNullOrEmpty(entity.PurchaseRequestId))
                            {
                                await prService.UpdateBudget(entity.PurchaseRequestId, false);
                            }

                            //update Status PCA Task
                            if (!string.IsNullOrEmpty(entity.JobPaId))
                            {
                                var pcaTask = await context.PcaTasks.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && context.RequestForQuotationTask.Any(z => !(z.IsDeleted ?? false) && z.RequestForQuotationId == entity.Id && z.TaskId == x.Id)).ToListAsync();
                                foreach (var item in pcaTask)
                                {
                                    if (approval.StatusApproved == Models.Um.StatusApproved.Rejected)
                                    {
                                        var rfqTask = await context.RequestForQuotationTask.AsNoTracking().Where(x => x.RequestForQuotationId == entity.Id && x.TaskId == item.Id).FirstOrDefaultAsync();
                                        item.Status = PCAStatus.Open;
                                        item.TotalPriceUsed = (item.TotalPriceUsed - rfqTask.TotalExchange);
                                        item.TotalPriceBalance = item.TotalPrice - item.TotalPriceUsed;
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

                            if (!string.IsNullOrEmpty(entity.PurchaseRequestId))
                            {
                                var pr = await context.PurchaseRequestDetail.Where(x => !(x.IsDeleted ?? false) && x.PurchaseRequestId == entity.PurchaseRequestId).ToListAsync();
                                foreach (PurchaseRequestDetail detail in pr)
                                {
                                    await prService.UpdateAllBudget(detail.ItemId, false, (detail.AmountExchange ?? 0), 0);
                                }
                            }
                        }
                    }
                    else
                    {
                        entity.Status = StatusTransactionName.OnProcess;
                    }
                    context.RequestForQuotation.Update(entity);

                    await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = entity.Status, Description = entity.RemarksProcess });
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

        public async Task<int> DeleteVendorSoftAsync(RequestForQuotationVendor entity)
        {
            context.PhoenixDelete(entity);
            var detail = await context.RequestForQuotationDetail.AsNoTracking().Where(x => x.VendorId == entity.VendorId && x.RequestForQuotationId == entity.RequestForQuotationId).ToListAsync();
            context.PhoenixDeleteRange(detail.ToArray());
            return await context.SaveChangesAsync();
        }

        public async Task<RequestForQuotation> Get(string id)
        {
            var result = await context.RequestForQuotation.Where(x => !(x.IsDeleted ?? false) && x.Id == id).FirstOrDefaultAsync();
            if (!string.IsNullOrEmpty(result.DetailSpecFile))
            {
                result.DetailSpecFileName = await context.Filemasters.Where(x => x.Id == result.DetailSpecFile).Select(x => x.Name).FirstOrDefaultAsync();
            }
            else
            {
                result.DetailSpecFileName = "";
            }
            if (result != null)
            {
                var job = await context.JobDetail.Where(x => x.Id == result.JobId).FirstOrDefaultAsync();
                if (job != null)
                {
                    result.JobName = job.JobName;
                    if (result.BusinessUnitId == null) result.BusinessUnitId = job.BusinessUnitDivisionId;
                }

                var category = await context.MainServiceCategorys.Where(x => x.Id == result.MainserviceCategoryId).FirstOrDefaultAsync();
                if (category != null) result.MainserviceCategoryName = category.Name;

                var employee = await context.EmployeeBasicInfos.Where(x => x.Id == result.CreatedBy).FirstOrDefaultAsync();
                if (employee != null) result.RequestorName = employee.NameEmployee;

                if (!string.IsNullOrEmpty(result.PurchaseRequestId))
                {
                    var pr = await context.PurchaseRequest.Where(x => x.Id == result.PurchaseRequestId).FirstOrDefaultAsync();
                    result.BudgetYear = pr.BudgetYear;
                    result.BudgetOwnerId = pr.BudgetOwnerId;
                    result.BudgetDivisionId = pr.RequestorDivisionId;
                    result.GetVatPercent = pr.VatPercent;
                }
            }
            return result;
        }

        public async Task<RequestForQuotationVendor> GetVendorById(string id)
        {
            var result = await context.RequestForQuotationVendor.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (result != null)
            {
                if (result.FilemasterId != null)
                {
                    result.FilemasterName = context.Filemasters.Where(x => x.Id == result.FilemasterId).Select(x => x.Name).FirstOrDefault();
                }
            }
            return result;
        }
        public async Task<List<RequestForQuotation>> GetRFQList(string MainserviceCategoryId, string status)
        {
            //var purchaseOrders = await context.PurchaseOrders.ToListAsync();
            if (MainserviceCategoryId == MainServiceCategoryOther.MainServiceCategoryId)
            {
                //other (internal)
                return await (from tb in context.RequestForQuotation
                              join sc in context.MainServiceCategorys on tb.MainserviceCategoryId equals sc.Id
                              join te in context.EmployeeBasicInfos on tb.CreatedBy equals te.Id
                              //join bu in context.BusinessUnits on tb.BusinessUnitId equals bu.Id
                              //join bt in context.BusinessUnitTypes on bu.BusinessUnitTypeId equals bt.Id
                              join pr in context.PurchaseRequest on tb.PurchaseRequestId equals pr.Id
                              join le in context.LegalEntity on tb.LegalEntityId equals le.Id
                              where !(tb.IsDeleted ?? false) &&
                              (string.IsNullOrEmpty(MainserviceCategoryId) ? true : tb.MainserviceCategoryId == MainserviceCategoryId) &&
                              (string.IsNullOrEmpty(status) ? true : (tb.Status ?? StatusTransactionName.Draft) == status) //&&
                                                                                                                           //bt.BusinessUnitLevel == BusinessUnitLevelCode.Division
                              select new RequestForQuotation
                              {
                                  Id = tb.Id,
                                  RequestForQuotationNumber = tb.RequestForQuotationNumber,
                                  RequestForQuotationDate = tb.RequestForQuotationDate,
                                  Status = tb.Status ?? StatusTransactionName.Draft,
                                  MainserviceCategoryName = sc.Name,
                                  RequestorName = te.NameEmployee,
                                  //BusinessUnitName = context.BusinessUnits.Where(x => x.Id == tb.BusinessUnitId).Select(x => x.UnitName).FirstOrDefault(),//bu.UnitName,
                                  LegalEntityName = le.LegalEntityName,
                                  DueDate = tb.DueDate,
                                  PurchaseRequestId = pr.RequestNumber,
                                  IsPreferedVendor = tb.IsPreferedVendor,
                                  ModifiedOn = tb.ModifiedOn,
                                  CurrencySymbol = context.Currencies.Where(x => x.Id == tb.RequestCurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault(),
                                  GrandTotal = context.RequestForQuotationVendor.Where(x => x.RequestForQuotationId == tb.Id && x.IsRequestPurchaseOrder == true).Select(x => x.TotalAmountVendor).FirstOrDefault()
                              }).OrderByDescending(x => x.ModifiedOn).ToListAsync();
            }
            else
            {
                //Media or Production
                return await (from tb in context.RequestForQuotation
                              join tj in context.JobDetail on tb.JobId equals tj.Id
                              join sc in context.MainServiceCategorys on tb.MainserviceCategoryId equals sc.Id
                              join te in context.EmployeeBasicInfos on tb.CreatedBy equals te.Id
                              //join bu in context.BusinessUnits on tb.BusinessUnitId equals bu.Id
                              //join bt in context.BusinessUnitTypes on bu.BusinessUnitTypeId equals bt.Id
                              join le in context.LegalEntity on tb.LegalEntityId equals le.Id
                              where !(tb.IsDeleted ?? false) &&
                              (string.IsNullOrEmpty(MainserviceCategoryId) ? true : tb.MainserviceCategoryId == MainserviceCategoryId) &&
                              (string.IsNullOrEmpty(status) ? true : (tb.Status ?? StatusTransactionName.Draft) == status) //&&
                                                                                                                           //bt.BusinessUnitLevel == BusinessUnitLevelCode.Division
                              select new RequestForQuotation
                              {
                                  Id = tb.Id,
                                  RequestForQuotationNumber = tb.RequestForQuotationNumber,
                                  RequestForQuotationDate = tb.RequestForQuotationDate,
                                  Status = tb.Status ?? StatusTransactionName.Draft,
                                  MainserviceCategoryName = sc.Name,
                                  RequestorName = te.NameEmployee,
                                  BusinessUnitName = context.BusinessUnits.Where(x => x.Id == tb.BusinessUnitId).Select(x => x.UnitName).FirstOrDefault(),//bu.UnitName,
                                  LegalEntityName = le.LegalEntityName,
                                  JobName = tj.JobNumber + " - " + tj.JobName,
                                  DueDate = tb.DueDate,
                                  IsPreferedVendor = tb.IsPreferedVendor,
                                  ModifiedOn = tb.ModifiedOn,
                                  CurrencySymbol = context.Currencies.Where(x => x.Id == tb.RequestCurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault(),
                                  GrandTotal = context.RequestForQuotationVendor.Where(x => x.RequestForQuotationId == tb.Id && x.IsRequestPurchaseOrder == true).Select(x => x.TotalAmountVendor).FirstOrDefault()
                              }).OrderByDescending(x => x.ModifiedOn).ToListAsync();
            }
        }

        public async Task<List<RequestForQuotationTask>> GetTaskList(string RfqId)
        {
            return await (from a in context.RequestForQuotationTask
                          join b in context.Uom on a.UomId equals b.Id
                          join c in context.RequestForQuotation on a.RequestForQuotationId equals c.Id
                          where !(a.IsDeleted ?? false) && a.RequestForQuotationId == RfqId
                          select new RequestForQuotationTask
                          {
                              Id = a.Id,
                              RequestForQuotationId = a.RequestForQuotationId,
                              TaskId = a.TaskId,
                              Task = a.Task,
                              Quantity = a.Quantity,
                              UomId = a.UomId,
                              UnitPrice = a.UnitPrice,
                              TotalPrice = a.TotalPrice,
                              JobId = a.JobId,
                              ItemTypeId = a.ItemTypeId,
                              ItemId = a.ItemId,
                              ItemCode = a.ItemCode,
                              ItemName = a.ItemName,
                              UomName = b.unit_name,
                              ItemTypeName = context.ItemType.Where(x => x.Id == a.ItemTypeId).Select(x => x.TypeName).FirstOrDefault(),
                              RemainingBudget = !string.IsNullOrEmpty(c.PurchaseRequestId) ?
                              context.BudgetMasters.Where(x => x.Id == a.ItemId).Select(x => x.TotalBudgetBalanceAllTransaction).FirstOrDefault() :
                              context.PcaTasks.Where(x => x.Id == a.TaskId).Select(x => x.TotalPriceBalance).FirstOrDefault(),
                              CurrencyId = a.CurrencyId,
                              CurrencySymbol = context.Currencies.Where(x => x.Id == c.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault(),
                              CurrencySymbolRequest = context.Currencies.Where(x => x.Id == a.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault(),
                              ExchangeRate = a.ExchangeRate,
                              TotalRequest = a.TotalRequest,
                              Operator = a.Operator,
                              TotalExchange = a.TotalExchange,
                              CreatedBy = a.CreatedBy,
                              CreatedOn = a.CreatedOn,
                              ModifiedBy = a.ModifiedBy,
                              ModifiedOn = a.ModifiedOn,
                              IsDeleted = a.IsDeleted
                          }).ToListAsync();
        }

        public async Task<List<RequestForQuotationVendor>> GetVendorList(string RfqId)
        {
            return await (from tb in context.RequestForQuotationVendor
                          join tv in context.Companys on tb.VendorId equals tv.Id
                          join tb1 in context.RequestForQuotation on tb.RequestForQuotationId equals tb1.Id
                          where tb.RequestForQuotationId == RfqId && !(tb.IsDeleted ?? false)
                          select new RequestForQuotationVendor
                          {
                              Id = tb.Id,
                              RequestForQuotationId = tb.RequestForQuotationId,
                              VendorId = tb.VendorId,
                              SubTotalVendor = tb.SubTotalVendor,
                              VatPercent = tb.VatPercent,
                              Vat = tb.Vat,
                              TotalAmountVendor = tb.TotalAmountVendor,
                              Description = tb.Description,
                              //TermOfPaymentVendor = tb.TermOfPaymentVendor,
                              IsRequestPurchaseOrder = tb.IsRequestPurchaseOrder,
                              //EmailRecipient = tb.EmailRecipient,
                              //EmailBcc = tb.EmailBcc,
                              //EmailCc = tb.EmailCc,
                              //EmailAttachment = tb.EmailAttachment,
                              VendorName = tv.CompanyName,
                              FilemasterId = tb.FilemasterId,
                              FilemasterName = context.Filemasters.Where(x => x.Id == tb.FilemasterId).Select(x => x.Name).FirstOrDefault(),
                              //ReferenceId = tb1.JobId == null ? tb1.PurchaseRequestId : tb1.JobId,
                              CurrencySymbol = context.Currencies.Where(x => x.Id == tb1.RequestCurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault()
                          }).ToListAsync();
        }

        public async Task<List<RequestForQuotationDetail>> GetDetailRFQList(string RfqId, string VendorId)
        {
            var result = await (from a in context.RequestForQuotationDetail
                                    //join b in context.Uom on a.UomId equals b.Id
                                    //join c in context.ItemType on a.ItemTypeId equals c.Id
                                join d in context.RequestForQuotation on a.RequestForQuotationId equals d.Id
                                where a.RequestForQuotationId == RfqId && (string.IsNullOrEmpty(VendorId) ? true : a.VendorId == VendorId) && !(a.IsDeleted ?? false)
                                select new RequestForQuotationDetail
                                {
                                    Id = a.Id,
                                    RequestForQuotationId = a.RequestForQuotationId,
                                    VendorId = a.VendorId,
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
                                    ItemTypeName = context.ItemType.Where(x => x.Id == a.ItemTypeId).Select(x => x.TypeName).FirstOrDefault(),
                                    UomName = context.Uom.Where(x => x.Id == a.UomId).Select(x => x.unit_symbol).FirstOrDefault(),
                                    RemainingBudget = !string.IsNullOrEmpty(d.PurchaseRequestId) ?
                                    context.BudgetMasters.Where(x => x.Id == a.ItemId).Select(x => x.TotalBudgetBalanceAllTransaction).FirstOrDefault() :
                                    context.PcaTasks.Where(x => x.Id == a.TaskId).Select(x => x.TotalPriceBalance).FirstOrDefault(),
                                    CurrencyId = a.CurrencyId,
                                    ExchangeRate = a.ExchangeRate,
                                    Operator = a.Operator,
                                    TotalRequest = a.TotalRequest,
                                    TotalExchangeRequest = a.TotalExchangeRequest,
                                    TotalExchangeAmount = a.TotalExchangeAmount,
                                    CurrencySymbol = context.Currencies.Where(x => x.Id == d.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault(),
                                    CurrencySymbolRequest = context.Currencies.Where(x => x.Id == a.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault(),
                                    ParentTaskId = a.ParentTaskId,
                                    CreatedBy = a.CreatedBy,
                                    CreatedOn = a.CreatedOn,
                                    ModifiedBy = a.ModifiedBy,
                                    ModifiedOn = a.ModifiedOn
                                }).ToListAsync();
            return result;
        }
        public async Task<dynamic> GetItem(string itemTypeId, string ItemId)
        {
            //if (itemTypeId == ItemTypeId.Inventory)
            //{
            //    return await context.Inventory.Where(x => !(x.IsDeleted ?? false) && x.Id == ItemId).Select(x => new { Id = x.Id, ItemNumber = x.SKUCode, ItemName = x.InventoryName }).FirstOrDefaultAsync();
            //}
            //else if (itemTypeId == ItemTypeId.FixedAsset)
            //{
            return await context.MasterBudgetCodes.Where(x => !(x.IsDeleted ?? false) && x.Id == ItemId).Select(x => new { Id = x.Id, ItemNumber = x.BudgetCode, ItemName = x.BudgetName }).FirstOrDefaultAsync();
            //}
            //return null;
        }
    }
}
