using Phoenix.Data.Models.Media;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models.Media.Transaction.MediaPlanTV;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Media.MasterData.MediaOrderStatus;

namespace Phoenix.Data.Services.Media
{
    public interface IMediaOrderTVService : IDataService<TrMediaOrderTV>
    {
        Task<List<EmployeeBasicInfo>> GetEmployee();
        Task<List<JobTitle>> GetJobTitle();
        Task<List<TmMediaOrderStatus>> GetMediaOrderStatus();
        Task<List<MediaOrderTVList>> GetList();
        Task<List<MediaOrderTVList>> GetApprovalList();
        Task<List<TrMediaOrderTVDetail>> GetDetailList(string id);
        Task<TrMediaOrderTVDetail> GetDetail(string id);
        Task<List<TrMediaOrderTVDetailSchedule>> GetDetailScheduleList(string id);
        Task<TrMediaPlanTV> GetMediaPlanTV(string id);
        Task<List<TrMediaPlanTVDetail>> GetMediaPlanTVDetail(string id);
        List<TrMediaPlanTVDetailSchedule> GetMediaPlanTVDetailSchedule(string id);
        Task<int> AddDetailAsync(TrMediaOrderTVDetail entity);
        Task<int> AddRangeDetailAsync(params TrMediaOrderTVDetail[] entities);
        Task<int> AddRangeDetailScheduleAsync(params TrMediaOrderTVDetailSchedule[] entities);
        Task<int> EditRangeDetailAsync(params TrMediaOrderTVDetail[] entities);
        Task<int> ApproveAsync(TrMediaOrderTV entity);
    }
    public class MediaOrderTVService : IMediaOrderTVService
    {
        readonly DataContext context;

        public MediaOrderTVService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(TrMediaOrderTV entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            entity.TmMediaOrderStatusId = MediaOrderStatusName.Draft;
            entity.EmployeeBasicInfoId = context.GetEmployeeId();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> AddRangeAsync(params TrMediaOrderTV[] entities)
        {
            await context.PhoenixAddRangeAsync(entities);
            return await context.SaveChangesAsync();
        }

        public async Task<int> AddDetailAsync(TrMediaOrderTVDetail entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> AddRangeDetailAsync(params TrMediaOrderTVDetail[] entities)
        {
            await context.PhoenixAddRangeAsync(entities);
            return await context.SaveChangesAsync();
        }

        public async Task<int> AddRangeDetailScheduleAsync(params TrMediaOrderTVDetailSchedule[] entities)
        {
            await context.PhoenixAddRangeAsync(entities);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteAsync(TrMediaOrderTV entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params TrMediaOrderTV[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(TrMediaOrderTV entity)
        {
            context.PhoenixDelete(entity);
            var details = context.TrMediaOrderTVDetails.Where(m => m.TrMediaOrderId == entity.Id);
            if (details.Any())
            {
                foreach (var detail in details)
                {
                    context.PhoenixDelete(detail);
                    var schedules = context.TrMediaOrderTVDetailSchedules.Where(m => m.TrMediaOrderDetailId == detail.Id);
                    if (schedules.Any())
                    {
                        foreach (var schedule in schedules)
                        {
                            context.PhoenixDelete(schedule);
                        }
                    }
                }
            }
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params TrMediaOrderTV[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(TrMediaOrderTV entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(TrMediaOrderTV entity)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditRangeDetailAsync(params TrMediaOrderTVDetail[] entities)
        {
            context.PhoenixEditRange(entities);
            return await context.SaveChangesAsync();
        }

        public async Task<List<TrMediaOrderTV>> Get() => await context.TrMediaOrderTVs.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).ToListAsync();

        public Task<List<TrMediaOrderTV>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<TrMediaOrderTV> Get(string id) => await context.TrMediaOrderTVs.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == id).AsNoTracking().FirstOrDefaultAsync();

        public async Task<List<EmployeeBasicInfo>> GetEmployee() => await context.EmployeeBasicInfos.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).AsNoTracking().ToListAsync();

        public async Task<List<JobTitle>> GetJobTitle() => await context.JobTitles.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).AsNoTracking().ToListAsync();

        public async Task<List<TmMediaOrderStatus>> GetMediaOrderStatus() => await context.TmMediaOrderStatuses.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).AsNoTracking().ToListAsync();

        public async Task<List<MediaOrderTVList>> GetList()
        {
            var mediaOrder = await this.Get();
            var employee = await this.GetEmployee();
            var jobTitle = await this.GetJobTitle();
            var mediaOrderStatus = await this.GetMediaOrderStatus();

            var query = from mo in mediaOrder
                        join emp in employee on mo.EmployeeBasicInfoId equals emp.Id into leftEmp
                        from emp in leftEmp.DefaultIfEmpty()
                        join job in jobTitle on emp.JobTitleId equals job.Id into leftJob
                        from job in leftJob.DefaultIfEmpty()
                        join stat in mediaOrderStatus on mo.TmMediaOrderStatusId equals stat.Id into leftStat
                        from stat in leftStat.DefaultIfEmpty()
                        select new MediaOrderTVList
                        {
                            Id = mo.Id,
                            MediaOrderNo = mo.MediaOrderNo,
                            EmployeeBasicInfoId = mo.EmployeeBasicInfoId,
                            EmployeeBasicInfo_Name = emp?.NameEmployee,
                            JobTitleId = emp?.JobTitleId,
                            JobTitle_Name = job?.TitleName,
                            TotalGrossValue = mo.TotalGrossValue,
                            GrandTotal = mo.GrandTotal,
                            TotalNetCost = mo.TotalNetCost,
                            Vat = mo.Vat,
                            TmMediaOrderStatusId = mo.TmMediaOrderStatusId,
                            TmMediaOrderStatus_Name = stat?.MediaOrderStatusName
                        };

            return query.ToList();
        }

        public async Task<List<MediaOrderTVList>> GetApprovalList()
        {
            var mediaOrder = await this.Get();
            var employee = await this.GetEmployee();
            var jobTitle = await this.GetJobTitle();
            var mediaOrderStatus = await this.GetMediaOrderStatus();

            var query = from mo in mediaOrder
                        join emp in employee on mo.EmployeeBasicInfoId equals emp.Id into leftEmp
                        from emp in leftEmp.DefaultIfEmpty()
                        join job in jobTitle on emp.JobTitleId equals job.Id into leftJob
                        from job in leftJob.DefaultIfEmpty()
                        join stat in mediaOrderStatus on mo.TmMediaOrderStatusId equals stat.Id into leftStat
                        from stat in leftStat.DefaultIfEmpty()
                        where stat.Id != MediaOrderStatusName.Draft
                        select new MediaOrderTVList
                        {
                            Id = mo.Id,
                            MediaOrderNo = mo.MediaOrderNo,
                            EmployeeBasicInfoId = mo.EmployeeBasicInfoId,
                            EmployeeBasicInfo_Name = emp?.NameEmployee,
                            JobTitleId = emp?.JobTitleId,
                            JobTitle_Name = job?.TitleName,
                            TotalGrossValue = mo.TotalGrossValue,
                            GrandTotal = mo.GrandTotal,
                            TotalNetCost = mo.TotalNetCost,
                            Vat = mo.Vat,
                            TmMediaOrderStatusId = mo.TmMediaOrderStatusId,
                            TmMediaOrderStatus_Name = stat?.MediaOrderStatusName
                        };

            return query.ToList();
        }

        public async Task<List<TrMediaOrderTVDetail>> GetDetailList(string id) => await context.TrMediaOrderTVDetails.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.TrMediaOrderId == id).AsNoTracking().ToListAsync();

        public async Task<TrMediaOrderTVDetail> GetDetail(string id) => await context.TrMediaOrderTVDetails.AsNoTracking().SingleOrDefaultAsync(x => !(x.IsDeleted ?? false) && x.Id == id);

        public async Task<TrMediaPlanTV> GetMediaPlanTV(string id) => await context.TrMediaPlanTVs.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id && !(x.IsDeleted ?? false));

        public async Task<List<TrMediaPlanTVDetail>> GetMediaPlanTVDetail(string id) => await context.TrMediaPlanTVDetails.AsNoTracking().Where(x => x.TrMediaPlanId == id && !(x.IsDeleted ?? false)).ToListAsync();

        public List<TrMediaPlanTVDetailSchedule> GetMediaPlanTVDetailSchedule(string id) => context.TrMediaPlanTVDetailSchedules.AsNoTracking().Where(x => x.TrMediaPlanDetailId == id && !(x.IsDeleted ?? false)).ToList();

        public async Task<int> ApproveAsync(TrMediaOrderTV entity)
        {
            context.PhoenixApprove(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<List<TrMediaOrderTVDetailSchedule>> GetDetailScheduleList(string id) => await context.TrMediaOrderTVDetailSchedules.AsNoTracking().Where(x => x.TrMediaOrderDetailId == id && !(x.IsDeleted ?? false)).ToListAsync();
    }
}
