using Phoenix.Data.Models.Finance;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Microsoft.AspNetCore.Http;
using Phoenix.ApiExtension.Extensions;

namespace Phoenix.Data.Services.Finance
{
    public interface IBudgetManagementService : IDataService<CashAdvance>
    {
        Task<DataSet> ReadExcel(IFormFile file);
    }

    public class BudgetManagementService : IBudgetManagementService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage CashAdvance
        /// </summary>
        /// <param name="context">Database context</param>
        public BudgetManagementService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(CashAdvance entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }
         public async Task<DataSet> ReadExcel(IFormFile file)
        {
            using (var reader = file.OpenReadStream())
            {
                var dataSet =  ExcelConverter.ExcelToDataSet(reader);
                return dataSet;
            }
        }

        public Task<int> AddRangeAsync(params CashAdvance[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(CashAdvance entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params CashAdvance[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteSoftAsync(CashAdvance entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteSoftRangeAsync(params CashAdvance[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(CashAdvance entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(CashAdvance entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CashAdvance>> Get() => await context.CashAdvances.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).ToListAsync();

        public Task<List<CashAdvance>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<CashAdvance> Get(string Id) => await context.CashAdvances.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
    }
}
