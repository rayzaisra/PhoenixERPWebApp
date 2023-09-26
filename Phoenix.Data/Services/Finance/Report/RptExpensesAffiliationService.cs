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
    public interface IRptExpensesAffiliationService
    {
        Task<DataTable> GetExpensesAffiliation(RptExpensesAffiliationFilter Filter);
    }
    public class RptExpensesAffiliationService : IRptExpensesAffiliationService
    {
        readonly DataContext context;
        public RptExpensesAffiliationService(DataContext context)
        {
            this.context = context;
        }

        public async Task<DataTable> GetExpensesAffiliation(RptExpensesAffiliationFilter Filter)
        {
            var param = new SqlParameter[]
                {
                    new SqlParameter("@p_to_date",(object)Filter.EndingDate??DBNull.Value),
                    new SqlParameter("@p_currency_id",(object)Filter.CurrencyId??DBNull.Value),
                    new SqlParameter("@p_affiliation_id",(object)Filter.AffiliationId??DBNull.Value),
                    new SqlParameter("@p_coa_level",(object)Filter.CoaLevel??DBNull.Value),
                };

            var result = await context.ExecuteStoredProcedure("fn.report_expenses_affiliation", param);
            return result;
        }
    }
}
