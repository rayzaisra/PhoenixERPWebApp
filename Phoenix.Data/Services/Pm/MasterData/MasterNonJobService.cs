using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Pm.MasterData;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data.Services.Pm.MasterData
{
    public interface IMasterNonJobService : IDataService<MasterNonJob>
    {
    }

    public class MasterNonJobService : IMasterNonJobService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage MasterOutlet
        /// </summary>
        /// <param name="context">Database context</param>
        public MasterNonJobService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(MasterNonJob entity)
        {
            entity.IsActive = true;
            entity.IsDeleted = false;
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params MasterNonJob[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(MasterNonJob entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params MasterNonJob[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(MasterNonJob entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params MasterNonJob[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(MasterNonJob entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(MasterNonJob entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MasterNonJob>> Get()
        {
            var result = await (from a in context.MasterNonjob
                                join b in context.BusinessUnits on a.BusinessUnitDivisionId equals b.Id into bleftjoin
                                from blj in bleftjoin.DefaultIfEmpty()
                                where !(a.IsDeleted ?? false)
                                select new MasterNonJob
                                {
                                    Id = a.Id,
                                    name = a.name,
                                    IsDefault = a.IsDefault,
                                    IsLocked = a.IsLocked,
                                    BusinessUnitName = blj.UnitName,
                                    description = a.description,
                                    BusinessUnitDivisionId = a.BusinessUnitDivisionId
                                }).ToListAsync();
            return result;
        }

        public Task<List<MasterNonJob>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<MasterNonJob> Get(string id) => await context.MasterNonjob.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == id).FirstOrDefaultAsync();
    }
}
