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
    public interface ICountryService : IDataService<Country>
    {
    }

    public class CountryService : ICountryService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage Country
        /// </summary>
        /// <param name="context">Database context</param>
        public CountryService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(Country entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params Country[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(Country entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params Country[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(Country entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params Country[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(Country entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(Country entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Country>> Get() => await context.Countrys.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).ToListAsync();

        public Task<List<Country>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<Country> Get(string Id) => await context.Countrys.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
    }
}