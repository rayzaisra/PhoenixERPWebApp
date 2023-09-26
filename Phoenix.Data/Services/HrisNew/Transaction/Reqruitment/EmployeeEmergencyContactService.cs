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
    public interface IEmployeeEmergencyContactService : IDataService<EmployeeEmergencyContact>
    {
		Task<List<FormModelEmployeeEmergencyContact>> GetByEmployeeBasicInfoId(string empid);

	}

    public class EmployeeEmergencyContactService : IEmployeeEmergencyContactService
	{
        readonly DataContext context;
        /// <summary>
        /// And endpoint to manage Termination
        /// </summary>
        /// <param name="context">Database context</param>
        public EmployeeEmergencyContactService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(EmployeeEmergencyContact entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }
		
		public Task<int> AddRangeAsync(params EmployeeEmergencyContact[] entities)
        {
			context.PhoenixAddRange(entities);
			return context.SaveChangesAsync();
		}

        public Task<int> DeleteAsync(EmployeeEmergencyContact entity)
        {
			context.Remove(entity);
			return context.SaveChangesAsync();
		}

        public Task<int> DeleteRageAsync(params EmployeeEmergencyContact[] entities)
        {
			context.RemoveRange(entities);
			return context.SaveChangesAsync();
		}

        public async Task<int> DeleteSoftAsync(EmployeeEmergencyContact entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params EmployeeEmergencyContact[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(EmployeeEmergencyContact entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(EmployeeEmergencyContact entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<EmployeeEmergencyContact>> Get() => await context.EmployeeEmergencyContacts.AsNoTracking().Where(x => x.IsDeleted != true).ToListAsync();

        public Task<List<EmployeeEmergencyContact>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<EmployeeEmergencyContact> Get(string Id) => await context.EmployeeEmergencyContacts.AsNoTracking().Where(x => x.IsDeleted != true && x.Id == Id).FirstOrDefaultAsync();
		public async Task<List<FormModelEmployeeEmergencyContact>> GetByEmployeeBasicInfoId(string empid)
		{
			return await context.EmployeeEmergencyContacts.AsNoTracking().Where(x => x.IsDeleted != true && x.EmployeeBasicInfoId == empid)
			.Select(x => new FormModelEmployeeEmergencyContact {
				EmergencyFamilyStatusId = x.FamilyStatusId,
				EmergencyPhoneNo = x.PhoneNo,
				EmergencyFullName = x.FullName,
				EmergencyOtherNo = x.OtherNo,
				EmergencyAddress = x.Address,
				Id = x.Id,
				FamilyStatus = string.IsNullOrEmpty(x.FamilyStatusId) ? "" : (from a in context.FamilyStatuss where a.Id == x.FamilyStatusId select a.FamilyStatusName).FirstOrDefault()
			}).ToListAsync();
		}
	}
}