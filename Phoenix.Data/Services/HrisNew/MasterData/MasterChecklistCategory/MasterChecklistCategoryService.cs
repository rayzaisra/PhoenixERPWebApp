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

namespace Phoenix.Data.Services.HrisNew.MasterData.MasterChecklistCategory
{
    public interface IMasterChecklistCategoryService : IDataService<EmployeeChecklistCategory>
    {
    }
    public class MasterChecklistCategoryService : IMasterChecklistCategoryService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage MasterOutlet
        /// </summary>
        /// <param name="context">Database context</param>
        public MasterChecklistCategoryService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(EmployeeChecklistCategory entity)
        {
            entity.IsActive = true;
            entity.IsDeleted = false;
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params EmployeeChecklistCategory[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(EmployeeChecklistCategory entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params EmployeeChecklistCategory[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(EmployeeChecklistCategory entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params EmployeeChecklistCategory[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(EmployeeChecklistCategory entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(EmployeeChecklistCategory entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<EmployeeChecklistCategory>> Get()
        {
            var result = await (from a in context.EmployeeChecklistCategorys
                                where !(a.IsDeleted ?? false)
                                select new EmployeeChecklistCategory
                                {
                                    Id = a.Id,
                                    ChecklistCategory = a.ChecklistCategory,
                                    IsDefault = a.IsDefault,
                                    IsLocked = a.IsLocked,
                                    BusinessUnitDivisionId = a.BusinessUnitDivisionId
                                }).ToListAsync();
            return result;
        }

        public Task<List<EmployeeChecklistCategory>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<EmployeeChecklistCategory> Get(string id) => await context.EmployeeChecklistCategorys.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == id).FirstOrDefaultAsync();
    }
}
