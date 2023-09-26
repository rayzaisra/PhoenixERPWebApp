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
    public interface IDeductionComponentService : IDataService<DeductionComponent>
    {
    }

    public class DeductionComponentService : IDeductionComponentService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage DeductionComponent
        /// </summary>
        /// <param name="context">Database context</param>
        public DeductionComponentService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(DeductionComponent entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params DeductionComponent[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(DeductionComponent entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params DeductionComponent[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(DeductionComponent entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params DeductionComponent[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(DeductionComponent entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(DeductionComponent entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DeductionComponent>> Get() => await context.DeductionComponents.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).OrderBy(x => x.CreatedOn).ToListAsync();

        public Task<List<DeductionComponent>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<DeductionComponent> Get(string Id) => await context.DeductionComponents.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
    }
}