using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Um;
using Phoenix.Data.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public class DoaService : IDoaService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage SalaryComponent
        /// </summary>
        /// <param name="context">Database context</param>
        public DoaService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(Doa entity)
        {
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params Doa[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(Doa entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params Doa[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(Doa entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params Doa[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(Doa entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(Doa entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Doa>> Get() => await context.DOAs.AsNoTracking().ToListAsync();
        public async Task<List<Doa>> GetRequestByUserId(string userId)
        {
            var data = await (from x in context.DOAs.Where(c=>c.IsDeleted.Equals(false))
                              join u in context.TmUserApps.Where(c => c.IsDeleted.Equals(false)) on x.AssignFrom equals u.Id
                              select new Doa
                              {
                                  Id = x.Id,
                                  AssignFrom = x.AssignFrom,
                                  AssignTo = x.AssignTo,
                                  FromDate = x.FromDate,
                                  ToDate = x.ToDate,
                                  IsAssignToApproved = x.IsAssignToApproved,
                                  ReasonDoa = x.ReasonDoa,
                                  ReasonRejectAssign = x.ReasonRejectAssign,
                                  IsActive = x.IsActive,
                                  EmployeeName = string.IsNullOrEmpty(u.AliasName) ? u.Id : u.AliasName
                              }).Where(x=>x.AssignFrom.Equals(userId) && x.ToDate > DateTime.Now).ToListAsync();
            return data;
        }
        public async Task<List<Doa>> GetNeedApprovalByUserId(string userId)
        {
            var data = await (from x in context.DOAs.Where(c => c.IsDeleted.Equals(false) && c.AssignTo.Equals(userId) && (c.IsAssignToApproved == null))
                              join u in context.TmUserApps.Where(c => c.IsDeleted.Equals(false) && c.Id.Equals(userId)) on x.AssignTo equals u.Id
                              select new Doa
                              {
                                  Id = x.Id,
                                  AssignFrom = x.AssignFrom,
                                  AssignTo = x.AssignTo,
                                  FromDate = x.FromDate,
                                  ToDate = x.ToDate,
                                  IsAssignToApproved = x.IsAssignToApproved,
                                  ReasonDoa = x.ReasonDoa,
                                  ReasonRejectAssign = x.ReasonRejectAssign,
                                  IsActive = x.IsActive,
                                  EmployeeName = string.IsNullOrEmpty(u.AliasName) ? u.Id : u.AliasName
                              }).OrderByDescending(x=>x.FromDate).ToListAsync();
            return data;
        }

        public Task<List<Doa>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }
        public async Task<List<UserDoaVm>> IsUserInDoa()
        {
            var userAppId = context.GetUserId();
            var now = DateTime.Now;
            var data = await(from x in context.DOAs
                             .Where(c=>c.AssignTo.Equals(userAppId) && c.IsAssignToApproved.Equals(true) && c.IsDeleted.Equals(false) 
                             && c.IsActive.Equals(false) && c.FromDate >= now && c.ToDate <= now)
                             join u in context.TmUserApps.Where(c=>c.IsDeleted.Equals(false) && c.IsActive.Equals(true)) on x.AssignFrom equals u.Id
                             select new UserDoaVm
                             {
                                 UserAppId = x.AssignFrom,
                                 EmployeeId = u.EmployeeBasicInfoId
                             }
                             ).ToListAsync();
            return data;
        }
        public async Task<Doa> Get(string Id) => await context.DOAs.AsNoTracking().Where(x => x.Id == Id).AsNoTracking().FirstOrDefaultAsync();
    }
}
