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
    public interface IMasterJobTypeService : IDataService<JobType>
    {
    }

    public class MasterJobTypeService : IMasterJobTypeService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage MasterOutlet
        /// </summary>
        /// <param name="context">Database context</param>
        public MasterJobTypeService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(JobType entity)
        {
            entity.IsActive = true;
            entity.IsDeleted = false;
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params JobType[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(JobType entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params JobType[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(JobType entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params JobType[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(JobType entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(JobType entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<JobType>> Get()
        {
            var result = await (from a in context.JobType
                                where !(a.IsDeleted ?? false)
                                select new JobType
                                {
                                    Id = a.Id,
                                    name = a.name,
                                    description = a.description,
                                    IsDefault = a.IsDefault,
                                    IsLocked = a.IsLocked,
                                    BusinessUnitDivisionId = a.BusinessUnitDivisionId
                                }).ToListAsync();
            return result;
        }

        public Task<List<JobType>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<JobType> Get(string id) => await context.JobType.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == id).FirstOrDefaultAsync();
    }
}
