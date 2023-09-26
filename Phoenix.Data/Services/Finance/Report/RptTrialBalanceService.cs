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
    public interface IRptTrialBalanceService
    {
        Task<List<RptTrialBalance>> Get(RptTrialBalanceFilter Filter);
        //Task<RptTrialBalanceValue> GetTotal(RptTrialBalanceFilter Filter);
    }
    public class RptTrialBalanceService : IRptTrialBalanceService
    {
        readonly DataContext context;
        public RptTrialBalanceService(DataContext context)
        {
            this.context = context;
        }

        public async Task<List<RptTrialBalance>> Get(RptTrialBalanceFilter Filter)
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

            var result = await context.ExecuteStoredProcedure<RptTrialBalance>("fn.report_trial_balance", param);
            return result;
        }

        //public async Task<RptTrialBalanceValue> GetTotal(RptTrialBalanceFilter Filter)
        //{
        //    var param = new SqlParameter[]
        //        {
        //            new SqlParameter("@p_legal_entity",(object)Filter.LegalEntityId??DBNull.Value),
        //            new SqlParameter("@p_business_unit_id",(object)Filter.BusinessUnitId??DBNull.Value),
        //            new SqlParameter("@p_to_date",(object)Filter.EndingDate??DBNull.Value),
        //            new SqlParameter("@p_currency_id",(object)Filter.CurrencyId??DBNull.Value),
        //            new SqlParameter("@p_display_zero_balance",Filter.DisplayZeroBalance),
        //            new SqlParameter("@p_coa_level",(object)Filter.CoaLevel??DBNull.Value),
        //        };

        //    var result = await context.ExecuteStoredProcedure<RptTrialBalanceValue>("fn.report_trial_balance_total", param);
        //    return result.FirstOrDefault();
        //}
    }
}
