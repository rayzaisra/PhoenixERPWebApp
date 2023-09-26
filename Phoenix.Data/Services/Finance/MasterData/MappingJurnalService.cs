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
    public interface IMappingJurnalService : IDataService<MappingJurnal>
    {
        Task<dynamic> GetList();
    }

    public class MappingJurnalService : IMappingJurnalService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage MappingJurnal
        /// </summary>
        /// <param name="context">Database context</param>
        public MappingJurnalService(DataContext context)
        {
            this.context = context; 
        }

        public async Task<int> AddAsync(MappingJurnal entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params MappingJurnal[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(MappingJurnal entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params MappingJurnal[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(MappingJurnal entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params MappingJurnal[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(MappingJurnal entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(MappingJurnal entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MappingJurnal>> Get() => await context.MappingJurnals.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).ToListAsync();

        public Task<List<MappingJurnal>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<MappingJurnal> Get(string Id) => await context.MappingJurnals.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();

        public async Task<dynamic> GetList()
        {
            var sql = await (from a in context.MappingJurnals
                             join b in context.BusinessUnits on a.EmployeeDivisionId equals b.BusinessUnitTypeId
                             join c in context.BusinessUnitTypes on b.BusinessUnitTypeId equals c.Id
                             where a.IsDeleted == false
                             select new
                             {
                                 a.Id,
                                 DivisionName = (from x in context.BusinessUnits
                                                 join y in context.BusinessUnitTypes on x.BusinessUnitTypeId equals y.Id
                                                 where y.BusinessUnitLevel == 800
                                                 select new
                                                 {
                                                     x.UnitName
                                                 }).FirstOrDefault(),
                                 DepartmentName = (from x in context.BusinessUnits
                                                   join y in context.BusinessUnitTypes on x.BusinessUnitTypeId equals y.Id
                                                   where y.BusinessUnitLevel == 700
                                                   select new
                                                   {
                                                       x.UnitName
                                                   }).FirstOrDefault(),
                                 a.Debit,
                                 a.Credit,
                                 a.ProsessName
                             }).ToListAsync();
            return sql;
        }
    }
}