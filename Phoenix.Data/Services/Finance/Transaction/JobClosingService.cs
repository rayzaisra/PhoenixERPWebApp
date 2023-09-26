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
using Phoenix.Data.Models.ViewModel;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface IJobClosingService
    {
        Task<dynamic> GetList();
        Task<JobClosingHeader> Get(string JobId);
        Task<List<Journal>> GetJournal(string Id);
        Task<List<VwJobClosingAr>> GetARList(string jobId);
        Task<List<VwJobClosingAp>> GetAPList(string jobId);
        Task<List<VwJobClosingSummary>> GetJobClosingSummary(string Id);
        Task<int> ApproveAsync(JobClosingHeader entity);
        Task<int> AddAsync(JobClosingHeader entity);
        Task<int> EditAsync(JobClosingHeader entity);
        Task<int> CreateJournal(JobClosingHeader entity);
        Task<int> DeleteSoftAsync(JobClosingHeader entity);
        Task<int> UpdateJournalAsync(Journal entity);
        Task<List<JobClosingDetailJournal>> GetEntryJournalList(string Id);
    }

    public class JobClosingService : IJobClosingService
    {

        readonly DataContext context;
        readonly FinancePeriodService financePeriod;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly Services.Um.IManageMenuService menuService;
        readonly StatusLogService log;

        /// <summary>
        /// And endpoint to manage Pca
        /// </summary>
        /// <param name="context">Database context</param>
        public JobClosingService(DataContext context, FinancePeriodService financePeriod, GlobalFunctionApproval globalFcApproval, Services.Um.IManageMenuService menuService, StatusLogService log)
        {
            this.context = context;
            this.financePeriod = financePeriod;
            this.globalFcApproval = globalFcApproval;
            this.menuService = menuService;
            this.log = log;
        }

        public async Task<int> AddAsync(JobClosingHeader entity)
        {
            return await AddEdit(entity, false);
        }

        public async Task<int> EditAsync(JobClosingHeader entity)
        {
            return await AddEdit(entity, true);
        }

        private async Task<int> AddEdit(JobClosingHeader entity, bool isEdit)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    entity.FinanceStatus = StatusTransaction.StatusName(entity.StatusApproval);
                    await Process(entity, isEdit);

                    var isApprove = true;
                    if (entity.FinanceStatus != StatusTransactionName.Draft)
                    {
                        var approval = await context.TrTemplateApprovals.AsNoTracking().Where(x => x.RefId == entity.Id).ToListAsync();
                        if (approval.Count > 0)
                        {
                            var userApproval = await context.TrUserApprovals.AsNoTracking().Where(x => approval.Any(y => y.Id == x.TrTempApprovalId)).ToListAsync();
                            var conditionUserApproval = await context.TrConditionUserApprovals.AsNoTracking().Where(x => userApproval.Any(y => y.Id == x.TrUserApprovalId)).ToListAsync();

                            context.TrConditionUserApprovals.RemoveRange(conditionUserApproval);
                            context.TrUserApprovals.RemoveRange(userApproval);
                            context.TrTemplateApprovals.RemoveRange(approval);
                        }

                        var dataMenu = await menuService.GetByUniqueName(MenuUnique.JobClosing).ConfigureAwait(false);
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            MenuId = dataMenu.Id,
                            RefId = entity.Id,
                            DetailLink = $"{ApprovalLink.JobClosing}?Id={entity.Id}"
                        };
                        var subGroupId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(context.HttpContext);
                        var divisionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(context.HttpContext);
                        var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
                        isApprove = await globalFcApproval.Save(vm, subGroupId, divisionId, employeeId);
                        if (isApprove)
                        {
                            await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = entity.FinanceStatus, Description = entity.RemarksProcess });
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
                        entity.FinanceStatus = StatusTransactionName.Draft;
                        context.JobClosingHeaders.Update(entity);
                        await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = entity.FinanceStatus, Description = entity.RemarksProcess });
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

        private async Task Process(JobClosingHeader entity, bool isEdit)
        {
            if (string.IsNullOrEmpty(entity.FinanceStatus)) entity.FinanceStatus = StatusTransactionName.Draft;
            if (isEdit)
            {
                context.PhoenixEdit(entity);

                if (!string.IsNullOrEmpty(entity.Id))
                {
                    var detail = context.JobClosingDetails.AsNoTracking().Where(x => x.JobClosingHeaderId == entity.Id).ToList();
                    if (detail.Count > 0)
                    {
                        context.JobClosingDetails.RemoveRange(detail);
                    }

                    var detailJournal = context.JobClosingDetailJournal.AsNoTracking().Where(x => x.JobClosingHeaderId == entity.Id).ToList();
                    if (detailJournal.Count > 0)
                    {
                        context.JobClosingDetailJournal.RemoveRange(detailJournal);
                    }
                }
            }
            else
            {
                entity.Id = await TransID.GetTransId(context, Code.JobClosing, DateTime.Today);
                entity.RequestCode = await TransID.GetTransId(context, Code.JournalVoucher, DateTime.Today);
                await context.PhoenixAddAsync(entity);
                await context.SaveChangesAsync();
            }

            if (entity.JournalEntry != null)
            {
                foreach (var item in entity.JournalEntry)
                {
                    if (string.IsNullOrEmpty(item.Id))
                    {
                        item.Id = Guid.NewGuid().ToString();
                    }

                    item.JobClosingHeaderId = entity.Id;
                    await context.PhoenixAddAsync(item);
                };
            }

            if (entity.JobClosingDetails != null)
            {
                foreach (var item in entity.JobClosingDetails)
                {
                    item.Id = Guid.NewGuid().ToString();
                    item.JobClosingHeaderId = entity.Id;
                    context.PhoenixAddAsync(item);
                }
            }
        }

        public async Task<int> CreateJournal(JobClosingHeader entity)
        {
            await financePeriod.IsClosePeriod(Convert.ToDateTime(entity.TransactionDate), entity.LegalEntityId, Models.Enum.FinancePeriodCode.JC);
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var isEdit = context.JobClosingHeaders.Where(x => x.Id == entity.Id).Count() > 0;
                    await Process(entity, isEdit);
                    await context.SaveChangesAsync();
                    //create journal tmp
                    var result = await context.Database.ExecuteSqlCommandAsync("EXEC fn.create_jurnal_closing_temp @p_job_closing_id, @p_request_by", new SqlParameter("p_job_closing_id", entity.Id), new SqlParameter("p_request_by", context.GetEmployeeId()));
                    transaction.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<int> ApproveAsync(JobClosingHeader entity)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var save = await globalFcApproval.UnsafeProcessApproval(entity.Id, entity.StatusApproval, entity.RemarksProcess, entity);
                    var approval = context.TrTemplateApprovals.Where(x => x.RefId == entity.Id && !(x.IsDeleted ?? false)).FirstOrDefault();
                    if (approval != null)
                    {
                        entity.FinanceStatus = StatusTransaction.StatusName(approval.StatusApproved.GetHashCode());
                        if (approval.StatusApproved == Models.Um.StatusApproved.Approved)
                        {
                            //create journal
                            await context.Database.ExecuteSqlCommandAsync("EXECUTE fn.store_data_jurnal @p_id", parameters: new[] { new SqlParameter("@p_id", entity.Id) });

                            var job = await context.JobDetail.AsNoTracking().Where(x => x.Id == entity.JobId).FirstOrDefaultAsync();
                            if (job != null)
                            {
                                job.JobStatusId = "Closed";
                                context.JobDetail.Update(job);

                                //proses update pca task menjadi closed
                                var pca = await context.Pcas.AsNoTracking().Where(x => x.JobId == job.Id).FirstOrDefaultAsync();
                                if (pca != null)
                                {
                                    var pcaTask = await context.PcaTasks.AsNoTracking().Where(x => x.PcaId == pca.Id).ToListAsync();
                                    foreach (var item in pcaTask)
                                    {
                                        item.Status = PCAStatus.Close;
                                        context.PcaTasks.Update(item);
                                    }
                                }
                            }

                            //Update status PO Jadi closed
                            var po = await context.PurchaseOrders.AsNoTracking().Where(x => x.JobId == entity.JobId).FirstOrDefaultAsync();
                            if (po != null)
                            {
                                po.Status = PurchaseOrderStatus.Closed;
                                context.PurchaseOrders.Update(po);
                            }
                        }
                    }
                    else
                    {
                        entity.FinanceStatus = StatusTransactionName.Draft;
                    }
                    context.JobClosingHeaders.Update(entity);

                    await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = entity.FinanceStatus, Description = entity.RemarksProcess });
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

        public async Task<int> DeleteSoftAsync(JobClosingHeader entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<dynamic> GetList()
        {
            var result = await (from a in context.JobDetail
                                join b in context.BusinessUnits on a.BusinessUnitDivisionId equals b.Id
                                join c in context.LegalEntity on a.LegalEntityId equals c.Id
                                join d in context.CompanyBrands on a.BrandId equals d.Id
                                join e in context.Companys on a.CompanyId equals e.Id
                                join f in context.ClientBrief on a.ClientBriefId equals f.Id
                                join z in context.BusinessUnitTypes on b.BusinessUnitTypeId equals z.Id
                                where !(a.IsDeleted ?? false) && d.IsClient == true && e.IsClient == true &&
                                z.BusinessUnitLevel == BusinessUnitLevelCode.Division &&
                                (a.JobStatusId.ToLower() == "delivered" || context.JobClosingHeaders.Any(x => x.JobId == a.Id))
                                select new
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    JobId = a.Id,
                                    a.JobNumber,
                                    a.JobName,
                                    DivisionName = b.UnitName,
                                    c.LegalEntityName,
                                    ClientName = e.CompanyName,
                                    BrandName = d.BrandName,
                                    f.CampaignName,
                                    a.Deadline,
                                    JobStatus = a.JobStatusId,
                                    FinanceStatus = context.JobClosingHeaders.Where(x => x.JobId == a.Id).Select(x => x.FinanceStatus).FirstOrDefault() ?? StatusTransactionName.Draft,
                                    TransactionDate = context.JobClosingHeaders.Where(x => x.JobId == a.Id).Select(x => x.TransactionDate).FirstOrDefault()
                                }).ToListAsync();
            return result;
        }

        public async Task<JobClosingHeader> Get(string JobId)
        {
            var result = await (from a in context.JobClosingHeaders
                                join b in context.JobDetail on a.JobId equals b.Id
                                where !(a.IsDeleted ?? false) && a.JobId == JobId
                                select new JobClosingHeader
                                {
                                    Id = a.Id,
                                    RequestCode = a.RequestCode,
                                    JobId = a.JobId,
                                    JobName = a.JobName,
                                    DivisiId = a.DivisiId,
                                    LegalEntityId = a.LegalEntityId,
                                    MainServiceCategoryId = b.MainServiceCategory,
                                    ClientId = a.ClientId,
                                    BrandId = a.BrandId,
                                    Deadline = a.Deadline,
                                    JobStatus = a.JobStatus,
                                    FinanceStatus = a.FinanceStatus,
                                    CreatedBy = a.CreatedBy,
                                    CreatedOn = a.CreatedOn,
                                    JobNumber = b.JobNumber,
                                    TransactionDate = a.TransactionDate,
                                    JobClosingDetails = context.JobClosingDetails.Where(x => x.JobClosingHeaderId == a.Id).ToList()
                                }).FirstOrDefaultAsync();
            if (result == null)
            {
                result = await context.JobDetail.AsNoTracking()
                             .Where(x => !(x.IsDeleted ?? false) && x.Id == JobId)
                             .Select(x => new JobClosingHeader()
                             {
                                 JobId = x.Id,
                                 JobNumber = x.JobNumber,
                                 JobName = x.JobName,
                                 DivisiId = x.BusinessUnitDivisionId,
                                 LegalEntityId = x.LegalEntityId,
                                 ClientId = x.CompanyId,
                                 BrandId = x.BrandId,
                                 Deadline = x.Deadline,
                                 JobStatus = x.JobStatusId,
                                 FinanceStatus = StatusTransactionName.Draft,
                                 JobClosingDetails = new List<JobClosingDetail>(),
                                 TransactionDate = DateTime.Now,
                                 MainServiceCategoryId = x.MainServiceCategory
                             }).FirstOrDefaultAsync();
            }
            return result;
        }

        public async Task<List<Journal>> GetJournal(string Id)
        {
            //var result = new List<Journal>();
            //if (context.JobClosingHeaders.Where(x => x.Id == Id && x.FinanceStatus == StatusTransactionName.Approved).Count() > 0)
            //{
            //    result = await (from a in context.Journal
            //                    join b in context.LegalEntity on a.LegalId equals b.Id
            //                    //join c in context.Affiliations on a.AffiliateId equals c.Id
            //                    join d in context.BusinessUnits on a.DivisiId equals d.Id
            //                    join e in context.ChartOfAccounts on a.AccountId equals e.Id
            //                    join z in context.BusinessUnitTypes on d.BusinessUnitTypeId equals z.Id
            //                    where !(a.IsDeleted ?? false) && a.ReferenceId == Id && z.BusinessUnitLevel == BusinessUnitLevelCode.Division && e.Level == "5"
            //                    select new Journal
            //                    {
            //                        TransactionDate = a.TransactionDate,
            //                        ReferenceDate = a.ReferenceDate,
            //                        AccountId = a.AccountId,
            //                        AccountName = e.Name5,
            //                        ReferenceNumber = a.ReferenceNumber,
            //                        Description = a.Description,
            //                        Debit = a.Debit,
            //                        Credit = a.Credit,
            //                        LegalEntityName = b.LegalEntityName,
            //                        DivisiName = d.UnitName,
            //                        AfiliationName = context.Affiliations.Where(x => x.Id == a.AffiliateId).Select(x => x.AffiliationName).FirstOrDefault(),
            //                        CodeRec = e.CodeRec,
            //                        CreatedOn = a.CreatedOn
            //                    }).OrderByDescending(x => x.Debit).ThenByDescending(x => x.Credit).ToListAsync();
            //}
            //else
            //{
            return await (from a in context.JournalTemps
                          join b in context.LegalEntity on a.LegalId equals b.Id
                          //join c in context.Affiliations on a.AffiliateId equals c.Id
                          join d in context.BusinessUnits on a.DivisiId equals d.Id
                          join e in context.ChartOfAccounts on a.AccountId equals e.Id
                          join z in context.BusinessUnitTypes on d.BusinessUnitTypeId equals z.Id
                          where !(a.IsDeleted ?? false) && a.ReferenceId == Id && z.BusinessUnitLevel == BusinessUnitLevelCode.Division && e.Level == "5"
                          select new Journal
                          {
                              Id = a.Id,
                              ReferenceId = a.ReferenceId,
                              TransactionDate = a.TransactionDate,
                              ReferenceDate = a.Referencedate,
                              AccountId = a.AccountId,
                              AccountName = e.Name5,
                              ReferenceNumber = a.ReferenceNumber,
                              Description = a.Description,
                              Debit = a.Debit,
                              Credit = a.Credit,
                              LegalEntityName = b.LegalEntityName,
                              DivisiName = d.UnitName,
                              AfiliationName = context.Affiliations.Where(x => x.Id == a.AffiliateId).Select(x => x.AffiliationName).FirstOrDefault(),
                              CodeRec = e.CodeRec,
                              CreatedOn = a.CreatedOn,
                              JobId = a.JobId,
                              JobIdName = context.JobDetail.Where(x => x.Id == a.JobId).Select(x => $"{x.Id} - {x.JobName}").FirstOrDefault(),
                              DivisiId = a.DivisiId,
                              LineCode = a.LineCode,
                              LegalId = a.LegalId
                          }).OrderByDescending(x => x.Debit).ThenByDescending(x => x.Credit).ToListAsync();
            //}
            //return result;
        }

        public async Task<List<VwJobClosingAr>> GetARList(string jobId) => await context.VwJobClosingAr.Where(x => x.JobId == jobId).ToListAsync();
        public async Task<List<VwJobClosingAp>> GetAPList(string jobId) => await context.VwJobClosingAp.Where(x => x.JobId == jobId).ToListAsync();
        public async Task<List<VwJobClosingSummary>> GetJobClosingSummary(string Id) => await context.ExecuteStoredProcedure<VwJobClosingSummary>("fn.report_sum_closing_ytd", new SqlParameter[] { new SqlParameter("@p_job_closing_id", (Id ?? "")) });

        public async Task<int> UpdateJournalAsync(Journal entity)
        {
            context.Database.ExecuteSqlCommand($"UPDATE fn.journal_temp SET Description={entity.Description} WHERE id={entity.Id}");
            return await context.SaveChangesAsync();
        }

        public async Task<List<JobClosingDetailJournal>> GetEntryJournalList(string Id)
        {
            return await (from a in context.JobClosingDetailJournal
                          join b in context.ChartOfAccounts on a.CoaId equals b.Id
                          where a.JobClosingHeaderId == Id
                          select new JobClosingDetailJournal
                          {
                              Id = a.Id,
                              JobClosingHeaderId = a.JobClosingHeaderId,
                              CoaId = a.CoaId,
                              Description = a.Description,
                              Condition = a.Condition,
                              Debit = a.Debit,
                              Credit = a.Credit,
                              TransactionDate = a.TransactionDate,
                              //CodeRec = b.CodeRec,
                              AccountName = $"({b.CodeRec}) {b.Name5}",
                              CreatedBy = a.CreatedBy,
                              CreatedOn = a.CreatedOn
                          }).ToListAsync();
        }
    }
}