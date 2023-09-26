using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface IRptBillingAndRevenueService
    {
        Task<List<RptBillingAndRevenue>> Get(RptBillingAndRevenueFilter Filter);
        Task<List<RptBillingAndRevenueJobId>> GetJob(RptBillingAndRevenueFilter Filter);
        Task<List<RptBillingAndRevenueSummary>> GetSummary(RptBillingAndRevenueFilter Filter);
        Task<List<RptBillingAndRevenueBusinessUnit>> GetBU(RptBillingAndRevenueFilter Filter);
    }

    public class RptBillingAndRevenueService : IRptBillingAndRevenueService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage RptProfitAndLoss
        /// </summary>
        /// <param name="context">Database context</param>
        public RptBillingAndRevenueService(DataContext context)
        {
            this.context = context;
        }

        public async Task<List<RptBillingAndRevenue>> Get(RptBillingAndRevenueFilter Filter)
        {
            var param = new SqlParameter[]
                 {
                    new SqlParameter("@p_from_date",(object)Filter.StartingDate??DBNull.Value),
                    new SqlParameter("@p_to_date",(object)Filter.EndingDate??DBNull.Value),
                    new SqlParameter("@p_mainservice_category_id",(object)Filter.MainServiceCategoryId??DBNull.Value),
                    new SqlParameter("@p_legal_entity",(object)Filter.LegalEntityId??DBNull.Value),
                    new SqlParameter("@p_business_unit_id",(object)Filter.BusinessUnitId??DBNull.Value),
                    new SqlParameter("@p_client_id",(object)Filter.ClientId??DBNull.Value),
                    new SqlParameter("@p_currency_id",(object)Filter.CurrencyId??DBNull.Value),
                    new SqlParameter("@p_reportType",(object)Filter.ReportType??DBNull.Value),
                    new SqlParameter("@p_dateType",(object)Filter.DateType??DBNull.Value),
                 };
            //var sp = Filter.ReportType.ToLower() == "actual" ? "fn.report_billrev_actual" : "fn.report_billing_revenue";
            var result = await context.ExecuteStoredProcedure<RptBillingAndRevenue>("fn.report_billing_revenue", param);
            return result;
        }

        public async Task<List<RptBillingAndRevenueJobId>> GetJob(RptBillingAndRevenueFilter Filter)
        {
            var param = new SqlParameter[]
                 {
                    new SqlParameter("@p_from_date",(object)Filter.StartingDate??DBNull.Value),
                    new SqlParameter("@p_to_date",(object)Filter.EndingDate??DBNull.Value),
                    new SqlParameter("@p_mainservice_category_id",(object)Filter.MainServiceCategoryId??DBNull.Value),
                    new SqlParameter("@p_legal_entity",(object)Filter.LegalEntityId??DBNull.Value),
                    new SqlParameter("@p_business_unit_id",(object)Filter.BusinessUnitId??DBNull.Value),
                    new SqlParameter("@p_client_id",(object)Filter.ClientId??DBNull.Value),
                    new SqlParameter("@p_currency_id",(object)Filter.CurrencyId??DBNull.Value),
                    //new SqlParameter("@p_reportType",(object)Filter.ReportType??DBNull.Value),
                    new SqlParameter("@p_job_id",(object)Filter.JobId??DBNull.Value),
                    new SqlParameter("@p_dateType",(object)Filter.DateType??DBNull.Value),
                 };
            var result = await context.ExecuteStoredProcedure<RptBillingAndRevenueJobId>("fn.report_billing_revenue_jobid", param);
            return result;
        }

        public async Task<List<RptBillingAndRevenueSummary>> GetSummary(RptBillingAndRevenueFilter Filter)
        {
            var param = new SqlParameter[]
                 {
                    new SqlParameter("@p_from_date",(object)Filter.StartingDate??DBNull.Value),
                    new SqlParameter("@p_to_date",(object)Filter.EndingDate??DBNull.Value),
                    new SqlParameter("@p_mainservice_category_id",(object)Filter.MainServiceCategoryId??DBNull.Value),
                    new SqlParameter("@p_legal_entity",(object)Filter.LegalEntityId??DBNull.Value),
                    new SqlParameter("@p_currency_id",(object)Filter.CurrencyId??DBNull.Value),
                    new SqlParameter("@p_dateType",(object)Filter.DateType??DBNull.Value),
                 };
            var result = await context.ExecuteStoredProcedure<RptBillingAndRevenueSummary>("fn.report_billing_revenue_summary", param);
            return result;
        }

        public async Task<List<RptBillingAndRevenueBusinessUnit>> GetBU(RptBillingAndRevenueFilter Filter)
        {
            var param = new SqlParameter[]
                 {
                    new SqlParameter("@p_from_date",(object)Filter.StartingDate??DBNull.Value),
                    new SqlParameter("@p_to_date",(object)Filter.EndingDate??DBNull.Value),
                    new SqlParameter("@p_mainservice_category_id",(object)Filter.MainServiceCategoryId??DBNull.Value),
                    new SqlParameter("@p_business_unit_id",(object)Filter.BusinessUnitId??DBNull.Value),
                    new SqlParameter("@p_currency_id",(object)Filter.CurrencyId??DBNull.Value),
                    new SqlParameter("@p_dateType",(object)Filter.DateType??DBNull.Value),
                 };
            var result = await context.ExecuteStoredProcedure<RptBillingAndRevenueBusinessUnit>("fn.report_billing_revenue_business_unit", param);
            return result;
        }
    }
}