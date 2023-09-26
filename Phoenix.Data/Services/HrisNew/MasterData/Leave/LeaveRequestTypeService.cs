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
    public interface ILeaveRequestTypeService : IDataService<LeaveRequestType>
    {
    }

    public class LeaveRequestTypeService : ILeaveRequestTypeService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage LeaveRequestType
        /// </summary>
        /// <param name="context">Database context</param>
        public LeaveRequestTypeService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(LeaveRequestType entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params LeaveRequestType[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(LeaveRequestType entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params LeaveRequestType[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(LeaveRequestType entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params LeaveRequestType[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(LeaveRequestType entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(LeaveRequestType entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<LeaveRequestType>> Get()
        {
            var query = await context.LeaveRequestTypes.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).OrderBy(x => x.Name).ToListAsync();
            foreach (var item in query)
            {
                item.GradeLevelEmployeeName = await context.JobGrades.AsNoTracking().Where(x => x.Id == item.GradeLevelEmployeeId).Select(x => x.GradeName).FirstOrDefaultAsync();
            }
            return query;
        }


        public Task<List<LeaveRequestType>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<LeaveRequestType> Get(string Id) {
            LeaveRequestType model = new LeaveRequestType();
            if (Id == "COMPENSATORY")
            {
                model.MinimumDurationLeave = 1;
                model.MaximumDurationLeave = 30;
                model.IsPaidLeave = false;
            }
            else
            {
                model = await context.LeaveRequestTypes.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
                model.GradeLevelEmployeeName = await context.JobGrades.AsNoTracking().Where(x => x.Id == model.GradeLevelEmployeeId).Select(x => x.GradeName).FirstOrDefaultAsync();

            }
            return model;
        } 
    }
}