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
    public interface IShareServicesService : IDataService<ShareServices>
    {
    }

    public class ShareServicesService : IShareServicesService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage ShareServices
        /// </summary>
        /// <param name="context">Database context</param>
        public ShareServicesService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(ShareServices entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params ShareServices[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(ShareServices entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params ShareServices[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(ShareServices entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params ShareServices[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(ShareServices entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(ShareServices entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ShareServices>> Get()
        {
            return await (from a in context.ShareServicess
                          join b in context.MainServiceCategorys on a.MainserviceCategoryId equals b.Id
                          where !(a.IsDeleted ?? false)
                          select new ShareServices
                          {
                              Id = a.Id,
                              ShareserviceName = a.ShareserviceName,
                              DetailShareservices = a.DetailShareservices,
                              MainserviceCategoryId = a.MainserviceCategoryId,
                              MainserviceCategoryName = b.Name
                          }).ToListAsync();
        }

        public Task<List<ShareServices>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<ShareServices> Get(string Id) => await context.ShareServicess.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
    }
}