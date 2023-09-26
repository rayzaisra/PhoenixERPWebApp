using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using Phoenix.ApiExtension.Extensions;
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface ICurrencyService : IDataService<Currency>
    {
        Task<Currency> GetCurrencyDefault();
        Task<int> UploadAsync(IFormFile file);
    }

    public class CurrencyService : ICurrencyService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage Currency
        /// </summary>
        /// <param name="context">Database context</param>
        public CurrencyService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(Currency entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            if (context.Currencies.Where(x => x.Id != entity.Id && x.CurrencyCode == entity.CurrencyCode).Count() > 0) throw new Exception("Currency Code already exists.");
            if (entity.IsDefault == false) { if (context.Currencies.Where(x => x.Id != entity.Id && x.IsDefault == true).Count() == 0) throw new Exception("The default currency cannot be changed because there must be one currency that must contain yes."); }
            if (entity.IsDefault == true) { context.Database.ExecuteSqlCommand($"UPDATE dbo.currency SET is_default=0 WHERE id != {entity.Id}"); }
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> UploadAsync(IFormFile file)
        {
            var stream = file.OpenReadStream();
            var excel = ExcelConverter.ExcelToEntity<CurrencyUpload>(stream);
            var currencyList = await context.Currencies.Where(x => !(x.IsDeleted ?? false)).ToListAsync();
            var messageError = string.Empty;
            int rowIndex = 1;
            var models = new List<Currency>();
            foreach (var item in excel)
            {
                rowIndex++;
                if (!decimal.TryParse(item.Buy, out decimal decimalBuy))
                {
                    messageError += $"Column [Buy at index D] Rows [{rowIndex}] <br/>";
                }

                if (!decimal.TryParse(item.Sell, out decimal decimalSell))
                {
                    messageError += $"Column [Sell at index E] Rows [{rowIndex}] <br/>";
                }

                if (string.IsNullOrEmpty(messageError))
                {
                    var get = currencyList.Where(x => x.CurrencyCode == item.CurrencyCode).FirstOrDefault() ?? new Currency();
                    if (string.IsNullOrEmpty(get.Id)) get.Id = Guid.NewGuid().ToString();
                    get.CurrencyName = item.CurrencyName;
                    get.CurrencySymbol = item.CurrencySymbol;
                    get.Buy = decimal.Parse(item.Buy);
                    get.Sell = decimal.Parse(item.Sell);
                    models.Add(get);
                }
            }

            if (!string.IsNullOrEmpty(messageError))
            {
                throw new Exception(messageError + "Is invalid for numeric type");
            }

            using (var transaction = context.Database.BeginTransaction())
            {
                await context.BulkInsertOrUpdateAsync(models);
                transaction.Commit();
            }
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params Currency[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(Currency entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params Currency[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(Currency entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params Currency[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(Currency entity)
        {
            if (context.Currencies.Where(x => x.Id != entity.Id && x.CurrencyCode == entity.CurrencyCode).Count() > 0) throw new Exception("Currency Code already exists.");
            if (entity.IsDefault == false) { if (context.Currencies.Where(x => x.Id != entity.Id && x.IsDefault == true).Count() == 0) throw new Exception("The default currency cannot be changed because there must be one currency that must contain yes."); }
            if (entity.IsDefault == true) { context.Database.ExecuteSqlCommand($"UPDATE dbo.currency SET is_default=0 WHERE id != {entity.Id}"); }
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(Currency entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Currency>> Get()
        {
            var result = await context.Currencies.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).ToListAsync();
            var currencyDefault = context.Currencies.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).FirstOrDefault();
            foreach (var item in result)
            {
                item.CurrencyIdDefault = currencyDefault.Id;
                item.CurrencySymbolDefault = currencyDefault.CurrencySymbol;
                item.IsDefault = (item.IsDefault ?? false);
            }
            return result;
        }

        public Task<List<Currency>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<Currency> Get(string Id)
        {
            var result = await context.Currencies.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
            if (result != null)
            {
                var currencyDefault = context.Currencies.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).FirstOrDefault();
                if (currencyDefault != null)
                {
                    result.CurrencyIdDefault = currencyDefault.Id;
                    result.CurrencySymbolDefault = currencyDefault.CurrencySymbol;
                }
            }
            return result;
        }

        public async Task<Currency> GetCurrencyDefault() => await context.Currencies.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).FirstOrDefaultAsync();

    }
}