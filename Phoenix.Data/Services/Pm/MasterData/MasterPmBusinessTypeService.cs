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
    public interface IMasterPmBusinessTypeService : IDataService<BusinessType>
    {
    }
    public class MasterPmBusinessTypeService : IMasterPmBusinessTypeService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage MasterOutlet
        /// </summary>
        /// <param name="context">Database context</param>
        public MasterPmBusinessTypeService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(BusinessType entity)
        {
            entity.IsActive = true;
            entity.IsDeleted = false;
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params BusinessType[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(BusinessType entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params BusinessType[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(BusinessType entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params BusinessType[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(BusinessType entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(BusinessType entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BusinessType>> Get()
        {
            var result = await (from a in context.BusinessType
                                where !(a.IsDeleted ?? false)
                                select new BusinessType
                                {
                                    Id = a.Id,
                                    proposeType = a.proposeType,
                                    IsDefault = a.IsDefault,
                                    IsLocked = a.IsLocked,
                                    BusinessUnitDivisionId = a.BusinessUnitDivisionId
                                }).ToListAsync();
            return result;
        }

        public Task<List<BusinessType>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<BusinessType> Get(string id) => await context.BusinessType.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == id).FirstOrDefaultAsync();
    }
}
