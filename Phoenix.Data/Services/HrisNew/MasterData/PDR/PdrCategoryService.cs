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
    public interface IPdrCategoryService : IDataService<PdrCategory>
    {
    }

    public class PdrCategoryService : IPdrCategoryService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage PdrCategory
        /// </summary>
        /// <param name="context">Database context</param>
        public PdrCategoryService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(PdrCategory entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params PdrCategory[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(PdrCategory entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params PdrCategory[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(PdrCategory entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params PdrCategory[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(PdrCategory entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(PdrCategory entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PdrCategory>> Get() => await context.PdrCategorys.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).OrderBy(x => x.No).ToListAsync();

        public Task<List<PdrCategory>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<PdrCategory> Get(string Id) => await context.PdrCategorys.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
    }
}