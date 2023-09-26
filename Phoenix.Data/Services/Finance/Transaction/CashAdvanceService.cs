using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Enum;
using Phoenix.Data.Models.Finance;
using Phoenix.Data.Models.Finance.Transaction;
using Phoenix.Data.Services.Um;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Finance
{
    public interface ICashAdvanceService
    {
        Task<int> AddEditAsync(CashAdvance entity, bool isEdit);
        Task<CashAdvance> Approve(CashAdvance entity);
        Task<CashAdvance> Reject(CashAdvance entity);
        Task<int> DeleteSoftAsync(CashAdvance entity);
        Task<CashAdvance> Get(string Id);
        Task<List<CashAdvance>> Get(string legalEntityId, string mainServiceCategoryId);
        Task<List<CashAdvanceDetail>> GetDetails(string id);
        Task<CashAdvance> GetCompanyInfo(string companyid);
    }

    public class CashAdvanceService : ICashAdvanceService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly IManageMenuService menuService;
        readonly StatusLogService log;
        /// <summary>
        /// And endpoint to manage CashAdvance
        /// </summary>
        /// <param name="context">Database context</param>
        public CashAdvanceService(DataContext context, GlobalFunctionApproval globalFcApproval, IManageMenuService menuService, StatusLogService log)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.menuService = menuService;
            this.log = log;
        }

        public async Task<int> AddEditAsync(CashAdvance entity, bool isEdit)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    entity.Status = StatusTransaction.StatusName(entity.StatusApproval);
                    if (isEdit)
                    {
                        context.PhoenixEdit(entity);
                    }
                    else
                    {
                        entity.Id = await TransID.GetTransId(context, Code.CashAdvance, DateTime.Today);
                        entity.CaNumber = entity.Id;
                        entity.RequestedBy = context.GetEmployeeId();
                        entity.RequestedOn = DateTime.Now;
                        await context.PhoenixAddAsync(entity);
                        await context.SaveChangesAsync();
                    }

                    var detail = new List<CashAdvanceDetail>();
                    if (!string.IsNullOrEmpty(entity.Id))
                    {
                        detail = context.CashAdvanceDetails.AsNoTracking().Where(x => x.CashAdvanceId == entity.Id).ToList();
                        if (detail.Count > 0)
                        {
                            context.CashAdvanceDetails.RemoveRange(detail);
                        }
                    }

                    if (entity.CashAdvanceDetails != null)
                    {
                        foreach (var item in entity.CashAdvanceDetails)
                        {

                            var requestDetail = detail.Where(x => x.TaskId == item.TaskId).FirstOrDefault();
                            decimal TotalExchangeRequest = 0;
                            if (requestDetail != null)
                            {
                                TotalExchangeRequest = (requestDetail.TotalExchange ?? 0);
                            }

                            item.Id = Guid.NewGuid().ToString();
                            item.CashAdvanceId = entity.Id;
                            await context.PhoenixAddAsync(item);

                            var taskDtl = await context.PcaTasks.AsNoTracking().Where(x => x.PcaId == entity.PcaId && x.Id == item.TaskId).FirstOrDefaultAsync();
                            taskDtl.Status = PCAStatus.CA;
                            taskDtl.TotalPriceUsed = ((taskDtl.TotalPriceUsed ?? 0) - TotalExchangeRequest) + item.TotalExchange;
                            taskDtl.TotalPriceBalance = (taskDtl.TotalPrice ?? 0) - taskDtl.TotalPriceUsed;

                            if (taskDtl.TotalPriceBalance == null)
                            {
                                throw new Exception("Remaining Budget Cannot be Empty.");
                            }

                            if (taskDtl.TotalPriceBalance < 0)
                            {
                                throw new Exception("the total exchange requests must be less than or equal to the remaining budget");
                            }

                            context.PhoenixEdit(taskDtl);
                        };
                    }

                    await context.SaveChangesAsync();
                    var pca = await context.Pcas.AsNoTracking().Where(x => x.Id == entity.PcaId).FirstOrDefaultAsync();
                    if (pca != null)
                    {
                        pca.TotalTaskPriceUsed = context.PcaTasks.Where(x => !(x.IsDeleted ?? false) && x.PcaId == pca.Id).Sum(x => (x.TotalPriceUsed ?? 0));
                        pca.TotalTaskPriceBalance = context.PcaTasks.Where(x => !(x.IsDeleted ?? false) && x.PcaId == pca.Id).Sum(x => (x.TotalPriceBalance ?? 0));
                        context.Pcas.Update(pca);
                    }

                    var isApprove = true;
                    if (entity.Status != StatusTransactionName.Draft)
                    {
                        var dataMenu = await menuService.GetByUniqueName(MenuUnique.CashAdvance).ConfigureAwait(false);
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            MenuId = dataMenu.Id,
                            RefId = entity.Id,
                            DetailLink = $"{ApprovalLink.CashAdvance}?Id={entity.Id}",
                            NotificationType = NotificationTypeCode.CashAdvanceApprovalNotification,
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
                        context.CashAdvances.Update(entity);
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

        public async Task<int> DeleteSoftAsync(CashAdvance entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<CashAdvance> Approve(CashAdvance entity)
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
                            //Add Payment Request
                            //var pca = await context.Pcas.AsNoTracking().Where(x => x.JobId == entity.JobId).FirstOrDefaultAsync();
                            //var totalRequest = context.CashAdvanceDetails.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.CashAdvanceId == entity.Id).Sum(x => x.TotalRequest);

                            var pca = await context.Pcas.AsNoTracking().Where(x => x.JobId == entity.JobId).FirstOrDefaultAsync();
                            var currencyDefault = await context.Currencies.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).FirstOrDefaultAsync();
                            //var currency = new Currency();
                            //if (pca != null) currency = await context.Currencies.Where(x => !(x.IsDeleted ?? false) && x.Id == pca.CurrencyId).FirstOrDefaultAsync();
                            //var totalRequest = context.CashAdvanceDetails.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.CashAdvanceId == entity.Id).Sum(x => x.TotalExchange);
                            var totalRequest = await (from a in context.CashAdvances
                                                      join b in context.CashAdvanceDetails on a.Id equals b.CashAdvanceId
                                                      where !(a.IsDeleted ?? false) && !(b.IsDeleted ?? false) && b.CashAdvanceId == entity.Id
                                                      group b by new { a.CurrencyId, a.ExchangeRate } into g
                                                      select new { total = g.Sum(x => g.Key.CurrencyId == currencyDefault.Id ? x.TotalRequest : x.TotalExchange) })
                                                      .Select(x => x.total).FirstOrDefaultAsync();
                            var payId = await TransID.GetTransId(context, Code.PaymentRequest, DateTime.Today);
                            PaymentRequest pay = new PaymentRequest()
                            {
                                Id = payId,
                                PaymentRequestDate = DateTime.Now,
                                PaymentRequestNumber = payId,
                                Status = PaymentRequestStatus.Open,
                                LegalEntityId = entity.LegalEntityId,
                                BusinessUnitId = entity.BusinessUnitId,
                                AffilicationId = entity.AffiliationId,
                                PaymentTypeId = PaymentType.CashAdvance,
                                Vat = 0,
                                DueDate = entity.CaPeriodTo,
                                PaytoName = entity.VendorName,
                                PaytoBank = entity.PayeeBankId,
                                TaxPayable = 0,
                                VatPercent = 0,
                                TaxPayablePercent = 0,
                                IsBalance = false,
                                //CurrencyId = pca.CurrencyId,
                                //ExchangeRate = (pca.CurrencyId == currencyDefault.Id) ? 1 : (decimal?)null,
                                CurrencyId = currencyDefault.Id,
                                ExchangeRate = 1, //karena ketika proses bank payment selalu mengikuti currency default.
                                SourceDoc = "CA",
                                Balance = 0,
                                TotalAmount = totalRequest,
                                Amount = totalRequest,
                                TotalAmountPay = totalRequest,
                                AmountPay = totalRequest,
                                AdvertisingTax = 0,
                                Discount = 0,
                                JobId = entity.JobId,
                                MainServiceCategoryId = entity.MainserviceCategoryId,
                                ReferenceNumberId = entity.CaNumber
                            };
                            await context.PhoenixAddAsync(pay);

                            //tidak perlu di update budget lagi dikarenakan sudah di update di awal
                            //update Status PCA Task
                            if (!string.IsNullOrEmpty(entity.JobId))
                            {
                                foreach (var detail in entity.CashAdvanceDetails)
                                {
                                    var pcaTask = await context.PcaTasks.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.PcaId == entity.PcaId && x.Id == detail.TaskId).FirstOrDefaultAsync();
                                    if (pcaTask == null) throw new Exception("PCA Task cannot be empty");
                                    pcaTask.Status = PCAStatus.Pay;
                                    //pcaTask.TotalPriceUsed = pcaTask.TotalPriceUsed + detail.TotalExchange;
                                    //pcaTask.TotalPriceBalance = pcaTask.TotalPrice - pcaTask.TotalPriceUsed;

                                    //if (pcaTask.TotalPriceBalance == null)
                                    //{
                                    //    throw new Exception("Remaining Budget Cannot be Empty.");
                                    //}

                                    //if (pcaTask.TotalPriceBalance < 0)
                                    //{
                                    //    throw new Exception("the total exchange requests must be less than or equal to the remaining budget");
                                    //}

                                    context.PhoenixEdit(pcaTask);
                                }

                                //await context.SaveChangesAsync();
                                //if (pca != null)
                                //{
                                //    pca.TotalTaskPriceUsed = context.PcaTasks.Where(x => !(x.IsDeleted ?? false) && x.PcaId == pca.Id).Sum(x => (x.TotalPriceUsed ?? 0));
                                //    pca.TotalTaskPriceBalance = context.PcaTasks.Where(x => !(x.IsDeleted ?? false) && x.PcaId == pca.Id).Sum(x => (x.TotalPriceBalance ?? 0));
                                //    context.Pcas.Update(pca);
                                //}
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

        public async Task<CashAdvance> Reject(CashAdvance entity)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var save = await globalFcApproval.UnsafeProcessApproval(entity.Id, StatusTransaction.Rejected, entity.RemarksProcess, entity);
                    var approval = context.TrTemplateApprovals.Where(x => x.RefId == entity.Id && !(x.IsDeleted ?? false)).FirstOrDefault();
                    if (approval != null)
                    {
                        entity.Status = StatusTransaction.StatusName(approval.StatusApproved.GetHashCode());
                        if (approval.StatusApproved == Models.Um.StatusApproved.Rejected)
                        {
                            ////update Status PCA Task
                            if (!string.IsNullOrEmpty(entity.JobId))
                            {
                                foreach (var detail in entity.CashAdvanceDetails)
                                {
                                    var pcaTask = await context.PcaTasks.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == detail.TaskId).FirstOrDefaultAsync();
                                    pcaTask.Status = PCAStatus.Open;
                                    pcaTask.TotalPriceUsed = pcaTask.TotalPriceUsed - detail.TotalExchange;
                                    pcaTask.TotalPriceBalance = pcaTask.TotalPrice - pcaTask.TotalPriceUsed;
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

        public async Task<CashAdvance> Get(string Id)
        {
            var model = await context.CashAdvances.Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
            if (!string.IsNullOrEmpty(model.Id))
            {
                var pca = await (from a in context.Pcas
                                 join b in context.Currencies on a.CurrencyId equals b.Id
                                 where a.Id == model.PcaId
                                 select new { a.CurrencyId, b.CurrencySymbol }).FirstOrDefaultAsync();
                if (pca != null)
                {
                    model.PCACurrencyId = pca.CurrencyId;
                    model.CurrencySymbol = pca.CurrencySymbol;
                }

                model.DefaultCurrencyId = context.Currencies.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).Select(x => x.Id).FirstOrDefault();
                model.CurrencySymbolExchange = context.Currencies.Where(x => x.Id == model.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault();

                var company = await context.Companys.Where(x => x.Id == model.VendorId).FirstOrDefaultAsync();
                if (company != null)
                {
                    model.VendorName = company.CompanyName;
                    model.PayeeBankId = company.BankName;
                    model.PayeeAccountNumber = company.AccountNumber;
                    model.PayeeAccountName = company.AccountName;
                    model.VendorAddress = company.OfficeAddress;
                    model.VendorPIC = company.ContactName;
                    model.VendorTaxNumber = company.TaxRegistrationNumber;
                }
            }
            return model;
        }

        public async Task<List<CashAdvance>> Get(string legalEntityId, string mainServiceCategoryId)
        {
            var legalEntity = string.IsNullOrEmpty(legalEntityId) ? "0" : legalEntityId;
            var mainService = string.IsNullOrEmpty(mainServiceCategoryId) ? "0" : mainServiceCategoryId;

            var result = await (from ca in context.CashAdvances
                                join rb in context.EmployeeBasicInfos on ca.RequestedBy equals rb.Id
                                join job in context.JobDetail on ca.JobId equals job.Id
                                join lg in context.LegalEntity on ca.LegalEntityId equals lg.Id
                                join bu in context.BusinessUnits on ca.BusinessUnitId equals bu.Id
                                join mc in context.MainServiceCategorys on ca.MainserviceCategoryId equals mc.Id
                                where !(ca.IsDeleted ?? false) &&
                                (legalEntity == "0" ? true : ca.LegalEntityId == legalEntity) &&
                                (mainService == "0" ? true : ca.MainserviceCategoryId == mainService)
                                select new CashAdvance
                                {
                                    Id = ca.Id,
                                    CaNumber = ca.CaNumber,
                                    CaDate = ca.CaDate,
                                    JobName = job.Id + " - " + job.JobName,
                                    BusinessUnitName = bu.UnitName,
                                    LegalEntityName = lg.LegalEntityName,
                                    MainServiceCategoryName = mc.Name,
                                    Status = ca.Status,
                                    RequestedBy = rb.NameEmployee,
                                    CreatedOn = ca.CreatedOn,
                                    ModifiedOn = ca.ModifiedOn,
                                    RequestBusinessUnitName = context.BusinessUnits.Where(x => x.Id == ca.RequestBusinessUnitId).Select(x => x.UnitName).FirstOrDefault(),
                                    Amount = ca.Amount,
                                    CurrencySymbol = context.Currencies.Where(x => x.Id == ca.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault(),
                                    StatusCASettlement = context.CASettlement.Where(x => x.CashAdvanceId == ca.Id).Select(x => x.Status).FirstOrDefault(),
                                    CaPeriodFrom = ca.CaPeriodFrom,
                                    CaPeriodTo = ca.CaPeriodTo,
                                    PcaId = ca.PcaId
                                }).OrderByDescending(x => x.ModifiedOn).ToListAsync();
            return result;
        }

        public async Task<List<CashAdvanceDetail>> GetDetails(string id)
        {
            var result = await (from caD in context.CashAdvanceDetails
                                join u in context.Uom on caD.UomId equals u.Id
                                join ca in context.CashAdvances on caD.CashAdvanceId equals ca.Id
                                join pca in context.Pcas on ca.PcaId equals pca.Id
                                where !(caD.IsDeleted ?? false) && caD.CashAdvanceId == id
                                select new CashAdvanceDetail
                                {
                                    Id = caD.Id,
                                    CashAdvanceId = id,
                                    TaskId = caD.TaskId,
                                    TaskName = caD.TaskName,
                                    Qty = caD.Qty,
                                    UomId = caD.UomId,
                                    UnitPrice = caD.UnitPrice,
                                    Amount = caD.Amount,
                                    TotalRequest = caD.TotalRequest,
                                    CreatedBy = caD.CreatedBy,
                                    CreatedOn = caD.CreatedOn,
                                    ModifiedBy = caD.ModifiedBy,
                                    ModifiedOn = caD.ModifiedOn,
                                    UomName = u.unit_symbol,
                                    RemainingBudget = context.PcaTasks.Where(x => x.Id == caD.TaskId && x.PcaId == ca.PcaId).Select(x => x.TotalPriceBalance).FirstOrDefault(),
                                    TotalExchange = caD.TotalExchange,
                                    CurrencySymbol = context.Currencies.Where(x => x.Id == pca.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault(),
                                    CurrencySymbolExchange = context.Currencies.Where(x => x.Id == ca.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault()
                                }).ToListAsync();
            return result;
        }

        public async Task<CashAdvance> GetCompanyInfo(string companyid)
        {
            var company = await context.Companys.Where(x => x.Id == companyid)
                .Select(a => new CashAdvance
                {
                    VendorId = a.Id,
                    VendorName = a.CompanyName,
                    VendorPIC = a.ContactName,
                    VendorTaxNumber = a.TaxRegistrationNumber,
                    VendorAddress = a.OfficeAddress,
                    PayeeBankId = a.BankName,
                    PayeeAccountName = a.AccountName,
                    PayeeAccountNumber = a.AccountNumber
                }).FirstOrDefaultAsync();
            return company;
        }
    }
}
