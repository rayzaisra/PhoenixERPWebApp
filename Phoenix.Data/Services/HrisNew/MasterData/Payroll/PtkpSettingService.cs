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
    public interface IPtkpSettingService : IDataService<PtkpSetting>
    {
    }

    public class PtkpSettingService : IPtkpSettingService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage PtkpSetting
        /// </summary>
        /// <param name="context">Database context</param>
        public PtkpSettingService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(PtkpSetting entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params PtkpSetting[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(PtkpSetting entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params PtkpSetting[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(PtkpSetting entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params PtkpSetting[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(PtkpSetting entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(PtkpSetting entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PtkpSetting>> Get() => await context.PtkpSettings.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).OrderBy(x=>x.Name).ToListAsync();

        public Task<List<PtkpSetting>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<PtkpSetting> Get(string Id) => await context.PtkpSettings.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
    }
}