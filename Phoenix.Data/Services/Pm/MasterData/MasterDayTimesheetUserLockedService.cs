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
    public interface IMasterDayTimesheetUserLockedService : IDataService<MasterDayTimesheetUserLocked>
    {
    }
    public class MasterDayTimesheetUserLockedService : IMasterDayTimesheetUserLockedService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage MasterOutlet
        /// </summary>
        /// <param name="context">Database context</param>
        public MasterDayTimesheetUserLockedService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(MasterDayTimesheetUserLocked entity)
        {
            entity.IsActive = true;
            entity.IsDeleted = false;
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params MasterDayTimesheetUserLocked[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(MasterDayTimesheetUserLocked entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params MasterDayTimesheetUserLocked[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(MasterDayTimesheetUserLocked entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params MasterDayTimesheetUserLocked[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(MasterDayTimesheetUserLocked entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(MasterDayTimesheetUserLocked entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MasterDayTimesheetUserLocked>> Get()
        {
            var result = await (from a in context.MasterDayTimesheetUserLocked
                                join b in context.BusinessUnits on a.BusinessUnitDivisionId equals b.Id into bleftjoin
                                from blj in bleftjoin.DefaultIfEmpty()
                                where !(a.IsDeleted ?? false)
                                select new MasterDayTimesheetUserLocked
                                {
                                    Id = a.Id,
                                    day = a.day,
                                    IsDefault = a.IsDefault,
                                    IsLocked = a.IsLocked,
                                    BusinessUnitName = blj.UnitName,
                                    description = a.description,
                                    BusinessUnitDivisionId = a.BusinessUnitDivisionId
                                }).ToListAsync();
            return result;
        }

        public Task<List<MasterDayTimesheetUserLocked>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<MasterDayTimesheetUserLocked> Get(string id) => await context.MasterDayTimesheetUserLocked.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == id).FirstOrDefaultAsync();
    }
}
