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
    public interface IFaultCategoryService : IDataService<FaultCategory>
    {
    }

    public class FaultCategoryService : IFaultCategoryService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage FaultCategory
        /// </summary>
        /// <param name="context">Database context</param>
        public FaultCategoryService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(FaultCategory entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params FaultCategory[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(FaultCategory entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params FaultCategory[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(FaultCategory entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params FaultCategory[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(FaultCategory entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(FaultCategory entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<FaultCategory>> Get() => await context.FaultCategorys.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).OrderBy(x => x.CreatedOn).ToListAsync();

        public Task<List<FaultCategory>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<FaultCategory> Get(string Id) => await context.FaultCategorys.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
    }
}