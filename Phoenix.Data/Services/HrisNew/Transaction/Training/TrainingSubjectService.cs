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
    public interface ITrainingSubjectService : IDataServiceHris<TrainingSubject>
    {
      
    }

    public class TrainingSubjectService : ITrainingSubjectService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage Overtime
        /// </summary>
        /// <param name="context">Database context</param>
        public TrainingSubjectService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(TrainingSubject entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> EditAsync(TrainingSubject entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }


        public async Task<int> DeleteSoftAsync(TrainingSubject entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<List<TrainingSubject>> Get()
        {
            return await context.TrainingSubject.Where(x => x.IsDeleted.Equals(false)).ToListAsync();
        }

        public async Task<TrainingSubject> Get(string Id)
        {
            return await context.TrainingSubject.Where(x => x.IsDeleted.Equals(false) && x.Id==Id).FirstOrDefaultAsync();
        }

        public Task<int> ApproveAsync(TrainingSubject entity)
        {
            throw new NotImplementedException();
        }
    }
}