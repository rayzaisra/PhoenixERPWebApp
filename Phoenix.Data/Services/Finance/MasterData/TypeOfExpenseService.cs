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
    public interface ITypeOfExpenseService : IDataService<TypeOfExpense>
    {
    }

    public class TypeOfExpenseService : ITypeOfExpenseService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage TypeOfExpense
        /// </summary>
        /// <param name="context">Database context</param>
        public TypeOfExpenseService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(TypeOfExpense entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params TypeOfExpense[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(TypeOfExpense entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params TypeOfExpense[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(TypeOfExpense entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params TypeOfExpense[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(TypeOfExpense entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(TypeOfExpense entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TypeOfExpense>> Get() => await context.TypeOfExpenses.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).ToListAsync();

        public Task<List<TypeOfExpense>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<TypeOfExpense> Get(string Id) => await context.TypeOfExpenses.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
    }
}