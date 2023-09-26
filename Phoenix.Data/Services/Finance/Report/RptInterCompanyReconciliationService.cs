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
    public interface IRptInterCompanyReconciliationService
    {
        Task<List<RptInterCompanyReconciliation>> Get(RptInterCompanyReconciliationFilter Filter);
    }
    public class RptInterCompanyReconciliationService : IRptInterCompanyReconciliationService
    {
        readonly DataContext context;
        public RptInterCompanyReconciliationService(DataContext context)
        {
            this.context = context;
        }

        public async Task<List<RptInterCompanyReconciliation>> Get(RptInterCompanyReconciliationFilter Filter)
        {
            var legalEntity = ""; if (Filter.MultiLegalEntityId != null) legalEntity = string.Join(",", Filter.MultiLegalEntityId);
            var company = ""; if (Filter.MultiCompanyId != null) company = string.Join(",", Filter.MultiCompanyId);
            var param = new SqlParameter[]
                {
                    new SqlParameter("@p_to_date",(object)Filter.EndingDate??DBNull.Value),
                    new SqlParameter("@p_currency_id",(object)Filter.CurrencyId??DBNull.Value),
                    new SqlParameter("@p_multi_legal_entity_id",legalEntity),
                    new SqlParameter("@p_multi_company_id",company),
                };

            var result = await context.ExecuteStoredProcedure<RptInterCompanyReconciliation>("fn.report_inter_company_reconciliation", param);
            return result;
        }
    }
}
