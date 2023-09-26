using System;
using System.Collections.Generic;
using System.Data;
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
    public interface ISummaryPayrollService
    {
        Task<DataTable> Get(int month, int year, string legalEntityId,string divisionId);
    }

    public class SummaryPayrollService : ISummaryPayrollService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage LeaveBalance
        /// </summary>
        /// <param name="context">Database context</param>
        public SummaryPayrollService(DataContext context)
        {
            this.context = context;
        }
        public async Task<DataTable> Get(int month, int year, string legalEntityId, string divisionId)
        {
            var UserSessionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
            var EmployeeSubcomponentAdditional = await context.EmployeeSubcomponentAdditionals.Where(x => x.IsEncrypt == true && x.IsDeleted == false).AsNoTracking().ToListAsync();
            if (EmployeeSubcomponentAdditional.Count > 0)
            {
                foreach (var item in EmployeeSubcomponentAdditional)
                {
                    item.AdditionalAmount = Phoenix.ApiExtension.Helpers.SecurityHelper.DecryptString(item.AdditionalAmount);
                    item.AdditionalPercent = Phoenix.ApiExtension.Helpers.SecurityHelper.DecryptString(item.AdditionalPercent);
                    item.IsEncrypt = false;
                    context.PhoenixEdit(item);
                }
            }

            var EmployeeSubcomponentDeduc = await context.EmployeeSubcomponentDeductions.Where(x => x.IsEncrypt == true && x.IsDeleted == false).AsNoTracking().ToListAsync();
            if (EmployeeSubcomponentDeduc.Count > 0)
            {
                foreach (var item in EmployeeSubcomponentDeduc)
                {
                    item.Amount = Phoenix.ApiExtension.Helpers.SecurityHelper.DecryptString(item.Amount);
                    item.Percent = Phoenix.ApiExtension.Helpers.SecurityHelper.DecryptString(item.Percent);
                    item.IsEncrypt = false;
                    context.PhoenixEdit(item);
                }
            }

            await context.SaveChangesAsync();

            var paramType = new SqlParameter[] {
                                new SqlParameter("@Month", month),
                                new SqlParameter("@Year", year),
                                new SqlParameter("@LegalEntityId", legalEntityId),
                                new SqlParameter("@divisionId",divisionId)
                            };

            var result = await context.ExecuteStoredProcedure("uspGetSummaryPayrollList", paramType);

            //EmployeeSubcomponentAdditional = await context.EmployeeSubcomponentAdditionals.Where(x => x.IsEncrypt == false && x.IsDeleted == false).AsNoTracking().ToListAsync();
            //if (EmployeeSubcomponentAdditional.Count > 0)
            //{
            //    foreach (var item in EmployeeSubcomponentAdditional)
            //    {
            //        item.AdditionalAmount = Phoenix.ApiExtension.Helpers.SecurityHelper.EncryptString(item.AdditionalAmount);
            //        item.AdditionalPercent = Phoenix.ApiExtension.Helpers.SecurityHelper.EncryptString(item.AdditionalPercent);
            //        item.IsEncrypt = true;
            //        context.PhoenixEdit(item);
            //    }
            //}
            //EmployeeSubcomponentDeduc = await context.EmployeeSubcomponentDeductions.Where(x => x.IsEncrypt == false && x.IsDeleted == false).AsNoTracking().ToListAsync();
            //if (EmployeeSubcomponentDeduc.Count > 0)
            //{
            //    foreach (var item in EmployeeSubcomponentDeduc)
            //    {
            //        item.Amount = Phoenix.ApiExtension.Helpers.SecurityHelper.EncryptString(item.Amount);
            //        item.Percent = Phoenix.ApiExtension.Helpers.SecurityHelper.EncryptString(item.Percent);
            //        item.IsEncrypt = true;
            //        context.PhoenixEdit(item);
            //    }
            //}
            //await context.SaveChangesAsync();

            //var paramType = new SqlParameter[] {
            //                    new SqlParameter("@Month", month),
            //                    new SqlParameter("@Year", year),
            //                    new SqlParameter("@LegalEntityId", legalEntityId)
            //                };

            //var result = await context.ExecuteStoredProcedure("uspGetSummaryPayrollList", paramType);
            return result;
        }
    }
}