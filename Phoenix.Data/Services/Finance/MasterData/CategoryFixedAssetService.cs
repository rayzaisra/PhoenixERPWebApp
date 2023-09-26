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
    public interface ICategoryFixedAssetService : IDataService<ItemCategories>
    {
    }

    public class CategoryFixedAssetService : ICategoryFixedAssetService
    {
        readonly DataContext context;

        public CategoryFixedAssetService(DataContext context)
        {
            this.context = context;
        }
        private async Task<int> AddEdit(ItemCategories entity, bool isEdit)
        {
            var isAlready = false;
            if (isEdit)
                isAlready = context.ItemCategories.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id != entity.Id).Count() > 0;


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


        public async Task<int> AddAsync(ItemCategories entity)
        {
            return await AddEdit(entity, false);
        }

        public Task<int> AddRangeAsync(params ItemCategories[] entities) => throw new NotImplementedException();

        public Task<int> DeleteAsync(ItemCategories entity) => throw new NotImplementedException();

        public Task<int> DeleteRageAsync(params ItemCategories[] entities) => throw new NotImplementedException();

        public async Task<int> DeleteSoftAsync(ItemCategories entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params ItemCategories[] entities) => throw new NotImplementedException();

        public async Task<int> EditAsync(ItemCategories entity)
        {
            return await AddEdit(entity, true);
        }

        public Task<int> EditRangeAsync(ItemCategories entity) => throw new NotImplementedException();

        public async Task<List<ItemCategories>> Get()
        {
            var result = await context.ItemCategories.Where(x => !(x.IsDeleted ?? false))
                .Select(x => new ItemCategories
                {
                    Id = x.Id,
                    CategoryName = x.CategoryName,
                    LifeTimeMonth = x.LifeTimeMonth,
                    COADebit = context.ChartOfAccounts.Where(y => y.Id == x.COADebit).Select(y => $"{y.CodeRec}-{y.Name5}").FirstOrDefault(),
                    COACredit = context.ChartOfAccounts.Where(y => y.Id == x.COACredit).Select(y => $"{y.CodeRec}-{y.Name5}").FirstOrDefault()
                }).ToListAsync();
            return result;
        }

        public Task<List<ItemCategories>> Get(int skip, int take) => throw new NotImplementedException();

        public async Task<ItemCategories> Get(string id)
        {
            var result = await context.ItemCategories.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == id).FirstOrDefaultAsync();
            return result;
        }
    }
}
