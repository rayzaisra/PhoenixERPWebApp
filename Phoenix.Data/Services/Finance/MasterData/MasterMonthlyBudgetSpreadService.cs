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
    public interface IMasterMonthlyBudgetSpreadService : IDataService<MasterMonthlyBudgetSpread>
    {
    }

    public class MasterMonthlyBudgetSpreadService : IMasterMonthlyBudgetSpreadService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage MasterMonthlyBudgetSpread
        /// </summary>
        /// <param name="context">Database context</param>
        public MasterMonthlyBudgetSpreadService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(MasterMonthlyBudgetSpread entity)
        {
            entity.Id = await TransID.GetTransId(context, Code.MonthlyBudgetSpread, DateTime.Today);
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params MasterMonthlyBudgetSpread[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(MasterMonthlyBudgetSpread entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params MasterMonthlyBudgetSpread[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(MasterMonthlyBudgetSpread entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params MasterMonthlyBudgetSpread[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(MasterMonthlyBudgetSpread entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(MasterMonthlyBudgetSpread entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MasterMonthlyBudgetSpread>> Get() => await context.MasterMonthlyBudgetSpreads.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).ToListAsync();

        public Task<List<MasterMonthlyBudgetSpread>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<MasterMonthlyBudgetSpread> Get(string Id) => await context.MasterMonthlyBudgetSpreads.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
    }
}