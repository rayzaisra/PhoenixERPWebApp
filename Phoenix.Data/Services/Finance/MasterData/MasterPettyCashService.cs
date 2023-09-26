using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;

namespace Phoenix.Data
{
    public interface IMasterPettyCashService : IDataService<MasterPettyCash>
    {
    }

    public class MasterPettyCashService : IMasterPettyCashService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage MasterPettyCash
        /// </summary>
        /// <param name="context">Database context</param>
        public MasterPettyCashService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(MasterPettyCash entity)
        {
            if (context.MasterPettyCashs.Where(x => !(x.IsDeleted ?? false) && x.LegalEntityId == entity.LegalEntityId && x.ChartAccountId == entity.ChartAccountId).Count() > 0)
            {
                var legalEntityName = await context.LegalEntity.Where(x => x.Id == entity.LegalEntityId).Select(x => x.LegalEntityName).FirstOrDefaultAsync() ?? "";
                var coaName = await context.ChartOfAccounts.Where(x => x.Id == entity.ChartAccountId).Select(x => x.Name5).FirstOrDefaultAsync() ?? "";
                throw new Exception($"Legal Entity : {legalEntityName} and COA : {coaName} already exists.");
            }

            string ID = await TransID.GetTransId(context, Code.MasterPettyCash, DateTime.Today);
            entity.Id = ID;
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params MasterPettyCash[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(MasterPettyCash entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params MasterPettyCash[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(MasterPettyCash entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params MasterPettyCash[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(MasterPettyCash entity)
        {
            if (context.MasterPettyCashs.Where(x => !(x.IsDeleted ?? false) && x.LegalEntityId == entity.LegalEntityId && x.ChartAccountId == entity.ChartAccountId && x.Id != entity.Id).Count() > 0)
            {
                var legalEntityName = await context.LegalEntity.Where(x => x.Id == entity.LegalEntityId).Select(x => x.LegalEntityName).FirstOrDefaultAsync() ?? "";
                var coaName = await context.ChartOfAccounts.Where(x => x.Id == entity.ChartAccountId).Select(x => x.Name5).FirstOrDefaultAsync() ?? "";
                throw new Exception($"Legal Entity : {legalEntityName} and Chart Account Name : {coaName} already exists.");
            }

            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(MasterPettyCash entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MasterPettyCash>> Get()
        {
            var result = await (from a in context.MasterPettyCashs
                                join c in context.Currencies on a.CurrencyId equals c.Id
                                join d in context.ChartOfAccounts on new { Id = a.ChartAccountId, Level = "5" } equals new { d.Id, d.Level }
                                join e in context.LegalEntity on a.LegalEntityId equals e.Id
                                join f in context.EmployeeBasicInfos on a.HolderUserId equals f.Id
                                join g in context.EmployeeBasicInfos on a.DelegateUserId equals g.Id
                                where !(a.IsDeleted ?? false)
                                select new MasterPettyCash
                                {
                                    Id = a.Id,
                                    BankAccountName = a.BankAccountName,
                                    BankAccountNumber = a.BankAccountNumber,
                                    Balance = a.Balance,
                                    CurrencyId = a.CurrencyId,
                                    CurrencyCode = c.CurrencyCode,
                                    ChartAccountId = a.ChartAccountId,
                                    ChartAccountName = d.Name5,
                                    ChartAccountNumber = d.CodeRec,
                                    LegalEntityId = a.LegalEntityId,
                                    LegalEntityName = e.LegalEntityName,
                                    HolderUserId = f.NameEmployee,
                                    DelegateUserId = g.NameEmployee,
                                    AmountInsused = a.AmountInsused,
                                    CreatedBy = a.CreatedBy,
                                    CreatedOn = a.CreatedOn
                                }).ToListAsync();

            return result;
        }

        public Task<List<MasterPettyCash>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<MasterPettyCash> Get(string Id) => await context.MasterPettyCashs.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
    }
}