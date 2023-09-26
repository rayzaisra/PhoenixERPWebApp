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
    public interface IEFakturService : IDataService<EFaktur>
    {
    }

    public class EFakturService : IEFakturService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage EFaktur
        /// </summary>
        /// <param name="context">Database context</param>
        public EFakturService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(EFaktur entity)
        {
            return await AddEditAsync(entity);
        }

        public Task<int> AddRangeAsync(params EFaktur[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(EFaktur entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params EFaktur[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(EFaktur entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params EFaktur[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(EFaktur entity)
        {
            return await AddEditAsync(entity);
        }

        private async Task<int> AddEditAsync(EFaktur entity)
        {
            //var result = await context.EFaktur.Where(x => !(x.IsDeleted ?? false) && x.Id != entity.Id && x.LegalEntityId == entity.LegalEntityId && (entity.StartDate >= x.StartDate && entity.StartDate <= x.EndDate || entity.EndDate >= x.StartDate && entity.EndDate <= x.EndDate)).ToListAsync();
            //if (result.Count() > 0)
            //{
            //    var legal = await context.LegalEntity.Where(x => !(x.IsDeleted ?? false) && x.Id == entity.LegalEntityId).FirstOrDefaultAsync();
            //    throw new Exception($"for Legal Entity {legal.LegalEntityName} and Period {Convert.ToDateTime(result.Min(x => x.StartDate)).ToString("dd MMM yyyy")} - {Convert.ToDateTime(result.Max(x => x.EndDate)).ToString("dd MMM yyyy")} already exist.");
            //}


            if (string.IsNullOrEmpty(entity.Id))
            {
                entity.Id = Guid.NewGuid().ToString();
                await context.PhoenixAddAsync(entity);
            }
            else
            {
                context.PhoenixEdit(entity);

                var details = await context.EFakturDetail.Where(x => x.EFakturId == entity.Id).ToListAsync();
                context.EFakturDetail.RemoveRange(details.ToArray());
            }

            //var start1 = entity.StartNumber.Substring(0, 3);
            //var start2 = entity.StartNumber.Substring(4, 3);
            //var start3 = entity.StartNumber.Substring(8, 2);
            //var start4 = entity.StartNumber.Substring(11, 8);

            ////var end1 = entity.EndNumber.Substring(0, 3);
            ////var end2 = entity.EndNumber.Substring(4, 3);
            ////var end3 = entity.EndNumber.Substring(8, 2);
            //var end4 = entity.EndNumber.Substring(11, 8);
            
            var start2 = entity.StartNumber.Substring(0, 3);
            var start3 = entity.StartNumber.Substring(4, 2);
            var start4 = entity.StartNumber.Substring(7, 8);

            var end4 = entity.EndNumber.Substring(7, 8);

            for (var i = int.Parse(start4); i <= int.Parse(end4); i++)
            {
                var detail = new EFakturDetail()
                {
                    Id = Guid.NewGuid().ToString(),
                    EFakturId = entity.Id,
                    vat_no = $"{start2.ToString().PadLeft(3, '0')}.{start3.ToString().PadLeft(2, '0')}.{i.ToString().PadLeft(8, '0')}",
                    IsUsed = false
                };
                context.PhoenixAdd(detail);
            }

            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(EFaktur entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<EFaktur>> Get()
        {
            return await (from a in context.EFaktur
                          join b in context.LegalEntity on a.LegalEntityId equals b.Id
                          where !(a.IsDeleted ?? false)
                          select new EFaktur
                          {
                              Id = a.Id,
                              Name = a.Name,
                              StartDate = a.StartDate,
                              EndDate = a.EndDate,
                              LegalEntityId = b.LegalEntityName,
                              StartNumber = a.StartNumber,
                              EndNumber = a.EndNumber,
                              Used = context.EFakturDetail.Where(x => !(x.IsDeleted ?? false) && x.EFakturId == a.Id && x.IsUsed == true).Count(),
                              Remaining = context.EFakturDetail.Where(x => !(x.IsDeleted ?? false) && x.EFakturId == a.Id && x.IsUsed == false).Count(),
                              CreatedBy = context.EmployeeBasicInfos.Where(x => x.Id == a.CreatedBy).Select(x => x.NameEmployee).FirstOrDefault()
                          }).ToListAsync();
        }

        public Task<List<EFaktur>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<EFaktur> Get(string Id)
        {
            var result = await context.EFaktur.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
            if (result != null) result.Used = context.EFakturDetail.Where(x => !(x.IsDeleted ?? false) && x.EFakturId == Id && x.IsUsed == true).Count();
            return result;
        }
    }
}