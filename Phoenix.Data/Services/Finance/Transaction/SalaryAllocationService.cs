using System;
using System.Collections.Generic;
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
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface ISalaryAllocationService : IDataService<SalaryAllocation>
    {
        Task<SalaryAllocation> GetByNumber(string type, string number);
        Task<List<SalaryAllocation>> GetList(SalaryAllocation model);
        Task<List<SalaryAllocationDetail>> GetDetailJournal(string Id);
        Task<List<Journal>> GetJournal(string Id);
        Task<List<Journal>> JournalHistoryList(string referenceId);
        Task<int> EditJournal(SalaryAllocation entity);
        Task<int> CreateJournal(SalaryAllocation entity);
        Task<int> ApproveAsync(SalaryAllocation entity);
    }

    public class SalaryAllocationService : ISalaryAllocationService
    {
        readonly DataContext context;
        readonly FinancePeriodService financePeriod;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly IManageMenuService menuService;
        readonly StatusLogService log;

        /// <summary>
        /// And endpoint to manage SalaryAllocation
        /// </summary>
        /// <param name="context">Database context</param>
        public SalaryAllocationService(DataContext context, FinancePeriodService financePeriod, GlobalFunctionApproval globalFcApproval, IManageMenuService menuService, StatusLogService log)
        {
            this.context = context;
            this.financePeriod = financePeriod;
            this.globalFcApproval = globalFcApproval;
            this.menuService = menuService;
            this.log = log;
        }

        public async Task<int> AddAsync(SalaryAllocation entity)
        {
            return await AddEdit(entity, false, false);
        }

        public Task<int> AddRangeAsync(params SalaryAllocation[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(SalaryAllocation entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params SalaryAllocation[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(SalaryAllocation entity)
        {
            context.PhoenixDelete(entity);

            var journalDetailList = await context.SalaryAllocationDetails.AsNoTracking().Where(x => x.SalaryAllocationId == entity.Id).ToListAsync();
            context.PhoenixDeleteRange(journalDetailList.ToArray());

            var journalList = await context.JournalTemps.AsNoTracking().Where(x => x.ReferenceId == entity.Id).ToListAsync();
            context.PhoenixDeleteRange(journalList.ToArray());
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params SalaryAllocation[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(SalaryAllocation entity)
        {
            return await AddEdit(entity, true, false);
        }

        public Task<int> EditRangeAsync(SalaryAllocation entity)
        {
            throw new NotImplementedException();
        }

        public async Task<int> CreateJournal(SalaryAllocation entity)
        {
            return await AddEdit(entity, false, true);
        }

        public async Task<int> EditJournal(SalaryAllocation entity)
        {
            return await AddEdit(entity, true, true);
        }

        private async Task<int> AddEdit(SalaryAllocation entity, bool isEdit, bool isJournal)
        {
            if (isJournal)
            {
                await financePeriod.IsClosePeriod(Convert.ToDateTime(entity.TransactionDate), entity.LegalEntityId, Models.Enum.FinancePeriodCode.SA);
            }

            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    entity.Status = StatusTransaction.StatusName(entity.StatusApproval);

                    var period = context.FinancePeriods.AsNoTracking().Where(x => entity.TransactionDate >= x.StartDate && entity.TransactionDate <= x.EndDate).FirstOrDefault();
                    if (period != null)
                    {
                        entity.FinancePeriodId = period.Id;
                        entity.FinancialYearId = period.FiscalYearId;
                    }

                    if (isEdit)
                    {
                        context.PhoenixEdit(entity);
                    }
                    else
                    {
                        entity.Id = await TransID.GetTransId(context, Code.SalaryAllocation, DateTime.Today);
                        entity.Code = await TransID.GetTransId(context, Code.JournalVoucher, DateTime.Today);

                        await context.PhoenixAddAsync(entity);
                        await context.SaveChangesAsync();
                    }

                    if (!string.IsNullOrEmpty(entity.Id))
                    {
                        var detail = context.SalaryAllocationDetails.AsNoTracking().Where(x => x.SalaryAllocationId == entity.Id).ToList();
                        if (detail.Count > 0)
                        {
                            context.SalaryAllocationDetails.RemoveRange(detail);
                        }
                    }

                    if (entity.SalaryAllocationDetails != null)
                    {
                        foreach (var item in entity.SalaryAllocationDetails)
                        {
                            item.SalaryAllocationId = entity.Id;
                            await context.PhoenixAddAsync(item);
                        }
                    }

                    //Create Journal
                    if (isJournal == true)
                    {
                        if (!string.IsNullOrEmpty(entity.Id))
                        {
                            var journal = context.JournalTemps.AsNoTracking().Where(x => x.ReferenceId == entity.Id).ToList();
                            if (journal.Count > 0)
                            {
                                context.JournalTemps.RemoveRange(journal);
                            }
                        }

                        var createJournal = entity.SalaryAllocationDetails
                            .Select(x => new JournalTemp
                            {
                                Id = Guid.NewGuid().ToString(),
                                Description = entity.Description,
                                Debit = x.Debit * entity.ExchangeRate,
                                Credit = x.Credit * entity.ExchangeRate,
                                ReferenceId = entity.Id,
                                ReferenceNumber = entity.Code,
                                AccountId = x.CoaId,
                                Referencedate = entity.TransactionDate,
                                TransactionDate = entity.TransactionDate,
                                CurrencyId = entity.CurrencyId,
                                //Unit = context.Currencies.AsNoTracking().Where(z => z.Id == entity.CurrencyId).Select(z => z.CurrencyCode).FirstOrDefault(),
                                ExchangeRate = entity.ExchangeRate,
                                LegalId = entity.LegalEntityId,
                                AffiliateId = entity.AffiliationId,
                                DivisiId = entity.BusinessUnitId,
                                MainserviceCategoryId = entity.MainServiceCategoryId,
                                LineCode = "1",
                                PcaId = entity.PcaId,
                                PceId = entity.PceId,
                                JobId = entity.JobId,
                                TrxSource = SourceJournal.SalaryAllocation,
                                //IsJobClosed = "N",
                                OriginalAmount = (x.Debit ?? 0) != 0 ? x.Debit : x.Credit,
                                DefaultCurrencyId = context.Currencies.AsNoTracking().Where(z => z.IsDefault == true).Select(z => z.Id).FirstOrDefault()
                            });

                        await context.PhoenixAddRangeAsync(createJournal.ToArray());
                    }

                    //var save = await context.SaveChangesAsync();
                    //transaction.Commit();
                    //return save;

                    var isApprove = true;
                    if (entity.Status != StatusTransactionName.Draft)
                    {
                        var dataMenu = await menuService.GetByUniqueName(MenuUnique.SalaryAllocation).ConfigureAwait(false);
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            MenuId = dataMenu.Id,
                            RefId = entity.Id,
                            DetailLink = $"{ApprovalLink.SalaryAllocation}?Id={entity.Id}",
                            NotificationType = NotificationTypeCode.SalaryAllocationNotification,
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
                        context.SalaryAllocations.Update(entity);
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


        public async Task<int> ApproveAsync(SalaryAllocation entity)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var save = await globalFcApproval.UnsafeProcessApproval(entity.Id, entity.StatusApproval, entity.RemarksProcess, entity);
                    var approval = context.TrTemplateApprovals.Where(x => x.RefId == entity.Id && !(x.IsDeleted ?? false)).FirstOrDefault();
                    if (approval != null)
                    {
                        entity.Status = StatusTransaction.StatusName(approval.StatusApproved.GetHashCode());
                        if (approval.StatusApproved == Models.Um.StatusApproved.Approved)
                        {
                            var createJournal = context.SalaryAllocationDetails.AsNoTracking().Where(x => x.SalaryAllocationId == entity.Id)
                                .Select(x => new Journal
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    Description = entity.Description,
                                    Debit = x.Debit * entity.ExchangeRate,
                                    Credit = x.Credit * entity.ExchangeRate,
                                    ReferenceId = entity.Id,
                                    ReferenceNumber = entity.Code,
                                    AccountId = x.CoaId,
                                    ReferenceDate = entity.TransactionDate,
                                    TransactionDate = entity.TransactionDate,
                                    CurrencyId = entity.CurrencyId,
                                    //Unit = context.Currencies.AsNoTracking().Where(z => z.Id == entity.CurrencyId).Select(z => z.CurrencyCode).FirstOrDefault(),
                                    ExchangeRate = entity.ExchangeRate,
                                    LegalId = entity.LegalEntityId,
                                    AffiliateId = entity.AffiliationId,
                                    DivisiId = entity.BusinessUnitId,
                                    MainserviceCategoryId = entity.MainServiceCategoryId,
                                    LineCode = "1",
                                    PcaId = entity.PcaId,
                                    PceId = entity.PceId,
                                    JobId = entity.JobId,
                                    TrxSource = SourceJournal.SalaryAllocation,
                                    //IsJobClosed = "Y",
                                    OriginalAmount = (x.Debit ?? 0) != 0 ? x.Debit : x.Credit,
                                    DefaultCurrencyId = context.Currencies.AsNoTracking().Where(z => z.IsDefault == true).Select(z => z.Id).FirstOrDefault()
                                });

                            await context.PhoenixAddRangeAsync(createJournal.ToArray());
                        }
                    }
                    else
                    {
                        entity.Status = StatusTransactionName.Draft;
                    }
                    context.SalaryAllocations.Update(entity);

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

        public async Task<List<SalaryAllocation>> GetList(SalaryAllocation model)
        {
            var result = await (from a in context.SalaryAllocations
                                join b in context.LegalEntity on a.LegalEntityId equals b.Id
                                join c in context.Affiliations on a.AffiliationId equals c.Id
                                join d in context.BusinessUnits on a.BusinessUnitId equals d.Id
                                where !(a.IsDeleted ?? false) &&
                                (string.IsNullOrEmpty(model.LegalEntityId) ? true : a.LegalEntityId == model.LegalEntityId) &&
                                ((model.TransactionDate != null && model.TransactionDateEnd != null) ? (a.TransactionDate >= model.TransactionDate && a.TransactionDate <= model.TransactionDateEnd) : true) &&
                                (string.IsNullOrEmpty(model.Code) ? true : a.Code == model.Code) &&
                                (string.IsNullOrEmpty(model.CurrencyId) ? true : a.CurrencyId == model.CurrencyId) &&
                                ((model.ExchangeRate == null) ? true : a.ExchangeRate == model.ExchangeRate) &&
                                (string.IsNullOrEmpty(model.Description) ? true : a.Description == model.Description) &&
                                (string.IsNullOrEmpty(model.JobId) ? true : a.JobId == model.JobId)
                                select new SalaryAllocation
                                {
                                    Id = a.Id,
                                    TransactionDate = a.TransactionDate,
                                    Code = a.Code,
                                    //ReferenceNumber = a.ReferenceNumber,
                                    Description = a.Description,
                                    LegalEntityName = b.LegalEntityName,
                                    BusinessUnitName = d.UnitName,
                                    AffiliationName = c.AffiliationName,
                                    PcaId = a.PcaId,
                                    PceId = a.PceId,
                                    JobId = a.JobId,
                                    Status = a.Status,
                                    CreatedOn = a.CreatedOn
                                }).OrderByDescending(x => x.CreatedOn).ToListAsync();
            return result;
        }

        public Task<List<SalaryAllocation>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<SalaryAllocation> Get(string Id)
        {
            var data = await context.SalaryAllocations.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
            var result = await GetByNumber(data.Type, data.ReferenceId) ?? new SalaryAllocation();
            if (data != null)
            {
                result.Id = data.Id;
                result.Code = data.Code;
                result.TransactionDate = data.TransactionDate;
                result.Description = data.Description;
                result.CurrencyId = data.CurrencyId;
                result.ExchangeRate = data.ExchangeRate;
                result.FinancialYearId = data.FinancialYearId;
                result.FinancePeriodId = data.FinancePeriodId;
                result.Status = data.Status;
                result.CreatedBy = data.CreatedBy;
                result.CreatedOn = data.CreatedOn;
                result.ModifiedBy = data.ModifiedBy;
                result.ModifiedOn = data.ModifiedOn;
            }
            return result;
        }

        public async Task<SalaryAllocation> GetByNumber(string type, string number)
        {
            var result = new SalaryAllocation();
            if (type == SalaryAllocationType.AccountPayable)
            {
                result = await (from ap in context.AccountPayables
                                join inv in context.InvoiceReceivables on ap.InvoiceReceivableId equals inv.Id
                                join client in context.Companys on inv.ClientId equals client.Id
                                join lg in context.LegalEntity on ap.LegalEntityId equals lg.Id
                                //join bu in context.BusinessUnits on ap.BusinessUnitId equals bu.Id
                                join mc in context.MainServiceCategorys on ap.MainserviceCategoryId equals mc.Id
                                //join af in context.Affiliations on ap.AffiliationId equals af.Id
                                where !(ap.IsDeleted ?? false) && ap.AccountPayableNumber == number
                                select new SalaryAllocation
                                {
                                    //Id = null,
                                    //Code = null,
                                    //TransactionDate = null,
                                    Type = SalaryAllocationType.AccountPayable,
                                    CurrencyId = inv.CurrencyId,
                                    ExchangeRate = inv.ExchangeRate,
                                    ReferenceId = ap.Id,
                                    ReferenceNumber = ap.AccountPayableNumber,
                                    //Description = null,
                                    LegalEntityId = inv.LegalEntityId,
                                    BusinessUnitId = inv.BusinessUnitId,
                                    AffiliationId = inv.AffiliationId,
                                    MainServiceCategoryId = inv.MainServiceCategoryId,
                                    JobId = inv.JobId,
                                    PceId = context.Pces.Where(x => x.PcaId == inv.PcaId).Select(x => x.Id).FirstOrDefault(),
                                    PcaId = inv.PcaId,
                                    ClientId = inv.ClientId,
                                    //FinancialYearId = null,
                                    //FinancePeriodId = null,
                                    //Status = null,
                                    LegalEntityName = lg.LegalEntityName,
                                    BusinessUnitName = context.BusinessUnits.Where(x => x.Id == ap.BusinessUnitId).Select(x => x.UnitName).FirstOrDefault(),
                                    AffiliationName = context.Affiliations.Where(x => x.Id == ap.AffiliationId).Select(x => x.AffiliationName).FirstOrDefault(),
                                    ClientName = client.CompanyName,
                                    MainServiceCategoryName = mc.Name,
                                    ViewFiscalYear = context.FinancialYears.Where(x => !(x.IsDeleted ?? false) && x.StartDate.Year == ap.TransactionDate.Value.Year && x.LegalEntityId == inv.LegalEntityId).Select(x => x.FinancialYearName).FirstOrDefault(),
                                    ViewPeriod = context.FinancePeriods.Where(x => !(x.IsDeleted ?? false) && (ap.TransactionDate >= x.StartDate && ap.TransactionDate <= x.EndDate) && x.LegalEntityId == inv.LegalEntityId).Select(x => x.FinancePeriodName).FirstOrDefault(),
                                    ViewDateTransaction = ap.TransactionDate
                                }).FirstOrDefaultAsync();
            }
            else if (type == SalaryAllocationType.BankPayment)
            {
                //Bank Payment sumber dari Cash Advance
                result = await (from ca in context.CashAdvances
                                join pr in context.PaymentRequest on ca.CaNumber equals pr.ReferenceNumberId
                                join lg in context.LegalEntity on ca.LegalEntityId equals lg.Id
                                //join bu in context.BusinessUnits on ca.BusinessUnitId equals bu.Id
                                join mc in context.MainServiceCategorys on ca.MainserviceCategoryId equals mc.Id
                                //join af in context.Affiliations on ca.AffiliationId equals af.Id
                                where !(ca.IsDeleted ?? false) && pr.PaymentRequestNumber == number && pr.Status == PaymentRequestStatus.Complete
                                select new SalaryAllocation
                                {
                                    //Id = null,
                                    //Code = null,
                                    //TransactionDate = null,
                                    Type = SalaryAllocationType.BankPayment,
                                    CurrencyId = pr.CurrencyId,
                                    //ExchangeRate = null,
                                    ReferenceId = pr.Id,
                                    ReferenceNumber = pr.PaymentRequestNumber,
                                    //Description = null,
                                    LegalEntityId = ca.LegalEntityId,
                                    BusinessUnitId = ca.BusinessUnitId,
                                    AffiliationId = ca.AffiliationId,
                                    MainServiceCategoryId = ca.MainserviceCategoryId,
                                    JobId = ca.JobId,
                                    PceId = ca.PceId,
                                    PcaId = ca.PcaId,
                                    ClientId = ca.ClientId,
                                    //FinancialYearId = null,
                                    //FinancePeriodId = null,
                                    //Status = null,
                                    LegalEntityName = lg.LegalEntityName,
                                    BusinessUnitName = context.BusinessUnits.Where(x => x.Id == ca.BusinessUnitId).Select(x => x.UnitName).FirstOrDefault(),
                                    AffiliationName = context.Affiliations.Where(x => x.Id == ca.AffiliationId).Select(x => x.AffiliationName).FirstOrDefault(),
                                    ClientName = ca.ClientName,
                                    MainServiceCategoryName = mc.Name,
                                    ViewFiscalYear = context.FinancialYears.Where(x => !(x.IsDeleted ?? false) && x.StartDate.Year == pr.PaymentRequestDate.Value.Year && x.LegalEntityId == ca.LegalEntityId).Select(x => x.FinancialYearName).FirstOrDefault(),
                                    ViewPeriod = context.FinancePeriods.Where(x => !(x.IsDeleted ?? false) && (pr.PaymentRequestDate >= x.StartDate && pr.PaymentRequestDate <= x.EndDate) && x.LegalEntityId == ca.LegalEntityId).Select(x => x.FinancePeriodName).FirstOrDefault(),
                                    ViewDateTransaction = pr.PaymentRequestDate
                                }).FirstOrDefaultAsync();

                if (result == null)
                {
                    //Bank Payment sumber dari Account Payable
                    result = await (from ap in context.AccountPayables
                                    join inv in context.InvoiceReceivables on ap.InvoiceReceivableId equals inv.Id
                                    join pr in context.PaymentRequest on ap.AccountPayableNumber equals pr.ReferenceNumberId
                                    join client in context.Companys on inv.ClientId equals client.Id
                                    join lg in context.LegalEntity on ap.LegalEntityId equals lg.Id
                                    //join bu in context.BusinessUnits on ap.BusinessUnitId equals bu.Id
                                    join mc in context.MainServiceCategorys on ap.MainserviceCategoryId equals mc.Id
                                    //join af in context.Affiliations on ap.AffiliationId equals af.Id
                                    where !(pr.IsDeleted ?? false) && pr.PaymentRequestNumber == number && pr.Status == PaymentRequestStatus.Complete
                                    select new SalaryAllocation
                                    {
                                        //Id = null,
                                        //Code = null,
                                        //TransactionDate = null,
                                        Type = SalaryAllocationType.BankPayment,
                                        CurrencyId = pr.CurrencyId,
                                        ExchangeRate = inv.ExchangeRate,
                                        ReferenceId = pr.Id,
                                        ReferenceNumber = pr.PaymentRequestNumber,
                                        //Description = null,
                                        LegalEntityId = inv.LegalEntityId,
                                        BusinessUnitId = inv.BusinessUnitId,
                                        AffiliationId = inv.AffiliationId,
                                        MainServiceCategoryId = inv.MainServiceCategoryId,
                                        JobId = inv.JobId,
                                        PceId = context.Pces.Where(x => x.PcaId == inv.PcaId).Select(x => x.Id).FirstOrDefault(),
                                        PcaId = inv.PcaId,
                                        ClientId = inv.ClientId,
                                        //FinancialYearId = null,
                                        //FinancePeriodId = null,
                                        //Status = null,
                                        LegalEntityName = lg.LegalEntityName,
                                        BusinessUnitName = context.BusinessUnits.Where(x => x.Id == ap.BusinessUnitId).Select(x => x.UnitName).FirstOrDefault(),
                                        AffiliationName = context.Affiliations.Where(x => x.Id == ap.AffiliationId).Select(x => x.AffiliationName).FirstOrDefault(),
                                        ClientName = client.CompanyName,
                                        MainServiceCategoryName = mc.Name,
                                        ViewFiscalYear = context.FinancialYears.Where(x => !(x.IsDeleted ?? false) && x.StartDate.Year == pr.PaymentRequestDate.Value.Year && x.LegalEntityId == inv.LegalEntityId).Select(x => x.FinancialYearName).FirstOrDefault(),
                                        ViewPeriod = context.FinancePeriods.Where(x => !(x.IsDeleted ?? false) && (pr.PaymentRequestDate >= x.StartDate && pr.PaymentRequestDate <= x.EndDate) && x.LegalEntityId == inv.LegalEntityId).Select(x => x.FinancePeriodName).FirstOrDefault(),
                                        ViewDateTransaction = pr.PaymentRequestDate
                                    }).FirstOrDefaultAsync();
                }

                if (result == null)
                {
                    //Bank Payment sumber dari Petty Cash
                    result = await (from ca in context.PettyCashs
                                    join pr in context.PaymentRequest on ca.Id equals pr.ReferenceNumberId
                                    join lg in context.LegalEntity on ca.LegalEntityId equals lg.Id
                                    //join bu in context.BusinessUnits on ca.BusinessUnitId equals bu.Id
                                    //join mc in context.MainServiceCategorys on ca.MainserviceCategoryId equals mc.Id
                                    //join af in context.Affiliations on ca.AffiliationId equals af.Id
                                    where !(ca.IsDeleted ?? false) && pr.PaymentRequestNumber == number && pr.Status == PaymentRequestStatus.Complete
                                    select new SalaryAllocation
                                    {
                                        //Id = null,
                                        //Code = null,
                                        //TransactionDate = null,
                                        Type = SalaryAllocationType.BankPayment,
                                        CurrencyId = pr.CurrencyId,
                                        //ExchangeRate = null,
                                        ReferenceId = pr.Id,
                                        ReferenceNumber = pr.PaymentRequestNumber,
                                        //Description = null,
                                        LegalEntityId = ca.LegalEntityId,
                                        //BusinessUnitId = ca.BusinessUnitId,
                                        //AffiliationId = ca.AffiliationId,
                                        //MainServiceCategoryId = ca.MainserviceCategoryId,
                                        //JobId = ca.JobId,
                                        //PceId = ca.PceId,
                                        //PcaId = ca.PcaId,
                                        //ClientId = ca.ClientId,
                                        //FinancialYearId = null,
                                        //FinancePeriodId = null,
                                        //Status = null,
                                        LegalEntityName = lg.LegalEntityName,
                                        //BusinessUnitName = bu.UnitName,
                                        //AffiliationName = af.AffiliationName,
                                        //ClientName = ca.ClientName,
                                        //MainServiceCategoryName = mc.Name,
                                        ViewFiscalYear = context.FinancialYears.Where(x => !(x.IsDeleted ?? false) && x.StartDate.Year == pr.PaymentRequestDate.Value.Year && x.LegalEntityId == ca.LegalEntityId).Select(x => x.FinancialYearName).FirstOrDefault(),
                                        ViewPeriod = context.FinancePeriods.Where(x => !(x.IsDeleted ?? false) && (pr.PaymentRequestDate >= x.StartDate && pr.PaymentRequestDate <= x.EndDate) && x.LegalEntityId == ca.LegalEntityId).Select(x => x.FinancePeriodName).FirstOrDefault(),
                                        ViewDateTransaction = pr.PaymentRequestDate
                                    }).FirstOrDefaultAsync();
                }
            }
            return result;
        }

        public async Task<List<SalaryAllocationDetail>> GetDetailJournal(string Id)
        {
            return await (from a in context.SalaryAllocationDetails
                          join b in context.ChartOfAccounts on a.CoaId equals b.Id
                          where a.SalaryAllocationId == Id
                          select new SalaryAllocationDetail
                          {
                              Id = a.Id,
                              SalaryAllocationId = a.SalaryAllocationId,
                              CoaId = a.CoaId,
                              Description = a.Description,
                              Debit = a.Debit,
                              Credit = a.Credit,
                              CodeRec = b.CodeRec,
                              AccountName = b.Name5,
                              Condition = a.Condition,
                              CreatedBy = a.CreatedBy,
                              CreatedOn = a.CreatedOn
                          }).ToListAsync();
        }

        public async Task<List<Journal>> GetJournal(string Id)
        {
            var result = new List<Journal>();
            if (context.SalaryAllocations.Where(x => x.Id == Id && x.Status == StatusTransactionName.Approved).Count() > 0)
            {
                result = await (from a in context.Journal
                                join b in context.LegalEntity on a.LegalId equals b.Id
                                //join c in context.Affiliations on a.AffiliateId equals c.Id
                                //join d in context.BusinessUnits on a.DivisiId equals d.Id
                                join e in context.ChartOfAccounts on a.AccountId equals e.Id
                                join f in context.Currencies on a.DefaultCurrencyId equals f.Id
                                where !(a.IsDeleted ?? false) && a.ReferenceId == Id
                                select new Journal
                                {
                                    Id = a.Id,
                                    AccountId = a.AccountId,
                                    AccountName = e.Name5,
                                    ReferenceNumber = a.ReferenceNumber,
                                    Description = a.Description,
                                    Debit = a.Debit,
                                    Credit = a.Credit,
                                    LegalEntityName = b.LegalEntityName,
                                    DivisiName = context.BusinessUnits.Where(x => x.Id == a.DivisiId).Select(x => x.UnitName).FirstOrDefault(),
                                    AfiliationName = context.Affiliations.Where(x => x.Id == a.AffiliateId).Select(x => x.AffiliationName).FirstOrDefault(),
                                    JobId = a.JobId,
                                    PceId = a.PceId,
                                    PcaId = a.PcaId,
                                    CodeRec = e.CodeRec,
                                    CurrencyId = a.CurrencyId,
                                    CurrencyName = f.CurrencyName,
                                    CurrencySymbol = f.CurrencySymbol,
                                    CreatedOn = a.CreatedOn
                                }).OrderByDescending(x => x.Debit).ThenByDescending(x => x.Credit).ToListAsync();
            }
            else
            {
                result = await (from a in context.JournalTemps
                                join b in context.LegalEntity on a.LegalId equals b.Id
                                //join c in context.Affiliations on a.AffiliateId equals c.Id
                                //join d in context.BusinessUnits on a.DivisiId equals d.Id
                                join e in context.ChartOfAccounts on a.AccountId equals e.Id
                                join f in context.Currencies on a.DefaultCurrencyId equals f.Id
                                where !(a.IsDeleted ?? false) && a.ReferenceId == Id
                                select new Journal
                                {
                                    Id = a.Id,
                                    AccountId = a.AccountId,
                                    AccountName = e.Name5,
                                    ReferenceNumber = a.ReferenceNumber,
                                    Description = a.Description,
                                    Debit = a.Debit,
                                    Credit = a.Credit,
                                    LegalEntityName = b.LegalEntityName,
                                    DivisiName = context.BusinessUnits.Where(x => x.Id == a.DivisiId).Select(x => x.UnitName).FirstOrDefault(),
                                    AfiliationName = context.Affiliations.Where(x => x.Id == a.AffiliateId).Select(x => x.AffiliationName).FirstOrDefault(),
                                    JobId = a.JobId,
                                    PceId = a.PceId,
                                    PcaId = a.PcaId,
                                    CodeRec = e.CodeRec,
                                    CurrencyId = a.CurrencyId,
                                    CurrencyName = f.CurrencyName,
                                    CurrencySymbol = f.CurrencySymbol,
                                    CreatedOn = a.CreatedOn
                                }).OrderByDescending(x => x.Debit).ThenByDescending(x => x.Credit).ToListAsync();
            }
            return result;
        }

        public async Task<List<Journal>> JournalHistoryList(string referenceId)
        {
            var result = await (from a in context.Journal
                                    //join b in context.LegalEntity on a.LegalId equals b.Id
                                    //join c in context.Affiliations on a.AffiliateId equals c.Id
                                    //join d in context.BusinessUnits on a.DivisiId equals d.Id
                                join e in context.ChartOfAccounts on a.AccountId equals e.Id
                                where !(a.IsDeleted ?? false) && a.ReferenceId == referenceId
                                select new Journal
                                {
                                    Id = a.Id,
                                    AccountId = a.AccountId,
                                    AccountName = e.Name5,
                                    //ReferenceNumber = a.ReferenceNumber,
                                    Description = a.Description,
                                    Debit = a.Debit,
                                    Credit = a.Credit,
                                    //LegalEntityName = b.LegalEntityName,
                                    //DivisiName = d.UnitName,
                                    //AfiliationName = c.AffiliationName,
                                    //JobId = a.JobId,
                                    //PceId = a.PceId,
                                    //PcaId = a.PcaId,
                                    CodeRec = e.CodeRec,
                                    CreatedOn = a.CreatedOn
                                }).OrderByDescending(x => x.Debit).ThenByDescending(x => x.Credit).ToListAsync();
            return result;
        }

        public Task<List<SalaryAllocation>> Get()
        {
            throw new NotImplementedException();
        }
    }
}