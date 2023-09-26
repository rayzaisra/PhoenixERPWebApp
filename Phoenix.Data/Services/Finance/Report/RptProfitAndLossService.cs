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
    public interface IRptProfitAndLossService
    {
        Task<DataTable> Get(RptProfitAndLossFilter Filter);
    }

    public class RptProfitAndLossService : IRptProfitAndLossService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage RptProfitAndLoss
        /// </summary>
        /// <param name="context">Database context</param>
        public RptProfitAndLossService(DataContext context)
        {
            this.context = context;
        }

        public async Task<DataTable> Get(RptProfitAndLossFilter Filter)
        {
            var param = new SqlParameter[]
               {
                    new SqlParameter("@p_legal_entity",(object)Filter.LegalEntityId??DBNull.Value),
                    new SqlParameter("@p_business_unit_id",(object)Filter.BusinessUnitId??DBNull.Value),
                    new SqlParameter("@p_from_date",(object)Filter.StartingDate??DBNull.Value),
                    new SqlParameter("@p_to_date",(object)Filter.EndingDate??DBNull.Value),
                    new SqlParameter("@p_currency_id",(object)Filter.CurrencyId??DBNull.Value),
                    new SqlParameter("@p_display_zero_balance",Filter.DisplayZeroBalance),
                    new SqlParameter("@p_coa_level",(object)Filter.CoaLevel??DBNull.Value),
                    new SqlParameter("@p_type_Date",(object)Filter.Type??DBNull.Value),
               };

            var result = await context.ExecuteStoredProcedure("fn.report_profit_and_loss", param);
            return result;
        }
    }
}