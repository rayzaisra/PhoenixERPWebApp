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
    public interface ICostSharingAllocationService
    {
        Task<int> AddAsync(CostSharingAllocation entity);
        Task<int> DeleteSoftAsync(CostSharingAllocation entity);
        Task<int> EditAsync(CostSharingAllocation entity);
        Task<CostSharingAllocation> Get(string Id);
        Task<List<CostSharingAllocation>> Get();
        Task<List<CostSharingAllocationDetail>> GetDetail(string csaId);
    }

    public class CostSharingAllocationService : ICostSharingAllocationService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage CostSharingAllocation
        /// </summary>
        /// <param name="context">Database context</param>
        public CostSharingAllocationService(DataContext context)
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
                        entity.Id = await TransID.GetTransId(context, Code.MasterAllocationRatio, DateTime.Today); //Guid.NewGuid().ToString();//Guid.NewGuid().ToString();
                        await context.PhoenixAddAsync(entity);
                        await context.SaveChangesAsync();
                    }

                    var csaDetails = (from a in entity.CostSharingAllocationDetails
                                      group a by new { a.BusinessUnitId, a.BusinessUnitName } into g
                                      select new { g.Key.BusinessUnitId, g.Key.BusinessUnitName, count = g.Count() }).Where(x => x.count > 1).ToList();

                    if (csaDetails.Count() > 0)
                    {
                        throw new Exception("Business unit " + string.Join(", ", csaDetails.Select(x => x.BusinessUnitName).Distinct()) + " already exists.");
                    }

                    foreach (var item in entity.CostSharingAllocationDetails)
                    {
                        item.Id = Guid.NewGuid().ToString();
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
                                    //join b in context.ChartOfAccounts on a.CoaId equals b.Id
                                where !(a.IsDeleted ?? false) //&& b.Level == "5" && a.IsMonthly == null
                                select new CostSharingAllocation
                                {
                                    Id = a.Id,
                                    CostSharingAllocationName = a.CostSharingAllocationName,
                                    CreatedOn = a.CreatedOn
                                    //CoaId = a.CoaId,
                                    //CodeRec = b.CodeRec,
                                    //CoaName = b.Name5
                                }).ToListAsync();
            return result;
        }

        public async Task<CostSharingAllocation> Get(string Id)
        {
            var result = await context.CostSharingAllocations.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
            ////if (result != null)
            ////{
            ////    result.CodeRec = await context.ChartOfAccounts.Where(x => x.Id == result.CoaId).Select(x => x.CodeRec).FirstOrDefaultAsync();
            ////}
            return result;
        }

        public async Task<List<CostSharingAllocationDetail>> GetDetail(string csaId)
        {
            var result = await (from a in context.CostSharingAllocationDetails
                                join b in context.BusinessUnits on a.BusinessUnitId equals b.Id
                                join c in context.BusinessUnitTypes on b.BusinessUnitTypeId equals c.Id
                                where !(a.IsDeleted ?? false) && a.CostSharingAllocationId == csaId && c.BusinessUnitLevel == BusinessUnitLevelCode.Division
                                select new CostSharingAllocationDetail
                                {
                                    Id = a.Id,
                                    CostSharingAllocationId = a.CostSharingAllocationId,
                                    BusinessUnitId = a.BusinessUnitId,
                                    CostSharingAllocationPercent = a.CostSharingAllocationPercent,
                                    CreatedBy = a.CreatedBy,
                                    CreatedOn = a.CreatedOn,
                                    BusinessUnitName = b.UnitName
                                }).ToListAsync();
            return result;
        }
    }
}