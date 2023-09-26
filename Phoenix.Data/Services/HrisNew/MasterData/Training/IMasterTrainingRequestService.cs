using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using Phoenix.Data.Models.HrisNew.MasterData.Training;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data.Services.HrisNew.MasterData.Training
{
	public interface IMasterTrainingRequestService : IDataService<MasterTrainingRequest>
	{
	}

	public class MasterTrainingRequestService : IMasterTrainingRequestService
	{
		readonly DataContext context;

		/// <summary>
		/// And endpoint to manage MasterTrainingRequest
		/// </summary>
		/// <param name="context">Database context</param>
		public MasterTrainingRequestService(DataContext context)
		{
			this.context = context;
		}

		public async Task<int> AddAsync(MasterTrainingRequest entity)
		{
			entity.Id = Guid.NewGuid().ToString();
			await context.PhoenixAddAsync(entity);
			return await context.SaveChangesAsync();
		}

		public Task<int> AddRangeAsync(params MasterTrainingRequest[] entities) => throw new NotImplementedException();

		public Task<int> DeleteAsync(MasterTrainingRequest entity) => throw new NotImplementedException();

		public Task<int> DeleteRageAsync(params MasterTrainingRequest[] entities) => throw new NotImplementedException();

		public async Task<int> DeleteSoftAsync(MasterTrainingRequest entity)
		{
			context.PhoenixDelete(entity);
			return await context.SaveChangesAsync();
		}

		public Task<int> DeleteSoftRangeAsync(params MasterTrainingRequest[] entities) => throw new NotImplementedException();

		public async Task<int> EditAsync(MasterTrainingRequest entity)
		{
			context.PhoenixEdit(entity);
			return await context.SaveChangesAsync();
		}

		public Task<int> EditRangeAsync(MasterTrainingRequest entity) => throw new NotImplementedException();

		public async Task<List<MasterTrainingRequest>> Get() => await context.MasterTrainingRequests.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).ToListAsync();

		public Task<List<MasterTrainingRequest>> Get(int skip, int take) => throw new NotImplementedException();

		public async Task<MasterTrainingRequest> Get(string Id) => await context.MasterTrainingRequests.AsNoTracking().Where(x => x.Id == Id).FirstOrDefaultAsync();
	}
}
