using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phoenix.ApiExtension.Extensions;
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface IEmployeeExperienceService : IDataService<EmployeeExperience>
    {
		Task<List<FormModelEmployeeExperience>> GetByEmployeeBasicInfoId(string empid);

	}

    public class EmployeeExperienceService : IEmployeeExperienceService
	{
        readonly DataContext context;
        /// <summary>
        /// And endpoint to manage Termination
        /// </summary>
        /// <param name="context">Database context</param>
        public EmployeeExperienceService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(EmployeeExperience entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }
		
		public Task<int> AddRangeAsync(params EmployeeExperience[] entities)
        {
			context.PhoenixAddRange(entities);
			return context.SaveChangesAsync();
		}

        public Task<int> DeleteAsync(EmployeeExperience entity)
        {
			context.Remove(entity);
			return context.SaveChangesAsync();
		}

        public Task<int> DeleteRageAsync(params EmployeeExperience[] entities)
        {
			context.RemoveRange(entities);
			return context.SaveChangesAsync();
		}

        public async Task<int> DeleteSoftAsync(EmployeeExperience entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params EmployeeExperience[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(EmployeeExperience entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(EmployeeExperience entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<EmployeeExperience>> Get() => await context.EmployeeExperiences.AsNoTracking().Where(x => x.IsDeleted != true).ToListAsync();

        public Task<List<EmployeeExperience>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<EmployeeExperience> Get(string Id) => await context.EmployeeExperiences.AsNoTracking().Where(x => x.IsDeleted != true && x.Id == Id).FirstOrDefaultAsync();
		public async Task<List<FormModelEmployeeExperience>> GetByEmployeeBasicInfoId(string empid)
		{
			return await context.EmployeeExperiences.AsNoTracking().Where(x => x.IsDeleted != true && x.EmployeeBasicInfoId == empid)
			.Select(x => new FormModelEmployeeExperience {
				CompanyName = x.CompanyName,
				EmplyeeExperiencePhone = x.Phone,
				EmplyeeExperienceStartDate = x.StartDate,
				EmplyeeExperienceEndDate = x.EndDate,
				Position = x.Position,
				Remark = x.Remark,
				EmplyeeExperienceAddress = x.Address,
				Id= x.Id
			}).ToListAsync();
		}
	}
}