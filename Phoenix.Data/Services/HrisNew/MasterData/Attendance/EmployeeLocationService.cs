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
    public interface IEmployeeLocationService : IDataService<EmployeeLocation>
    {
    }

    public class EmployeeLocationService : IEmployeeLocationService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage EmployeeLocation
        /// </summary>
        /// <param name="context">Database context</param>
        public EmployeeLocationService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(EmployeeLocation entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params EmployeeLocation[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(EmployeeLocation entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params EmployeeLocation[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(EmployeeLocation entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params EmployeeLocation[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(EmployeeLocation entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(EmployeeLocation entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<EmployeeLocation>> Get() => await context.EmployeeLocations.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).OrderBy(x => x.CreatedOn).ToListAsync();

        public Task<List<EmployeeLocation>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<EmployeeLocation> Get(string Id) => await context.EmployeeLocations.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();

        public async Task<dynamic> GetList()
        {
            var sql = await (from a in context.EmployeeLocations
                             join b in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals b.Id
                             join c in context.MasterLocations on a.LocationId equals c.Id
                             where a.IsDeleted == false
                             select new
                             {
                                 Location = c.LocationName,
                                 EmployeeName = b.NameEmployee,
                                 CreatedOn = a.CreatedOn
                             }).OrderBy(x => x.CreatedOn).ToListAsync();
            return sql;
        }
    }
}