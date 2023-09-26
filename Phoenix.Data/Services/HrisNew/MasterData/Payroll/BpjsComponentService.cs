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
    public interface IBpjsComponentService : IDataService<BpjsComponent>
    {
    }

    public class BpjsComponentService : IBpjsComponentService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage BpjsComponent
        /// </summary>
        /// <param name="context">Database context</param>
        public BpjsComponentService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(BpjsComponent entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params BpjsComponent[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(BpjsComponent entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params BpjsComponent[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(BpjsComponent entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params BpjsComponent[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(BpjsComponent entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(BpjsComponent entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BpjsComponent>> Get() => await context.BpjsComponents.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).OrderBy(x => x.CreatedOn).ToListAsync();

        public Task<List<BpjsComponent>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<BpjsComponent> Get(string Id) => await context.BpjsComponents.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
    }
}