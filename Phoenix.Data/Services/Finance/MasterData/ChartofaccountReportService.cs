using System;
using System.Collections.Generic;
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
    public interface IChartofaccountReportService : IDataService<ChartofaccountReport>
    {
    }

    public class ChartofaccountReportService : IChartofaccountReportService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage ChartofaccountReport
        /// </summary>
        /// <param name="context">Database context</param>
        public ChartofaccountReportService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(ChartofaccountReport entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params ChartofaccountReport[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(ChartofaccountReport entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params ChartofaccountReport[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(ChartofaccountReport entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params ChartofaccountReport[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(ChartofaccountReport entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(ChartofaccountReport entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ChartofaccountReport>> Get() => await context.ChartofaccountReports.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).ToListAsync();

        public Task<List<ChartofaccountReport>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<ChartofaccountReport> Get(string Id) => await context.ChartofaccountReports.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
    }
}