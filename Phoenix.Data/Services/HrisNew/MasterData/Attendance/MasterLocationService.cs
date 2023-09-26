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
    public interface IMasterLocationService : IDataService<MasterLocation>
    {
    }

    public class MasterLocationService : IMasterLocationService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage MasterLocation
        /// </summary>
        /// <param name="context">Database context</param>
        public MasterLocationService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(MasterLocation entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params MasterLocation[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(MasterLocation entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params MasterLocation[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(MasterLocation entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params MasterLocation[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(MasterLocation entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(MasterLocation entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MasterLocation>> Get() => await context.MasterLocations.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).OrderBy(x => x.CreatedOn).ToListAsync();

        public Task<List<MasterLocation>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<MasterLocation> Get(string Id) => await context.MasterLocations.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
    }
}