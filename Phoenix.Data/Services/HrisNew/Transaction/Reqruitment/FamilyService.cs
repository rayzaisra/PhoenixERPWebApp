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
    public interface IFamilyService : IDataService<Family>
    {
		Task<List<FormModelFamily>> GetByEmployeeBasicInfoId(string empid);

	}

    public class FamilyService : IFamilyService
	{
        readonly DataContext context;
        /// <summary>
        /// And endpoint to manage Termination
        /// </summary>
        /// <param name="context">Database context</param>
        public FamilyService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(Family entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }
		
		public Task<int> AddRangeAsync(params Family[] entities)
        {
			context.PhoenixAddRange(entities);
			return context.SaveChangesAsync();
		}

        public Task<int> DeleteAsync(Family entity)
        {
			context.Remove(entity);
			return context.SaveChangesAsync();
		}

        public Task<int> DeleteRageAsync(params Family[] entities)
        {
			context.RemoveRange(entities);
			return context.SaveChangesAsync();
		}

        public async Task<int> DeleteSoftAsync(Family entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params Family[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(Family entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(Family entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Family>> Get() => await context.Familys.AsNoTracking().Where(x => x.IsDeleted != true).ToListAsync();

        public Task<List<Family>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<Family> Get(string Id) => await context.Familys.AsNoTracking().Where(x => x.IsDeleted != true && x.Id == Id).FirstOrDefaultAsync();
		public async Task<List<FormModelFamily>> GetByEmployeeBasicInfoId(string empid)
		{
			return await context.Familys.AsNoTracking().Where(x => x.IsDeleted != true && x.EmployeeBasicInfoId == empid)
			.Select(x => new FormModelFamily {
				fName = x.Name,
				fPhoneNo = x.PhoneNo,
				Birthday = x.Birthday,
				fOtherNo = x.OtherNo,
				fEducationLevelId = x.EducationLevelId,
				PlaceBirth = x.PlaceBirth,
				FamilyStatusId = x.FamilyStatusId,
				fReligion = x.ReligionId,
				fGender = x.Gender,
				fAddress = x.Address,
				fInstitutionId = x.InstitutionId,
				Description = x.Description,
				fMarriedStatusId = x.MarriedStatusId,
				Id = x.Id,
				fFamilyStatus = string.IsNullOrEmpty(x.FamilyStatusId) ? "" : (from a in context.FamilyStatuss where a.Id == x.FamilyStatusId select a.FamilyStatusName).FirstOrDefault()
			}).ToListAsync();
		}
	}
}