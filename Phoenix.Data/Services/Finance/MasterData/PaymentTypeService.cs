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
using Phoenix.Data.Models.Finance.Master;

namespace Phoenix.Data
{
    public interface IPaymentTypeService : IDataService<PaymentType>
    {
    }

    public class PaymentTypeService : IPaymentTypeService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage PaymentType
        /// </summary>
        /// <param name="context">Database context</param>
        public PaymentTypeService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(PaymentType entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params PaymentType[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(PaymentType entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params PaymentType[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(PaymentType entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params PaymentType[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(PaymentType entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(PaymentType entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PaymentType>> Get() => await context.PaymentType.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).ToListAsync();

        public Task<List<PaymentType>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<PaymentType> Get(string Id) => await context.PaymentType.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
    }
}