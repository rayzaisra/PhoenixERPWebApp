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
    public interface IRptPreFinancingService
    {
        Task<List<RptPreFinancing>> Get(RptPreFinancingFilter Filter);
    }
    public class RptPreFinancingService : IRptPreFinancingService
    {
        readonly DataContext context;
        public RptPreFinancingService(DataContext context)
        {
            this.context = context;
        }

        public async Task<List<RptPreFinancing>> Get(RptPreFinancingFilter Filter)
        {
            int? year = null;
            if (Filter.EndingDate != null) year = Filter.EndingDate.Value.Year;
            var param = new SqlParameter[]
                {
                    new SqlParameter("@p_legal_entity_id",(object)Filter.LegalEntityId??DBNull.Value),
                    new SqlParameter("@p_business_unit_id",(object)Filter.BusinessUnitId??DBNull.Value),
                    new SqlParameter("@p_company_id",(object)Filter.CompanyId??DBNull.Value),
                    new SqlParameter("@p_year",(object)year??DBNull.Value),
                    new SqlParameter("@p_currency_id",(object)Filter.CurrencyId??DBNull.Value),
                };

            var result = await context.ExecuteStoredProcedure<RptPreFinancing>("fn.report_pre_financing", param);
            return result;
        }
    }
}
