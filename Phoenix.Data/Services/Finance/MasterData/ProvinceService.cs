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
    public interface IProvinceService : IDataService<Province>
    {
    }

    public class ProvinceService : IProvinceService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage Province
        /// </summary>
        /// <param name="context">Database context</param>
        public ProvinceService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(Province entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params Province[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(Province entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params Province[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(Province entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params Province[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(Province entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(Province entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Province>> Get()
        {
            var result = await (from a in context.Provinces
                                join b in context.Countrys on a.CountryId equals b.Id
                                where !(a.IsDeleted ?? false)
                                select new Province
                                {
                                    Id = a.Id,
                                    ProvinceName = a.ProvinceName,
                                    CountryId = a.CountryId,
                                    CountryName = b.CountryName,
                                    CreatedOn = a.CreatedOn
                                }).ToListAsync();
            return result;
        }

        public Task<List<Province>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<Province> Get(string Id) => await context.Provinces.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
    }
}