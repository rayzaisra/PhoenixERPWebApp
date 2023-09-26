using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Finance.Master;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface IChartOfAccountService
    {
        Task<int> AddLevel1Async(ChartOfAccount entity);
        Task<int> EditLevel1Async(ChartOfAccount entity);
        Task<int> AddLevel2Async(ChartOfAccount entity);
        Task<int> EditLevel2Async(ChartOfAccount entity);
        Task<int> AddLevel3Async(ChartOfAccount entity);
        Task<int> EditLevel3Async(ChartOfAccount entity);
        Task<int> AddLevel4Async(ChartOfAccount entity);
        Task<int> EditLevel4Async(ChartOfAccount entity);
        Task<int> AddLevel5Async(ChartOfAccount entity);
        Task<int> EditLevel5Async(ChartOfAccount entity);
        Task<int> DeleteSoftAsync(ChartOfAccount entity);
        Task<List<ChartOfAccount>> GetLevel1();
        Task<List<ChartOfAccount>> GetLevel2();
        Task<List<ChartOfAccount>> GetLevel3();
        Task<List<ChartOfAccount>> GetLevel4();
        Task<List<ChartOfAccount>> GetLevel5();
        Task<ChartOfAccount> GetLevel1(string Id);
        Task<ChartOfAccount> GetLevel2(string Id);
        Task<ChartOfAccount> GetLevel3(string Id);
        Task<ChartOfAccount> GetLevel4(string Id);
        Task<ChartOfAccount> GetLevel5(string Id);
        Task<ChartOfAccount> Get(string Id);
    }

    public class ChartOfAccountService : IChartOfAccountService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage Chartofaccount
        /// </summary>
        /// <param name="context">Database context</param>
        public ChartOfAccountService(DataContext context)
        {
            this.context = context;
        }
        public async Task<int> AddLevel1Async(ChartOfAccount entity)
        {
            await CheckCodeRec(entity.CodeRec);
            entity.Id = Guid.NewGuid().ToString();
            entity.Name2 = entity.Name1;
            entity.Name3 = entity.Name1;
            entity.Name4 = entity.Name1;
            entity.Name5 = entity.Name1;
            entity.Report = await ReportCOAById(entity.ChartofaccountReportId);
            entity.Level = "1";
            entity.Group = "G";
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> EditLevel1Async(ChartOfAccount entity)
        {
            await CheckCodeRec(entity.CodeRec);
            entity.Name2 = entity.Name1;
            entity.Name3 = entity.Name1;
            entity.Name4 = entity.Name1;
            entity.Name5 = entity.Name1;
            entity.Report = await ReportCOAById(entity.ChartofaccountReportId);
            entity.Level = "1";
            entity.Group = "G";

            var coa = await context.ChartOfAccounts.AsNoTracking().Where(x => x.Id == entity.Id).FirstOrDefaultAsync();
            if (coa != null)
            {
                var processUpdate = await context.ChartOfAccounts.AsNoTracking().Where(x => CoaCodeLevel(coa.CodeRec, x.CodeRec, 1) && x.Id != entity.Id).ToListAsync();
                foreach (var item in processUpdate)
                {
                    if (item.Level == "1")
                    {
                        item.Name2 = entity.Name1;
                        item.Name3 = entity.Name1;
                        item.Name4 = entity.Name1;
                        item.Name5 = entity.Name1;
                    }

                    item.Name1 = entity.Name1;
                    item.ChartofaccountReportId = entity.ChartofaccountReportId;
                    item.Report = entity.Report;
                    item.Condition = entity.Condition;
                    context.PhoenixEdit(item);
                }
            }

            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> AddLevel2Async(ChartOfAccount entity)
        {
            await CheckCodeRec(entity.CodeRec);
            entity.Id = Guid.NewGuid().ToString();
            entity.Name3 = entity.Name2;
            entity.Name4 = entity.Name2;
            entity.Name5 = entity.Name2;
            entity.Level = "2";
            entity.Group = "G";

            var reportCOA = await ReportCOAByCodeRec(entity.CodeRec1);
            entity.ChartofaccountReportId = reportCOA.ChartofaccountReportId;
            entity.Report = reportCOA.ReportName;
            entity.Condition = reportCOA.Condition;

            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> EditLevel2Async(ChartOfAccount entity)
        {
            await CheckCodeRec(entity.CodeRec);
            entity.Name3 = entity.Name2;
            entity.Name4 = entity.Name2;
            entity.Name5 = entity.Name2;
            entity.Level = "2";
            entity.Group = "G";

            var reportCOA = await ReportCOAByCodeRec(entity.CodeRec1);
            entity.ChartofaccountReportId = reportCOA.ChartofaccountReportId;
            entity.Report = reportCOA.ReportName;
            entity.Condition = reportCOA.Condition;
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> AddLevel3Async(ChartOfAccount entity)
        {
            await CheckCodeRec(entity.CodeRec);
            entity.Id = Guid.NewGuid().ToString();
            entity.Name4 = entity.Name3;
            entity.Name5 = entity.Name3;
            entity.Level = "3";
            entity.Group = "G";

            var reportCOA = await ReportCOAByCodeRec(entity.CodeRec1);
            entity.ChartofaccountReportId = reportCOA.ChartofaccountReportId;
            entity.Report = reportCOA.ReportName;
            entity.Condition = reportCOA.Condition;

            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> EditLevel3Async(ChartOfAccount entity)
        {
            await CheckCodeRec(entity.CodeRec);
            entity.Name4 = entity.Name3;
            entity.Name5 = entity.Name3;
            entity.Level = "3";
            entity.Group = "G";

            var reportCOA = await ReportCOAByCodeRec(entity.CodeRec1);
            entity.ChartofaccountReportId = reportCOA.ChartofaccountReportId;
            entity.Report = reportCOA.ReportName;
            entity.Condition = reportCOA.Condition;
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> AddLevel4Async(ChartOfAccount entity)
        {
            await CheckCodeRec(entity.CodeRec);
            entity.Id = Guid.NewGuid().ToString();
            entity.Name5 = entity.Name4;
            entity.Level = "4";
            entity.Group = "G";

            var reportCOA = await ReportCOAByCodeRec(entity.CodeRec1);
            entity.ChartofaccountReportId = reportCOA.ChartofaccountReportId;
            entity.Report = reportCOA.ReportName;
            entity.Condition = reportCOA.Condition;

            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> EditLevel4Async(ChartOfAccount entity)
        {
            await CheckCodeRec(entity.CodeRec);
            entity.Name5 = entity.Name4;
            entity.Level = "4";
            entity.Group = "G";

            var reportCOA = await ReportCOAByCodeRec(entity.CodeRec1);
            entity.ChartofaccountReportId = reportCOA.ChartofaccountReportId;
            entity.Report = reportCOA.ReportName;
            entity.Condition = reportCOA.Condition;
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> AddLevel5Async(ChartOfAccount entity)
        {
            await CheckCodeRec(entity.CodeRec);
            entity.Id = Guid.NewGuid().ToString();
            entity.Level = "5";
            entity.Group = "D";

            var reportCOA = await ReportCOAByCodeRec(entity.CodeRec1);
            entity.ChartofaccountReportId = reportCOA.ChartofaccountReportId;
            entity.Report = reportCOA.ReportName;
            entity.Condition = reportCOA.Condition;

            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> EditLevel5Async(ChartOfAccount entity)
        {
            await CheckCodeRec(entity.CodeRec);
            entity.Level = "5";
            entity.Group = "D";

            var reportCOA = await ReportCOAByCodeRec(entity.CodeRec1);
            entity.ChartofaccountReportId = reportCOA.ChartofaccountReportId;
            entity.Report = reportCOA.ReportName;
            entity.Condition = reportCOA.Condition;
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteSoftAsync(ChartOfAccount entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }
        private async Task<dynamic> ReportCOAByCodeRec(string codeRec1)
        {
            var result = await (from a in context.ChartofaccountReports.AsNoTracking()
                                join b in context.ChartOfAccounts on a.Id equals b.ChartofaccountReportId
                                where b.CodeRec == codeRec1
                                select new { b.ChartofaccountReportId, a.ReportName, b.Condition }).FirstOrDefaultAsync();
            return result;
        }

        private async Task CheckCodeRec(string codeRec)
        {
            var checkIsDelete = await context.ChartOfAccounts.AsNoTracking().Where(x => x.CodeRec == codeRec && (x.IsDeleted ?? false)).FirstOrDefaultAsync();
            if (checkIsDelete != null) context.ChartOfAccounts.Remove(checkIsDelete);
        }

        private async Task<string> ReportCOAById(string reportId) => await context.ChartofaccountReports.AsNoTracking().Where(x => x.Id == reportId).Select(x => x.ReportName).FirstOrDefaultAsync();
        public async Task<List<ChartOfAccount>> GetLevel1() => await context.ChartOfAccounts.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Level == "1").OrderBy(x => x.CodeRec).ToListAsync();
        public async Task<List<ChartOfAccount>> GetLevel2() => await context.ChartOfAccounts.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Level == "2").OrderBy(x => x.CodeRec).ToListAsync();
        public async Task<List<ChartOfAccount>> GetLevel3() => await context.ChartOfAccounts.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Level == "3").OrderBy(x => x.CodeRec).ToListAsync();
        public async Task<List<ChartOfAccount>> GetLevel4() => await context.ChartOfAccounts.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Level == "4").OrderBy(x => x.CodeRec).ToListAsync();
        public async Task<List<ChartOfAccount>> GetLevel5()
        {
            var result = await context.ChartOfAccounts.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Level == "5")
                .Select(x => new ChartOfAccount
                {
                    Id = x.Id,
                    CodeRec = x.CodeRec,
                    Name1 = $"{x.CodeRec.Split('.', StringSplitOptions.None)[0]}.0.00.00.00 {x.Name1}",
                    Name2 = $"{x.CodeRec.Split('.', StringSplitOptions.None)[0]}.{x.CodeRec.Split('.', StringSplitOptions.None)[1]}.00.00.00 {x.Name2}",
                    Name3 = $"{x.CodeRec.Split('.', StringSplitOptions.None)[0]}.{x.CodeRec.Split('.', StringSplitOptions.None)[1]}.{x.CodeRec.Split('.', StringSplitOptions.None)[2]}.00.00 {x.Name3}",
                    Name4 = $"{x.CodeRec.Split('.', StringSplitOptions.None)[0]}.{x.CodeRec.Split('.', StringSplitOptions.None)[1]}.{x.CodeRec.Split('.', StringSplitOptions.None)[2]}.{x.CodeRec.Split('.', StringSplitOptions.None)[3]}.00 {x.Name4}",
                    Name5 = x.Name5,
                    Level = x.Level,
                    Group = x.Group,
                    Condition = x.Condition,
                    Report = x.Report,
                    ReffTrxCode = x.ReffTrxCode,
                    ReffTrxId = x.ReffTrxId,
                    ChartofaccountReportId = x.ChartofaccountReportId
                }).OrderBy(x => x.CodeRec).ToListAsync();
            return result;
        }

        public async Task<ChartOfAccount> GetLevel1(string Id) => await context.ChartOfAccounts.AsNoTracking().Where(x => x.Id == Id && x.Level == "1").FirstOrDefaultAsync();

        public async Task<ChartOfAccount> GetLevel2(string Id)
        {
            var result = await context.ChartOfAccounts.AsNoTracking().Where(x => x.Id == Id && x.Level == "2").FirstOrDefaultAsync();
            result.CodeRec1 = await CodeRec(result.CodeRec, 1);
            return result;
        }

        public async Task<ChartOfAccount> GetLevel3(string Id)
        {
            var result = await context.ChartOfAccounts.AsNoTracking().Where(x => x.Id == Id && x.Level == "3").FirstOrDefaultAsync();
            result.CodeRec1 = await CodeRec(result.CodeRec, 1);
            result.CodeRec2 = await CodeRec(result.CodeRec, 2);
            return result;
        }

        public async Task<ChartOfAccount> GetLevel4(string Id)
        {
            var result = await context.ChartOfAccounts.AsNoTracking().Where(x => x.Id == Id && x.Level == "4").FirstOrDefaultAsync();
            result.CodeRec1 = await CodeRec(result.CodeRec, 1);
            result.CodeRec2 = await CodeRec(result.CodeRec, 2);
            result.CodeRec3 = await CodeRec(result.CodeRec, 3);
            return result;
        }

        public async Task<ChartOfAccount> GetLevel5(string Id)
        {
            var result = await context.ChartOfAccounts.AsNoTracking().Where(x => x.Id == Id && x.Level == "5").FirstOrDefaultAsync();
            result.CodeRec1 = await CodeRec(result.CodeRec, 1);
            result.CodeRec2 = await CodeRec(result.CodeRec, 2);
            result.CodeRec3 = await CodeRec(result.CodeRec, 3);
            result.CodeRec4 = await CodeRec(result.CodeRec, 4);
            return result;
        }

        private async Task<string> CodeRec(string codeRec, int level)
        {
            return await context.ChartOfAccounts.AsNoTracking().Where(x => CoaCodeLevel(codeRec, x.CodeRec, level) && x.Level == level.ToString()).Select(x => x.CodeRec).FirstOrDefaultAsync();
        }

        private bool CoaCodeLevel(string codeRec, string codeRecNew, int level)
        {
            var result = "";
            var resultNew = "";
            var array = codeRec.Split(".");
            var arrayNew = codeRecNew.Split(".");
            if (array.First() == arrayNew.First())
            {
                for (int i = 1; i <= array.Length; i++)
                {
                    if (i > level) break;
                    result += array[i - 1];
                    resultNew += arrayNew[i - 1];
                };
            }
            else
            {
                return false;
            }
            return (result == resultNew);
        }
        public async Task<ChartOfAccount> Get(string Id) => await context.ChartOfAccounts.AsNoTracking().Where(x => x.Id == Id).FirstOrDefaultAsync();
    }
}