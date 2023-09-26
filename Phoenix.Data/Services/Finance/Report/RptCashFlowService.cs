using System;
using System.Collections.Generic;
using System.Data;
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
    public interface IRptCashFlowService
    {
        Task<DataTable> Get(RptCashFlowFilter Filter);
    }

    public class RptCashFlowService : IRptCashFlowService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage RptProfitAndLoss
        /// </summary>
        /// <param name="context">Database context</param>
        public RptCashFlowService(DataContext context)
        {
            this.context = context;
        }

        public async Task<DataTable> Get(RptCashFlowFilter Filter)
        {
            var param = new SqlParameter[]
               {
                    new SqlParameter("@p_from_date",(object)Filter.StartDate??DBNull.Value),
                    new SqlParameter("@p_to_date",(object)Filter.EndingDate??DBNull.Value),
                    new SqlParameter("@p_currency_id",(object)Filter.CurrencyId??DBNull.Value),
                    new SqlParameter("@p_bank_id",(object)Filter.BankId??DBNull.Value),
               };

            var result = await context.ExecuteStoredProcedure("fn.report_cash_flow", param);
            return result;
        }
    }
}