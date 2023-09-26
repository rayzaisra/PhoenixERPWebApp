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
    public interface IPaymentMethodService : IDataService<PaymentMethod>
    {
    }

    public class PaymentMethodService : IPaymentMethodService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage PaymentMethod
        /// </summary>
        /// <param name="context">Database context</param>
        public PaymentMethodService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(PaymentMethod entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params PaymentMethod[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(PaymentMethod entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params PaymentMethod[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(PaymentMethod entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params PaymentMethod[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(PaymentMethod entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(PaymentMethod entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PaymentMethod>> Get() => await context.PaymentMethods.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).ToListAsync();

        public Task<List<PaymentMethod>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<PaymentMethod> Get(string Id) => await context.PaymentMethods.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
    }
}