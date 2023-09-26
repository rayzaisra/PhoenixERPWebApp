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
    public interface IMasterOutletService : IDataService<MasterOutlet>
    {
    }

    public class MasterOutletService : IMasterOutletService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage MasterOutlet
        /// </summary>
        /// <param name="context">Database context</param>
        public MasterOutletService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(MasterOutlet entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params MasterOutlet[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(MasterOutlet entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params MasterOutlet[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(MasterOutlet entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params MasterOutlet[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(MasterOutlet entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(MasterOutlet entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MasterOutlet>> Get()
        {
            var result = await (from a in context.MasterOutlets
                                join b in context.Companys on a.ClientId equals b.Id
                                join e in context.Countrys on a.CountryId equals e.Id
                                join c in context.Provinces on a.ProvinceId equals c.Id
                                join d in context.Citys on a.CityId equals d.Id
                                where !(a.IsDeleted ?? false) && (b.IsClient ?? false)
                                select new MasterOutlet
                                {
                                    Id = a.Id,
                                    OutletName = a.OutletName,
                                    Address = a.Address,
                                    CountryName = e.CountryName,
                                    ProvinceName = c.ProvinceName,
                                    CityName = d.CityName,
                                    ZipCode = a.ZipCode,
                                    Phone = a.Phone,
                                    MobilePhone = a.MobilePhone,
                                    Email = a.Email,
                                    PicName = a.PicName,
                                    TypeOutlet = a.TypeOutlet,
                                    ClientName = b.CompanyName
                                }).ToListAsync();
            return result;
        }

        public Task<List<MasterOutlet>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<MasterOutlet> Get(string Id) => await context.MasterOutlets.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
    }
}