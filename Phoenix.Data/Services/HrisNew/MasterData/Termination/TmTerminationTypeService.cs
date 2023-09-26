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
    public interface ITmTerminationTypeService : IDataService<TmTerminationType>
    {
    }

    public class TmTerminationTypeService : ITmTerminationTypeService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage TmTerminationType
        /// </summary>
        /// <param name="context">Database context</param>
        public TmTerminationTypeService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(TmTerminationType entity)
        {
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params TmTerminationType[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(TmTerminationType entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params TmTerminationType[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(TmTerminationType entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params TmTerminationType[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(TmTerminationType entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(TmTerminationType entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TmTerminationType>> Get() => await context.TmTerminationTypes.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).OrderBy(x => x.CreatedOn).ToListAsync();

        public Task<List<TmTerminationType>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<TmTerminationType> Get(string Id) => await context.TmTerminationTypes.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
    }
}