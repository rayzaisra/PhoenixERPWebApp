using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    public interface IBankService : IDataService<Bank>
    {
    }

    public class BankService : IBankService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage Bank
        /// </summary>
        /// <param name="context">Database context</param>
        public BankService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(Bank entity)
        {
            return await AddEdit(entity, false);
        }

        public Task<int> AddRangeAsync(params Bank[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(Bank entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params Bank[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(Bank entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params Bank[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(Bank entity)
        {
            return await AddEdit(entity, true);
        }

        public Task<int> EditRangeAsync(Bank entity)
        {
            throw new NotImplementedException();
        }

        private async Task<int> AddEdit(Bank entity, bool isEdit)
        {
            var isAlready = false;
            if (isEdit) isAlready = context.Banks.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.CoaId == entity.CoaId && x.Id != entity.Id).Count() > 0;
            else isAlready = context.Banks.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.CoaId == entity.CoaId).Count() > 0;

            if (isAlready)
            {
                var coa = await (from a in context.ChartOfAccounts
                                 join b in context.Banks on a.Id equals b.CoaId
                                 where a.Id == entity.CoaId
                                 select new { a.Name5, b.BankName }).FirstOrDefaultAsync();
                throw new Exception($"chart of account {coa.Name5} already exists in {coa.BankName}, please choose another chart of account!");
            }

            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (isEdit)
                    {
                        context.PhoenixEdit(entity);

                        await context.Database.ExecuteSqlCommandAsync(@"update b 
                                set account_id=@AccountId 
                                from fn.payment_request a
                                inner join fn.journal_temp b on a.id = b.reference_id and a.account_id=b.account_id
                                where a.bank_id=@Id AND a.status!=@Status",
                                new SqlParameter("@Id", entity.Id),
                                new SqlParameter("@AccountId", entity.CoaId),
                                new SqlParameter("@Status", PaymentRequestStatus.Complete));

                        await context.Database.ExecuteSqlCommandAsync(@"update fn.payment_request 
                                set account_id=@CoaId, account_number=@AccountNumber 
                                where bank_id=@Id AND status!=@Status",
                                new SqlParameter("@Id", entity.Id),
                                new SqlParameter("@CoaId", entity.CoaId),
                                new SqlParameter("@AccountNumber", entity.AccountNumber),
                                new SqlParameter("@Status", PaymentRequestStatus.Complete));
                    }
                    else
                    {
                        entity.Id = Guid.NewGuid().ToString();
                        await context.PhoenixAddAsync(entity);
                    }

                    var save = await context.SaveChangesAsync();
                    transaction.Commit();
                    return save;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message.ToString());
                }
            }
        }
        public async Task<List<Bank>> Get()
        {
            var result = await (from a in context.Banks
                                join b in context.Currencies on a.CurrencyId equals b.Id
                                join c in context.LegalEntity on a.LegalEntityId equals c.Id
                                where !(a.IsDeleted ?? false)
                                select new Bank
                                {
                                    Id = a.Id,
                                    AccountNumber = a.AccountNumber,
                                    BankCode = a.BankCode,
                                    BankName = a.BankName,
                                    SwiftCode = a.SwiftCode,
                                    CurrencyId = a.CurrencyId,
                                    CurrencyName = b.CurrencyName + " (" + b.CurrencySymbol + ")",
                                    LegalEntityName = c.LegalEntityName
                                }).ToListAsync();
            return result;
        }

        public Task<List<Bank>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<Bank> Get(string Id)
        {
            var result = await context.Banks.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
            if (result != null) result.CodeRec = await context.ChartOfAccounts.Where(x => x.Id == result.CoaId).Select(x => x.CodeRec).FirstOrDefaultAsync();
            return result;
        }
    }
}