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
    public interface ICityService : IDataService<City>
    {
    }

    public class CityService : ICityService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage City
        /// </summary>
        /// <param name="context">Database context</param>
        public CityService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(City entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params City[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(City entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params City[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(City entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params City[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(City entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(City entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<City>> Get()
        {
            var result = await (from a in context.Citys
                                join b in context.Provinces on a.ProvinceId equals b.Id
                                join c in context.Countrys on a.CountryId equals c.Id
                                where !(a.IsDeleted ?? false)
                                select new City
                                {
                                    Id = a.Id,
                                    ProvinceId = a.ProvinceId,
                                    ProvinceName = b.ProvinceName,
                                    CountryId = a.CountryId,
                                    CountryName = c.CountryName,
                                    CityName = a.CityName,
                                    CreatedOn = a.CreatedOn
                                }).ToListAsync();
            return result;
        }

        public Task<List<City>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<City> Get(string Id) => await context.Citys.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
    }
}