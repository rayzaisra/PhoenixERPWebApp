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
    public interface IMainServiceCategoryService : IDataService<MainServiceCategory>
    {
    }

    public class MainServiceCategoryService : IMainServiceCategoryService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage MainServiceCategory
        /// </summary>
        /// <param name="context">Database context</param>
        public MainServiceCategoryService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(MainServiceCategory entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params MainServiceCategory[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(MainServiceCategory entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params MainServiceCategory[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(MainServiceCategory entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params MainServiceCategory[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(MainServiceCategory entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(MainServiceCategory entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MainServiceCategory>> Get() => await context.MainServiceCategorys.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).ToListAsync();

        public Task<List<MainServiceCategory>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<MainServiceCategory> Get(string Id) => await context.MainServiceCategorys.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
    }
}