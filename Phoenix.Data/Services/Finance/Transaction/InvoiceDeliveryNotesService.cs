using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Finance.Transaction;
using Phoenix.Data.Models.ViewModel;
using Phoenix.Data.Services.Um;
using Phoenix.Shared.Core.PrincipalHelpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Finance.Transaction
{
    public interface IInvoiceDeliveryNotesService
    {
        Task<List<InvoiceDeliveryNotes>> Get(string status);
        Task<InvoiceDeliveryNotes> GetById(string id);
        Task<List<AccountReceivable>> GetInvoiceChoiceList(string invoiceid, string clientid, string brandid, string divisionid, string legalid);
        Task<List<AccountReceivable>> ReadDetail(string Id);
        Task<int> AddAsync(InvoiceDeliveryNotes entity);
        Task<int> EditAsync(InvoiceDeliveryNotes entity);
    }
    public class InvoiceDeliveryNotesService : IInvoiceDeliveryNotesService
    {
        readonly DataContext context;
        readonly StatusLogService log;
        readonly FileService uploadFile;

        public InvoiceDeliveryNotesService(DataContext context, StatusLogService log, FileService uploadFile)
        {
            this.context = context;
            this.log = log;
            this.uploadFile = uploadFile;
        }

        public async Task<List<InvoiceDeliveryNotes>> Get(string status)
        {
            return await (from a in context.InvoiceDeliveryNotes
                          join b in context.Companys on a.ClientId equals b.Id
                          join c in context.LegalEntity on a.LegalEntityId equals c.Id
                          where string.IsNullOrEmpty(status) ? true : a.Status == status
                          select new InvoiceDeliveryNotes
                          {
                              Id = a.Id,
                              CreatedOn = a.CreatedOn,
                              InvoiceNumber = a.InvoiceNumber,
                              ClientName = b.CompanyName,
                              Status = a.Status,
                              ReceivedBy = a.ReceivedBy,
                              Attention = a.Attention,
                              LegalEntityId = c.LegalEntityName
                          }).ToListAsync();
        }

        public async Task<InvoiceDeliveryNotes> GetById(string id)
        {
            var result = await (from a in context.InvoiceDeliveryNotes
                                join b in context.Companys on a.ClientId equals b.Id
                                where a.Id == id
                                select new InvoiceDeliveryNotes
                                {
                                    Id = a.Id,
                                    InvoiceNumber = a.InvoiceNumber,
                                    ClientId = a.ClientId,
                                    Attention = a.Attention,
                                    ReceivedBy = a.ReceivedBy,
                                    ReceivedOn = a.ReceivedOn,
                                    DeliveredBy = a.DeliveredBy,
                                    DeliveredOn = a.DeliveredOn,
                                    Status = a.Status,
                                    ClientName = b.CompanyName,
                                    LegalEntityId = a.LegalEntityId,
                                    Remarks = a.Remarks,
                                    FileMasterId = a.FileMasterId,
                                    FileMasterName = context.Filemasters.Where(x => x.Id == a.FileMasterId).Select(x => x.Name).FirstOrDefault(),
                                    CreatedByName = context.EmployeeBasicInfos.Where(x => x.Id == a.CreatedBy).Select(x => x.NameEmployee).FirstOrDefault(),
                                    CreatedBy = a.CreatedBy,
                                    CreatedOn = a.CreatedOn,
                                    ModifiedBy = a.ModifiedBy,
                                    ModifiedOn = a.ModifiedOn
                                }).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<AccountReceivable>> GetInvoiceChoiceList(string invoiceid, string clientid, string brandid, string divisionid, string legalid)
        {
            var invoice = context.InvoiceClients.Where(x => string.IsNullOrEmpty(x.MultiPceId)).Select(x => new { x.Id, x.Vat, TotalAmount = (x.TotalAmount ?? 0) }).Distinct()
                .Union(context.InvoiceClients.Where(x => !string.IsNullOrEmpty(x.MultiPceId)).GroupBy(x => new { x.MultiPceId, x.Vat }).Select(x => new { Id = x.Key.MultiPceId, x.Key.Vat, TotalAmount = x.Sum(z => z.GrandTotal ?? 0) }).Distinct());

            var result = await (from a in context.AccountReceivables
                                join b in context.Currencies on a.CurrencyId equals b.Id
                                //join c in context.CompanyBrands on a.BrandId equals c.Id
                                join d in context.Companys on a.ClientId equals d.Id
                                join e in context.LegalEntity on a.LegalEntityId equals e.Id
                                join f in invoice on a.AccountReceivableNumber equals f.Id
                                where (invoiceid == "0" ? true : a.Id.Contains(invoiceid)) &&
                                      (clientid == "0" ? true : a.ClientId == clientid) &&
                                      (brandid == "0" ? true : (a.BrandId ?? "").Contains(brandid)) &&
                                      (divisionid == "0" ? true : a.BusinessUnitId == divisionid) &&
                                      (legalid == "0" ? true : a.LegalEntityId == legalid) &&
                                      !context.InvoiceDeliveryNotes.Any(x => x.InvoiceNumber.Contains(a.AccountReceivableNumber))
                                select new AccountReceivable
                                {
                                    Id = a.Id,
                                    AccountReceivableNumber = a.AccountReceivableNumber,
                                    TransactionDate = a.TransactionDate,
                                    VatNo = a.VatNo,
                                    CurrencyCode = b.CurrencyCode,
                                    Amount = a.Amount,
                                    ClientId = a.ClientId,
                                    ClientName = d.CompanyName,
                                    ClientBrand = string.Join(',', context.CompanyBrands.Where(x => a.BrandId.Contains(x.Id)).Select(x => x.BrandName).ToArray()),
                                    LegalEntityId = a.LegalEntityId,
                                    LegalEntity = e.LegalEntityName,
                                    BusinessUnitName = context.BusinessUnits.Where(x => x.Id == a.BusinessUnitId).Select(x => x.UnitName).FirstOrDefault(),
                                    DueDate = a.DueDate,
                                    TotalAmount = f.TotalAmount,
                                    VATAmount = f.Vat,
                                    CurrencyName = b.CurrencyName,
                                    CurrencySymbol = b.CurrencySymbol
                                }).ToListAsync();
            return result;
        }

        public async Task<List<AccountReceivable>> ReadDetail(string Id)
        {
            var result = await (from a in context.AccountReceivables
                                join b in context.Currencies on a.CurrencyId equals b.Id
                                //join c in context.CompanyBrands on a.BrandId equals c.Id
                                join d in context.Companys on a.ClientId equals d.Id
                                join e in context.LegalEntity on a.LegalEntityId equals e.Id
                                where context.InvoiceDeliveryNotes.Any(x => x.InvoiceNumber.Contains(a.AccountReceivableNumber) && x.Id == Id)
                                select new AccountReceivable
                                {
                                    Id = a.Id,
                                    AccountReceivableNumber = a.AccountReceivableNumber,
                                    TransactionDate = a.TransactionDate,
                                    VatNo = a.VatNo,
                                    CurrencyCode = b.CurrencyCode,
                                    Amount = a.Amount,
                                    ClientId = a.ClientId,
                                    ClientName = d.CompanyName,
                                    ClientBrand = string.Join(',', context.CompanyBrands.Where(x => a.BrandId.Contains(x.Id)).Select(x => x.BrandName).ToArray()),
                                    LegalEntityId = a.LegalEntityId,
                                    LegalEntity = e.LegalEntityName,
                                    BusinessUnitName = context.BusinessUnits.Where(x => x.Id == a.BusinessUnitId).Select(x => x.UnitName).FirstOrDefault(),
                                    DueDate = a.DueDate,
                                    CurrencyName = b.CurrencyName,
                                    CurrencySymbol = b.CurrencySymbol
                                }).ToListAsync();
            return result;
        }

        public async Task<int> AddAsync(InvoiceDeliveryNotes entity)
        {
            return await AddEdit(entity, false);
        }

        public async Task<int> EditAsync(InvoiceDeliveryNotes entity)
        {
            return await AddEdit(entity, true);
        }

        private async Task<int> AddEdit(InvoiceDeliveryNotes entity, bool isEdit)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var file1 = await uploadFile.Upload(entity.DocFile, null);
                    if (!string.IsNullOrEmpty(file1))
                    {
                        entity.FileMasterId = file1;
                    }

                    string invId = "";
                    foreach (var i in entity.listAR)
                    {
                        invId += i.AccountReceivableNumber + ";";
                        //todo update AR
                        var model = await context.AccountReceivables.AsNoTracking().Where(x => x.Id == i.Id).FirstOrDefaultAsync();
                        model.InvoiceDeliveryDate = entity.DeliveredOn;
                        model.InvoiceDeliveryNotesId = entity.Id;
                        context.PhoenixEdit(model);
                    }
                    invId = invId.Remove(invId.Length - 1);
                    entity.InvoiceNumber = invId;

                    if (isEdit)
                    {
                        entity.Status = InvoiceDeliveryNoteStatus.Closed;
                        context.PhoenixEdit(entity);
                    }
                    else
                    {
                        entity.Id = await TransID.GetTransId(context, Code.InvoiceDeliveryNotes, DateTime.Today);
                        entity.Status = InvoiceDeliveryNoteStatus.Open;
                        context.PhoenixAdd(entity);
                    }

                    //context.PhoenixEdit(entity);
                    await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = entity.Status, Description = entity.Remarks });
                    var save = await context.SaveChangesAsync();
                    transaction.Commit();
                    return save;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message.ToString());
                }
            }
        }
    }
}
