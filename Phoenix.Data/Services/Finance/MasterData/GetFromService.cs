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
    public interface IGetFromService : IDataService<GetFrom>
    {
        Task<GetFrom> GetAsync(string id);
    }


    public class GetFromService : IGetFromService
    {
        readonly DataContext context;

        public GetFromService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(GetFrom entity)
        {
            return await AddEdit(entity, false);
        }

        public Task<int> AddRangeAsync(params GetFrom[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(GetFrom entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params GetFrom[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(GetFrom entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params GetFrom[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(GetFrom entity)
        {
            return await AddEdit(entity, true);
        }

        public Task<int> EditRangeAsync(GetFrom entity)
        {
            throw new NotImplementedException();
        }

        private async Task<int> AddEdit(GetFrom entity, bool isEdit)
        {
            var isAlready = false;
            if (isEdit)
                isAlready = context.GetFroms.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id != entity.Id).Count() > 0;


            if (isEdit)
            {
                context.PhoenixEdit(entity);
                return await context.SaveChangesAsync();
            }
            else
            {
                entity.Id = Guid.NewGuid().ToString();
                await context.PhoenixAddAsync(entity);
                return await context.SaveChangesAsync();
            }
        }

        public async Task<List<GetFrom>> Get()
        {
            var result = await (from a in context.GetFroms
                               where !(a.IsDeleted ?? false)
                               select new GetFrom
                               {
                                   Id = a.Id,
                                   GetFromName = a.GetFromName,
                                   Description = a.Description,
                                   CreatedBy = a.CreatedBy,
                                   CreatedOn = a.CreatedOn,

                               }).ToListAsync();
            return result;
        }

        public Task<List<GetFrom>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<GetFrom> Get(string id)
        {
            var result = await context.GetFroms.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == id).FirstOrDefaultAsync();
            return result;
        }

        public async Task<GetFrom> GetAsync(string id)
        {
            var result = await context.GetFroms.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == id).FirstOrDefaultAsync();
            return result;
        }
    
    }
}
