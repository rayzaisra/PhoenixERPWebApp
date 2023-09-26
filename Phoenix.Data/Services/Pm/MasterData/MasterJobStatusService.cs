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
    public interface IMasterJobStatusService : IDataService<JobStatus>
    {
    }

    public class MasterJobStatusService : IMasterJobStatusService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage MasterOutlet
        /// </summary>
        /// <param name="context">Database context</param>
        public MasterJobStatusService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(JobStatus entity)
        {
            entity.name = entity.StatusName;
            entity.Id = entity.StatusName;
            entity.IsActive = true;
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params JobStatus[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(JobStatus entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params JobStatus[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(JobStatus entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params JobStatus[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(JobStatus entity)
        {
            entity.name = entity.StatusName;
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(JobStatus entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<JobStatus>> Get()
        {
            var result = await (from a in context.PmJobStatus
                                where !(a.IsDeleted ?? false)
                                select new JobStatus
                                {
                                    Id = a.Id,
                                    name = a.name,
                                    OwnerId = a.OwnerId,
                                    IsDefault = a.IsDefault,
                                    IsLocked = a.IsLocked,
                                    StatusName = a.name,
                                    StatusOrder = a.OwnerId,
                                     
                                }).ToListAsync();
            return result;
        }

        public Task<List<JobStatus>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<JobStatus> Get(string Id) => await context.PmJobStatus.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
    }
}
