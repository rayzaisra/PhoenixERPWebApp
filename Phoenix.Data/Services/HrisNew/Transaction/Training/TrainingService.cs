using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Um;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface ITrainingService : IDataServiceHris<Training>
    {
      
    }

    public class TrainingService : ITrainingService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage Overtime
        /// </summary>
        /// <param name="context">Database context</param>
        public TrainingService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(Training entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> EditAsync(Training entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }


        public async Task<int> DeleteSoftAsync(Training entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<List<Training>> Get()
        {
            return await context.Training.Where(x => x.IsDeleted.Equals(false)).ToListAsync();
        }

        public async Task<Training> Get(string Id)
        {
            return await context.Training.Where(x => x.IsDeleted.Equals(false) && x.Id==Id).FirstOrDefaultAsync();
        }

        public Task<int> ApproveAsync(Training entity)
        {
            throw new NotImplementedException();
        }
    }
}