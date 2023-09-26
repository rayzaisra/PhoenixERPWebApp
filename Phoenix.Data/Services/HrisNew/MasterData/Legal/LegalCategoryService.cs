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
    public interface ILegalCategoryService : IDataService<LegalCategory>
    {
    }

    public class LegalCategoryService : ILegalCategoryService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage LegalCategory
        /// </summary>
        /// <param name="context">Database context</param>
        public LegalCategoryService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(LegalCategory entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params LegalCategory[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(LegalCategory entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params LegalCategory[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(LegalCategory entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params LegalCategory[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(LegalCategory entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(LegalCategory entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<LegalCategory>> Get() => await context.LegalCategorys.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).OrderBy(x => x.CreatedOn).ToListAsync();

        public Task<List<LegalCategory>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<LegalCategory> Get(string Id) => await context.LegalCategorys.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
    }
}