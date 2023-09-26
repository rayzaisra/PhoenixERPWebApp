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
    public interface IEducationService : IDataService<Education>
    {
		Task<List<FormModelEducation>> GetByEmployeeBasicInfoId(string empid);

	}

    public class EducationService : IEducationService
	{
        readonly DataContext context;
        /// <summary>
        /// And endpoint to manage Termination
        /// </summary>
        /// <param name="context">Database context</param>
        public EducationService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(Education entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }
		
		public Task<int> AddRangeAsync(params Education[] entities)
        {
			context.PhoenixAddRange(entities);
			return context.SaveChangesAsync();
		}

        public Task<int> DeleteAsync(Education entity)
        {
			context.Remove(entity);
			return context.SaveChangesAsync();
		}

        public Task<int> DeleteRageAsync(params Education[] entities)
        {
			context.RemoveRange(entities);
			return context.SaveChangesAsync();
		}

        public async Task<int> DeleteSoftAsync(Education entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params Education[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(Education entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(Education entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Education>> Get() => await context.Educations.AsNoTracking().Where(x => x.IsDeleted != true).ToListAsync();

        public Task<List<Education>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<Education> Get(string Id) => await context.Educations.AsNoTracking().Where(x => x.IsDeleted != true && x.Id == Id).FirstOrDefaultAsync();
		public async Task<List<FormModelEducation>> GetByEmployeeBasicInfoId(string empid)
		{
			return await context.Educations.AsNoTracking().Where(x => x.IsDeleted != true && x.EmployeeBasicInfoId == empid)
			.Select(x => new FormModelEducation {
				EEducationLevelId = x.EducationLevelId,
				EMajors = x.Majors,
				EStartEducation = x.StartEducation,
				EEndEducation = x.EndEducation,
				EInstitutionId = x.InstitutionId,
				EQualification = x.Qualification,
				OtherInstitution = x.OtherInstitution,
				EAddress = x.Address,
				Id = x.Id,
				Institution = string.IsNullOrEmpty(x.InstitutionId) ? "" : (from a in context.Institutions where a.Id == x.InstitutionId select a.InstitutionName).FirstOrDefault()
			}).ToListAsync();
		}
	}
}