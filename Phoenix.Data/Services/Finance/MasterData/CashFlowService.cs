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
    public interface ICashFlowService : IDataService<CashFlow>
    {
    }

    public class CashFlowService : ICashFlowService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage CashFlow
        /// </summary>
        /// <param name="context">Database context</param>
        public CashFlowService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(CashFlow entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params CashFlow[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(CashFlow entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params CashFlow[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(CashFlow entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params CashFlow[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(CashFlow entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(CashFlow entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CashFlow>> Get()
        {
            var result = await (from a in context.CashFlows
                                join b in context.Banks on a.BankId equals b.Id
                                join c in context.Currencies on a.CurrencyId equals c.Id
                                select new CashFlow
                                {
                                    Id = a.Id,
                                    Type = a.Type,
                                    BankId = a.BankId,
                                    CurrencyId = a.CurrencyId,
                                    Condition = a.Condition,
                                    Amount = a.Amount,
                                    Description = a.Description,
                                    CFDate = a.CFDate,
                                    CreatedBy = a.CreatedBy,
                                    CreatedOn = a.CreatedOn,
                                    ModifiedBy = a.ModifiedBy,
                                    ModifiedOn = a.ModifiedOn,
                                    BankName = b.BankName,
                                    CurrencyName = c.CurrencyName
                                }).ToListAsync();
            //await context.CashFlows.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).ToListAsync();
            return result;
        }

        public Task<List<CashFlow>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<CashFlow> Get(string Id) => await context.CashFlows.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
    }
}