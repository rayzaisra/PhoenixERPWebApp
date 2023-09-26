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
    public interface IRptBudgetService
    {
        Task<List<RptBudget>> Get(RptBudgetFilter Filter);
    }

    public class RptBudgetService : IRptBudgetService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage RptProfitAndLoss
        /// </summary>
        /// <param name="context">Database context</param>
        public RptBudgetService(DataContext context)
        {
            this.context = context;
        }

        public async Task<List<RptBudget>> Get(RptBudgetFilter Filter)
        {
            var result = await (from a in context.BudgetMasterHeaders
                                join b in context.BudgetMasters on a.Id equals b.BudgetMasterHeaderId
                                join c in context.BudgetMasterDetails on b.Id equals c.BudgetMasterId
                                join d in context.ChartOfAccounts on b.AccountId equals d.Id
                                join e in context.BusinessUnits on c.BusinessUnitId equals e.Id
                                where !(a.IsDeleted ?? false) && !(b.IsDeleted ?? false) && !(c.IsDeleted ?? false)
                                && a.BudgetYear == Filter.Year
                                && a.Source == Filter.Source
                                && (string.IsNullOrEmpty(Filter.BusinessUnitId) ? true : c.BusinessUnitId == Filter.BusinessUnitId)
                                && (Filter.DisplayAmountWaitingApproval ? (b.Status == StatusTransactionName.WaitingApproval || b.Status == StatusTransactionName.Approved) : b.Status == StatusTransactionName.Approved)
                                && (string.IsNullOrEmpty(Filter.Status) ? true : a.Status == Filter.Status)
                                && (Filter.Version == null ? true : a.Version == Filter.Version)
                                select new
                                {
                                    a.Status,
                                    CodeRec = (Filter.CoaLevel == "1" ? $"{d.CodeRec.Substring(0, 1)}.0.00.00.00" :
                                                Filter.CoaLevel == "2" ? $"{d.CodeRec.Substring(0, 3)}.00.00.00" :
                                                Filter.CoaLevel == "3" ? $"{d.CodeRec.Substring(0, 6)}.00.00" :
                                                Filter.CoaLevel == "4" ? $"{d.CodeRec.Substring(0, 9)}.00" :
                                                d.CodeRec),
                                    a.Version,
                                    BusinessUnitId = (string.IsNullOrEmpty(Filter.BusinessUnitId) ? "" : c.BusinessUnitId),
                                    UnitName = (string.IsNullOrEmpty(Filter.BusinessUnitId) ? "" : e.UnitName),
                                    c.Budget01,
                                    c.Budget02,
                                    c.Budget03,
                                    c.Budget04,
                                    c.Budget05,
                                    c.Budget06,
                                    c.Budget07,
                                    c.Budget08,
                                    c.Budget09,
                                    c.Budget10,
                                    c.Budget11,
                                    c.Budget12,
                                }
                                into tmp
                                group tmp by new
                                {
                                    tmp.Status,
                                    tmp.CodeRec,
                                    tmp.Version,
                                    tmp.BusinessUnitId,
                                    tmp.UnitName
                                } into g
                                select new RptBudget
                                {
                                    AccountId = Guid.NewGuid().ToString(),//g.Key.AccountId,
                                    CodeRec = g.Key.CodeRec,
                                    AccountName = $"{g.Key.CodeRec}-{context.ChartOfAccounts.Where(x => x.CodeRec == g.Key.CodeRec).Select(x => x.Name5).FirstOrDefault()}",
                                    BusinessUnitId = g.Key.BusinessUnitId,
                                    BusinessUnitName = g.Key.UnitName,
                                    Status = g.Key.Status,
                                    Version = g.Key.Version,
                                    Budget01 = g.Sum(x => x.Budget01),
                                    Budget02 = g.Sum(x => x.Budget02),
                                    Budget03 = g.Sum(x => x.Budget03),
                                    Budget04 = g.Sum(x => x.Budget04),
                                    Budget05 = g.Sum(x => x.Budget05),
                                    Budget06 = g.Sum(x => x.Budget06),
                                    Budget07 = g.Sum(x => x.Budget07),
                                    Budget08 = g.Sum(x => x.Budget08),
                                    Budget09 = g.Sum(x => x.Budget09),
                                    Budget10 = g.Sum(x => x.Budget10),
                                    Budget11 = g.Sum(x => x.Budget11),
                                    Budget12 = g.Sum(x => x.Budget12)
                                }).ToListAsync();
            return result;
        }
    }
}