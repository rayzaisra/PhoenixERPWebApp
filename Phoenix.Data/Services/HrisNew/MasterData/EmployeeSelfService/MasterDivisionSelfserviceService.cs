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
    public interface IMasterDivisionSelfserviceService : IDataService<MasterDivisionSelfservice>
    {
    }

    public class MasterDivisionSelfserviceService : IMasterDivisionSelfserviceService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage MasterDivisionSelfservice
        /// </summary>
        /// <param name="context">Database context</param>
        public MasterDivisionSelfserviceService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(MasterDivisionSelfservice entity)
        {
            if (context.MasterDivisionSelfservices.Where(x => x.BusinessUnitId == entity.BusinessUnitId && !(x.IsDeleted ?? false)).Count() > 0)
            {
                throw new Exception("data already exists");
            }

            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params MasterDivisionSelfservice[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(MasterDivisionSelfservice entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params MasterDivisionSelfservice[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(MasterDivisionSelfservice entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params MasterDivisionSelfservice[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(MasterDivisionSelfservice entity)
        {
            if (context.MasterDivisionSelfservices.Where(x => x.Id != entity.Id && x.BusinessUnitId == entity.BusinessUnitId && !(x.IsDeleted ?? false)).Count() > 0)
            {
                throw new Exception("data already exists");
            }

            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(MasterDivisionSelfservice entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MasterDivisionSelfservice>> Get() => await context.MasterDivisionSelfservices.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).OrderBy(x => x.CreatedOn).ToListAsync();

        public Task<List<MasterDivisionSelfservice>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<MasterDivisionSelfservice> Get(string Id) => await context.MasterDivisionSelfservices.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
    }
}