using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Enum;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface IMasterBudgetCodeService : IDataService<MasterBudgetCode>
    {
        Task<List<MasterBudgetCode>> GetList(string type);
    }

    public class MasterBudgetCodeService : IMasterBudgetCodeService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage MasterBudgetCode
        /// </summary>
        /// <param name="context">Database context</param>
        public MasterBudgetCodeService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(MasterBudgetCode entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            if (entity.Source == BudgetCodeType.CAPEX)
            {
                entity.BudgetCode = await TransID.GetTransId(context, Code.BudgetCodeCapex, DateTime.Today);
            }
            else
            {
                entity.BudgetCode = await TransID.GetTransId(context, Code.BudgetCodeOpex, DateTime.Today);
            }
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params MasterBudgetCode[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(MasterBudgetCode entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params MasterBudgetCode[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(MasterBudgetCode entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params MasterBudgetCode[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(MasterBudgetCode entity)
        {
            var last = await context.MasterBudgetCodes.AsNoTracking().AsNoTracking().Where(x => x.Id == entity.Id).FirstOrDefaultAsync();
            var budget = await context.BudgetMasters.AsNoTracking().Where(x => x.MasterBudgetCodeId == entity.Id).ToListAsync();

            //update coa
            if (last.CoaId != entity.CoaId)
            {
                foreach (var item in budget.Where(x => x.MasterBudgetCodeId == entity.Id && x.AccountId == last.CoaId).ToList())
                {
                    item.AccountId = entity.CoaId;
                    context.PhoenixEdit(item);
                }
            }

            //update division
            if (last.HrisDivisionId != entity.HrisDivisionId)
            {
                foreach (var item2 in budget.Where(x => x.MasterBudgetCodeId == entity.Id && x.HrisDivisionId == last.HrisDivisionId).ToList())
                {
                    item2.HrisDivisionId = entity.HrisDivisionId;
                    context.PhoenixEdit(item2);
                }
            }

            //update budget owner
            if (last.BugetOwnerId != entity.BugetOwnerId)
            {
                foreach (var item2 in budget.Where(x => x.MasterBudgetCodeId == entity.Id && x.BugetOwnerId == last.BugetOwnerId).ToList())
                {
                    item2.BugetOwnerId = entity.BugetOwnerId;
                    context.PhoenixEdit(item2);
                }
            }

            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(MasterBudgetCode entity)
        {
            throw new NotImplementedException();
        }
        public async Task<List<MasterBudgetCode>> GetList(string type)
        {
            var result = await (from a in context.MasterBudgetCodes
                                join b in context.BusinessUnits on a.HrisDivisionId equals b.Id
                                join c in context.EmployeeBasicInfos on a.BugetOwnerId equals c.Id
                                where !(a.IsDeleted ?? false) && a.Source == type
                                select new MasterBudgetCode
                                {
                                    Id = a.Id,
                                    BudgetCode = a.BudgetCode,
                                    BudgetName = a.BudgetName,
                                    CoaName = a.CoaName,
                                    HrisDivisionName = b.UnitName,
                                    BugetOwnerId = a.BugetOwnerId,
                                    BugetOwnerName = c.NameEmployee,
                                    CreatedOn = a.CreatedOn
                                }).ToListAsync();
            return result;
        }

        public Task<List<MasterBudgetCode>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<MasterBudgetCode> Get(string Id)
        {
            var result = await context.MasterBudgetCodes.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
            if (result != null)
            {
                result.CodeRec = await context.ChartOfAccounts.Where(x => x.Id == result.CoaId).Select(x => x.CodeRec).FirstOrDefaultAsync();
            }
            return result;
        }

        public Task<List<MasterBudgetCode>> Get()
        {
            throw new NotImplementedException();
        }
    }
}