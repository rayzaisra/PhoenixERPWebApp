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
    public interface IMasterWorkingTimeService : IDataService<MasterWorkingTime>
    {
    }

    public class MasterWorkingTimeService : IMasterWorkingTimeService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage MasterWorkingTime
        /// </summary>
        /// <param name="context">Database context</param>
        public MasterWorkingTimeService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(MasterWorkingTime entity)
        {
            var list = await context.MasterWorkingTimes.Where(x => x.Id != entity.Id && x.IsDefault == true).ToListAsync();
            Parallel.ForEach(list, (item) =>
            {
                item.IsDefault = false;
                context.PhoenixEdit(item);
            });

            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params MasterWorkingTime[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(MasterWorkingTime entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params MasterWorkingTime[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(MasterWorkingTime entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params MasterWorkingTime[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(MasterWorkingTime entity)
        {
            var list = await context.MasterWorkingTimes.Where(x => x.Id != entity.Id && x.IsDefault == true).ToListAsync();
            Parallel.ForEach(list, (item) =>
            {
                item.IsDefault = false;
                context.PhoenixEdit(item);
            });

            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(MasterWorkingTime entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MasterWorkingTime>> Get() => await context.MasterWorkingTimes.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).ToListAsync();

        public Task<List<MasterWorkingTime>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<MasterWorkingTime> Get(string Id) => await context.MasterWorkingTimes.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
    }
}