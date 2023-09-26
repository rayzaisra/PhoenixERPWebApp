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
    public interface IItemTypeService : IDataService<ItemType>
    {
        Task<ItemType> GetAsync(string id);
    }

    public class ItemTypeService : IItemTypeService
    {
        readonly DataContext context;

        public ItemTypeService(DataContext context)
        {
            this.context = context;
        }
        private async Task<int> AddEdit(ItemType entity, bool isEdit)
        {
            var isAlready = false;
            if (isEdit)
                isAlready = context.ItemType.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id != entity.Id).Count() > 0;


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


        public async Task<int> AddAsync(ItemType entity)
        {
            return await AddEdit(entity, false);
        }

        public Task<int> AddRangeAsync(params ItemType[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(ItemType entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params ItemType[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteSoftAsync(ItemType entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteSoftRangeAsync(params ItemType[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(ItemType entity)
        {
            return await AddEdit(entity, true);
        }

        public Task<int> EditRangeAsync(ItemType entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ItemType>> Get()
        {
            var result = await(from a in context.ItemType
                               where !(a.IsDeleted ?? false)
                               select new ItemType
                               {
                                   Id = a.Id,
                                   TypeName = a.TypeName,
                                   IsFixedAsset = a.IsFixedAsset,
                                   Description = a.Description,
                                   CreatedBy = a.CreatedBy,
                                   CreatedOn = a.CreatedOn,

                               }).ToListAsync();
            return result;
        }

        public Task<List<ItemType>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<ItemType> Get(string id)
        {
            var result = await context.ItemType.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == id).FirstOrDefaultAsync();
            return result;
        }

        public async Task<ItemType> GetAsync(string id)
        {
            var result = await context.ItemType.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == id).FirstOrDefaultAsync();
            return result;
        }
    }
}
