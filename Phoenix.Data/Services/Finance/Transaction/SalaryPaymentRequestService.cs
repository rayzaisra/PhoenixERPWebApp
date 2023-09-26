using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Enum;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Um;
namespace Phoenix.Data
{
    public interface ISalaryPaymentRequestService
    {
        Task<List<SalaryPaymentRequest>> Get();
        Task<SalaryPaymentRequest> Get(string Id);
        Task<string> GetPayrollId(string legalEntityId, int month, int year);
        Task<List<SalaryPaymentRequestDetail>> GetDetails(string Id, string legalEntityId, int month, int year);
        Task<List<Journal>> GetJournal(string Id);
        Task<int> AddAsync(SalaryPaymentRequest entity);
        Task<int> EditAsync(SalaryPaymentRequest entity);
        Task<int> CreateJournalAsync(SalaryPaymentRequest entity);
        Task<int> Approve(SalaryPaymentRequest entity);
        Task<int> Reject(SalaryPaymentRequest entity);
        Task<int> DeleteSoftAsync(SalaryPaymentRequest entity);
    }

    public class SalaryPaymentRequestService : ISalaryPaymentRequestService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly IManageMenuService menuService;
        readonly StatusLogService log;
        readonly FinancePeriodService financePeriod;
        private readonly IManageTransactionApprovalService trApprovalService;
        private readonly IManageTemplateApprovalService trTemplateApprovalService;

        /// <summary>
        /// And endpoint to manage SalaryPaymentRequest
        /// </summary>
        /// <param name="context">Database context</param>
        public SalaryPaymentRequestService(DataContext context, GlobalFunctionApproval globalFcApproval, IManageMenuService menuService, StatusLogService log, FinancePeriodService financePeriod, IManageTransactionApprovalService trApprovalService, IManageTemplateApprovalService trTemplateApprovalService)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.menuService = menuService;
            this.log = log;
            this.financePeriod = financePeriod;
            this.trApprovalService = trApprovalService;
            this.trTemplateApprovalService = trTemplateApprovalService;
        }

        public async Task<List<SalaryPaymentRequest>> Get()
        {
            var result = await (from a in context.SalaryPaymentRequests
                                join b in context.LegalEntity on a.LegalEntityId equals b.Id
                                join c in context.EmployeeBasicInfos on a.CreatedBy equals c.Id
                                where !(a.IsDeleted ?? false)
                                select new SalaryPaymentRequest
                                {
                                    Id = a.Id,
                                    LegalEntityId = a.LegalEntityId,
                                    CurrencyId = a.CurrencyId,
                                    SalaryPaymentDate = a.SalaryPaymentDate,
                                    ExchangeRate = a.ExchangeRate,
                                    TotalAmount = a.TotalAmount,
                                    Status = a.Status,
                                    CreatedBy = a.CreatedBy,
                                    CreatedOn = a.CreatedOn,
                                    RequestBy = c.NameEmployee,
                                    LegalEntityName = b.LegalEntityName,
                                    CurrencySymbol = context.Currencies.Where(x => x.Id == a.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault()
                                }).OrderByDescending(x => x.CreatedOn).ToListAsync();
            return result;
        }

        public async Task<SalaryPaymentRequest> Get(string Id)
        {
            var result = await context.SalaryPaymentRequests.Where(x => x.Id == Id).FirstOrDefaultAsync();
            if (result != null)
            {
                result.RequestBy = await context.EmployeeBasicInfos.Where(x => x.Id == result.CreatedBy).Select(x => x.NameEmployee).FirstOrDefaultAsync();
            }
            return result;
        }

        public async Task<string> GetPayrollId(string legalEntityId, int month, int year)
        {
            return await (from a in context.EmployeeBasicInfos
                          join f in context.SalaryProcesss on a.Id equals f.EmployeeBasicInfoId
                          join g in context.SalaryProcessHeaders on f.SalaryProcessHeaderId equals g.Id
                          join i in context.TrTemplateApprovals on g.Id equals i.RefId
                          where a.LegalEntityId == legalEntityId && g.IsTmp == false && i.StatusApproved.GetHashCode() == StatusTransaction.Approved &&
                          g.Month == month && g.Year == year
                          group new { g } by new { g.Code } into grp
                          select new
                          {
                              grp.Key.Code
                          }).Select(x => x.Code).FirstOrDefaultAsync();
        }

        public async Task<List<SalaryPaymentRequestDetail>> GetDetails(string Id, string legalEntityId, int month, int year)
        {
            if (string.IsNullOrEmpty(Id) || Id == "0")
            {
                if (string.IsNullOrEmpty(legalEntityId) || (month == 0 && year == 0))
                {
                    return new List<SalaryPaymentRequestDetail>();
                }

                return await (from a in context.EmployeeBasicInfos
                              join b in context.BusinessUnitJobLevels on a.BusinessUnitJobLevelId equals b.Id
                              join c in context.BusinessUnits on b.BusinessUnitDivisionId equals c.Id
                              join d in context.BusinessUnitTypes on c.BusinessUnitTypeId equals d.Id
                              join f in context.SalaryProcessDetailPayroll on a.Id equals f.EmployeeBasicInfoId
                              join g in context.SalaryProcessHeaders on f.SalaryProcessHeaderId equals g.Id
                              join i in context.TrTemplateApprovals on g.Id equals i.RefId
                              where c.Ishris == true && d.BusinessUnitLevel == BusinessUnitLevelCode.Division &&
                              a.LegalEntityId == legalEntityId && g.IsTmp == false && i.StatusApproved.GetHashCode() == StatusTransaction.Approved &&
                              g.Month == month && g.Year == year
                              group new { c, f } by new { c.Id, c.UnitName } into grp
                              select new SalaryPaymentRequestDetail
                              {
                                  DivisionId = grp.Key.Id,
                                  DivisionName = grp.Key.UnitName,
                                  Amount = grp.Sum(x => (x.f.THP ?? 0))
                              }).ToListAsync();
            }
            else
            {
                return await context.SalaryPaymentRequestDetails.Where(x => x.SalaryPaymentRequestId == Id).ToListAsync();
            }
        }

        public async Task<List<Journal>> GetJournal(string Id)
        {
            var result = await (from a in context.JournalTemps
                                join b in context.LegalEntity on a.LegalId equals b.Id
                                join e in context.ChartOfAccounts on a.AccountId equals e.Id
                                join f in context.Currencies on a.DefaultCurrencyId equals f.Id
                                where !(a.IsDeleted ?? false) && a.ReferenceId == Id
                                select new Journal
                                {
                                    TransactionDate = a.TransactionDate,
                                    AccountName = $"{e.CodeRec} - {e.Name5}",
                                    Description = a.Description,
                                    LegalEntityName = b.LegalEntityName,
                                    CurrencyName = f.CurrencyName,
                                    Debit = a.Debit,
                                    Credit = a.Credit
                                }).OrderByDescending(x => x.Debit).ThenByDescending(x => x.Credit).ToListAsync();
            return result;
        }
        public async Task<int> AddAsync(SalaryPaymentRequest entity)
        {
            return await AddEdit(entity, false, false);
        }

        public async Task<int> EditAsync(SalaryPaymentRequest entity)
        {
            return await AddEdit(entity, true, false);
        }

        public async Task<int> CreateJournalAsync(SalaryPaymentRequest entity)
        {
            bool isEdit = false;
            if (!string.IsNullOrEmpty(entity.Id)) isEdit = true;
            return await AddEdit(entity, isEdit, true);
        }

        public async Task<int> Approve(SalaryPaymentRequest model)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var save = await globalFcApproval.UnsafeProcessApproval(model.Id, StatusTransaction.Approved, model.LogDescription, model);
                    var approval = context.TrTemplateApprovals.AsNoTracking().Where(x => x.RefId == model.Id && !(x.IsDeleted ?? false)).FirstOrDefault();
                    if (approval != null)
                    {
                        model.Status = StatusTransactionName.WaitingApproval;

                        if (approval.StatusApproved == Models.Um.StatusApproved.Approved)
                        {
                            model.Status = StatusTransactionName.Approved;
                            await financePeriod.IsClosePeriod(Convert.ToDateTime(model.SalaryPaymentDate), model.LegalEntityId, Models.Enum.FinancePeriodCode.SPR);

                            var createJournal = context.MapCoaTransaksi.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.ModuleCode == SourceDoc.SalaryPaymentRequest)
                             .Select(x => new Journal
                             {
                                 Id = Guid.NewGuid().ToString(),
                                 Description = model.Description,
                                 Debit = x.DK == "d" ? (model.TotalAmount * model.ExchangeRate) : 0,
                                 Credit = x.DK == "k" ? (model.TotalAmount * model.ExchangeRate) : 0,
                                 ReferenceId = model.Id,
                                 ReferenceNumber = model.Id,
                                 AccountId = x.CoaId,
                                 ReferenceDate = model.CreatedOn,
                                 TransactionDate = model.TransactionDate,
                                 CurrencyId = model.CurrencyId,
                                 //Unit = context.Currencies.AsNoTracking().Where(z => z.Id == model.CurrencyId).Select(z => z.CurrencyCode).FirstOrDefault(),
                                 ExchangeRate = model.ExchangeRate,
                                 LegalId = model.LegalEntityId,
                                 //AffiliateId = model.AffiliationId,
                                 //DivisiId = model.BusinessUnitId,
                                 LineCode = "1",
                                 TrxSource = SourceJournal.SalaryPaymentRequest,
                                 //IsJobClosed = "Y",
                                 OriginalAmount = model.TotalAmount,
                                 DefaultCurrencyId = context.Currencies.AsNoTracking().Where(z => z.IsDefault == true).Select(z => z.Id).FirstOrDefault()
                             });

                            await context.PhoenixAddRangeAsync(createJournal.ToArray());

                            //Add Payment Request 
                            var payId = await TransID.GetTransId(context, Code.PaymentRequest, DateTime.Today);
                            PaymentRequest pay = new PaymentRequest()
                            {
                                Id = payId,
                                PaymentRequestNumber = payId,
                                PaymentRequestDate = DateTime.Now,
                                //DueDate = model.CaPeriodTo,
                                //PaymentTypeId = model.CashAdvance,
                                //BankName ="",
                                //BankId ="",
                                //AffilicationId = model.AffiliationId,,
                                //AccountId = null,
                                CurrencyId = model.CurrencyId,
                                ExchangeRate = model.ExchangeRate,
                                LegalEntityId = model.LegalEntityId,
                                //BusinessUnitId = model.BusinessUnitId,
                                Status = PaymentRequestStatus.Open,
                                Charges = 0,
                                Amount = model.TotalAmount,
                                AmountPay = model.TotalAmount,
                                TotalAmountPay = model.TotalAmount,
                                Vat = 0,
                                VatPercent = 0,
                                TaxPayable = 0,
                                TaxPayablePercent = 0,
                                AdvertisingTax = 0,
                                //InvoiceReceivableId="",
                                ReferenceNumberId = model.Id,
                                //PaytoName = model.VendorName,
                                Discount = 0,
                                TotalAmount = model.TotalAmount,
                                //JobId = null
                                //PurchaseRequestId=null,
                                MainServiceCategoryId = MainServiceCategoryOther.MainServiceCategoryId,
                                SourceDoc = SourceDoc.SalaryPaymentRequest,
                                //Recon=null,
                                //TaxPayCoaId=null,
                                //PaytoBank = model.PayeeBankId,
                                PaytoCoaId = context.MapCoaTransaksi.Where(x => !(x.IsDeleted ?? false) && x.ModuleCode == SourceDoc.SalaryPaymentRequest && x.MainserviceCategoryId == MainServiceCategoryOther.MainServiceCategoryId && x.LineCode == "AP").Select(X => X.CoaId).FirstOrDefault(),
                                //BankChargesCoaId=null,
                                Balance = 0,
                                //IsMultiple=null,
                                //Remarks=null,
                                //PairingGroupId=null,
                                IsBalance = false,
                                //PayToPettyCash=null,
                                //PayToAccountNumber = model.BankAccountNumber,
                                PayToType = "3", //Vendor
                                PayToVendor = model.LegalEntityId,
                                //AccountNumber = null,
                                //TaxPayableCoa=null,
                            };
                            await context.PhoenixAddAsync(pay);

                            var payDetail = new PaymentRequestDetail
                            {
                                Id = Guid.NewGuid().ToString(),
                                PaymentRequestId = payId,
                                CurrencyId = model.CurrencyId,
                                Qty = 1,
                                PayUnitPrice = model.TotalAmount,
                                ReferenceId = model.Id,
                                AccountId = context.MapCoaTransaksi.Where(x => !(x.IsDeleted ?? false) && x.ModuleCode == SourceDoc.SalaryPaymentRequest && x.MainserviceCategoryId == MainServiceCategoryOther.MainServiceCategoryId && x.LineCode == "AP").Select(X => X.CoaId).FirstOrDefault(),
                                //JobId = null,
                                //Vat = null,
                                //TaxPayable = null,
                                SouceDoc = SourceDoc.SalaryPaymentRequest,
                                //PairingGroupId = null,
                                //TaxPayableCoaId = null
                            };

                            await context.PhoenixAddAsync(payDetail);

                            context.PhoenixEdit(model);
                            await log.AddAsync(new StatusLog() { TransactionId = model.Id, Status = model.Status, Description = model.LogDescription });
                        }
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

        public async Task<int> Reject(SalaryPaymentRequest model)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var save = await globalFcApproval.UnsafeProcessApproval(model.Id, StatusTransaction.Rejected, model.LogDescription, model);
                    var approval = context.TrTemplateApprovals.AsNoTracking().Where(x => x.RefId == model.Id && !(x.IsDeleted ?? false)).FirstOrDefault();
                    if (approval != null)
                    {
                        model.Status = StatusTransactionName.WaitingApproval;
                        if (approval.StatusApproved == Models.Um.StatusApproved.Rejected)
                        {
                            model.Status = StatusTransactionName.Rejected;
                        }
                        context.PhoenixEdit(model);
                        await log.AddAsync(new StatusLog() { TransactionId = model.Id, Status = model.Status, Description = model.LogDescription });
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

        private async Task<int> AddEdit(SalaryPaymentRequest model, bool isEdit, bool isJournal)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (context.SalaryPaymentRequests.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.LegalEntityId == model.LegalEntityId && x.CurrencyId == model.CurrencyId && x.SalaryPaymentDate.Value.Date == model.SalaryPaymentDate.Value.Date && x.Id != model.Id).Count() > 0)
                    {
                        var currency = await context.Currencies.AsNoTracking().Where(x => x.Id == model.CurrencyId).FirstOrDefaultAsync();
                        var legal = await context.LegalEntity.Where(x => !(x.IsDeleted ?? false) && x.Id == model.LegalEntityId).FirstOrDefaultAsync();
                        throw new Exception($"legal entity: {legal.LegalEntityName}, currency: {currency.CurrencyName}, month & year: {model.SalaryPaymentDate.Value.ToString("MMM yyyy")} already exists");
                    }


                    if (!string.IsNullOrEmpty(model.Id))
                    {
                        var detail = await context.SalaryPaymentRequestDetails.AsNoTracking().Where(x => x.SalaryPaymentRequestId == model.Id).ToListAsync();
                        foreach (var item in detail)
                        {
                            context.SalaryPaymentRequestDetails.Remove(item);
                        }
                    }

                    model.TotalAmount = model.Details.Sum(x => x.Amount);

                    if (isEdit)
                    {
                        context.PhoenixEdit(model);
                    }
                    else
                    {
                        model.Id = await TransID.GetTransId(context, Code.SalaryPaymentRequest, DateTime.Today);
                        await context.PhoenixAddAsync(model);
                        await context.SaveChangesAsync();
                    }

                    if (model.Details != null)
                    {
                        if (model.Details.Count() > 0)
                        {
                            foreach (var item in model.Details)
                            {
                                item.Id = Guid.NewGuid().ToString();
                                item.SalaryPaymentRequestId = model.Id;
                                await context.PhoenixAddAsync(item);
                            }
                        }
                    }

                    if (isJournal == true)
                    {
                        if (!string.IsNullOrEmpty(model.Id))
                        {
                            var journal = context.JournalTemps.AsNoTracking().Where(x => x.ReferenceId == model.Id).ToList();
                            if (journal.Count > 0)
                            {
                                context.JournalTemps.RemoveRange(journal);
                            }
                        }

                        var createJournal = context.MapCoaTransaksi.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.ModuleCode == SourceDoc.SalaryPaymentRequest)
                            .Select(x => new JournalTemp
                            {
                                Id = Guid.NewGuid().ToString(),
                                Description = model.Description,
                                Debit = x.DK == "d" ? (model.TotalAmount * model.ExchangeRate) : 0,
                                Credit = x.DK == "k" ? (model.TotalAmount * model.ExchangeRate) : 0,
                                ReferenceId = model.Id,
                                ReferenceNumber = model.Id,
                                AccountId = x.CoaId,
                                Referencedate = model.CreatedOn,
                                TransactionDate = model.TransactionDate,
                                CurrencyId = model.CurrencyId,
                                //Unit = context.Currencies.AsNoTracking().Where(z => z.Id == model.CurrencyId).Select(z => z.CurrencyCode).FirstOrDefault(),
                                ExchangeRate = model.ExchangeRate,
                                LegalId = model.LegalEntityId,
                                //AffiliateId = model.AffiliationId,
                                //DivisiId = model.BusinessUnitId,
                                LineCode = "1",
                                TrxSource = SourceJournal.SalaryPaymentRequest,
                                //IsJobClosed = "N",
                                OriginalAmount = model.TotalAmount,
                                DefaultCurrencyId = context.Currencies.AsNoTracking().Where(z => z.IsDefault == true).Select(z => z.Id).FirstOrDefault()
                            });

                        await context.PhoenixAddRangeAsync(createJournal.ToArray());
                    }

                    var isApprove = true;
                    if (model.Status != StatusTransactionName.Draft)
                    {
                        var dataMenu = await menuService.GetByUniqueName(MenuUnique.SalaryPaymentRequest).ConfigureAwait(false);
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            MenuId = dataMenu.Id,
                            RefId = model.Id,
                            DetailLink = $"{ApprovalLink.SalaryPaymentRequest}?Id={model.Id}",
                            NotificationType = NotificationTypeCode.SalaryPaymentRequestNotification,
                            Module = NotificationModuleType.ModuleFinance
                        };
                        var subGroupId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(context.HttpContext);
                        var divisionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(context.HttpContext);
                        var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
                        isApprove = await globalFcApproval.Save(vm, subGroupId, divisionId, employeeId);
                        if (isApprove)
                        {
                            await log.AddAsync(new StatusLog() { TransactionId = model.Id, Status = model.Status, Description = model.LogDescription });
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
                        context.SalaryPaymentRequests.Update(model);
                        await log.AddAsync(new StatusLog() { TransactionId = model.Id, Status = model.Status, Description = model.LogDescription });
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

        public async Task<int> DeleteSoftAsync(SalaryPaymentRequest entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }
    }
}