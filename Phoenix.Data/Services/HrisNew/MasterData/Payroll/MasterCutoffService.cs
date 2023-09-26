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
    public interface IMasterCutoffService : IDataService<MasterCutoff>
    {
    }

    public class MasterCutoffService : IMasterCutoffService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage MasterCutoff
        /// </summary>
        /// <param name="context">Database context</param>
        public MasterCutoffService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(MasterCutoff entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params MasterCutoff[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(MasterCutoff entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params MasterCutoff[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(MasterCutoff entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params MasterCutoff[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(MasterCutoff entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(MasterCutoff entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MasterCutoff>> Get() => await context.MasterCutoffs.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).OrderBy(x => x.CreatedOn).ToListAsync();

        public Task<List<MasterCutoff>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<MasterCutoff> Get(string Id) => await context.MasterCutoffs.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
    }
}