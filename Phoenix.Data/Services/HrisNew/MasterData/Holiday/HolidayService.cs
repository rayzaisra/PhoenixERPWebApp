using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phoenix.ApiExtension.Extensions;
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface IHolidayService : IDataService<Holiday>
    {
        Task<dynamic> GetHolidayDateList(DateTime startDate, DateTime endDate);
    }

    public class HolidayService : IHolidayService
    {
        readonly DataContext context;
        /// <summary>
        /// And endpoint to manage Termination
        /// </summary>
        /// <param name="context">Database context</param>
        public HolidayService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(Holiday entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params Holiday[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(Holiday entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params Holiday[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(Holiday entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params Holiday[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(Holiday entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(Holiday entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Holiday>> Get() => await context.Holidays.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).ToListAsync();

        public Task<List<Holiday>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<Holiday> Get(string Id) => await context.Holidays.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();

        public async Task<dynamic> GetHolidayDateList(DateTime startDate, DateTime endDate)
        {
            return await context.Holidays.Where(x => x.Date >= startDate && x.Date <= endDate).Select(x => new { x.Id, x.Name, x.Date }).ToListAsync();
        }
    }
}