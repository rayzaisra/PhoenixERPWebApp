using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Finance.Transaction
{
    public interface IInvoiceReceivableService
    {
        Task<InvoiceReceivable> Get(string Id);
        Task<InvoiceReceivable> GetPoById(string poId);
        Task<List<InvoiceReceivable>> GetHistoryPayment(string poId);
        Task<List<InvoiceReceivable>> GetList(string status, string businessUnit, string legalEntity, string invoiceNumber, string invoiceVendor);
        Task<List<AccountPayableList>> GetAPList(FilterInvoiceReceivable search);
        Task<List<InvoiceReceivableDetail>> GetDetailInvoiceList(string poId, string paymentTo, string invId);
        Task<List<Journal>> GetJournal(string invId);
        Task<int> AddAsync(InvoiceReceivable entity);
        Task<int> EditAsync(InvoiceReceivable entity);
        Task<int> CreateJournalAsync(InvoiceReceivable entity);
        Task<int> CreateJournalAsync(JournalTemp entity);
        Task<int> UpdateJournalAsync(Journal entity);
    }

    public class InvoiceReceivableService : IInvoiceReceivableService
    {
        readonly DataContext context;
        readonly IPurchaseOrderService poService;
        readonly StatusLogService log;
        readonly FileService uploadFile;
        readonly FinancePeriodService financePeriod;
        readonly IPurchaseRequestService prService;

        public InvoiceReceivableService(DataContext context, IPurchaseOrderService poService, StatusLogService log, FileService uploadFile, FinancePeriodService financePeriod, IPurchaseRequestService prService)
        {
            this.context = context;
            this.poService = poService;
            this.log = log;
            this.uploadFile = uploadFile;
            this.financePeriod = financePeriod;
            this.prService = prService;
        }

        public async Task<InvoiceReceivable> Get(string Id)
        {
            var data = await context.InvoiceReceivables.Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
            if (data != null)
            {
                data = await GetFile(data);
                data.PurchaseOrder = await poService.Get(data.PoId);
                data.AccountPayableNumber = context.AccountPayables.Where(x => x.InvoiceReceivableId == data.Id).Select(x => x.AccountPayableNumber).FirstOrDefault();
                data.TransactionDate = context.TemporaryAccountPayables.Where(x => x.InvoiceReceivableId == data.Id).Select(x => x.TransactionDate).FirstOrDefault();
                if (data.PurchaseOrder != null) data.Company = await context.Companys.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == data.PurchaseOrder.CompanyId).FirstOrDefaultAsync();

                var tap = await context.TemporaryAccountPayables.Where(x => x.InvoiceReceivableId == Id).FirstOrDefaultAsync() ?? new TemporaryAccountPayable();
                data.CurrencySymbol = context.Currencies.Where(x => x.Id == data.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault();
                data.CurrencyIdDefault = (context.JournalTemps.Where(x => x.ReferenceId == tap.TapNumber).Select(x => x.DefaultCurrencyId)).FirstOrDefault() ??
                                         context.Currencies.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).Select(x => x.Id).FirstOrDefault();
                data.CurrencySymbolDefault = (from o in context.JournalTemps
                                              join p in context.Currencies on o.DefaultCurrencyId equals p.Id
                                              where o.ReferenceId == tap.TapNumber
                                              select p.CurrencySymbol).FirstOrDefault() ??
                                              context.Currencies.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).Select(x => x.CurrencySymbol).FirstOrDefault();
            }
            return data;
        }

        public async Task<InvoiceReceivable> GetPoById(string poId)
        {
            var data = new InvoiceReceivable();
            var po = await poService.Get(poId);

            if (po != null)
            {

                var totalpayment = 0;
                if ((po.PercentPaymentTerm1 ?? 0) != 0) totalpayment++;
                if ((po.PercentPaymentTerm2 ?? 0) != 0) totalpayment++;
                if ((po.PercentPaymentTerm3 ?? 0) != 0) totalpayment++;

                var paymentTo = context.InvoiceReceivables.Where(y => y.PoId == poId && y.InvoiceStatusId != InvoiceReceivedStatus.Rejected).Count() + 1;
                var paymentPercentage = (paymentTo == 1 ? po.PercentPaymentTerm1 : paymentTo == 2 ? po.PercentPaymentTerm2 : paymentTo == 3 ? po.PercentPaymentTerm3 : 0);

                if (totalpayment < paymentTo) totalpayment = paymentTo;
                var payment = paymentTo.ToString() + " of " + totalpayment.ToString();
                var detail = await GetDetailInvoiceList(poId, payment, null);
                data.NetAmount = detail.Sum(x => x.InvoiceValue);
                data.DiscountPercent = (po.DiscountPercent ?? 0);
                data.DiscountAmount = data.NetAmount * ((po.DiscountPercent ?? 0) / 100);
                data.ValueAddedTaxPercent = po.VatPercent;
                data.ValueAddedTax = data.NetAmount * (data.ValueAddedTaxPercent / 100);
                data.TotalCostVendor = (data.NetAmount + data.ValueAddedTax) - data.DiscountAmount;
                data.PaymentTo = payment;
                data.PurchaseOrder = po;
                data.Company = await context.Companys.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == po.CompanyId).FirstOrDefaultAsync();
            }
            else
            {

                data.PurchaseOrder = new PurchaseOrder();
            }
            return data;
        }

        public async Task<List<InvoiceReceivable>> GetHistoryPayment(string poId)
        {
            return await (from inv in context.InvoiceReceivables
                          where !(inv.IsDeleted ?? false) && inv.PoId == poId && inv.InvoiceStatusId == InvoiceReceivedStatus.Posted
                          select new InvoiceReceivable
                          {
                              Id = inv.Id,
                              InvoiceNumber = inv.InvoiceNumber,
                              InvoiceDate = inv.InvoiceDate,
                              InvoiceVendor = inv.InvoiceVendor,
                              InvoiceStatusId = inv.InvoiceStatusId == null ? "1" : inv.InvoiceStatusId,
                              NetAmount = inv.NetAmount,
                              TotalCostVendor = inv.TotalCostVendor,
                              AccountPayableNumber = context.AccountPayables.Where(x => x.InvoiceReceivableId == inv.Id).Select(x => x.AccountPayableNumber).FirstOrDefault(),
                              CurrencySymbol = context.Currencies.Where(x => x.Id == inv.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault()
                          }).OrderByDescending(x => x.CreatedOn).ToListAsync();
        }

        public async Task<List<InvoiceReceivable>> GetList(string status, string businessUnit, string legalEntity, string invoiceNumber, string invoiceVendor)
        {
            return await (from inv in context.InvoiceReceivables
                          join client in context.Companys on inv.ClientId equals client.Id
                          //join bu in context.BusinessUnits on inv.BusinessUnitId equals bu.Id
                          join lgl in context.LegalEntity on inv.LegalEntityId equals lgl.Id
                          where !(inv.IsDeleted ?? false) &&
                          ((status ?? "0") == "0" ? true : inv.InvoiceStatusId == status) &&
                          ((businessUnit ?? "0") == "0" ? true : inv.BusinessUnitId == businessUnit) &&
                          ((legalEntity ?? "0") == "0" ? true : inv.LegalEntityId == legalEntity) &&
                          ((invoiceNumber ?? "0") == "0" ? true : inv.InvoiceNumber == invoiceNumber) &&
                          ((invoiceVendor ?? "0") == "0" ? true : inv.InvoiceVendor == invoiceVendor)
                          select new InvoiceReceivable
                          {
                              Id = inv.Id,
                              InvoiceNumber = inv.InvoiceNumber,
                              InvoiceDate = inv.InvoiceDate,
                              InvoiceVendor = inv.InvoiceVendor,
                              InvoiceStatusId = inv.InvoiceStatusId == null ? "1" : inv.InvoiceStatusId,
                              ClientName = client.CompanyName,
                              BusinessUnitName = context.BusinessUnits.Where(x => x.Id == inv.BusinessUnitId).Select(x => x.UnitName).FirstOrDefault(),//bu.UnitName,
                              LegalEntityName = lgl.LegalEntityName,
                              ModifiedOn = inv.ModifiedOn
                          }).OrderByDescending(x => x.ModifiedOn).ToListAsync();
        }

        public async Task<List<AccountPayableList>> GetAPList(FilterInvoiceReceivable search)
        {
            var param = new SqlParameter[]
               {
                    new SqlParameter("@Status",(object)search.Status??DBNull.Value),
                    new SqlParameter("@BusinessUnitId",(object)search.BusinessUnitId??DBNull.Value),
                    new SqlParameter("@LegalEntityId",(object)search.LegalEntityId??DBNull.Value),
                    new SqlParameter("@InvoiceReceivableId",(object)search.InvoiceReceivableId??DBNull.Value),
                    new SqlParameter("@AccountPayableNumber",(object)search.AccountPayableNumber??DBNull.Value),
                    new SqlParameter("@VendorId",(object)search.VendorId??DBNull.Value),
                    new SqlParameter("@StartingDate",(object)search.StartingDate??DBNull.Value),
                    new SqlParameter("@EndingDate",(object)search.EndingDate??DBNull.Value),
                    new SqlParameter("@DateType",(object)search.DateType??DBNull.Value),
               };

            var result = await context.ExecuteStoredProcedure<AccountPayableList>("fn.get_ap_list", param);
            return result;

            //return await (from inv in context.InvoiceReceivables
            //              join client in context.Companys on inv.ClientId equals client.Id
            //              //join bu in context.BusinessUnits on inv.BusinessUnitId equals bu.Id
            //              join lgl in context.LegalEntity on inv.LegalEntityId equals lgl.Id
            //              join po in context.PurchaseOrders on inv.PoId equals po.Id
            //              join msc in context.MainServiceCategorys on inv.MainServiceCategoryId equals msc.Id
            //              where !(inv.IsDeleted ?? false) &&
            //              (string.IsNullOrEmpty(search.Status) ? true : inv.InvoiceStatusId == search.Status) &&
            //              (string.IsNullOrEmpty(search.BusinessUnitId) ? true : inv.BusinessUnitId == search.BusinessUnitId) &&
            //              (string.IsNullOrEmpty(search.LegalEntityId) ? true : inv.LegalEntityId == search.LegalEntityId) &&
            //              (string.IsNullOrEmpty(search.InvoiceReceivableId) ? true : inv.InvoiceNumber == search.InvoiceReceivableId) &&
            //              (string.IsNullOrEmpty(search.VendorId) ? true : inv.ClientId == search.VendorId)
            //              select new AccountPayable
            //              {
            //                  Id = inv.Id,
            //                  InvoiceReceivableId = inv.Id,
            //                  AccountPayableNumber = context.AccountPayables.Where(x => x.OurRefId == inv.Id).Select(x => x.AccountPayableNumber).FirstOrDefault(),
            //                  TransactionDate = context.AccountPayables.Where(x => x.OurRefId == inv.Id).Select(x => x.TransactionDate).FirstOrDefault(),
            //                  InvoiceVendorNumber = inv.InvoiceVendor,
            //                  Status = inv.InvoiceStatusId == null ? "1" : inv.InvoiceStatusId,
            //                  ClientName = client.CompanyName,
            //                  BusinessUnitName = context.BusinessUnits.Where(x => x.Id == inv.BusinessUnitId).Select(x => x.UnitName).FirstOrDefault(),//bu.UnitName,
            //                  LegalEntityName = lgl.LegalEntityName,
            //                  PurchaseOrderId = po.PurchaseOrderNumber,
            //                  PaymentRequestNumber = string.IsNullOrEmpty(po.CashAdvanceId) ?
            //                                         context.PaymentRequest.Where(x => x.ReferenceNumberId.Contains(context.AccountPayables.Where(z => z.OurRefId == inv.Id).Select(z => z.AccountPayableNumber).FirstOrDefault())).Select(x => x.PaymentRequestNumber).FirstOrDefault() :
            //                                         (from a in context.PaymentRequest
            //                                          join b in context.CASettlement on a.Id equals b.PaymentRequestId
            //                                          join c in context.CASettlementJournalReverse on b.SettlementNumber equals c.JvReferenceNo
            //                                          where c.IsDraft == false && a.ReferenceNumberId.Contains(po.CashAdvanceId)
            //                                          select c.Code).FirstOrDefault(),
            //                  PaymentRequestStatus = string.IsNullOrEmpty(po.CashAdvanceId) ?
            //                                        context.PaymentRequest.Where(x => x.ReferenceNumberId.Contains(context.AccountPayables.Where(z => z.OurRefId == inv.Id).Select(z => z.AccountPayableNumber).FirstOrDefault())).Select(x => PaymentRequestStatus.GetStatus(x.Status)).FirstOrDefault() :
            //                                        (from a in context.PaymentRequest
            //                                         join b in context.CASettlement on a.Id equals b.PaymentRequestId
            //                                         join c in context.CASettlementJournalReverse on b.SettlementNumber equals c.JvReferenceNo
            //                                         where c.IsDraft == false && a.ReferenceNumberId.Contains(po.CashAdvanceId)
            //                                         select PaymentRequestStatus.GetStatus(a.Status)).FirstOrDefault(),
            //                  ModifiedOn = inv.ModifiedOn,
            //                  MainServiceCategoryName = msc.Name,
            //                  AmountOriginal = 0,
            //                  ExchangeRate = inv.ExchangeRate,
            //                  Amount = 0, //amount exchange
            //                  PaymentRequestAmount = 0
            //              }).Where(x => (string.IsNullOrEmpty(search.AccountPayableNumber) ? true : x.AccountPayableNumber == search.AccountPayableNumber) &&
            //                    (search.DateType == "Day" ? (search.EndingDate == null ? true : x.TransactionDate == search.EndingDate) :
            //                    search.DateType == "Month" ? (search.EndingDate == null ? true : x.TransactionDate.Value.Year == search.EndingDate.Value.Year && x.TransactionDate.Value.Month == search.EndingDate.Value.Month) :
            //                    search.DateType == "Year" ? (search.EndingDate == null ? true : x.TransactionDate.Value.Year == search.EndingDate.Value.Year) :
            //                    search.DateType == "Custom Range" ? (search.StartingDate == null && search.EndingDate == null ? true : x.TransactionDate >= search.StartingDate && x.TransactionDate <= search.EndingDate) : true)
            //                ).OrderByDescending(x => x.ModifiedOn).ToListAsync();
        }

        public async Task<List<InvoiceReceivableDetail>> GetDetailInvoiceList(string poId, string payment, string invId)
        {
            var list = new List<InvoiceReceivableDetail>();
            var isAdd = false;
            if (invId == null) isAdd = true; else isAdd = context.InvoiceReceivableDetails.Where(x => !(x.IsDeleted ?? false) && x.InvoiceReceivableId == invId).Count() == 0;
            if (isAdd)
            {
                var totalInvoice = await (from a in context.InvoiceReceivables
                                          join b in context.InvoiceReceivableDetails on a.Id equals b.InvoiceReceivableId
                                          where a.PoId == poId
                                          select new { b.TaskId, totalInvoice = (b.InvoiceValue ?? 0) }
                                    ).ToListAsync();

                var paymentTo = int.Parse(payment.Split("of").FirstOrDefault().Trim());
                list = await (from a in context.PurchaseOrderDetails
                              join b in context.Uom on a.UomId equals b.Id
                              join c in context.ItemType on a.ItemTypeId equals c.Id
                              join d in context.PurchaseOrders on a.PurchaseOrderId equals d.Id
                              where a.PurchaseOrderId == poId && !(a.IsDeleted ?? false)
                              select new InvoiceReceivableDetail
                              {
                                  Id = a.Id,
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
                                  PaymentTo = paymentTo,
                                  InvoiceValue = (paymentTo == 1 ? a.Amount * (d.PercentPaymentTerm1 / 100) :
                                               paymentTo == 2 ? ((d.PercentPaymentTerm2 ?? 0) == 0 ? (a.Amount - totalInvoice.Where(x => x.TaskId == a.TaskId).Sum(x => x.totalInvoice)) : a.Amount * (d.PercentPaymentTerm2 / 100)) :
                                               paymentTo == 3 ? ((d.PercentPaymentTerm2 ?? 0) == 0 ? (a.Amount - totalInvoice.Where(x => x.TaskId == a.TaskId).Sum(x => x.totalInvoice)) : a.Amount * (d.PercentPaymentTerm3 / 100)) :
                                               (a.Amount - totalInvoice.Where(x => x.TaskId == a.TaskId).Sum(x => x.totalInvoice))),
                                  AmountPaid = 0,
                                  Balance = 0,
                                  //AmountPaid = (a.Amount - totalInvoice.Where(x => x.TaskId == a.TaskId).Sum(x => x.totalInvoice)) +
                                  //             (paymentTo == 1 ? a.Amount * (d.PercentPaymentTerm1 / 100) :
                                  //             paymentTo == 2 ? ((d.PercentPaymentTerm2 ?? 0) == 0 ? (a.Amount - totalInvoice.Where(x => x.TaskId == a.TaskId).Sum(x => x.totalInvoice)) : a.Amount * (d.PercentPaymentTerm2 / 100)) :
                                  //             paymentTo == 3 ? ((d.PercentPaymentTerm2 ?? 0) == 0 ? (a.Amount - totalInvoice.Where(x => x.TaskId == a.TaskId).Sum(x => x.totalInvoice)) : a.Amount * (d.PercentPaymentTerm3 / 100)) :
                                  //             (a.Amount - totalInvoice.Where(x => x.TaskId == a.TaskId).Sum(x => x.totalInvoice))), //amount paid = invoice value + invoice yang sudah di bayarkan sebelumnya //yang sudah di bayar / di proses
                                  //Balance = a.Amount -
                                  //             ((a.Amount - totalInvoice.Where(x => x.TaskId == a.TaskId).Sum(x => x.totalInvoice)) +
                                  //             (paymentTo == 1 ? a.Amount * (d.PercentPaymentTerm1 / 100) :
                                  //             paymentTo == 2 ? ((d.PercentPaymentTerm2 ?? 0) == 0 ? (a.Amount - totalInvoice.Where(x => x.TaskId == a.TaskId).Sum(x => x.totalInvoice)) : a.Amount * (d.PercentPaymentTerm2 / 100)) :
                                  //             paymentTo == 3 ? ((d.PercentPaymentTerm2 ?? 0) == 0 ? (a.Amount - totalInvoice.Where(x => x.TaskId == a.TaskId).Sum(x => x.totalInvoice)) : a.Amount * (d.PercentPaymentTerm3 / 100)) :
                                  //             (a.Amount - totalInvoice.Where(x => x.TaskId == a.TaskId).Sum(x => x.totalInvoice)))),//balance = amount-amount paid  //total yg sudah di bayarkan dan juga yg baru di buatkan invoice.
                                  ItemTypeName = c.TypeName,
                                  UomName = b.unit_symbol,
                                  CurrencySymbol = context.Currencies.Where(x => x.Id == d.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault()
                              }).ToListAsync();
            }
            else
            {
                var model = await context.InvoiceReceivables.Where(x => x.Id == invId).FirstOrDefaultAsync();
                if (model != null)
                {
                    var isAccount = false;
                    if (model.MainServiceCategoryId == MainServiceCategoryOther.MainServiceCategoryId) { isAccount = context.InvoiceReceivableDetails.Where(x => x.InvoiceReceivableId == invId && !string.IsNullOrEmpty(x.AccountId)).Count() > 0; }

                    if (isAccount)
                    {
                        list = await (from a in context.InvoiceReceivableDetails
                                      join b in context.Uom on a.UomId equals b.Id
                                      join c in context.ItemType on a.ItemTypeId equals c.Id
                                      join d in context.ChartOfAccounts on a.AccountId equals d.Id
                                      join e in context.InvoiceReceivables on a.InvoiceReceivableId equals e.Id
                                      where a.InvoiceReceivableId == invId && !(a.IsDeleted ?? false)
                                      select new InvoiceReceivableDetail
                                      {
                                          Id = a.Id,
                                          InvoiceReceivableId = a.InvoiceReceivableId,
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
                                          PaymentTo = a.PaymentTo,
                                          InvoiceValue = a.InvoiceValue,
                                          AmountPaid = a.AmountPaid,
                                          Balance = a.Balance,
                                          AccountId = a.AccountId,
                                          AccountName = d.Name5,
                                          CodeRec = d.CodeRec,
                                          ItemTypeName = c.TypeName,
                                          UomName = b.unit_symbol,
                                          Pph = a.Pph,
                                          PphAmount = a.PphAmount,
                                          CurrencySymbol = context.Currencies.Where(x => x.Id == e.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault()
                                      }).ToListAsync();
                    }
                    else
                    {
                        list = await (from a in context.InvoiceReceivableDetails
                                      join b in context.Uom on a.UomId equals b.Id
                                      join c in context.ItemType on a.ItemTypeId equals c.Id
                                      join e in context.InvoiceReceivables on a.InvoiceReceivableId equals e.Id
                                      where a.InvoiceReceivableId == invId && !(a.IsDeleted ?? false)
                                      select new InvoiceReceivableDetail
                                      {
                                          Id = a.Id,
                                          InvoiceReceivableId = a.InvoiceReceivableId,
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
                                          PaymentTo = a.PaymentTo,
                                          InvoiceValue = a.InvoiceValue,
                                          AmountPaid = a.AmountPaid,
                                          Balance = a.Balance,
                                          AccountId = a.AccountId,
                                          ItemTypeName = c.TypeName,
                                          UomName = b.unit_symbol,
                                          Pph = a.Pph,
                                          PphAmount = a.PphAmount,
                                          CurrencySymbol = context.Currencies.Where(x => x.Id == e.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault()
                                      }).ToListAsync();
                    }
                }
            }
            return list;
        }


        public async Task<List<Journal>> GetJournal(string invId)
        {
            return await (from tb in context.JournalTemps
                          join tap in context.TemporaryAccountPayables on tb.ReferenceId equals tap.Id
                          join coa in context.ChartOfAccounts on tb.AccountId equals coa.Id
                          //join bu in context.BusinessUnits on tb.DivisiId equals bu.Id
                          where tap.InvoiceReceivableId == invId
                          select new Journal
                          {
                              Id = tb.Id,
                              ReferenceId = tb.ReferenceId,
                              AccountId = coa.Id,
                              CodeRec = coa.CodeRec,
                              AccountName = coa.Name5,
                              JobId = tb.JobId,
                              Description = tb.Description,
                              JobIdName = context.JobDetail.Where(x => x.Id == tb.JobId).Select(x => $"{x.Id}-{x.JobName}").FirstOrDefault(),
                              DivisiId = tb.DivisiId,
                              DivisiName = context.BusinessUnits.Where(x => x.Id == tb.DivisiId).Select(x => x.UnitName).FirstOrDefault(),//bu.UnitName,
                              Credit = tb.Credit,
                              Debit = tb.Debit,
                              LineCode = tb.LineCode,
                              LegalId = tb.LegalId,
                              FixedAssetId = tb.FixedAssetId,
                              TaskId = tb.TaskId,
                              ItemTypeId = tb.ItemTypeId,
                              FixedAssetName = context.FixedAsset.Where(x => x.Id == tb.FixedAssetId).Select(x => $"{x.Id} - {x.FixedAssetName}").FirstOrDefault(),
                              CurrencySymbol = context.Currencies.Where(x => x.Id == tb.DefaultCurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault()
                          }).OrderByDescending(x => x.Debit).ThenByDescending(x => x.Credit).ToListAsync();
        }

        public async Task<int> AddAsync(InvoiceReceivable entity)
        {
            return await AddEdit(entity, false, false);
        }

        public async Task<int> EditAsync(InvoiceReceivable entity)
        {
            return await AddEdit(entity, true, false);
        }

        public async Task<int> CreateJournalAsync(InvoiceReceivable entity)
        {
            return await AddEdit(entity, true, true);
        }

        private async Task<int> AddEdit(InvoiceReceivable model, bool isEdit, bool isJournal)
        {
            if (isJournal)
            {
                await financePeriod.IsClosePeriod(Convert.ToDateTime(model.TransactionDate), model.LegalEntityId, Models.Enum.FinancePeriodCode.AP);
            }
            else
            {
                if (model.MainServiceCategoryId == MainServiceCategoryId.Others)
                {
                    var journalCheck = await (from a in context.JournalTemps
                                              join b in context.TemporaryAccountPayables on a.ReferenceId equals b.Id
                                              where b.InvoiceReceivableId == model.Id
                                              group a by new { a.ReferenceId } into g
                                              select new { ReferenceId = g.Key.ReferenceId, Debit = g.Sum(x => x.Debit ?? 0), Credit = g.Sum(x => x.Credit ?? 0) }).ToListAsync();

                    if (journalCheck.Where(x => x.Debit != x.Credit).Count() > 0)
                    {
                        throw new Exception("the journal value is not the same, please recheck the debit and credit values");
                    }
                }
            }

            var entity = await ProcessUpload(model);
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (isEdit)
                    {
                        context.PhoenixEdit(entity);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        var invNumber = await TransID.GetTransId(context, Code.InvoiceReceived, DateTime.Today);
                        entity.InvoiceStatusId = InvoiceReceivedStatus.Open;
                        entity.Id = invNumber;
                        entity.InvoiceNumber = invNumber;
                        await context.PhoenixAddAsync(entity);
                        await context.SaveChangesAsync();
                    }

                    if (entity.InvoiceReceivableDetails != null)
                    {
                        if (!string.IsNullOrEmpty(entity.Id))
                        {
                            var detail = context.InvoiceReceivableDetails.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.InvoiceReceivableId == entity.Id).ToList();
                            if (detail.Count > 0)
                            {
                                context.InvoiceReceivableDetails.RemoveRange(detail);
                            }
                        }


                        var totalInvoice = await (from a in context.InvoiceReceivables
                                                  join b in context.InvoiceReceivableDetails on a.Id equals b.InvoiceReceivableId
                                                  where a.PoId == model.PoId && a.Id != model.Id
                                                  select new { b.TaskId, totalInvoice = (b.InvoiceValue ?? 0) }
                                            ).ToListAsync();

                        foreach (var item in entity.InvoiceReceivableDetails)
                        {
                            item.Id = Guid.NewGuid().ToString();
                            item.InvoiceReceivableId = entity.Id;
                            item.AmountPaid = (item.InvoiceValue + totalInvoice.Where(x => x.TaskId == item.TaskId).Sum(x => x.totalInvoice));//total yg sudah di bayarkan dan juga yg baru di buatkan invoice.
                            item.Balance = item.Amount - item.AmountPaid;  //sisa budget

                            await context.PhoenixAddAsync(item);
                        };


                        if (entity.InvoiceReceivableDetails.Where(x => x.Balance < 0).Count() > 0)
                        {
                            //if (entity.InvoiceReceivableDetails.Where(x => x.InvoiceValue != x.AmountPaid).Count() > 0)
                            //{
                            var taskName = string.Join(", ", entity.InvoiceReceivableDetails.Where(x => x.Balance <= 0).Select(x => x.Task).ToArray());
                            throw new Exception($"invoice value cannot exceed the purchase order value, please check again for detailed invoice list for name '{taskName}'");
                            //}
                        }
                    }

                    //update information payment to
                    if (!string.IsNullOrEmpty(model.PaymentTo))
                    {
                        var invoicePayment = await context.InvoiceReceivables.AsNoTracking().Where(x => x.PoId == model.PoId && x.Id != model.Id).ToListAsync();
                        foreach (var item in invoicePayment)
                        {
                            var paymentTo = int.Parse(item.PaymentTo.Split("of").FirstOrDefault().Trim());
                            var totalPayment = invoicePayment.Count() + 1;
                            item.PaymentTo = $"{paymentTo} of {totalPayment}";
                            context.Update(item);
                        };
                    }

                    var tap = await context.TemporaryAccountPayables.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.InvoiceReceivableId == entity.Id).FirstOrDefaultAsync();
                    //update status TAP jika data tap sudah ada.
                    if (!isJournal && tap != null)
                    {
                        tap.Status = entity.InvoiceStatusId;
                        context.PhoenixEdit(tap);
                    }

                    if (isJournal)
                    {
                        //sumber crud TAP = EXECUTE fn.prc_push_ap_temp;
                        string TAPNumber = "";
                        if (tap == null) TAPNumber = await TransID.GetTransId(context, Code.TAP, DateTime.Today); else TAPNumber = tap.TapNumber;

                        var tmp = await context.VwjournalAp.AsNoTracking()
                            .Where(x => x.InvoiceReceivableId == entity.Id)
                            .Select(x => new TemporaryAccountPayable
                            {
                                Id = TAPNumber,
                                TapNumber = TAPNumber,
                                InvoiceReceivableId = x.InvoiceReceivableId,
                                LegalEntityId = x.LegalEntityId,
                                BusinessUnitId = x.BusinessUnitId,
                                AffiliationId = x.AffiliationId,
                                AccountId = x.CoaId,
                                VendorId = x.VendorId,
                                JobId = x.JobId,
                                TransactionDate = model.TransactionDate,
                                CurrencyId = x.CurrencyId,
                                Amount = x.Credit ?? 0,
                                ExchangeRate = x.ExchangeRate ?? 0,
                                BankAccountDestination = null,
                                Charges = null,
                                Remarks = null,
                                IsJournal = "N",
                                DK = "K",
                                Status = x.InvoiceStatusId,
                                MainServiceCategoryId = x.MainserviceCategoryId,
                                CompanyBrandId = x.CompanyBrandId,
                                TaxNumber = x.TaxNumber,
                                Periode = DateTime.Now,
                                TaxDate = x.TaxDate,
                                InvoiceVendorNumber = x.InvoiceVendor,
                                APType = "1",
                                AllocationRatioId = x.AllocationRatioId,
                                InvoiceReceivedDate = x.InvoiceDate,
                                OurRef = x.InvoiceNumber,
                                OurRefId = x.InvoiceReceivableId,
                                DueDays = 3,
                                PurchaseOrderId = x.PurchaseOrderId,
                                LineCode = x.LineCode
                            }).FirstOrDefaultAsync();

                        if (tmp != null)
                        {
                            tmp.JobName = await context.JobDetail.AsNoTracking().Where(x => x.Id == tmp.JobId).Select(x => x.JobName).FirstOrDefaultAsync();

                            if (tap == null)
                            {
                                await context.PhoenixAddAsync(tmp);
                                await context.SaveChangesAsync();
                            }
                            else
                            {
                                context.PhoenixEdit(tmp);
                                await context.SaveChangesAsync();

                                var detail = context.TemporaryAccountPayableDetail.AsNoTracking().Where(x => x.AccountPayableTemporaryId == tap.Id).ToList();
                                if (detail.Count > 0) context.TemporaryAccountPayableDetail.RemoveRange(detail);
                            }

                            var tmpDetail = await (from a in context.VwJournalApDetail
                                                   where a.InvoiceReceivableId == entity.Id &&
                                                   ((a.Debit ?? 0) != 0 || (a.Credit ?? 0) != 0)
                                                   select new TemporaryAccountPayableDetail
                                                   {
                                                       Id = Guid.NewGuid().ToString(),
                                                       AccountPayableTemporaryId = tmp.Id,
                                                       EvidentNumber = tmp.Id,
                                                       PayableBalance = null,
                                                       AmountPaid = a.DK.ToLower() == "d" ? a.Debit : a.Credit,
                                                       Discount = null,
                                                       Tax = null,
                                                       EndingBalance = null,
                                                       RowIndex = null,
                                                       ItemName = a.ItemName,
                                                       ItemDescription = a.Name5,
                                                       Qty = 1,
                                                       UnitPrice = (a.DK.ToLower() == "d" ? a.Debit : a.Credit) ?? 0,
                                                       Amount = (a.DK.ToLower() == "d" ? a.Debit : a.Credit) ?? 0,
                                                       UomId = a.UomId,
                                                       ItemTypeId = a.ItemTypeId,
                                                       DK = a.DK,
                                                       AccountId = a.CoaId,
                                                       LineCode = a.LineCode,
                                                       TaxPayCoaId = a.TaxPayCoaId,
                                                       TaxPayablePct = a.TaxPayablePct
                                                   }).AsNoTracking().ToListAsync();
                            await context.PhoenixAddRangeAsync(tmpDetail.ToArray());
                            await context.SaveChangesAsync();
                            await context.Database.ExecuteSqlCommandAsync("EXECUTE fn.create_jurnal_temp_ap2 @p_account_payable_temp_id", new SqlParameter("@p_account_payable_temp_id", TAPNumber));
                        }
                    }

                    if (entity.InvoiceStatusId == InvoiceReceivedStatus.Posted)
                    {
                        var po = await context.PurchaseOrders.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == entity.PoId).FirstOrDefaultAsync();
                        if (po != null)
                        {
                            //Melakukan update status close purchase order otomatis jika total nilai invoice = nilai po.
                            var invoice = context.InvoiceReceivables.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.PoId == entity.PoId && x.InvoiceStatusId == InvoiceReceivedStatus.Posted).Sum(x => x.TotalCostVendor);
                            if (invoice >= po.GrandTotal)
                            {
                                po.Status = PurchaseOrderStatus.Closed;
                                context.PurchaseOrders.Update(po);
                            }


                            ////tidak bisa melakukan update budget global karena di invoice received itu bisa request invoice lebih dari 1.
                            //var poDetails = await context.PurchaseOrderDetails.Where(x => !(x.IsDeleted ?? false) && x.PurchaseOrderId == entity.PoId).ToListAsync();
                            ////melakukan update budget pca
                            //if (!string.IsNullOrEmpty(entity.JobId))
                            //{
                            //    foreach (var detail in entity.InvoiceReceivableDetails)
                            //    {
                            //        var detailsPo = poDetails.Where(x => x.TaskId == detail.TaskId).FirstOrDefault();
                            //        if (detailsPo == null) throw new Exception("PO Task cannot be empty");
                            //        var pcaTask = await context.PcaTasks.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.PcaId == entity.JobId && x.Id == detail.TaskId).FirstOrDefaultAsync();
                            //        if (pcaTask == null) throw new Exception("PCA Task cannot be empty");
                            //        pcaTask.TotalPriceUsed = (pcaTask.TotalPriceUsed - detailsPo.TotalExchangeAmount) + (detail.InvoiceValue * entity.ExchangeRate);
                            //        pcaTask.TotalPriceBalance = pcaTask.TotalPrice - pcaTask.TotalPriceUsed;

                            //        //PCA task update Closenya ketika proses job closing
                            //        if (pcaTask.TotalPriceBalance > 0) pcaTask.Status = PCAStatus.Open;
                            //        else pcaTask.Status = PCAStatus.Close;

                            //        context.PhoenixEdit(pcaTask);
                            //    }

                            //    await context.SaveChangesAsync();
                            //    var pca = await context.Pcas.AsNoTracking().Where(x => x.Id == entity.JobId).FirstOrDefaultAsync();
                            //    if (pca != null)
                            //    {
                            //        pca.TotalTaskPriceUsed = context.PcaTasks.Where(x => !(x.IsDeleted ?? false) && x.PcaId == pca.Id).Sum(x => (x.TotalPriceUsed ?? 0));
                            //        pca.TotalTaskPriceBalance = context.PcaTasks.Where(x => !(x.IsDeleted ?? false) && x.PcaId == pca.Id).Sum(x => (x.TotalPriceBalance ?? 0));
                            //        context.Pcas.Update(pca);
                            //    }
                            //}

                            //Melakukan Update Budget Master
                            if (!string.IsNullOrEmpty(po.PurchaseRequestId))
                            {

                                ////tidak bisa melakukan update budget global karena di invoice received itu bisa request invoice lebih dari 1.
                                //foreach (var detail in entity.InvoiceReceivableDetails)
                                //{
                                //    var detailsPo = poDetails.Where(x => x.TaskId == detail.TaskId).FirstOrDefault();
                                //    if (detailsPo == null) throw new Exception("PO Task cannot be empty");
                                //    await prService.UpdateAllBudget(detail.ItemId, true, (detailsPo.TotalExchangeAmount ?? 0), ((detail.InvoiceValue * entity.ExchangeRate) ?? 0));
                                //}

                                await UpdateBudget(entity.Id, (entity.ExchangeRate ?? 1));
                            }
                        }

                        var APNumber = await TransID.GetTransId(context, Code.AP, DateTime.Today);
                        var ap = await context.TemporaryAccountPayables.AsNoTracking()
                            .Where(x => !(x.IsDeleted ?? false) && x.InvoiceReceivableId == entity.Id)
                            .Select(x => new AccountPayable
                            {
                                Id = APNumber,
                                AccountPayableNumber = APNumber,
                                LegalEntityId = x.LegalEntityId,
                                BusinessUnitId = x.BusinessUnitId,
                                AffiliationId = x.AffiliationId,
                                AccountId = x.AccountId,
                                VendorId = x.VendorId,
                                JobId = x.JobId,
                                TransactionDate = x.TransactionDate,
                                CurrencyId = x.CurrencyId,
                                Amount = x.Amount,
                                ExchangeRate = x.ExchangeRate,
                                BankAccountDestination = x.BankAccountDestination,
                                Charges = x.Charges,
                                Remarks = x.Remarks,
                                InvoiceReceivableId = x.InvoiceReceivableId,
                                IsJournal = x.IsJournal,
                                DK = x.DK,
                                Status = entity.InvoiceStatusId,
                                MainserviceCategoryId = x.MainServiceCategoryId,
                                CompanyBrandId = x.CompanyBrandId,
                                TaxNumber = x.TaxNumber,
                                Periode = x.Periode,
                                TaxDate = x.TaxDate,
                                InvoiceVendorNumber = x.InvoiceVendorNumber,
                                //YourRef = x.YourRef,
                                AllocationRatioId = x.AllocationRatioId,
                                InvoiceReceivedDate = x.InvoiceReceivedDate,
                                JobName = x.JobName,
                                OurRef = x.OurRef,
                                OurRefId = x.OurRefId,
                                DueDays = x.DueDays,
                                PurchaseOrderId = x.PurchaseOrderId,
                                LineCode = x.LineCode,
                                TemporaryAccountPayableId = x.Id,
                                IsPayment = context.PurchaseOrders.Any(y => y.Id == x.PurchaseOrderId && !string.IsNullOrEmpty(y.CashAdvanceId)) ? (string.IsNullOrEmpty(model.TaxPayableCoaId) && model.TaxPayable == 0 ? true : false) : false,
                            }).FirstOrDefaultAsync();
                        await context.PhoenixAddAsync(ap);
                        await context.SaveChangesAsync();

                        var apDetail = await (from a in context.TemporaryAccountPayableDetail
                                              join b in context.TemporaryAccountPayables on a.AccountPayableTemporaryId equals b.Id
                                              where !(b.IsDeleted ?? false) && b.InvoiceReceivableId == entity.Id
                                              select new AccountPayableDetail
                                              {
                                                  Id = Guid.NewGuid().ToString(),
                                                  AccountPayableId = ap.Id,
                                                  EvidentNumber = ap.Id,
                                                  PayableBalance = a.PayableBalance,
                                                  AmountPaid = a.AmountPaid,
                                                  Discount = a.Discount,
                                                  Tax = a.Tax,
                                                  EndingBalance = a.EndingBalance,
                                                  RowIndex = a.RowIndex,
                                                  ItemName = a.ItemName,
                                                  ItemDescription = a.ItemDescription,
                                                  Qty = a.Qty,
                                                  UnitPrice = a.UnitPrice,
                                                  Amount = a.Amount,
                                                  UomId = a.UomId,
                                                  ItemTypeId = a.ItemTypeId,
                                                  DK = a.DK,
                                                  AccountId = a.AccountId,
                                                  LineCode = a.LineCode,
                                                  TaxPayCoaId = a.TaxPayCoaId,
                                                  TaxPayablePct = a.TaxPayablePct
                                              }).ToListAsync();

                        await context.PhoenixAddRangeAsync(apDetail.ToArray());


                        var journalTmp = await (from a in context.JournalTemps
                                                join b in context.TemporaryAccountPayables on a.ReferenceId equals b.Id
                                                where b.InvoiceReceivableId == entity.Id && (a.FixedAssetId ?? "") != ""
                                                select a).ToListAsync();

                        foreach (var item in journalTmp)
                        {
                            context.Database.ExecuteSqlCommand($"UPDATE fn.fixed_asset SET coa_id={item.AccountId}, registration_date={item.Referencedate} WHERE id={item.FixedAssetId}");
                        }

                        await context.SaveChangesAsync();
                        await context.Database.ExecuteSqlCommandAsync("EXECUTE fn.create_jurnal_ap2 @p_account_payable_id", new SqlParameter("@p_account_payable_id", APNumber));
                    }
                    if (!isJournal) await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = InvoiceReceivedStatus.StatusName(entity.InvoiceStatusId), Description = entity.Description });
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

        private async Task<InvoiceReceivable> ProcessUpload(InvoiceReceivable entity)
        {
            var filePoObject = await uploadFile.Upload(entity.filePoObject, null);
            if (!string.IsNullOrEmpty(filePoObject))
            {
                entity.PoObject = filePoObject;
            }

            var fileTaxObject = await uploadFile.Upload(entity.fileTaxObject, null);
            if (!string.IsNullOrEmpty(fileTaxObject))
            {
                entity.TaxObject = fileTaxObject;
            }

            var fileContractObject = await uploadFile.Upload(entity.fileContractObject, null);
            if (!string.IsNullOrEmpty(fileContractObject))
            {
                entity.ContractObject = fileContractObject;
            }

            var fileDoObject = await uploadFile.Upload(entity.fileDoObject, null);
            if (!string.IsNullOrEmpty(fileDoObject))
            {
                entity.DoObject = fileDoObject;
            }

            var fileGrObject = await uploadFile.Upload(entity.fileGrObject, null);
            if (!string.IsNullOrEmpty(fileGrObject))
            {
                entity.GrObject = fileGrObject;
            }

            var fileInvObject = await uploadFile.Upload(entity.fileInvObject, null);
            if (!string.IsNullOrEmpty(fileInvObject))
            {
                entity.InvObject = fileInvObject;
            }
            return entity;
        }

        private async Task<InvoiceReceivable> GetFile(InvoiceReceivable data)
        {
            if (data.PoObject != null)
            {
                var PoObject = await context.Filemasters.Where(x => x.Id == data.PoObject).FirstOrDefaultAsync();
                if (PoObject != null) data.filePoObjectName = PoObject.Name;
            }

            if (data.TaxObject != null)
            {
                var TaxObject = await context.Filemasters.Where(x => x.Id == data.TaxObject).FirstOrDefaultAsync();
                if (TaxObject != null) data.fileTaxObjectName = TaxObject.Name;
            }

            if (data.ContractObject != null)
            {
                var ContractObject = await context.Filemasters.Where(x => x.Id == data.ContractObject).FirstOrDefaultAsync();
                if (ContractObject != null) data.fileContractObjectName = ContractObject.Name;
            }

            if (data.DoObject != null)
            {
                var DoObject = await context.Filemasters.Where(x => x.Id == data.DoObject).FirstOrDefaultAsync();
                if (DoObject != null) data.fileDoObjectName = DoObject.Name;
            }

            if (data.GrObject != null)
            {
                var GrObject = await context.Filemasters.Where(x => x.Id == data.GrObject).FirstOrDefaultAsync();
                if (GrObject != null) data.fileGrObjectName = GrObject.Name;
            }

            if (data.InvObject != null)
            {
                var InvObject = await context.Filemasters.Where(x => x.Id == data.InvObject).FirstOrDefaultAsync();
                if (InvObject != null) data.fileInvObjectName = InvObject.Name;
            }
            return data;
        }

        public async Task<bool> UpdateBudget(string invoiceReceivableId, decimal exchangeRate)
        {
            var poDetail = await context.InvoiceReceivableDetails.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.InvoiceReceivableId == invoiceReceivableId && !string.IsNullOrEmpty(x.ItemId)).GroupBy(x => x.ItemId).Select(x => new { x.First().ItemId, total = x.Sum(y => y.InvoiceValue * exchangeRate) }).ToListAsync();
            if (poDetail.Count > 0)
            {
                var budget = await context.BudgetMasters.AsNoTracking().Where(x => poDetail.Any(z => z.ItemId == x.Id)).ToListAsync();
                var budgetDetail = await context.BudgetMasterDetails.AsNoTracking().Where(x => budget.Any(z => z.Id == x.BudgetMasterId)).ToListAsync();

                //update budget Capex or Opex
                foreach (var item in budget)
                {
                    var get = poDetail.FirstOrDefault(x => x.ItemId == item.Id);
                    if (get != null)
                    {
                        item.TotalBudgetUsed += (get.total ?? 0);
                        item.TotalBudgetBalance = item.TotalBudget - item.TotalBudgetUsed;
                        //if (item.TotalBudgetUsed > item.TotalBudget)
                        //{
                        //    throw new Exception("the total budget for this year has exceeded the specified capacity. please check that the input budget cannot exceed the total budget that was set.");
                        //}
                    }
                    context.PhoenixEdit(item);
                };

                //update budget Capex or Opex detail
                //var message = "the total budget for this month has already exceeded the specified capacity. please double check the input budget must not exceed the total budget that has been set.";
                foreach (var item in budgetDetail)
                {
                    var get = poDetail.FirstOrDefault(x => x.ItemId == item.BudgetMasterId);
                    if (get != null)
                    {
                        var month = DateTime.Now.Month;
                        if (month == 1) { item.UsedBudget01 += (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.UsedBudget01 > item.Budget01) throw new Exception(message); }
                        if (month == 2) { item.UsedBudget02 += (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.UsedBudget02 > item.Budget02) throw new Exception(message); }
                        if (month == 3) { item.UsedBudget03 += (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.UsedBudget03 > item.Budget03) throw new Exception(message); }
                        if (month == 4) { item.UsedBudget04 += (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.UsedBudget04 > item.Budget04) throw new Exception(message); }
                        if (month == 5) { item.UsedBudget05 += (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.UsedBudget05 > item.Budget05) throw new Exception(message); }
                        if (month == 6) { item.UsedBudget06 += (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.UsedBudget06 > item.Budget06) throw new Exception(message); }
                        if (month == 7) { item.UsedBudget07 += (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.UsedBudget07 > item.Budget07) throw new Exception(message); }
                        if (month == 8) { item.UsedBudget08 += (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.UsedBudget08 > item.Budget08) throw new Exception(message); }
                        if (month == 9) { item.UsedBudget09 += (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.UsedBudget09 > item.Budget09) throw new Exception(message); }
                        if (month == 10) { item.UsedBudget10 += (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.UsedBudget10 > item.Budget10) throw new Exception(message); }
                        if (month == 11) { item.UsedBudget11 += (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.UsedBudget11 > item.Budget11) throw new Exception(message); }
                        if (month == 12) { item.UsedBudget12 += (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.UsedBudget12 > item.Budget12) throw new Exception(message); }

                        context.PhoenixEdit(item);
                    }
                };
            }
            return true;
        }

        public async Task<int> CreateJournalAsync(JournalTemp entity)
        {
            var data = (from a in context.InvoiceReceivables
                        join b in context.TemporaryAccountPayables on a.Id equals b.InvoiceReceivableId
                        where a.Id == entity.ReferenceId
                        select new
                        {
                            TapId = b.Id,
                            b.TapNumber,
                            b.TransactionDate,
                            a.Remarks,
                            a.CurrencyId,
                            a.ExchangeRate,
                            a.TotalCostVendor,
                            a.PcaId,
                            //PceId
                            a.JobId,
                            //coaId
                            a.PoId,
                            //type_of_expense_id
                            a.MainServiceCategoryId,
                            //shareservices_id
                            a.BusinessUnitId,
                            a.LegalEntityId,
                            a.AffiliationId,
                            //line_code
                            //trx_source
                        }).FirstOrDefault();

            if (data == null)
            {
                throw new Exception("Invoice Receivable Cannot be Empty");
            }

            entity.TransactionDate = data.TransactionDate;
            entity.ReferenceId = data.TapId;
            entity.ReferenceNumber = data.TapNumber;
            entity.Referencedate = data.TransactionDate;
            entity.CurrencyId = data.CurrencyId;
            entity.ExchangeRate = data.ExchangeRate;
            entity.PcaId = data.PcaId;
            entity.JobId = data.JobId;
            entity.MainserviceCategoryId = data.MainServiceCategoryId;
            entity.DivisiId = data.BusinessUnitId;
            entity.LegalId = data.LegalEntityId;
            entity.AffiliateId = data.AffiliationId;
            entity.LineCode = "2";
            entity.TrxSource = "AP";
            entity.Debit = entity.Debit ?? 0;
            entity.Credit = entity.Credit ?? 0;

            var pce = context.Pces.Where(x => x.PcaId == data.PcaId).FirstOrDefault();
            if (pce != null)
            {
                entity.PceId = pce.Id;
                entity.TypeOfExpenseId = pce.TypeOfExpenseId;
                entity.ShareserviceId = pce.ShareservicesId;
            }

            context.PhoenixAdd(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> UpdateJournalAsync(Journal entity)
        {
            context.Database.ExecuteSqlCommand($"UPDATE fn.journal_temp SET account_id={entity.AccountId}, debit={entity.Debit}, Description={entity.Description}, fixed_asset_id={entity.FixedAssetId} WHERE id={entity.Id}");
            //context.Database.ExecuteSqlCommand($"UPDATE fn.Journal SET Description={entity.Description} WHERE reference_id={entity.ReferenceId} AND account_id={entity.AccountId} AND ISNULL(business_unit_id,'')={entity.DivisiId ?? ""} AND ISNULL(legal_id,'')={entity.LegalId ?? ""} AND Debit={entity.Debit} AND Credit={entity.Credit} AND ISNULL(job_id,'')={ entity.JobId ?? ""}");
            //context.Database.ExecuteSqlCommand($"UPDATE fn.journal_temp SET Description={entity.Description} WHERE reference_id={entity.ReferenceId} AND account_id={entity.AccountId} AND ISNULL(business_unit_id,'')={entity.DivisiId ?? ""} AND ISNULL(legal_id,'')={entity.LegalId ?? ""} AND Debit={entity.Debit} AND Credit={entity.Credit}  AND ISNULL(job_id,'')={ entity.JobId ?? ""}");
            return await context.SaveChangesAsync();
        }
    }
}