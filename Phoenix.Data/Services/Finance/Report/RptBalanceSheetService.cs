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
    public interface IRptBalanceSheetService
    {
        Task<List<RptBalanceSheet>> GetBalanceSheet(RptBalanceSheetFilter Filter);
        Task<DataTable> GetBalanceSheetAffiliation(RptBalanceSheetAffiliationFilter Filter);
        Task<List<RptBalanceSheetComparison>> GetBalanceSheetComparison(RptBalanceSheetFilter Filter);
    }
    public class RptBalanceSheetService : IRptBalanceSheetService
    {
        readonly DataContext context;
        public RptBalanceSheetService(DataContext context)
        {
            this.context = context;
        }

        public async Task<List<RptBalanceSheet>> GetBalanceSheet(RptBalanceSheetFilter Filter)
        {
            var param = new SqlParameter[]
                {
                    new SqlParameter("@p_legal_entity",(object)Filter.LegalEntityId??DBNull.Value),
                    new SqlParameter("@p_business_unit_id",(object)Filter.BusinessUnitId??DBNull.Value),
                    new SqlParameter("@p_to_date",(object)Filter.EndingDate??DBNull.Value),
                    new SqlParameter("@p_currency_id",(object)Filter.CurrencyId??DBNull.Value),
                    new SqlParameter("@p_display_zero_balance",Filter.DisplayZeroBalance),
                    new SqlParameter("@p_coa_level",(object)Filter.CoaLevel??DBNull.Value),
                };

            var result = await context.ExecuteStoredProcedure<RptBalanceSheet>("fn.report_balance_sheet", param);
            return result;
        }

        public async Task<DataTable> GetBalanceSheetAffiliation(RptBalanceSheetAffiliationFilter Filter)
        {
            var param = new SqlParameter[]
                {
                    new SqlParameter("@p_affiliation",(object)Filter.AffiliationId??DBNull.Value),
                    new SqlParameter("@p_to_date",(object)Filter.EndingDate??DBNull.Value),
                    new SqlParameter("@p_currency_id",(object)Filter.CurrencyId??DBNull.Value),
                    new SqlParameter("@p_display_zero_balance",Filter.DisplayZeroBalance),
                    new SqlParameter("@p_coa_level",(object)Filter.CoaLevel??DBNull.Value),
                    //new SqlParameter("@p_type_Date",(object)Filter.Type??DBNull.Value),
                };

            var result = await context.ExecuteStoredProcedure("fn.report_balance_sheet_affiliation", param);
            return result;
        }

        public async Task<List<RptBalanceSheetComparison>> GetBalanceSheetComparison(RptBalanceSheetFilter Filter)
        {
            var param = new SqlParameter[]
                {
                    new SqlParameter("@p_legal_entity",(object)Filter.LegalEntityId??DBNull.Value),
                    new SqlParameter("@p_business_unit_id",(object)Filter.BusinessUnitId??DBNull.Value),
                    new SqlParameter("@p_to_date",(object)Filter.EndingDate??DBNull.Value),
                    new SqlParameter("@p_currency_id",(object)Filter.CurrencyId??DBNull.Value),
                    new SqlParameter("@p_display_zero_balance",Filter.DisplayZeroBalance),
                    new SqlParameter("@p_coa_level",(object)Filter.CoaLevel??DBNull.Value),
                };

            var result = await context.ExecuteStoredProcedure<RptBalanceSheetComparison>("fn.report_balance_sheet_comparison", param);
            return result;
        }

    }
}
