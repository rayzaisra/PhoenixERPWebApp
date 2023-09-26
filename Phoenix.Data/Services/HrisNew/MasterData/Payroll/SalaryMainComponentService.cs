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
    public interface ISalaryMainComponentService : IDataService<SalaryMainComponent>
    {
    }

    public class SalaryMainComponentService : ISalaryMainComponentService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage SalaryMainComponent
        /// </summary>
        /// <param name="context">Database context</param>
        public SalaryMainComponentService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(SalaryMainComponent entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params SalaryMainComponent[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(SalaryMainComponent entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params SalaryMainComponent[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(SalaryMainComponent entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params SalaryMainComponent[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(SalaryMainComponent entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(SalaryMainComponent entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SalaryMainComponent>> Get() => await context.SalaryMainComponents.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).OrderBy(x => x.CreatedOn).ToListAsync();

        public Task<List<SalaryMainComponent>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<SalaryMainComponent> Get(string Id) => await context.SalaryMainComponents.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
    }
}