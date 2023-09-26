using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    public interface IPayrollCompareableService
    {
        Task<List<PayrollCompareable>> Get(int month, int year);
    }

    public class PayrollCompareableService : IPayrollCompareableService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage LeaveBalance
        /// </summary>
        /// <param name="context">Database context</param>
        public PayrollCompareableService(DataContext context)
        {
            this.context = context;
        }

        public async Task<List<PayrollCompareable>> Get(int month, int year)
        {
            var param = new SqlParameter[]
            {
                new SqlParameter("@Month",month),
                new SqlParameter("@Year",year)
            };

            var result = await context.ExecuteStoredProcedure<PayrollCompareable>("uspGetPayrollCompareable", param);
            return result;
        }
    }
}