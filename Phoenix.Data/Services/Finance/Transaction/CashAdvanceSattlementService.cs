using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Enum;
using Phoenix.Data.Models.Finance;
using Phoenix.Data.Models.Finance.Transaction;
using Phoenix.Data.Services.Um;
using Phoenix.Shared.Core.PrincipalHelpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Finance
{
    public interface ICashAdvanceSattlementService
    {
        Task<List<CashAdvance>> Get(string legalEntityId, string mainServiceCategoryId);
        Task<CASettlement> Get(string caId);
        Task<int> AddEditAsync(CASettlement entity, bool isEdit, bool isJournal);
        Task<List<CASettlementDetail>> GetDetail(string caId);
        Task<List<Journal>> GetJournal(string casId);
        Task<CASettlement> Approve(CASettlement entity);
        Task<CASettlement> Reject(CASettlement entity);
    }
    public class CashAdvanceSattlementService : ICashAdvanceSattlementService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly IManageMenuService menuService;
        readonly FinancePeriodService financePeriod;
        readonly FileService uploadFile;
        readonly StatusLogService log;
        readonly ICashAdvanceService caService;

        public CashAdvanceSattlementService(DataContext context, GlobalFunctionApproval globalFcApproval, IManageMenuService menuService, FinancePeriodService financePeriod, FileService uploadFile, StatusLogService log, ICashAdvanceService caService)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.menuService = menuService;
            this.financePeriod = financePeriod;
            this.uploadFile = uploadFile;
            this.log = log;
            this.caService = caService;
        }

        public async Task<List<CashAdvance>> Get(string legalEntityId, string mainServiceCategoryId)
        {
            var legalEntity = string.IsNullOrEmpty(legalEntityId) ? "0" : legalEntityId;
            var mainService = string.IsNullOrEmpty(mainServiceCategoryId) ? "0" : mainServiceCategoryId;

            var result = await (from ca in context.CashAdvances
                                join cas in context.CASettlement on ca.Id equals cas.CashAdvanceId
                                join rb in context.EmployeeBasicInfos on ca.RequestedBy equals rb.Id
                                join job in context.JobDetail on ca.JobId equals job.Id
                                join lg in context.LegalEntity on ca.LegalEntityId equals lg.Id
                                join bu in context.BusinessUnits on ca.BusinessUnitId equals bu.Id
                                join mc in context.MainServiceCategorys on ca.MainserviceCategoryId equals mc.Id
                                where !(ca.IsDeleted ?? false) && !(cas.IsDeleted ?? false) && (ca.Status == StatusTransactionName.Approved || ca.Status == StatusTransactionName.Completed) &&
                                (legalEntity == "0" ? true : ca.LegalEntityId == legalEntity) &&
                                (mainService == "0" ? true : ca.MainserviceCategoryId == mainService)
                                select new CashAdvance
                                {
                                    Id = ca.Id,
                                    CaNumber = ca.CaNumber,
                                    SettlementNumber = cas.SettlementNumber,
                                    CaDate = cas.CaSettlementDate ?? DateTime.Now,
                                    JobName = job.Id + " - " + job.JobName,
                                    BusinessUnitName = bu.UnitName,
                                    LegalEntityName = lg.LegalEntityName,
                                    MainServiceCategoryName = mc.Name,
                                    Status = ca.Status,
                                    StatusCASettlement = cas.Status,
                                    CaPeriodTo = ca.CaPeriodTo,
                                    RequestedBy = rb.NameEmployee,
                                    CreatedOn = ca.CreatedOn,
                                    ModifiedOn = ca.ModifiedOn,
                                    RequestBusinessUnitName = context.BusinessUnits.Where(x => x.Id == ca.RequestBusinessUnitId).Select(x => x.UnitName).FirstOrDefault(),
                                    Amount = cas.Amount,
                                    CurrencySymbol = context.Currencies.Where(x => x.Id == cas.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault(),
                                    PONumber = context.PurchaseOrders.Where(x => x.CashAdvanceSettlementId == cas.Id).Select(x => x.PurchaseOrderNumber).FirstOrDefault(),
                                    InvoiceReceivableNumber = (from ir in context.InvoiceReceivables
                                                               join po in context.PurchaseOrders on ir.PoId equals po.Id
                                                               where !(ir.IsDeleted ?? false) && po.CashAdvanceSettlementId == cas.Id
                                                               select ir.InvoiceNumber
                                                              ).FirstOrDefault(),
                                    TAPNumber = (from tap in context.TemporaryAccountPayables
                                                 join po in context.PurchaseOrders on tap.PurchaseOrderId equals po.Id
                                                 where !(tap.IsDeleted ?? false) && po.CashAdvanceSettlementId == cas.Id
                                                 select tap.TapNumber
                                                ).FirstOrDefault(),
                                    APNumber = (from ap in context.AccountPayables
                                                join po in context.PurchaseOrders on ap.PurchaseOrderId equals po.Id
                                                where !(ap.IsDeleted ?? false) && po.CashAdvanceSettlementId == cas.Id
                                                select ap.AccountPayableNumber
                                                ).FirstOrDefault(),
                                    //IsReverse = (from a in context.VwStatementOfAp
                                    //             join b in context.PurchaseOrders on a.PONumber equals b.PurchaseOrderNumber
                                    //             where b.CashAdvanceSettlementId == cas.Id && a.Balance > 0
                                    //             select a.AccountPayableNumber
                                    //             ).Count() > 0
                                    IsReverse = context.CASettlementJournalReverse.Where(x => !(x.IsDeleted ?? false) && x.IsDraft == false && x.JvReferenceNo == cas.SettlementNumber).Count() == 0 &&
                                                (from ap in context.AccountPayables
                                                 join po in context.PurchaseOrders on ap.PurchaseOrderId equals po.Id
                                                 where !(ap.IsDeleted ?? false) && po.CashAdvanceSettlementId == cas.Id
                                                 select ap.AccountPayableNumber
                                                ).Count() > 0
                                }).OrderByDescending(x => x.ModifiedOn).ToListAsync();
            return result;
        }

        public async Task<CASettlement> Get(string caId)
        {
            var result = await context.CASettlement.Where(x => x.IsDeleted.Equals(false) && x.CashAdvanceId == caId).FirstOrDefaultAsync();
            if (result == null) result = new CASettlement();
            result.CashAdvance = await caService.Get(caId);
            result.CurrencySymbol = context.Currencies.Where(x => x.Id == result.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault();
            result.FileReceiptName = context.Filemasters.Where(x => x.Id == result.FileReceipt).Select(x => x.Name).FirstOrDefault();
            result.SectionUploadEvidenceName = context.Filemasters.Where(x => x.Id == result.SectionUploadEvidence).Select(x => x.Name).FirstOrDefault();
            return result;
        }

        public async Task<int> AddEditAsync(CASettlement entity, bool isEdit, bool isJournal)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    entity.Status = StatusTransaction.StatusName(entity.StatusApproval);

                    if (entity.receiptFile != null)
                    {
                        var receiptFile = await uploadFile.Upload(entity.receiptFile, null);
                        if (!string.IsNullOrEmpty(receiptFile))
                        {
                            entity.FileReceipt = receiptFile;
                        }
                    }

                    if (entity.SectionUploadEvidenceFile != null)
                    {
                        var evidenceFile = await uploadFile.Upload(entity.SectionUploadEvidenceFile, null);
                        if (!string.IsNullOrEmpty(evidenceFile))
                        {
                            entity.SectionUploadEvidence = evidenceFile;
                        }
                    }

                    if (isEdit)
                    {
                        context.PhoenixEdit(entity);
                    }
                    else
                    {
                        entity.Id = await TransID.GetTransId(context, Code.CashAdvanceSattlement, DateTime.Today);
                        entity.SettlementNumber = entity.Id;
                        if (entity.CaSettlementDate == null) entity.CaSettlementDate = DateTime.Now;
                        await context.PhoenixAddAsync(entity);
                        await context.SaveChangesAsync();
                    }

                    if (!string.IsNullOrEmpty(entity.Id))
                    {
                        var detail = context.CASettlementDetail.AsNoTracking().Where(x => x.CaSettlementId == entity.Id).ToList();
                        if (detail.Count > 0)
                        {
                            context.CASettlementDetail.RemoveRange(detail);
                        }
                    }

                    if (entity.CASettlementDetail != null)
                    {
                        foreach (var item in entity.CASettlementDetail)
                        {
                            item.Id = Guid.NewGuid().ToString();
                            item.CaSettlementId = entity.Id;
                            //item.AccountId = "4415002503333080";
                            //item.AccountCode = "2.1.01.01.01"; 
                            await context.PhoenixAddAsync(item);
                        }
                    }

                    ////Create Journal (journal di cash advance settlement detail sudah tidak ada - request mas daus 24-03-2020)
                    //if (isJournal)
                    //{
                    //    await context.SaveChangesAsync();
                    //    await context.Database.ExecuteSqlCommandAsync("EXECUTE fn.create_jurnal_cas_temp @p_cas_id", parameters: new[] { new SqlParameter("@p_cas_id", entity.Id) });
                    //    var save = await context.SaveChangesAsync();
                    //}

                    var isApprove = true;
                    if (entity.Status != StatusTransactionName.Draft)
                    {
                        var dataMenu = await menuService.GetByUniqueName(MenuUnique.CashAdvanceSettlement).ConfigureAwait(false);
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            MenuId = dataMenu.Id,
                            RefId = entity.Id,
                            DetailLink = $"{ApprovalLink.CashAdvanceSettlement}?Id={entity.Id}",
                            NotificationType = NotificationTypeCode.CashAdvanceSettlementApprovalNotification,
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
                        context.CASettlement.Update(entity);
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

        public async Task<List<CASettlementDetail>> GetDetail(string caId)
        {
            var result = await (from x in context.CashAdvanceDetails
                                join a in context.CASettlementDetail on x.Id equals a.CaDetailId
                                join b in context.Uom on a.UomId equals b.Id
                                join c in context.CASettlement on a.CaSettlementId equals c.Id
                                join ca in context.CashAdvances on x.CashAdvanceId equals ca.Id
                                join pca in context.Pcas on ca.PcaId equals pca.Id
                                where !(x.IsDeleted ?? false) && x.CashAdvanceId == caId
                                select new CASettlementDetail
                                {
                                    Id = a.Id,
                                    CaSettlementId = a.CaSettlementId,
                                    CaDetailId = x.Id,
                                    TaskId = x.TaskId,
                                    TaskName = x.TaskName,
                                    UomId = a.UomId,
                                    UomName = b.unit_symbol,
                                    UnitPrice = a.UnitPrice,
                                    Qty = a.Qty,
                                    Amount = a.Amount,
                                    TotalRequest = a.TotalRequest,
                                    ApprovedBy = a.ApprovedBy,
                                    ApprovedOn = a.ApprovedOn,
                                    CreatedBy = a.CreatedBy,
                                    CreatedOn = a.CreatedOn,
                                    IsDeleted = a.IsDeleted,
                                    ModifiedBy = a.ModifiedBy,
                                    ModifiedOn = a.ModifiedOn,
                                    PurchaseDate = a.PurchaseDate,
                                    CurrencySymbol = context.Currencies.Where(x => x.Id == pca.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault(),
                                    CurrencySymbolRequest = context.Currencies.Where(x => x.Id == c.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault(),
                                    TotalExchangeRequest = a.TotalExchangeRequest,
                                    TotalExchangeAmount = a.TotalExchangeAmount
                                }).ToListAsync();

            //if (result.Count() == 0)
            //{
            //    result = await context.CashAdvanceDetails.Where(x => !(x.IsDeleted ?? false) && x.CashAdvanceId == caId)
            //                    .Select(x => new CASettlementDetail
            //                    {
            //                        Id = x.Id,
            //                        CaDetailId = x.Id,
            //                        TaskId = x.TaskId,
            //                        TaskName = x.TaskName,
            //                        UomId = x.UomId,
            //                        UnitPrice = x.UnitPrice,
            //                        Qty = x.Qty,
            //                        Amount = x.Amount,
            //                        TotalRequest = x.TotalRequest
            //                    }).ToListAsync();
            //}
            return result;
        }

        public async Task<List<Journal>> GetJournal(string casId)
        {
            var data = await (from tmpj in context.JournalTemps
                              join tbu in context.BusinessUnits on tmpj.DivisiId equals tbu.Id
                              join st in context.CASettlement on tmpj.ReferenceId equals st.Id
                              join coa in context.ChartOfAccounts on tmpj.AccountId equals coa.Id
                              where !(tmpj.IsDeleted ?? false) && tmpj.ReferenceId == casId
                              select new Journal
                              {
                                  AccountId = tmpj.AccountId,
                                  CodeRec = coa.CodeRec,
                                  JobId = tmpj.JobId,
                                  AccountName = coa.Name5,
                                  Description = coa.Name5,
                                  JobIdName = st.JobId,
                                  DivisiId = tmpj.DivisiId,
                                  DivisiName = tbu.UnitName,
                                  Credit = tmpj.Credit,
                                  Debit = tmpj.Debit,
                                  LineCode = tmpj.LineCode,
                                  CurrencySymbol = context.Currencies.Where(x => x.Id == tmpj.DefaultCurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault()
                              }).OrderByDescending(x => x.Debit).ThenByDescending(x => x.Credit).ToListAsync();

            return data;
        }

        public async Task<CASettlement> Approve(CASettlement entity)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var save = await globalFcApproval.UnsafeProcessApproval(entity.Id, StatusTransaction.Approved, entity.RemarksProcess, entity);
                    var approval = context.TrTemplateApprovals.Where(x => x.RefId == entity.Id && !(x.IsDeleted ?? false)).FirstOrDefault();
                    if (approval != null)
                    {
                        entity.Status = StatusTransaction.StatusName(approval.StatusApproved.GetHashCode());
                        if (approval.StatusApproved == Models.Um.StatusApproved.Approved)
                        {
                            //Add Purchase Order
                            if (context.PurchaseOrders.AsNoTracking().Where(x => x.CashAdvanceId == entity.Id).Count() == 0)
                            {
                                var casDetail = await context.CASettlementDetail.AsNoTracking().Where(x => x.CaSettlementId == entity.Id).ToListAsync();// && (x.TotalRequest - x.Amount) > 0).ToListAsync();
                                var ca = await context.CashAdvances.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == entity.CashAdvanceId).FirstOrDefaultAsync() ?? new CashAdvance();
                                if (casDetail.Count() > 0)
                                {
                                    var jobNumber = await context.JobDetail.AsNoTracking().Where(x => x.Id == entity.JobId).Select(x => x.JobNumber).FirstOrDefaultAsync();
                                    var poId = await TransID.GetTransId(context, Code.PO, DateTime.Today); //PO NUmber ke generate ketika approve terakhir
                                    var headerPo = new PurchaseOrder();
                                    headerPo.Id = poId;
                                    headerPo.PurchaseOrderNumber = poId;
                                    headerPo.PurchaseOrderDate = DateTime.Now;
                                    //headerPo.DeliveryDate = entity.DeliveryDate;
                                    headerPo.BusinessUnitId = entity.BusinessUnitId;
                                    headerPo.LegalEntityId = entity.LegalEntityId;
                                    headerPo.AffiliationId = entity.AffiliationId;
                                    headerPo.JobId = entity.JobId;
                                    headerPo.JobNumber = jobNumber;
                                    headerPo.CompanyId = entity.VendorId;
                                    headerPo.CurrencyId = entity.CurrencyId;
                                    headerPo.ExchangeRate = entity.ExchangeRate;
                                    headerPo.DiscountPercent = 0;
                                    headerPo.DiscountAmount = 0;
                                    headerPo.PercentPaymentTerm1 = 100;
                                    headerPo.PercentPaymentTerm2 = 0;
                                    headerPo.PercentPaymentTerm3 = 0;
                                    headerPo.PaymentTerm1 = entity.TotalAmount;
                                    headerPo.PaymentTerm2 = 0;
                                    headerPo.PaymentTerm3 = 0;
                                    headerPo.SubTotal = entity.Amount;
                                    headerPo.VatPercent = Convert.ToDecimal(entity.VatPercent ?? 0);
                                    headerPo.Vat = entity.Vat;
                                    headerPo.InvoicingType = "SINGLE";
                                    //headerPo.PaymentTerm1 = (ca.Amount - entity.Amount) + ((ca.Amount - entity.Amount) * (headerPo.VatPercent / 100));
                                    //headerPo.SubTotal = (ca.Amount - entity.Amount);
                                    //headerPo.Vat = (ca.Amount - entity.Amount) * (headerPo.VatPercent / 100);
                                    //headerPo.GrandTotal = (ca.Amount - entity.Amount) + ((ca.Amount - entity.Amount) * (headerPo.VatPercent / 100));
                                    headerPo.GrandTotal = entity.TotalAmount;
                                    headerPo.MainserviceCategoryId = entity.MainserviceCategoryId;
                                    headerPo.CashAdvanceId = entity.CashAdvanceId;
                                    headerPo.PaymentRequestId = entity.PaymentRequestId;
                                    headerPo.CashAdvanceSettlementId = entity.Id;
                                    headerPo.TypeOfExpenseId = entity.PceTypeId;
                                    //headerPo.SpecialRequestId = SpecialRequestType.TypeCashAdvanceId;
                                    headerPo.PaymentMethodId = PaymentType.CashAdvance;
                                    headerPo.Status = StatusTransactionName.Draft;

                                    var Companys = await context.Companys.AsNoTracking().Where(x => x.Id == headerPo.CompanyId && x.IsClient.Equals(false) && x.IsVendor.Equals(true)).FirstOrDefaultAsync();
                                    if (Companys != null)
                                    {
                                        headerPo.CompanyName = Companys.CompanyName;
                                        headerPo.NumberNpwp = Companys.TaxRegistrationNumber;
                                        headerPo.CompanyReference = Companys.OfficeAddress;
                                        headerPo.VendorPicName = Companys.ContactName;
                                    }
                                    await context.PhoenixAddAsync(headerPo);

                                    foreach (CASettlementDetail detail in casDetail)
                                    {
                                        var modelDetailPo = new PurchaseOrderDetail();
                                        modelDetailPo.Id = Guid.NewGuid().ToString();
                                        modelDetailPo.PurchaseOrderId = headerPo.Id;
                                        modelDetailPo.TaskId = detail.TaskId;
                                        modelDetailPo.Task = detail.TaskName;
                                        //modelDetailPo.ItemId = detail.ItemId;
                                        //modelDetailPo.ItemCode = detail.ItemCode;
                                        //modelDetailPo.ItemName = detail.ItemName;
                                        modelDetailPo.UomId = detail.UomId;
                                        modelDetailPo.Qty = detail.Qty;
                                        modelDetailPo.UnitPrice = detail.UnitPrice;
                                        modelDetailPo.Amount = detail.Amount;
                                        modelDetailPo.TotalRequest = detail.Amount;
                                        modelDetailPo.TotalExchangeRequest = detail.TotalExchangeAmount;
                                        modelDetailPo.TotalExchangeAmount = detail.TotalExchangeAmount;

                                        ////yang masuk ke po adalah sisa dari ca request - ca settlement
                                        //modelDetailPo.UnitPrice = (detail.TotalRequest - detail.Amount) / detail.Qty;
                                        //modelDetailPo.Amount = detail.Qty * modelDetailPo.UnitPrice;
                                        //modelDetailPo.TotalRequest = modelDetailPo.Amount;
                                        //modelDetailPo.TotalExchangeRequest = (detail.TotalExchangeRequest - detail.TotalExchangeAmount);
                                        //modelDetailPo.TotalExchangeAmount = modelDetailPo.TotalExchangeRequest;

                                        //modelDetailPo.ItemTypeId = detail.ItemTypeId;
                                        await context.PhoenixAddRangeAsync(modelDetailPo);
                                    }
                                }

                                ////Create Journal (journal di cash advance settlement detail sudah tidak ada - request mas daus 24-03-2020)
                                //await context.Database.ExecuteSqlCommandAsync("EXECUTE fn.store_data_jurnal @p_id", parameters: new[] { new SqlParameter("@p_id", entity.Id) });

                                entity.Status = StatusTransactionName.Completed;
                                entity.RemarksProcess = entity.RemarksProcess;

                                //update CA
                                if (!string.IsNullOrEmpty(ca.Id))
                                {
                                    ca.Status = entity.Status;
                                    ca.ModifiedOn = DateTime.Now;
                                    context.PhoenixEdit(ca);
                                }

                                //update Status PCA Task
                                if (!string.IsNullOrEmpty(entity.JobId))
                                {
                                    foreach (var detail in entity.CASettlementDetail)
                                    {
                                        var pcaTask = await context.PcaTasks.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.PcaId == entity.PcaId && x.Id == detail.TaskId).FirstOrDefaultAsync();
                                        if (pcaTask == null) throw new Exception("PCA Task cannot be empty");
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

                                    await context.SaveChangesAsync();
                                    var pca = await context.Pcas.AsNoTracking().Where(x => x.Id == entity.PcaId).FirstOrDefaultAsync();
                                    if (pca != null)
                                    {
                                        pca.TotalTaskPriceUsed = context.PcaTasks.Where(x => !(x.IsDeleted ?? false) && x.PcaId == pca.Id).Sum(x => (x.TotalPriceUsed ?? 0));
                                        pca.TotalTaskPriceBalance = context.PcaTasks.Where(x => !(x.IsDeleted ?? false) && x.PcaId == pca.Id).Sum(x => (x.TotalPriceBalance ?? 0));
                                        context.Pcas.Update(pca);
                                    }
                                }
                            }
                        }
                    }

                    await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = entity.Status, Description = entity.RemarksProcess });
                    context.PhoenixEdit(entity);
                    save = await context.SaveChangesAsync();
                    transaction.Commit();
                    return entity;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<CASettlement> Reject(CASettlement entity)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var save = await globalFcApproval.UnsafeProcessApproval(entity.Id, StatusTransaction.Rejected, entity.RemarksProcess, entity);
                    var approval = context.TrTemplateApprovals.Where(x => x.RefId == entity.Id && !(x.IsDeleted ?? false)).FirstOrDefault();
                    if (approval != null)
                    {
                        entity.Status = StatusTransactionName.Revised;
                        if (approval.StatusApproved == Models.Um.StatusApproved.Rejected)
                        {
                            //if (!string.IsNullOrEmpty(entity.JobId))
                            //{
                            //    foreach (var item in context.CASettlementDetail)
                            //    {
                            //        var pcaTask = await context.PcaTasks.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == item.TaskId && x.PcaId == entity.PcaId).FirstOrDefaultAsync();
                            //        pcaTask.TotalPriceUsed = pcaTask.TotalPriceUsed - item.TotalRequestExchange;
                            //        pcaTask.TotalPriceBalance = pcaTask.TotalPrice - pcaTask.TotalPriceUsed;
                            //        context.PhoenixEdit(pcaTask);
                            //    }

                            //    await context.SaveChangesAsync();
                            //    var pca = await context.Pcas.AsNoTracking().Where(x => x.Id == entity.PcaId).FirstOrDefaultAsync();
                            //    if (pca != null)
                            //    {
                            //        pca.TotalTaskPriceUsed = context.PcaTasks.Where(x => !(x.IsDeleted ?? false) && x.PcaId == pca.Id).Sum(x => (x.TotalPriceUsed ?? 0));
                            //        pca.TotalTaskPriceBalance = context.PcaTasks.Where(x => !(x.IsDeleted ?? false) && x.PcaId == pca.Id).Sum(x => (x.TotalPriceBalance ?? 0));
                            //        context.Pcas.Update(pca);
                            //    }
                            //}
                        }
                    }
                    await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = entity.Status, Description = entity.RemarksProcess });
                    context.PhoenixEdit(entity);
                    save = await context.SaveChangesAsync();
                    transaction.Commit();
                    return entity;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}