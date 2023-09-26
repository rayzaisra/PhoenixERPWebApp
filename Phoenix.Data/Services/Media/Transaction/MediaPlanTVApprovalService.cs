using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;
using Phoenix.Data.Models.Media.Transaction.MediaPlanTV;
using Phoenix.Data.Models;

namespace Phoenix.Data.Services.Media.Transaction
{
    public interface IMediaPlanTVApprovalService
    {
        Task<TrMediaPlanTV> Get(string Id);
        Task<TrMediaPlanTVDetail> GetDetail(string id);
        Task<List<TrMediaPlanTV>> GetMediaPlanTVsApproval();
        Task<List<TrMediaPlanTVDetail>> GetMediaPlanTVDetails(string mediaPlanTvId);
        Task<List<TrMediaPlanTVDetailSchedule>> GetDetailScheduleList(string id);
    }
    public class MediaPlanTVApprovalService : IMediaPlanTVApprovalService
    {
        readonly DataContext context;
        public MediaPlanTVApprovalService(DataContext context)
        {
            this.context = context;
        }

        public async Task<List<TrMediaPlanTV>> GetMediaPlanTVsApproval()
        {
            var data = await (from tv in context.TrMediaPlanTVs
                              join emp in context.EmployeeBasicInfos on tv.EmployeeBasicInfoId equals emp.Id
                              join job in context.JobTitles on emp.JobTitleId equals job.Id into j
                              from job in j.DefaultIfEmpty()
                              join stat in context.TmMediaPlanStatuses on tv.TmMediaPlanStatusId equals stat.Id
                              where tv.TmMediaPlanStatusId == MediaPlanStatusName.NeedApproval
                              select new TrMediaPlanTV
                              {
                                  Id = tv.Id,
                                  MediaPlanNo = tv.MediaPlanNo,
                                  Version = tv.Version,
                                  Duration = tv.Duration,
                                  TargetAudience = tv.TargetAudience,
                                  MediaTypeId = tv.MediaTypeId,
                                  PeriodCampaignStart = tv.PeriodCampaignStart,
                                  PeriodCampaignEnd = tv.PeriodCampaignEnd,
                                  DatePrepared = tv.DatePrepared,
                                  RevisionNo = tv.RevisionNo,
                                  TotalNetCost = tv.TotalNetCost,
                                  AgencyServiceFee = tv.AgencyServiceFee,
                                  AgencyServiceFeePercent = tv.AgencyServiceFeePercent,
                                  TaxBased = tv.TaxBased,
                                  Vat = tv.Vat,
                                  GrandTotal = tv.GrandTotal,
                                  TmMediaPlanRequestTypeId = tv.TmMediaPlanRequestTypeId,
                                  TrMediaFileMasterId = tv.TrMediaFileMasterId,
                                  TmMediaPlanStatusId = tv.TmMediaPlanStatusId,
                                  EmployeeBasicInfoId = tv.EmployeeBasicInfoId,
                                  CreatedBy = tv.CreatedBy,
                                  CreatedOn = tv.CreatedOn,
                                  ModifiedBy = tv.ModifiedBy,
                                  ModifiedOn = tv.ModifiedOn,
                                  ApprovedBy = tv.ApprovedBy,
                                  ApprovedOn = tv.ApprovedOn,
                                  IsActive = tv.IsActive,
                                  IsLocked = tv.IsLocked,
                                  IsDefault = tv.IsDefault,
                                  IsDeleted = tv.IsDeleted,
                                  OwnerId = tv.OwnerId,
                                  DeletedBy = tv.DeletedBy,
                                  DeletedOn = tv.DeletedOn,
                                  RequesterName = emp.NameEmployee,
                                  JobTitleName = job.TitleName,
                                  MediaPlanStatusName = stat.MediaPlanStatusName
                              }).ToListAsync();

            return data;
        }

        public async Task<TrMediaPlanTV> Get(string Id) => await context.TrMediaPlanTVs.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).AsNoTracking().FirstOrDefaultAsync();

        public async Task<TrMediaPlanTVDetail> GetDetail(string id) => await context.TrMediaPlanTVDetails.AsNoTracking().SingleOrDefaultAsync(x => !(x.IsDeleted ?? false) && x.Id == id);

        public async Task<List<TrMediaPlanTVDetailSchedule>> GetDetailScheduleList(string id) => await context.TrMediaPlanTVDetailSchedules.AsNoTracking().Where(x => x.TrMediaPlanDetailId == id && !(x.IsDeleted ?? false)).ToListAsync();

        public async Task<List<TrMediaPlanTVDetail>> GetMediaPlanTVDetails(string mediaPlanTvId)
        {
            var data = await (from tvDetail in context.TrMediaPlanTVDetails
                              join tv in context.TrMediaPlanTVs on tvDetail.TrMediaPlanId equals tv.Id
                              join type in context.TmProgramTypes on tvDetail.TmProgramTypeId equals type.Id into ty
                              from types in ty.DefaultIfEmpty()
                              join categoryType in context.TmProgramCategoryTypes on tvDetail.TmProgramCategoryTypeId equals categoryType.Id into ct
                              from category in ct.DefaultIfEmpty()
                              join pos in context.TmProgramPositions on tvDetail.TmProgramPositionId equals pos.Id into ps
                              from position in ps.DefaultIfEmpty()
                              where tv.Id == mediaPlanTvId
                              select new TrMediaPlanTVDetail
                              {
                                  Id = tvDetail.Id,
                                  TrMediaPlanId = tvDetail.TrMediaPlanId,
                                  TmProgramCategoryTypeId = tvDetail.TmProgramCategoryTypeId,
                                  TmMediaTypeId = tvDetail.TmMediaTypeId,
                                  VendorId = tvDetail.VendorId,
                                  TmProgramPositionId = tvDetail.TmProgramPositionId,
                                  TmProgramTypeId = tvDetail.TmProgramTypeId,
                                  ProgramName = tvDetail.ProgramName,
                                  DayStart = tvDetail.DayStart,
                                  DayEnd = tvDetail.DayEnd,
                                  TimeStart = tvDetail.TimeStart,
                                  TimeEnd = tvDetail.TimeEnd,
                                  Duration = tvDetail.Duration,
                                  Currency = tvDetail.Currency,
                                  BaseRate30 = tvDetail.BaseRate30,
                                  UsedRate = tvDetail.UsedRate,
                                  Surcharge = tvDetail.Surcharge,
                                  GrossRate = tvDetail.GrossRate,
                                  Disc = tvDetail.Disc,
                                  Bvc = tvDetail.Bvc,
                                  Bonus = tvDetail.Bonus,
                                  EstimateTvr = tvDetail.EstimateTvr,
                                  Tvr = tvDetail.Tvr,
                                  Index = tvDetail.Index,
                                  Cprp = tvDetail.Cprp,
                                  TotalSpot = tvDetail.TotalSpot,
                                  TotalTarps = tvDetail.TotalTarps,
                                  GrossComitedBonus = tvDetail.GrossComitedBonus,
                                  GrossTakenBonus = tvDetail.GrossTakenBonus,
                                  RemaniningBonus = tvDetail.RemaniningBonus,
                                  TotalGrossValue = tvDetail.TotalGrossValue,
                                  TotalNetCost = tvDetail.TotalNetCost,
                                  CreatedBy = tvDetail.CreatedBy,
                                  CreatedOn = tvDetail.CreatedOn,
                                  ModifiedBy = tvDetail.ModifiedBy,
                                  ModifiedOn = tvDetail.ModifiedOn,
                                  ApprovedBy = tvDetail.ApprovedBy,
                                  ApprovedOn = tvDetail.ApprovedOn,
                                  IsActive = tvDetail.IsActive,
                                  IsLocked = tvDetail.IsLocked,
                                  IsDefault = tvDetail.IsDefault,
                                  IsDeleted = tvDetail.IsDeleted,
                                  OwnerId = tvDetail.OwnerId,
                                  DeletedBy = tvDetail.DeletedBy,
                                  DeletedOn = tvDetail.DeletedOn,
                                  BroadcastTypeName = types.ProgramTypeName,
                                  TypeProgramName = category.ProgramCategoryName,
                                  PositionName = position.ProgramPositionName
                              }).ToListAsync();

            return data;
        }

    }
}
