using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Finance.Transaction;
using Phoenix.Data.Services.Um;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Phoenix.Data.Services.Finance.Transaction
{
    public interface IInvoiceClientService
    {
        Task<List<InvoiceClient>> GetInvoiceList(FilterInvoiceClient search);
        Task<InvoiceClient> GetCompanyInfo(string companyid);
        Task<Bank> GetInfoBank(string idbank);
        Task<InvoiceClient> GetAffiliation(string divisiid);
        Task<List<PceTask>> GetPCEDetail(string pceno);
        Task<List<InvoiceClientItemOther>> GetInvoiceDetail(string Id);
        Task<List<InvoiceClient>> ReadHistoryInvoice(string invoiceId);
        Task<InvoiceClient> Get(string Id);
        Task<List<InvoiceClient>> GetPceList(string clientid, string legalid, string divisionid, string pceId, string jobIdName, string currencyId);
        Task<List<Journal>> GetDataJournal(string Id);
        Task<int> AddAsync(InvoiceClient entity);
        Task<int> EditAsync(InvoiceClient entity);
        Task<int> CreateJournalAsync(InvoiceClient entity);
        Task<int> Approve(InvoiceClient entity);
        //Task<InvoiceClient> ForceClose(InvoiceClient entity);
        Task<int> Reject(InvoiceClient entity);
        Task<int> UpdateJournalAsync(Journal entity);
    }
    public class InvoiceClientService : IInvoiceClientService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly IManageMenuService menuService;
        readonly StatusLogService log;
        readonly FileService uploadFile;
        readonly FinancePeriodService financePeriod;
        private readonly IManageTransactionApprovalService trApprovalService;
        private readonly IManageTemplateApprovalService trTemplateApprovalService;

        public InvoiceClientService(DataContext context, GlobalFunctionApproval globalFcApproval, IManageMenuService menuService, StatusLogService log, FileService uploadFile, FinancePeriodService financePeriod, IManageTransactionApprovalService trApprovalService, IManageTemplateApprovalService trTemplateApprovalService)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.menuService = menuService;
            this.log = log;
            this.uploadFile = uploadFile;
            this.financePeriod = financePeriod;
            this.trApprovalService = trApprovalService;
            this.trTemplateApprovalService = trTemplateApprovalService;
        }

        public async Task<List<InvoiceClient>> GetInvoiceList(FilterInvoiceClient search)
        {
            var month = (search.EndingDate.HasValue ? search.EndingDate.Value.Month : 0);
            var year = (search.EndingDate.HasValue ? search.EndingDate.Value.Year : 0);

            var result = await (from a in context.InvoiceClients
                                join e in context.Companys on a.ClientId equals e.Id
                                join g in context.LegalEntity on a.LegalId equals g.Id
                                join i in context.Banks on a.AccountId equals i.Id
                                join j in context.MainServiceCategorys on a.MainServiceCategory equals j.Id
                                join h in context.CompanyBrands on a.BrandId equals h.Id
                                join k in context.Pces on a.PceId equals k.Id
                                where string.IsNullOrEmpty(a.MultiPceId)
                                && !(a.IsDeleted ?? false) && (string.IsNullOrEmpty(search.Status) ? true : a.Status == search.Status)
                                && (search.DateType == "Day" ? (search.EndingDate == null ? true : a.InvoiceDate == search.EndingDate) :
                                    search.DateType == "Month" ? (search.EndingDate == null ? true : (a.InvoiceDate.HasValue ? a.InvoiceDate.Value.Year : 0) == year && (a.InvoiceDate.HasValue ? a.InvoiceDate.Value.Month : 0) == month) :
                                    search.DateType == "Year" ? (search.EndingDate == null ? true : (a.InvoiceDate.HasValue ? a.InvoiceDate.Value.Year : 0) == year) :
                                    search.DateType == "Custom Range" ? (search.StartingDate == null && search.EndingDate == null ? true : a.InvoiceDate >= search.StartingDate && a.InvoiceDate <= search.EndingDate) : true)
                                select new InvoiceClient
                                {
                                    Id = a.Id,
                                    PceId = a.PceId,
                                    InvoiceDate = a.InvoiceDate,
                                    JobName = $"{a.JobId} - {a.JobName}",
                                    ClientName = e.CompanyName,
                                    //GrandTotal = b.Amount,
                                    GrandTotal = a.GrandTotal,
                                    LegalEntityName = g.LegalEntityName,
                                    DivisionName = context.BusinessUnits.Where(x => x.Id == a.BusinessUnitId).Select(x => x.UnitName).FirstOrDefault(),
                                    Status = a.Status,
                                    MultiPceId = a.MultiPceId,
                                    CurrencySymbol = context.Currencies.Where(x => x.Id == a.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault(),
                                    MainServiceCategoryName = j.Name,
                                    ClientBrandName = h.BrandName,
                                    ClientTaxNumber = e.TaxRegistrationNumber,
                                    ExchangeRate = a.ExchangeRate,
                                    POClientNo = k.PoClient,
                                }).ToListAsync();
            return result;
        }

        public async Task<InvoiceClient> Get(string id)
        {
            var result = await (from z in context.InvoiceClients
                                join c in context.Currencies on z.CurrencyId equals c.Id
                                join d in context.MainServiceCategorys on z.MainServiceCategory equals d.Id
                                join e in context.LegalEntity on z.LegalId equals e.Id
                                join h in context.CompanyBrands on z.BrandId equals h.Id
                                join i in context.Companys on z.ClientId equals i.Id
                                where z.Id == id && z.MultiPceId == null
                                select new InvoiceClient
                                {
                                    Id = z.Id,
                                    PceId = z.PceId,
                                    PceSeq = z.PceSeq,
                                    BrandId = z.BrandId,
                                    ClientId = z.ClientId,
                                    AlternateClientId = z.AlternateClientId,
                                    CurrencyId = z.CurrencyId,
                                    InvoiceDate = z.InvoiceDate,
                                    InvoiceNumber = z.InvoiceNumber,
                                    StartPeriod = z.StartPeriod,
                                    EndPeriod = z.EndPeriod,
                                    VatNo = z.VatNo,
                                    Vat = z.Vat,
                                    VatExchangeRate = z.VatExchangeRate,
                                    TotalAmount = z.TotalAmount,
                                    ExchangeRate = z.ExchangeRate,
                                    JobName = context.JobDetail.Where(x => x.Id == z.JobId).Select(x => x.JobName).FirstOrDefault(),//z.JobName,
                                    JobId = z.JobId,
                                    Remarks = z.Remarks,
                                    EstimateNumber = z.EstimateNumber,
                                    RefNumber = z.RefNumber,
                                    YourRef = z.YourRef,
                                    Status = z.Status,
                                    AccountId = z.AccountId,
                                    Asf = z.Asf,
                                    AsfAmount = z.AsfAmount,
                                    GrandTotal = z.GrandTotal,
                                    MultiPceId = z.MultiPceId,
                                    PayDescription = z.PayDescription,
                                    PoDocFileId = z.PoDocFileId,
                                    VatDocFileId = z.VatDocFileId,
                                    MainServiceCategory = z.MainServiceCategory,
                                    LegalId = z.LegalId,
                                    AffiliationId = z.AffiliationId,
                                    BusinessUnitId = z.BusinessUnitId,
                                    POClientNo = z.POClientNo,
                                    InvoiceBilled = string.IsNullOrEmpty(z.PceId) ? null : z.InvoiceBilled,
                                    PceTypeCrossBilling = z.PceTypeCrossBilling,
                                    CreatedBy = z.CreatedBy,
                                    CreatedOn = z.CreatedOn,
                                    LegalEntityName = e.LegalEntityName,
                                    TermOfPayment = h.TermOfPayment,
                                    JobStatus = context.JobDetail.Where(x => x.Id == z.JobId).Select(x => x.JobStatusId).FirstOrDefault(),
                                    ClientName = i.CompanyName,
                                    ClientBrandName = h.BrandName,
                                    ClientInvAddress = h.BrandAddress,
                                    ClientTaxNumber = i.TaxRegistrationNumber,
                                    ClientPIC = h.ContactName,
                                    totalamtplusasf = z.TotalAmount + (z.AsfAmount ?? 0),
                                    VAtPercentage = Math.Ceiling((decimal)((z.Vat / (z.TotalAmount + (z.AsfAmount ?? 0))) * 100)),
                                    PceBalance = string.IsNullOrEmpty(z.PceId) ? null : context.Pces.Where(x => x.Id == z.PceId).Select(x => x.SubtotalOriginal ?? x.Subtotal).FirstOrDefault() - z.InvoiceBilled,
                                    AccountBankNumber = context.Banks.Where(x => x.Id == z.AccountId).Select(x => x.AccountNumber).FirstOrDefault(),
                                    InvoiceTo = $"{z.PceSeq} of {context.InvoiceClients.Where(x => x.PceId == z.PceId).Max(x => x.PceSeq)}",
                                    TotalPCE = context.Pces.Where(x => x.Id == z.PceId).Select(x => x.SubtotalOriginal ?? x.Subtotal).FirstOrDefault(),
                                    CurrencySymbol = c.CurrencySymbol,
                                    CurrencyIdDefault = (context.JournalTemps.Where(x => x.ReferenceId == z.Id).Select(x => x.DefaultCurrencyId)).FirstOrDefault() ??
                                                            context.Currencies.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).Select(x => x.Id).FirstOrDefault(),
                                    CurrencySymbolDefault = (from o in context.JournalTemps
                                                             join p in context.Currencies on o.DefaultCurrencyId equals p.Id
                                                             where o.ReferenceId == z.Id
                                                             select p.CurrencySymbol).FirstOrDefault() ??
                                                            context.Currencies.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).Select(x => x.CurrencySymbol).FirstOrDefault()
                                }).AsNoTracking().FirstOrDefaultAsync();
            return await GetFile(result);
        }

        private async Task<InvoiceClient> GetFile(InvoiceClient data)
        {
            if (data.PoDocFileId != null)
            {
                var a = await context.Filemasters.Where(x => x.Id == data.PoDocFileId).FirstOrDefaultAsync();
                if (a != null) data.PoDocFileName = a.Name;
            }

            if (data.VatDocFileId != null)
            {
                var b = await context.Filemasters.Where(x => x.Id == data.VatDocFileId).FirstOrDefaultAsync();
                if (b != null) data.VatDocFileName = b.Name;
            }
            return data;
        }

        public async Task<List<InvoiceClient>> GetPceList(string clientid, string legalid, string divisionid, string pceId, string jobIdName, string currencyId)
        {
            var result = await (from b in context.Pces
                                join c in context.JobDetail on b.JobId equals c.Id
                                join d in context.LegalEntity on c.LegalEntityId equals d.Id
                                join f in context.BusinessUnits on c.BusinessUnitDivisionId equals f.Id
                                join g in context.CompanyBrands on c.BrandId equals g.Id
                                join h in context.Companys on g.CompanyId equals h.Id
                                where b.Status == StatusTransactionName.Approved
                                      && (clientid == "0" ? true : g.CompanyId == clientid)
                                      && (legalid == "0" ? true : c.LegalEntityId == legalid)
                                      && (divisionid == "0" ? true : c.BusinessUnitDivisionId == divisionid)
                                      && (pceId == "0" ? true : b.Id.Contains(pceId))
                                      && (jobIdName == "0" ? true : ($"{c.Id} - {c.JobName}").Contains(jobIdName))
                                      && (currencyId == "0" ? true : (context.PceTasks.Where(x => x.PceId == b.Id && !string.IsNullOrEmpty(x.CurrencyId)).Select(x => x.CurrencyId).FirstOrDefault() ?? b.CurrencyId) == currencyId)
                                      //&& b.PceReference == null
                                      && b.TotalBalanceInvoice > 0
                                      && !context.InvoiceClients.Any(x => x.PceId == b.Id && x.Status != InvoiceClientStatus.Approved)
                                select new InvoiceClient
                                {
                                    PceId = b.Id,
                                    Asf = b.OtherFeePercentage,
                                    AsfAmount = b.OtherFeeOriginal ?? b.OtherFee,
                                    VAtPercentage = Math.Ceiling((decimal)((b.Vat / (b.Subtotal + b.OtherFee)) * 100)),
                                    Vat = b.VatOriginal ?? b.Vat,
                                    TotalAmount = b.SubtotalOriginal ?? b.Subtotal,
                                    GrandTotal = b.TotalOriginal ?? b.Total,
                                    ClientName = h.CompanyName,
                                    ClientBrandName = g.BrandName,
                                    Status = InvoiceClientStatus.Open,
                                    InvoiceBilled = b.TotalUsedInvoice,
                                    PceBalance = b.TotalBalanceInvoice,
                                    JobId = c.Id,
                                    JobName = c.JobName,
                                    JobStatus = c.JobStatusId,
                                    LegalId = d.Id,
                                    LegalEntityName = d.LegalEntityName,
                                    BusinessUnitId = f.Id,
                                    DivisionName = f.UnitName,
                                    ClientId = h.Id,
                                    BrandId = g.Id,
                                    CurrencyId = context.PceTasks.Where(x => x.PceId == b.Id && !string.IsNullOrEmpty(x.CurrencyId)).Select(x => x.CurrencyId).FirstOrDefault() ?? b.CurrencyId,
                                    CurrencyName = (from y in context.PceTasks
                                                    join z in context.Currencies on (y.CurrencyId ?? b.CurrencyId) equals z.Id
                                                    where y.PceId == b.Id
                                                    select z.CurrencyName).FirstOrDefault(),
                                    MainServiceCategory = b.MainserviceCategoryId,
                                    ExchangeRate = context.PceTasks.Where(x => x.PceId == b.Id && !string.IsNullOrEmpty(x.CurrencyId)).Select(x => x.ExchangeRate).FirstOrDefault() ?? Convert.ToDecimal(b.CurrencyId == CurrencyIDR.CurrencyId ? "1" : "0"),
                                    TermOfPayment = g.TermOfPayment,
                                    POClientNo = b.PoClient,
                                    ClientInvAddress = g.BrandAddress,
                                    ClientTaxNumber = h.TaxRegistrationNumber,
                                    ClientPIC = g.ContactName,
                                    AffiliationId = c.AffiliationId,
                                    CreatedOn = b.CreatedOn,
                                    PceSeq = (context.InvoiceClients.Where(x => x.PceId == b.Id).Max(x => x.PceSeq) ?? 0) + 1,
                                    TotalPCE = b.SubtotalOriginal ?? b.Subtotal,
                                    PceTypeCrossBilling = b.TypeOfExpenseId == TypeOfExpenseCrossBilling.TypeOfExpenseId,
                                    CurrencySymbol = (from y in context.PceTasks
                                                      join z in context.Currencies on (y.CurrencyId ?? b.CurrencyId) equals z.Id
                                                      where y.PceId == b.Id
                                                      select z.CurrencySymbol).FirstOrDefault(),
                                    CurrencyIdDefault = context.Currencies.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).Select(x => x.Id).FirstOrDefault(),
                                    CurrencySymbolDefault = context.Currencies.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).Select(x => x.CurrencySymbol).FirstOrDefault()
                                }).OrderByDescending(x => x.CreatedOn).ToListAsync();
            return result;
        }

        public async Task<InvoiceClient> GetCompanyInfo(string companyid)
        {
            var company = await (
                from a in context.Companys
                join b in context.CompanyBrands on a.Id equals b.CompanyId
                where b.Id == companyid
                select new InvoiceClient
                {
                    ClientPIC = b.ContactName,
                    ClientTaxNumber = a.TaxRegistrationNumber,
                    ClientInvAddress = b.BrandAddress,
                    TermOfPayment = b.TermOfPayment,
                    Asf = b.ASFValue == null ? 0 : b.ASFValue
                }).AsNoTracking().FirstOrDefaultAsync();
            return company;
        }

        public async Task<Bank> GetInfoBank(string idbank) => await context.Banks.Where(x => x.Id == idbank).FirstOrDefaultAsync();

        public async Task<InvoiceClient> GetAffiliation(string divisiid) => await context.BusinessUnits.Where(x => x.Id == divisiid).Select(x => new InvoiceClient { AffiliationId = x.AffiliationId }).FirstOrDefaultAsync();

        public async Task<List<PceTask>> GetPCEDetail(string pceno)
        {
            var result = await (from a in context.Pces
                                join x in context.PceTasks on a.Id equals x.PceId
                                where x.PceId == pceno && string.IsNullOrEmpty(x.ParentId) && x.TotalBalanceInvoice > 0
                                select new PceTask()
                                {
                                    Id = x.Id,
                                    PceId = x.PceId,
                                    TaskName = x.TaskName,
                                    Quantity = x.Quantity,
                                    UomId = x.UomId,
                                    Price = x.Price,
                                    IsPpn = x.IsPpn,
                                    Pph = x.Pph,
                                    IsAsf = x.IsAsf,
                                    Asf = x.Asf,
                                    Ppn = x.Ppn,
                                    Cost = x.Cost,
                                    Percentage = x.Percentage,
                                    Margin = x.Margin,
                                    UnitPrice = x.TotalOriginal != null ? (x.Operator == "*" ? (x.UnitPrice * x.ExchangeRate) : (x.UnitPrice / x.ExchangeRate)) : x.UnitPrice,
                                    Total = x.TotalOriginal ?? x.Total,
                                    Departement = x.Departement,
                                    IsRateCard = x.IsRateCard,
                                    ParentId = x.ParentId,
                                    TotalUsedInvoice = x.TotalUsedInvoice,
                                    TotalBalanceInvoice = x.TotalBalanceInvoice,
                                    CurrencySymbol = context.Currencies.Where(y => y.Id == (x.CurrencyId ?? a.CurrencyId)).Select(y => y.CurrencySymbol).FirstOrDefault()
                                }).ToListAsync();
            return result;
        }

        public async Task<List<InvoiceClientItemOther>> GetInvoiceDetail(string Id)
        {
            var result = await (from a in context.InvoiceClients
                                join x in context.InvoiceClientItemOthers on a.Id equals x.InvoiceId
                                where x.InvoiceId == Id
                                select new InvoiceClientItemOther
                                {
                                    Id = x.Id,
                                    ParentId = x.ParentId,
                                    InvoiceId = x.InvoiceId,
                                    Amount = x.Amount,
                                    Sequence = x.Sequence,
                                    AccountID = x.AccountID,
                                    PceID = x.PceID,
                                    JobID = x.JobID,
                                    TaskId = x.TaskId,
                                    AmountPercentage = x.AmountPercentage,
                                    Quantity = x.Quantity,
                                    TaskName = x.TaskName,
                                    UnitPrice = x.UnitPrice,
                                    AmountBilled = x.AmountBilled,
                                    Balance = context.PceTasks.Where(z => z.PceId == x.PceID && z.Id == x.TaskId).Select(z => z.TotalBalanceInvoice).FirstOrDefault(),
                                    IsAsf = context.PceTasks.Where(z => z.PceId == x.PceID && z.Id == x.TaskId).Select(z => z.IsAsf).FirstOrDefault(),
                                    CurrencySymbol = context.Currencies.Where(x => x.Id == a.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault()
                                }).ToListAsync();
            return result;
        }

        public async Task<List<InvoiceClient>> ReadHistoryInvoice(string pceId)
        {
            if (string.IsNullOrEmpty(pceId)) return new List<InvoiceClient>();
            var result = await (from a in context.InvoiceClients
                                where a.PceId == pceId && a.Status == InvoiceClientStatus.Approved
                                select new InvoiceClient
                                {
                                    InvoiceNumber = a.InvoiceNumber,
                                    InvoiceDate = a.InvoiceDate,
                                    TotalAmount = a.TotalAmount,
                                    Asf = a.Asf,
                                    AsfAmount = (a.AsfAmount ?? 0),
                                    totalamtplusasf = a.TotalAmount + (a.AsfAmount ?? 0),
                                    VAtPercentage = Math.Ceiling((decimal)((a.Vat / (a.TotalAmount + (a.AsfAmount ?? 0))) * 100)),
                                    Vat = a.Vat,
                                    GrandTotal = a.GrandTotal,
                                    CurrencySymbol = context.Currencies.Where(x => x.Id == a.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault()
                                }).ToListAsync();
            return result;
        }

        public async Task<List<Journal>> GetDataJournal(string Id)
        {
            var result = await (from tb in context.JournalTemps
                                join coa in context.ChartOfAccounts on tb.AccountId equals coa.Id
                                join inv in context.InvoiceClients on tb.ReferenceId equals inv.Id
                                where tb.ReferenceId == Id
                                select new Journal
                                {
                                    Id = tb.Id,
                                    ReferenceId = tb.ReferenceId,
                                    AccountId = coa.Id,
                                    CodeRec = coa.CodeRec,
                                    AccountName = coa.Name5,
                                    JobId = tb.JobId,
                                    Description = tb.Description,
                                    JobIdName = $"{inv.JobId} - {inv.JobName}",
                                    DivisiName = context.BusinessUnits.Where(x => x.Id == tb.DivisiId).Select(x => x.UnitName).FirstOrDefault(),
                                    LegalEntityName = context.LegalEntity.Where(x => x.Id == tb.LegalId).Select(x => x.LegalEntityName).FirstOrDefault(),
                                    Credit = tb.Credit,
                                    Debit = tb.Debit,
                                    LineCode = tb.LineCode,
                                    DivisiId = tb.DivisiId,
                                    LegalId = tb.LegalId,
                                    CurrencySymbol = context.Currencies.Where(x => x.Id == tb.DefaultCurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault()
                                }).OrderByDescending(x => x.Debit).ThenByDescending(x => x.Credit).ToListAsync();
            return result;
        }

        public async Task<int> AddAsync(InvoiceClient entity)
        {
            return await AddEdit(entity, false, false);
        }

        public async Task<int> EditAsync(InvoiceClient entity)
        {
            return await AddEdit(entity, true, false);
        }

        public async Task<int> CreateJournalAsync(InvoiceClient entity)
        {
            await financePeriod.IsClosePeriod(Convert.ToDateTime(entity.InvoiceDate), entity.LegalId, Models.Enum.FinancePeriodCode.AR);
            bool isEdit = false;
            if (!string.IsNullOrEmpty(entity.Id)) isEdit = true;
            return await AddEdit(entity, isEdit, true);
        }

        public async Task<int> Approve(InvoiceClient model)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var save = await globalFcApproval.UnsafeProcessApproval(model.Id, StatusTransaction.Approved, model.LogDescription, model);
                    var approval = context.TrTemplateApprovals.AsNoTracking().Where(x => x.RefId == model.Id && !(x.IsDeleted ?? false)).FirstOrDefault();
                    if (approval != null)
                    {
                        model.Status = InvoiceClientStatus.WaitingApproval;

                        if (approval.StatusApproved == Models.Um.StatusApproved.Approved)
                        {
                            //var grandTotal = context.InvoiceClients.Where(x => x.PceId == model.PceId).Sum(x => x.GrandTotal);
                            decimal subTotal = 0;
                            if (!string.IsNullOrEmpty(model.PceId))
                            {
                                subTotal = context.InvoiceClients.Where(x => x.PceId == model.PceId).Sum(x => x.TotalAmount) ?? 0;
                                model.InvoiceBilled = subTotal;
                            }

                            model.Status = InvoiceClientStatus.Approved;

                            //post data AR
                            AccountReceivable accountReceivable = new AccountReceivable()
                            {
                                AccountReceivableNumber = model.Id,
                                Id = model.Id,
                                Amount = model.GrandTotal,
                                BrandId = model.BrandId,
                                JobId = model.JobId,
                                LegalEntityId = model.LegalId,
                                AffilitionId = model.AffiliationId,
                                Status = ARStatus.Open,
                                BankAccountDestination = model.AccountId,
                                ExchangeRate = model.ExchangeRate == null ? 0 : model.ExchangeRate,
                                CurrencyId = model.CurrencyId,
                                TransactionDate = model.InvoiceDate,
                                BusinessUnitId = model.BusinessUnitId,
                                VatNo = model.VatNo,
                                ClientId = model.ClientId
                            };

                            await context.PhoenixAddAsync(accountReceivable);

                            if (!string.IsNullOrEmpty(model.PceId))
                            {
                                //single pce
                                await context.SaveChangesAsync();
                                var pce = await context.Pces.AsNoTracking().Where(x => x.Id == model.PceId).FirstOrDefaultAsync();
                                pce.TotalUsedInvoice = subTotal;
                                pce.TotalBalanceInvoice = (pce.SubtotalOriginal ?? pce.Subtotal) - pce.TotalUsedInvoice;
                                //pce.CurrentInvoice = model.InvoiceBilled;
                                context.Pces.Update(pce);

                                var task = await context.PceTasks.AsNoTracking().Where(x => x.PceId == model.PceId).ToListAsync();
                                var itemOther = await (from a in context.InvoiceClients
                                                       join b in context.InvoiceClientItemOthers on a.Id equals b.InvoiceId
                                                       where a.PceId == model.PceId
                                                       group b by new { a.PceId, b.TaskId } into g
                                                       select new
                                                       {
                                                           g.Key.PceId,
                                                           g.Key.TaskId,
                                                           AmountBilled = g.Sum(x => x.AmountBilled ?? 0)
                                                       }).AsNoTracking().ToListAsync();
                                foreach (var item in task)
                                {
                                    var other = itemOther.Where(x => x.TaskId == item.Id).FirstOrDefault();
                                    item.TotalUsedInvoice = other.AmountBilled;
                                    item.TotalBalanceInvoice = (item.TotalOriginal ?? item.Total) - item.TotalUsedInvoice;
                                    context.PceTasks.Update(item);
                                }
                            }
                        }

                        context.PhoenixEdit(model);
                        await log.AddAsync(new StatusLog() { TransactionId = model.Id, Status = InvoiceClientStatus.StatusName(model.Status), Description = model.LogDescription });
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

        //public async Task<InvoiceClient> ForceClose(InvoiceClient entity)
        //{
        //    try
        //    {
        //        entity.Status = InvoiceClientStatus.Approved;

        //        await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = InvoiceClientStatus.StatusName(entity.Status), Description = entity.LogDescription });
        //        context.PhoenixEdit(entity);
        //        var save = await context.SaveChangesAsync();
        //        return entity;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message.ToString());
        //    }
        //}

        public async Task<int> Reject(InvoiceClient model)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var save = await globalFcApproval.UnsafeProcessApproval(model.Id, StatusTransaction.Rejected, model.LogDescription, model);
                    var approval = context.TrTemplateApprovals.AsNoTracking().Where(x => x.RefId == model.Id && !(x.IsDeleted ?? false)).FirstOrDefault();
                    if (approval != null)
                    {
                        model.Status = InvoiceClientStatus.WaitingApproval;
                        if (approval.StatusApproved == Models.Um.StatusApproved.Rejected)
                        {
                            model.Status = InvoiceClientStatus.Rejected;
                        }
                        context.PhoenixEdit(model);
                        await log.AddAsync(new StatusLog() { TransactionId = model.Id, Status = InvoiceClientStatus.StatusName(model.Status), Description = model.LogDescription });
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

        private async Task<int> AddEdit(InvoiceClient entity, bool isEdit, bool isJournal)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var model = await ProcessUpload(entity);
                    if (!string.IsNullOrEmpty(model.Id))
                    {
                        var detail = context.InvoiceClientItemOthers.AsNoTracking().Where(x => x.InvoiceId == model.Id).ToList();
                        if (detail.Count > 0)
                        {
                            context.InvoiceClientItemOthers.RemoveRange(detail.ToArray());
                        }
                    }

                    if (isEdit)
                    {
                        context.PhoenixEdit(model);
                    }
                    else
                    {
                        model.Id = await TransID.GetTransId(context, Code.InvoiceClientCode, DateTime.Today);
                        model.InvoiceNumber = model.Id;
                        model.PceSeq = string.IsNullOrEmpty(model.PceId) ? 1 : (context.InvoiceClients.Where(x => x.PceId == model.PceId).Count() + 1);
                        model.PceTypeCrossBilling = model.PceTypeCrossBilling ?? false;

                        await context.PhoenixAddAsync(model);
                        await context.SaveChangesAsync();
                    }

                    if (isEdit)
                    {
                        context.Database.ExecuteSqlCommand($"UPDATE fn.e_faktur_detail SET is_used=0, reff_number=NULL WHERE reff_number={entity.InvoiceNumber}");
                        if (!string.IsNullOrEmpty(model.VatNo))
                        {
                            context.Database.ExecuteSqlCommand($"UPDATE fn.e_faktur_detail SET is_used=1, reff_number={entity.InvoiceNumber} WHERE vat_no={entity.VatNo}");
                        }
                    }

                    if (model.items_other != null)
                    {
                        foreach (var item in model.items_other)
                        {
                            item.Id = Guid.NewGuid().ToString();
                            item.InvoiceId = model.Id;
                            item.PceID = model.PceId;
                            item.JobID = model.JobId;
                            item.Sequence = model.PceSeq;
                            await context.AddAsync(item);
                        };
                    }

                    await context.SaveChangesAsync();
                    if (isJournal)
                    {
                        await context.Database.ExecuteSqlCommandAsync("EXECUTE fn.create_jurnal_ar2_temp @p_account_receivable_id", parameters: new[] { new SqlParameter("@p_account_receivable_id", model.Id) });
                    }

                    var isApprove = true;
                    if (model.Status != InvoiceClientStatus.Open)
                    {
                        var dataMenu = await menuService.GetByUniqueName(MenuUnique.InvoiceClient).ConfigureAwait(false);
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            MenuId = dataMenu.Id,
                            RefId = model.Id,
                            DetailLink = $"{ApprovalLink.InvoiceClient}?Id={model.Id}",
                            NotificationType = NotificationTypeCode.InvoiceClient,
                            Module = NotificationModuleType.ModuleFinance
                        };
                        var subGroupId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(context.HttpContext);
                        var divisionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(context.HttpContext);
                        var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
                        isApprove = await globalFcApproval.Save(vm, subGroupId, divisionId, employeeId);
                        if (isApprove)
                        {
                            await log.AddAsync(new StatusLog() { TransactionId = model.Id, Status = InvoiceClientStatus.StatusName(model.Status), Description = model.LogDescription });
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
                        context.InvoiceClients.Update(model);
                        await log.AddAsync(new StatusLog() { TransactionId = model.Id, Status = InvoiceClientStatus.StatusName(model.Status), Description = model.LogDescription });
                        var save = await context.SaveChangesAsync();
                        transaction.Commit();
                        return save;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message.ToString());
                }
            }
        }

        private async Task<InvoiceClient> ProcessUpload(InvoiceClient entity)
        {
            var file1 = await uploadFile.Upload(entity.VatDocFile, null);
            if (!string.IsNullOrEmpty(file1))
            {
                entity.VatDocFileId = file1;
            }

            var file2 = await uploadFile.Upload(entity.PoDocFile, null);
            if (!string.IsNullOrEmpty(file2))
            {
                entity.PoDocFileId = file2;
            }

            return entity;
        }

        public async Task<int> UpdateJournalAsync(Journal entity)
        {
            context.Database.ExecuteSqlCommand($"UPDATE fn.journal_temp SET Description={entity.Description} WHERE id={entity.Id}");
            return await context.SaveChangesAsync();
        }
    }
}
