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
    public interface IRptBudgetBalanceService
    {
        Task<List<RptBudgetBalance>> Get(RptBudgetBalanceFilter Filter);
    }

    public class RptBudgetBalanceService : IRptBudgetBalanceService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage RptProfitAndLoss
        /// </summary>
        /// <param name="context">Database context</param>
        public RptBudgetBalanceService(DataContext context)
        {
            this.context = context;
        }

        public async Task<List<RptBudgetBalance>> Get(RptBudgetBalanceFilter Filter)
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
                                    c.AcrBudget01,
                                    c.AcrBudget02,
                                    c.AcrBudget03,
                                    c.AcrBudget04,
                                    c.AcrBudget05,
                                    c.AcrBudget06,
                                    c.AcrBudget07,
                                    c.AcrBudget08,
                                    c.AcrBudget09,
                                    c.AcrBudget10,
                                    c.AcrBudget11,
                                    c.AcrBudget12,
                                    c.RealBudgetAcr01,
                                    c.RealBudgetAcr02,
                                    c.RealBudgetAcr03,
                                    c.RealBudgetAcr04,
                                    c.RealBudgetAcr05,
                                    c.RealBudgetAcr06,
                                    c.RealBudgetAcr07,
                                    c.RealBudgetAcr08,
                                    c.RealBudgetAcr09,
                                    c.RealBudgetAcr10,
                                    c.RealBudgetAcr11,
                                    c.RealBudgetAcr12,
                                    c.UsedBudget01,
                                    c.UsedBudget02,
                                    c.UsedBudget03,
                                    c.UsedBudget04,
                                    c.UsedBudget05,
                                    c.UsedBudget06,
                                    c.UsedBudget07,
                                    c.UsedBudget08,
                                    c.UsedBudget09,
                                    c.UsedBudget10,
                                    c.UsedBudget11,
                                    c.UsedBudget12,
                                    a.Status,
                                    CodeRec = (Filter.CoaLevel == "1" ? $"{d.CodeRec.Substring(0, 1)}.0.00.00.00" :
                                                Filter.CoaLevel == "2" ? $"{d.CodeRec.Substring(0, 3)}.00.00.00" :
                                                Filter.CoaLevel == "3" ? $"{d.CodeRec.Substring(0, 6)}.00.00" :
                                                Filter.CoaLevel == "4" ? $"{d.CodeRec.Substring(0, 9)}.00" :
                                                d.CodeRec),
                                    a.Version,
                                    BusinessUnitId = (string.IsNullOrEmpty(Filter.BusinessUnitId) ? "" : c.BusinessUnitId),
                                    UnitName = (string.IsNullOrEmpty(Filter.BusinessUnitId) ? "" : e.UnitName)
                                }
                                into tmp
                                group tmp by new
                                {
                                    tmp.Status,
                                    tmp.CodeRec,
                                    tmp.Version,
                                    tmp.BusinessUnitId,
                                    tmp.UnitName
                                }
                                into g
                                select new RptBudgetBalance
                                {
                                    AccountId = Guid.NewGuid().ToString(),//g.Key.AccountId,
                                    CodeRec = g.Key.CodeRec,
                                    AccountName = $"{g.Key.CodeRec}-{context.ChartOfAccounts.Where(x => x.CodeRec == g.Key.CodeRec).Select(x => x.Name5).FirstOrDefault()}",
                                    BusinessUnitId = g.Key.BusinessUnitId,
                                    BusinessUnitName = g.Key.UnitName,
                                    TotalBudget = g.Sum(x => x.Budget01) + g.Sum(x => x.Budget02) + g.Sum(x => x.Budget03) + g.Sum(x => x.Budget04) + g.Sum(x => x.Budget05) + g.Sum(x => x.Budget06) +
                                                g.Sum(x => x.Budget07) + g.Sum(x => x.Budget08) + g.Sum(x => x.Budget09) + g.Sum(x => x.Budget10) + g.Sum(x => x.Budget11) + g.Sum(x => x.Budget12),
                                    TotalPR = g.Sum(x => x.AcrBudget01) + g.Sum(x => x.AcrBudget02) + g.Sum(x => x.AcrBudget03) + g.Sum(x => x.AcrBudget04) + g.Sum(x => x.AcrBudget05) + g.Sum(x => x.AcrBudget06) +
                                                g.Sum(x => x.AcrBudget07) + g.Sum(x => x.AcrBudget08) + g.Sum(x => x.AcrBudget09) + g.Sum(x => x.AcrBudget10) + g.Sum(x => x.AcrBudget11) + g.Sum(x => x.AcrBudget12),
                                    TotalPO = g.Sum(x => x.RealBudgetAcr01) + g.Sum(x => x.RealBudgetAcr02) + g.Sum(x => x.RealBudgetAcr03) + g.Sum(x => x.RealBudgetAcr04) + g.Sum(x => x.RealBudgetAcr05) + g.Sum(x => x.RealBudgetAcr06) +
                                                g.Sum(x => x.RealBudgetAcr07) + g.Sum(x => x.RealBudgetAcr08) + g.Sum(x => x.RealBudgetAcr09) + g.Sum(x => x.RealBudgetAcr10) + g.Sum(x => x.RealBudgetAcr11) + g.Sum(x => x.RealBudgetAcr12),
                                    TotalAP = g.Sum(x => x.UsedBudget01) + g.Sum(x => x.UsedBudget02) + g.Sum(x => x.UsedBudget03) + g.Sum(x => x.UsedBudget04) + g.Sum(x => x.UsedBudget05) + g.Sum(x => x.UsedBudget06) +
                                                g.Sum(x => x.UsedBudget07) + g.Sum(x => x.UsedBudget08) + g.Sum(x => x.UsedBudget09) + g.Sum(x => x.UsedBudget10) + g.Sum(x => x.UsedBudget11) + g.Sum(x => x.UsedBudget12),
                                }).ToListAsync();
            return result;
        }
    }
}