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
using Phoenix.Shared.Core.PrincipalHelpers;

namespace Phoenix.Data
{
    public interface IMasterMonthlyBudgetService
    {
        Task<int> AddAsync(CostSharingAllocation entity);
        Task<int> DeleteSoftAsync(CostSharingAllocation entity);
        Task<int> EditAsync(CostSharingAllocation entity);
        Task<CostSharingAllocation> Get(string Id);
        Task<List<CostSharingAllocation>> Get();
        Task<List<CostSharingAllocationDetail>> GetDetail(string csaId);
    }
    public class MasterMonthlyBudgetService : IMasterMonthlyBudgetService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage CostSharingAllocation
        /// </summary>
        /// <param name="context">Database context</param>
        public MasterMonthlyBudgetService(DataContext context)
        {
            this.context = context;
        }
        public async Task<int> AddAsync(CostSharingAllocation entity)
        {
            return await AddEdit(entity, false);
        }

        public async Task<int> DeleteSoftAsync(CostSharingAllocation entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> EditAsync(CostSharingAllocation entity)
        {
            return await AddEdit(entity, true);
        }

        private async Task<int> AddEdit(CostSharingAllocation entity, bool isEdit)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (isEdit)
                    {
                        context.PhoenixEdit(entity);

                        if (!string.IsNullOrEmpty(entity.Id))
                        {
                            var detail = context.CostSharingAllocationDetails.AsNoTracking().Where(x => x.CostSharingAllocationId == entity.Id).ToList();
                            if (detail.Count > 0)
                            {
                                context.CostSharingAllocationDetails.RemoveRange(detail);
                            }
                        }
                    }
                    else
                    {
                        entity.Id = Guid.NewGuid().ToString();
                        await context.PhoenixAddAsync(entity);
                        //await context.SaveChangesAsync();
                    }

                    foreach (var item in entity.CostSharingAllocationDetails)
                    {
                        item.Id = Guid.NewGuid().ToString();
                        //item.BusinessUnitId = ClainJwtPrincipalHelper.GetBusinessUnitDivisi(context.HttpContext);
                        item.CostSharingAllocationId = entity.Id;
                        context.PhoenixAdd(item);
                    }

                    var save = await context.SaveChangesAsync();
                    transaction.Commit();
                    return save;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<List<CostSharingAllocation>> Get()
        {
            var result = await (from a in context.CostSharingAllocations
                                where !(a.IsDeleted ?? false) && a.IsMonthly == true
                                select new CostSharingAllocation
                                {
                                    Id = a.Id,
                                    CostSharingAllocationName = a.CostSharingAllocationName,
                                    CreatedOn = a.CreatedOn
                                }).ToListAsync();
            return result;
        }

        public async Task<CostSharingAllocation> Get(string Id)
        {
            var result = await context.CostSharingAllocations.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<CostSharingAllocationDetail>> GetDetail(string csaId)
        {
            var result = await (from a in context.CostSharingAllocationDetails
                                where !(a.IsDeleted ?? false) && a.CostSharingAllocationId == csaId
                                select new CostSharingAllocationDetail
                                {
                                    Id = a.Id,
                                    CostSharingAllocationId = a.CostSharingAllocationId,
                                    MonthlyDescription = a.MonthlyDescription,
                                    CostSharingAllocationPercent = a.CostSharingAllocationPercent,
                                    CreatedBy = a.CreatedBy,
                                    CreatedOn = a.CreatedOn
                                }).ToListAsync();
            return result;
        }
    }
}
