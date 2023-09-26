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
    public interface IJobCardService
    {
        Task<dynamic> GetList();
        Task<dynamic> Get(string jobId);
        Task<List<VwReportJobCard>> GetReportJobCard(string jobId);
        Task<List<VwReportJobCardCobAp>> GetReportJobCardCOBAp(string jobId);
        Task<List<VwReportJobCardCobCa>> GetReportJobCardCOBCa(string jobId);
        Task<List<VwReportJobCardCobAr>> GetReportJobCardCOBAr(string jobId);
        Task<List<VwReportJobCardCobBr>> GetReportJobCardCOBBr(string jobId);
        Task<List<VwReportJobCardCobBp>> GetReportJobCardCOBBp(string jobId);
        Task<List<VwReportJobCardRfqCa>> GetReportJobCardRfqCa(string jobId);
    }

    public class JobCardService : IJobCardService
    {

        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage Pca
        /// </summary>
        /// <param name="context">Database context</param>
        public JobCardService(DataContext context)
        {
            this.context = context;
        }

        public async Task<dynamic> GetList()
        {
            ////yang bisa akses ke pca adalah pm team, creator dan approval
            //var userId = context.GetUserId();
            //var employeeId = context.GetEmployeeId();

            //var team = await (from a in context.PmTeams
            //                  join b in context.PmTeamMembers on a.Id equals b.TeamId
            //                  where !(a.IsDeleted ?? false) && !(b.IsDeleted ?? false) && b.ApplicationUserId == userId
            //                  select new { a.JobId }).ToListAsync();

            var result = await (from g in context.Pces
                                join a in context.JobDetail on g.JobId equals a.Id
                                join b in context.BusinessUnits on a.BusinessUnitDivisionId equals b.Id
                                join c in context.LegalEntity on a.LegalEntityId equals c.Id
                                //join d in context.CompanyBrands on a.BrandId equals d.Id
                                //join e in context.Companys on a.CompanyId equals e.Id
                                //join f in context.ClientBrief on a.ClientBriefId equals f.Id
                                where !(a.IsDeleted ?? false) //&& d.IsClient == true && e.IsClient == true
                                select new
                                {
                                    JobId = a.Id,
                                    a.JobNumber,
                                    a.JobName,
                                    DivisionName = b.UnitName,
                                    c.LegalEntityName,
                                    ClientName = context.Companys.Where(x => x.Id == a.CompanyId).Select(x => x.CompanyName).FirstOrDefault(),
                                    BrandName = context.CompanyBrands.Where(x => x.Id == a.BrandId).Select(x => x.BrandName).FirstOrDefault(),
                                    CampaignName = context.ClientBrief.Where(x => x.Id == a.ClientBriefId).Select(x => x.CampaignName).FirstOrDefault(),
                                    a.CreatedOn,
                                    a.CreatedBy
                                })
                                //.Where(x => team.Any(y => y.JobId == x.JobId) || x.CreatedBy == employeeId)
                                .OrderByDescending(x => x.CreatedOn).ToListAsync();
            return result;
        }

        public async Task<dynamic> Get(string jobId)
        {
            var result = await (from a in context.JobDetail
                                join b in context.BusinessUnits on a.BusinessUnitDivisionId equals b.Id
                                join c in context.LegalEntity on a.LegalEntityId equals c.Id
                                //join d in context.CompanyBrands on a.BrandId equals d.Id
                                //join e in context.Companys on a.CompanyId equals e.Id
                                //join f in context.ClientBrief on a.ClientBriefId equals f.Id
                                join g in context.Pces on a.Id equals g.JobId
                                where !(a.IsDeleted ?? false) //&& d.IsClient == true && e.IsClient == true 
                                && a.Id == jobId
                                select new
                                {
                                    JobId = a.Id,
                                    a.JobNumber,
                                    a.JobName,
                                    DivisionName = b.UnitName,
                                    c.LegalEntityName,
                                    ClientId = a.CompanyId,
                                    ClientName = context.Companys.Where(x => x.Id == a.CompanyId).Select(x => x.CompanyName).FirstOrDefault(),
                                    BrandName = context.CompanyBrands.Where(x => x.Id == a.BrandId).Select(x => x.BrandName).FirstOrDefault(),
                                    CampaignName = context.ClientBrief.Where(x => x.Id == a.ClientBriefId).Select(x => x.CampaignName).FirstOrDefault(),
                                    g.MainserviceCategoryId,
                                    g.ShareservicesId,
                                    g.CurrencyId,
                                    JobStatus = a.JobStatusId,
                                    JobStarted = a.StartJob,
                                    BusinessUnitId = a.BusinessUnitDivisionId,
                                    a.LegalEntityId,
                                    AgingUpToDay = a.CreatedOn == null ? Convert.ToDouble(0) : ((a.StartJob == null ? DateTime.Now : Convert.ToDateTime(a.StartJob)) - Convert.ToDateTime(a.CreatedOn)).TotalDays,
                                    g.TypeOfExpenseId,
                                    g.PcaId,
                                    PceId = g.Id,
                                    Date = g.ApprovedOn,
                                    TotalInvoice = context.InvoiceReceivables.Where(x => !(x.IsDeleted ?? false) && x.JobId == a.Id && !(x.InvoiceStatusId == InvoiceReceivedStatus.Posted || x.InvoiceStatusId == InvoiceReceivedStatus.Rejected)).Count(),
                                    TotalPO = context.PurchaseOrders.Where(x => !(x.IsDeleted ?? false) && x.JobId == a.Id).Count(),
                                    //TotalCAUnsettle = 0,
                                    TotalPettyCashUnsettle = context.PettyCashDetails.Where(x => !(x.IsDeleted ?? false) && x.JobId == a.Id && context.PettyCashs.Any(y => !(y.Status == PettyCashStatus.Approved) && y.Id == x.PettyCashId)).Count(),
                                    TotalJournalEntryUnsettle = context.JournalEntryDetails.Where(x => !(x.IsDeleted ?? false) && x.JobId == a.Id && context.JournalEntrys.Any(y => y.IsDraft == true && y.Id == x.JournalEntryId)).Count(),
                                    TotalARUnclosed = context.AccountReceivables.Where(x => !(x.IsDeleted ?? false) && x.JobId == a.Id && x.Status == StatusTransactionName.Draft).Count(),
                                    TotalAmountBR = context.BankReceived.Where(x => !(x.IsDeleted ?? false) && x.JobId.Contains(a.Id) && (x.Status == PaymentRequestStatus.Reconsiliation || x.Status == PaymentRequestStatus.Complete)).Count(),
                                    TotalAmountBP = context.PaymentRequest.Where(x => !(x.IsDeleted ?? false) && x.JobId.Contains(a.Id) && (x.Status == PaymentRequestStatus.Reconsiliation || x.Status == PaymentRequestStatus.Complete)).Count()
                                }).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<VwReportJobCard>> GetReportJobCard(string jobId)
        {
            var param = new SqlParameter[] { new SqlParameter("@p_job_id", jobId) };
            var result = await context.ExecuteStoredProcedure<VwReportJobCard>("fn.report_job_card", param);
            return result;
        }

        public async Task<List<VwReportJobCardCobAp>> GetReportJobCardCOBAp(string jobId)
        {
            var param = new SqlParameter[] { new SqlParameter("@p_job_id", jobId) };
            var result = await context.ExecuteStoredProcedure<VwReportJobCardCobAp>("fn.report_job_card_cob_ap", param);
            return result;
        }

        public async Task<List<VwReportJobCardCobCa>> GetReportJobCardCOBCa(string jobId)
        {
            var param = new SqlParameter[] { new SqlParameter("@p_job_id", jobId) };
            var result = await context.ExecuteStoredProcedure<VwReportJobCardCobCa>("fn.report_job_card_cob_ca", param);
            return result;
        }

        public async Task<List<VwReportJobCardCobAr>> GetReportJobCardCOBAr(string jobId)
        {
            var param = new SqlParameter[] { new SqlParameter("@p_job_id", jobId) };
            var result = await context.ExecuteStoredProcedure<VwReportJobCardCobAr>("fn.report_job_card_cob_ar", param);
            return result;
        }

        public async Task<List<VwReportJobCardCobBr>> GetReportJobCardCOBBr(string jobId)
        {
            var param = new SqlParameter[] {
                new SqlParameter("@p_job_id", jobId)};
            var result = await context.ExecuteStoredProcedure<VwReportJobCardCobBr>("fn.report_job_card_cob_br", param);
            return result;
        }

        public async Task<List<VwReportJobCardCobBp>> GetReportJobCardCOBBp(string jobId)
        {
            var param = new SqlParameter[] { new SqlParameter("@p_job_id", jobId) };
            var result = await context.ExecuteStoredProcedure<VwReportJobCardCobBp>("fn.report_job_card_cob_bp", param);
            return result;
        }

        public async Task<List<VwReportJobCardRfqCa>> GetReportJobCardRfqCa(string jobId)
        {
            var param = new SqlParameter[] { new SqlParameter("@p_job_id", jobId) };
            var result = await context.ExecuteStoredProcedure<VwReportJobCardRfqCa>("fn.report_job_card_rfq_ca", param);
            return result;
        }
    }
}