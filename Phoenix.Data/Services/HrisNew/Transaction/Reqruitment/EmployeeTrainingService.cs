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
    public interface IEmployeeTrainingService : IDataService<EmployeeTraining>
    {
		Task<List<FormModelEmployeeTraining>> GetByEmployeeBasicInfoId(string empid);

	}

    public class EmployeeTrainingService : IEmployeeTrainingService
	{
        readonly DataContext context;
        /// <summary>
        /// And endpoint to manage Termination
        /// </summary>
        /// <param name="context">Database context</param>
        public EmployeeTrainingService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(EmployeeTraining entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }
		
		public Task<int> AddRangeAsync(params EmployeeTraining[] entities)
        {
			context.PhoenixAddRange(entities);
			return context.SaveChangesAsync();
		}

        public Task<int> DeleteAsync(EmployeeTraining entity)
        {
			context.Remove(entity);
			return context.SaveChangesAsync();
		}

        public Task<int> DeleteRageAsync(params EmployeeTraining[] entities)
        {
			context.RemoveRange(entities);
			return context.SaveChangesAsync();
		}

        public async Task<int> DeleteSoftAsync(EmployeeTraining entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params EmployeeTraining[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(EmployeeTraining entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(EmployeeTraining entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<EmployeeTraining>> Get() => await context.EmployeeTrainings.AsNoTracking().Where(x => x.IsDeleted != true).ToListAsync();

        public Task<List<EmployeeTraining>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<EmployeeTraining> Get(string Id) => await context.EmployeeTrainings.AsNoTracking().Where(x => x.IsDeleted != true && x.Id == Id).FirstOrDefaultAsync();
		public async Task<List<FormModelEmployeeTraining>> GetByEmployeeBasicInfoId(string empid)
		{
			return await context.EmployeeTrainings.AsNoTracking().Where(x => x.IsDeleted != true && x.EmployeeBasicInfoId == empid)
			.Select(x => new FormModelEmployeeTraining {
				NameTraining = x.NameTraining,
				EmployeeTrainingInstitutionId = x.InstitutionId,
				EmployeeTrainingAddress = x.Address,
				StartTraining = x.StartTraining,
				EndTraining = x.EndTraining,
				NoCertification = x.NoCertification,
				EmployeeTrainingDescription = x.Description,
				Id= x.Id,
				ExpiredCertificate = x.ExpiredCertificate,
				EmployeeTrainingInstitution = string.IsNullOrEmpty(x.InstitutionId) ? "" : (from a in context.Institutions where a.Id == x.InstitutionId select a.InstitutionName).FirstOrDefault()
			}).ToListAsync();
		}
	}
}