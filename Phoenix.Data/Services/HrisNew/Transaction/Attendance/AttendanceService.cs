using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Um;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface IAttendanceService
    {
        Task<int> CheckIn(Attendance entity);
        Task<int> CheckOut(Attendance entity);
        Task<dynamic> GetList(string dtStart, string dtEnd, int isNotClockOut);
        Task<dynamic> ReportList(string month, string year, string legalEntityId = null);
        Task<Attendance> GetById();
        Task<AttendanceStatus> getStatusAttendanceById();
        Task<Attendance> Get(string Id);
        Task<dynamic> GetReportById(string employeeId, int month, int year);
        Task<Attendance> CheckStatus(string userid = null);
    }

    public class AttendanceService : IAttendanceService
    {
        readonly DataContext context;
        private readonly IManageUserAppService userApp;
        /// <summary>ReportList
        /// And endpoint to manage Attendance
        /// </summary>
        /// <param name="context">Database context</param>
        public AttendanceService(DataContext context, IManageUserAppService userApp)
        {
            this.context = context;
            this.userApp = userApp;
        }

        public async Task<int> CheckIn(Attendance entity)
        {
            return await CheckInOut(entity, true);

        }

        public async Task<int> CheckOut(Attendance entity)
        {
            return await CheckInOut(entity, false);
        }

        private async Task<int> CheckInOut(Attendance entity, bool IsCheckIn)
        {
            MasterWorkingTime wrkTime = new MasterWorkingTime();
            var empinfo = context.EmployeeBasicInfos.AsNoTracking().Where(x => x.IsDeleted != true && x.Id == entity.EmployeeBasicInfoId).FirstOrDefault() ?? new EmployeeBasicInfo();
            if (!string.IsNullOrEmpty(empinfo.MasterWorkingTimeId))
            {
                //throw new Exception("This Employee has no master working time, please choose one master working time first for this employee.");
                wrkTime = await context.MasterWorkingTimes.AsNoTracking().Where(x => x.IsDeleted != true && x.Id == empinfo.MasterWorkingTimeId).FirstOrDefaultAsync();
            }
            else
            {
                wrkTime = await context.MasterWorkingTimes.AsNoTracking().Where(x => x.IsDeleted != true && x.IsDefault == true).FirstOrDefaultAsync();
            }

            //var edit = await context.Attendances.AsNoTracking().Where(x => Convert.ToDateTime(x.DateAttendance).Date == DateTime.Now.Date 
            //&& x.Type == (IsCheckIn ? "ClockIn" : "ClockOut") && x.EmployeeBasicInfoId == entity.EmployeeBasicInfoId).FirstOrDefaultAsync();

            //Convert.ToDateTime(x.DateAttendance).Date == (entity.DateAttendance == null ? DateTime.Now.Date : entity.DateAttendance.Value.Date) &&
            var edit = await context.Attendances.AsNoTracking().Where(x => x.EmployeeBasicInfoId == entity.EmployeeBasicInfoId && x.Id == entity.Id).FirstOrDefaultAsync();
            var datecheckin = DateTime.Now;

            //Untuk Checkin Selalu mengambil jam yg pertama kali click
            //Untuk CheckOut Selalu mengambil jam yg terakhir kali click
            if (IsCheckIn && edit != null)
            {
                entity.DateAttendance = edit.DateAttendance;
                entity.TimeAttendance = edit.TimeAttendance;
            }
            else
            {
                //entity.DateAttendance = datecheckin.Date;
                //entity.TimeAttendance = datecheckin.TimeOfDay;

                if (IsCheckIn)
                {
                    entity.DateAttendance = entity.clockinval.Value.Date;
                    entity.TimeAttendance = entity.clockinval.Value.TimeOfDay;
                    if (entity.TimeAttendance > wrkTime.TimeIn)
                    {
                        entity.Late = entity.TimeAttendance.Value.Subtract(wrkTime.TimeIn.Value);
                    }
                }
                else
                {
                    entity.DateAttendanceOut = entity.clockoutval.Value.Date;
                    entity.TimeAttendanceOut = entity.clockoutval.Value.TimeOfDay;
                    if (entity.DateAttendanceOut.Value.Date > entity.DateAttendance.Value.Date)
                    {
                        TimeSpan totalHour = entity.DateAttendanceOut.Value.Add(entity.TimeAttendanceOut.Value).Subtract(entity.DateAttendance.Value.Add(entity.TimeAttendance.Value));
                        entity.OverTime = Math.Truncate(totalHour.TotalHours).ToString("00") + ":" + totalHour.Minutes.ToString("00");
                    }
                    else
                    {
                        if (entity.TimeAttendanceOut > wrkTime.TimeOut)
                        {
                            //entity.OverTime = entity.TimeAttendanceOut.Value.Subtract(wrkTime.TimeOut.Value);
                            TimeSpan totalHour = entity.TimeAttendanceOut.Value.Subtract(wrkTime.TimeOut.Value);
                            entity.OverTime = Math.Truncate(totalHour.TotalHours).ToString("00") + ":" + totalHour.Minutes.ToString("00");
                        }
                    }
                }
            }

            //        if (IsCheckIn)
            //        {
            //            entity.Type = "ClockIn";

            //            if (entity.TimeAttendance > wrkTime.TimeIn)
            //            {
            //	//entity.Late = DateTime.Parse(((TimeSpan)entity.TimeAttendance).ToString(@"hh\:mm\:ss")).Subtract(DateTime.Parse(((TimeSpan)wrkTime.TimeIn).ToString(@"hh\:mm\:ss")));
            //	entity.Late = entity.TimeAttendance.Value.Subtract(wrkTime.TimeIn.Value);
            //}
            //        }
            //        else
            //        {
            //            entity.Type = "ClockOut";
            //            if (entity.TimeAttendance > wrkTime.TimeOut)
            //            {
            //	//entity.OverTime = DateTime.Parse(((TimeSpan)entity.TimeAttendance).ToString(@"hh\:mm\:ss")).Subtract(DateTime.Parse(((TimeSpan)wrkTime.TimeOut).ToString(@"hh\:mm\:ss")));
            //	entity.OverTime = entity.TimeAttendance.Value.Subtract(wrkTime.TimeOut.Value);
            //}
            //        }

            if (edit != null)
            {
                //entity.Id = edit.Id;
                context.PhoenixEdit(entity);
            }
            else
            {
                entity.Id = Guid.NewGuid().ToString();
                await context.PhoenixAddAsync(entity);
            }

            //if(string.IsNullOrEmpty(empinfo.LocationId))
            //{
            //	if (context.EmployeeLocations.AsNoTracking().Where(x => x.LocationId == MasterLocationDefault.LocationNava).Count() == 0)
            //	{
            //		var employeeLocation = new EmployeeLocation()
            //		{
            //			Id = Guid.NewGuid().ToString(),
            //			EmployeeBasicInfoId = entity.EmployeeBasicInfoId,
            //			LocationId = MasterLocationDefault.LocationNava
            //		};
            //		await context.PhoenixAddAsync(employeeLocation);
            //	}
            //}


            return await context.SaveChangesAsync();
        }

        public async Task<Attendance> Get(string Id) => await context.Attendances.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();

        public async Task<Attendance> GetById()
        {
            //var employeeId = context.GetEmployeeId();
            var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
            RequestorService request = new RequestorService(context);
            var attendance = await GetAttendance(employeeId);
            attendance.requestEmployee = request.GetEmployeeInfo(attendance.requestEmployee);
            return attendance;
        }

        public async Task<AttendanceStatus> getStatusAttendanceById()
        {
            AttendanceStatus resp = new AttendanceStatus();
            var model = await GetById();
            var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
            resp.IsCheckIn = model.TimeAttendance != null;
            resp.IsCheckOut = (!resp.IsCheckIn) ? true : model.TimeAttendanceOut != null;
            resp.CheckInDateTime = model.TimeAttendance;
            resp.CheckOutDateTime = model.TimeAttendanceOut;
            resp.EmployeeBasicId = employeeId;
            resp.CheckInLang = model.Lng;
            resp.CheckInLat = model.Lat;


            return resp;
        }

        public async Task<Attendance> GetAttendance(string employeeId)
        {
            DateTime attdDate = DateTime.Today;
            DateTime? latestAttendanceDate = await context.Attendances.AsNoTracking()
                .Where(x => x.DateAttendance.Value.Month == attdDate.Month && x.DateAttendance.Value.Year == attdDate.Year && x.EmployeeBasicInfoId == employeeId)
                .Select(x => x.DateAttendance).OrderByDescending(y => y).FirstOrDefaultAsync();

            if (latestAttendanceDate != null)
                attdDate = latestAttendanceDate.Value;

            Attendance lastData = await context.Attendances.AsNoTracking().Where(x => x.EmployeeBasicInfoId == employeeId && x.DateAttendance.Value.Date == attdDate.Date).FirstOrDefaultAsync();
            bool isClockInButNotClockOut = false;
            if (lastData != null)
            {
                isClockInButNotClockOut = lastData.DateAttendanceOut == null;
            }

            Attendance attendance = new Attendance();
            if (isClockInButNotClockOut)
            {
                attendance = await
                (
                    from a in context.EmployeeBasicInfos
                        //join c in context.MasterLocations on (a.LocationId ?? MasterLocationDefault.LocationNava) equals c.Id
                    join d in context.Attendances on a.Id equals d.EmployeeBasicInfoId
                    where a.IsDeleted != true && d.Id == lastData.Id
                    select new Attendance
                    {
                        Id = d.Id,
                        EmployeeBasicInfoId = a.Id,
                        //Lat = c.MapLat,
                        //Lng = c.MapLong,
                        //LocationName = c.LocationName,
                        Lat = d.Lat,
                        Lng = d.Lng,
                        LocationName = d.LocationName,
                        //Lat = "-6.2196191",
                        //Lng = "106.8313018",
                        //LocationName = "Head Office",
                        DateAttendance = d.DateAttendance,
                        TimeAttendance = d.TimeAttendance,
                        Type = d.Type,
                        CreatedOn = d.CreatedOn,
                        ModifiedBy = d.ModifiedBy,
                        ModifiedOn = d.ModifiedOn,
                        ApprovedBy = d.ApprovedBy,
                        ApprovedOn = d.ApprovedOn,
                        IsActive = d.IsActive,
                        IsLocked = d.IsLocked,
                        IsDefault = d.IsDefault,
                        IsDeleted = d.IsDeleted,
                        OwnerId = d.OwnerId,
                        DeletedBy = d.DeletedBy,
                        DeletedOn = d.DeletedOn,
                        DateAttendanceOut = null,
                        TimeAttendanceOut = null,
                        requestEmployee = new InfoEmployee
                        {
                            EmployeeId = a.Id,
                            EmployeeName = a.NameEmployee,
                            JobTitle = a.BusinessUnitJobLevelId == null ? "" :
                                                          (from r in context.BusinessUnitJobLevels
                                                           join s in context.JobTitles on r.JobTitleId equals s.Id
                                                           where r.Id == a.BusinessUnitJobLevelId
                                                           select s.TitleName).FirstOrDefault(),
                            Grade = a.BusinessUnitJobLevelId == null ? "" :
                                                          (from r in context.BusinessUnitJobLevels
                                                           join s in context.JobGrades on r.JobGradeId equals s.Id
                                                           where r.Id == a.BusinessUnitJobLevelId
                                                           select s.GradeName).FirstOrDefault()
                        }
                    }).FirstOrDefaultAsync();
            }
            else
            {
                //var attdOut = await context.Attendances.Where(z => z.IsDeleted != true && z.Type == "ClockOut" && z.EmployeeBasicInfoId == employeeId 
                //		&& z.DateAttendance.Value.Date == DateTime.Now.Date).Select(x => x.TimeAttendance).FirstOrDefaultAsync();

                attendance = await (
                    from a in context.EmployeeBasicInfos
                        //join c in context.MasterLocations on (a.LocationId ?? MasterLocationDefault.LocationNava) equals c.Id
                    join d in context.Attendances on a.Id equals d.EmployeeBasicInfoId
                    where a.IsDeleted != true && a.Id == employeeId && d.DateAttendance.Value.Date == DateTime.Now.Date
                    select new Attendance
                    {
                        Id = d.Id,
                        EmployeeBasicInfoId = a.Id,
                        //Lat = c.MapLat,
                        //Lng = c.MapLong,
                        //LocationName = c.LocationName,
                        Lat = "-6.2196191",
                        Lng = "106.8313018",
                        LocationName = "Head Office",
                        DateAttendance = d.DateAttendance,
                        TimeAttendance = d.TimeAttendance,
                        Type = d.Type,
                        CreatedOn = d.CreatedOn,
                        ModifiedBy = d.ModifiedBy,
                        ModifiedOn = d.ModifiedOn,
                        ApprovedBy = d.ApprovedBy,
                        ApprovedOn = d.ApprovedOn,
                        IsActive = d.IsActive,
                        IsLocked = d.IsLocked,
                        IsDefault = d.IsDefault,
                        IsDeleted = d.IsDeleted,
                        OwnerId = d.OwnerId,
                        DeletedBy = d.DeletedBy,
                        DeletedOn = d.DeletedOn,
                        DateAttendanceOut = d.DateAttendanceOut,
                        TimeAttendanceOut = d.TimeAttendanceOut,
                        requestEmployee = new InfoEmployee
                        {
                            EmployeeId = a.Id,
                            EmployeeName = a.NameEmployee,
                            JobTitle = a.BusinessUnitJobLevelId == null ? "" :
                                                          (from r in context.BusinessUnitJobLevels
                                                           join s in context.JobTitles on r.JobTitleId equals s.Id
                                                           where r.Id == a.BusinessUnitJobLevelId
                                                           select s.TitleName).FirstOrDefault(),
                            Grade = a.BusinessUnitJobLevelId == null ? "" :
                                                          (from r in context.BusinessUnitJobLevels
                                                           join s in context.JobGrades on r.JobGradeId equals s.Id
                                                           where r.Id == a.BusinessUnitJobLevelId
                                                           select s.GradeName).FirstOrDefault()
                        }
                    }).FirstOrDefaultAsync();
            }


            if (attendance == null)
            {
                attendance = await (from a in context.EmployeeBasicInfos
                                        //join c in context.MasterLocations on (a.LocationId ?? MasterLocationDefault.LocationNava) equals c.Id
                                    where a.IsDeleted != true && a.Id == employeeId //&& c.Id == MasterLocationDefault.LocationNava
                                    select new Attendance
                                    {
                                        EmployeeBasicInfoId = a.Id,
                                        //Lat = c.MapLat,
                                        //Lng = c.MapLong,
                                        //LocationName = c.LocationName,
                                        Lat = "-6.2196191",
                                        Lng = "106.8313018",
                                        LocationName = "Head Office",
                                        requestEmployee = new InfoEmployee
                                        {
                                            EmployeeId = a.Id,
                                            EmployeeName = a.NameEmployee,
                                            JobTitle = a.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobTitles on r.JobTitleId equals s.Id
                                                                           where r.Id == a.BusinessUnitJobLevelId
                                                                           select s.TitleName).FirstOrDefault(),
                                            Grade = a.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                           where r.Id == a.BusinessUnitJobLevelId
                                                                           select s.GradeName).FirstOrDefault()
                                        }
                                    }).FirstOrDefaultAsync();
            }
            return attendance;
        }

        public async Task<dynamic> GetList(string dtStart, string dtEnd, int isNotClockOut)
        {
            DateTime dtMonth = DateTime.ParseExact(dtStart, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
            DateTime dtYear = DateTime.ParseExact(dtEnd, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);

            //var employeeId = context.GetEmployeeId();
            var workingHour = await context.MasterWorkingTimes.AsNoTracking().Where(x => x.IsDefault == true).FirstOrDefaultAsync();
            var group = await userApp.GetByUserLogin();
            var HCAdmin = group.Where(x => x.GroupId == GroupAcess.HCAdmin || x.GroupId == GroupAcess.SuperAdmin).Count() > 0;

            List<AttendanceReport> data = new List<AttendanceReport>();

            if (!HCAdmin)
            {
                var subordinateforthisemployee = await GetSubordinate();

                data = await (
                from a in context.Attendances
                join b in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals b.Id
                join c in context.MasterWorkingTimes on (b.MasterWorkingTimeId ?? workingHour.Id) equals c.Id
                join d in subordinateforthisemployee on b.BusinessUnitJobLevelId equals d
                where a.IsDeleted != true && (a.DateAttendance.Value.Date >= dtMonth.Date && a.DateAttendance.Value.Date <= dtYear.Date)
                    && (isNotClockOut == 1 ? a.DateAttendanceOut == null : a.DateAttendanceOut == a.DateAttendanceOut)
                select new AttendanceReport
                {
                    EmployeeName = b.NameEmployee,
                    Id = Guid.NewGuid().ToString(),
                    DayIn = Convert.ToDateTime(a.DateAttendance).ToString("ddd"),
                    DateIn = a.DateAttendance,
                    DayOut = (a.TimeAttendanceOut == null) ? "" : Convert.ToDateTime(a.DateAttendanceOut).ToString("ddd"),
                    DateOut = a.DateAttendanceOut,
                    WorkingHour = (a.TimeAttendance == null ? "" : a.TimeAttendance.Value.ToString(@"hh\:mm")) + " - " + (a.TimeAttendanceOut == null ? "" : a.TimeAttendanceOut.Value.ToString(@"hh\:mm")) + ((a.DateAttendanceOut.Value > a.DateAttendance.Value) ? "(+1 Day)" : ""),
                    EarlyIn = (a.TimeAttendance == null) ? "" : ((c.TimeIn > a.TimeAttendance) ? c.TimeIn.Value.Subtract(a.TimeAttendance.Value).ToString(@"hh\:mm") : ""),
                    EarlyOut = (a.TimeAttendanceOut == null) ? "" : ((c.TimeOut > a.TimeAttendanceOut) ? c.TimeOut.Value.Subtract(a.TimeAttendanceOut.Value).ToString(@"hh\:mm") : ""),
                    LateIn = (a.TimeAttendance > c.TimeIn) ? a.TimeAttendance.Value.Subtract(c.TimeIn.Value).ToString(@"hh\:mm") : "",
                    LateOut = (a.TimeAttendanceOut == null) ? "" : (a.TimeAttendanceOut > c.TimeOut) ? a.TimeAttendanceOut.Value.Subtract(c.TimeOut.Value).ToString(@"hh\:mm") : "",
                    //TotalHour = (a.TimeAttendanceOut == null) ? "" : a.TimeAttendanceOut.Value.Subtract(a.TimeAttendance.Value).ToString(@"hh\:mm")
                    TotalHour = (a.OverTime == null) ? "" : a.OverTime
                }).OrderByDescending(x => x.DateIn).AsNoTracking().ToListAsync();
            }
            else
            {
                data = await (
                from a in context.Attendances
                join b in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals b.Id
                join c in context.MasterWorkingTimes on (b.MasterWorkingTimeId ?? workingHour.Id) equals c.Id
                where a.IsDeleted != true && (a.DateAttendance.Value.Date >= dtMonth.Date && a.DateAttendance.Value.Date <= dtYear.Date)
                    && (isNotClockOut == 1 ? a.DateAttendanceOut == null : a.DateAttendanceOut == a.DateAttendanceOut)
                select new AttendanceReport
                {
                    EmployeeName = b.NameEmployee,
                    Id = Guid.NewGuid().ToString(),
                    DayIn = Convert.ToDateTime(a.DateAttendance).ToString("ddd"),
                    DateIn = a.DateAttendance,
                    DayOut = (a.TimeAttendanceOut == null) ? "" : Convert.ToDateTime(a.DateAttendanceOut).ToString("ddd"),
                    DateOut = a.DateAttendanceOut,
                    WorkingHour = (a.TimeAttendance == null ? "" : a.TimeAttendance.Value.ToString(@"hh\:mm")) + " - " + (a.TimeAttendanceOut == null ? "" : a.TimeAttendanceOut.Value.ToString(@"hh\:mm")) + ((a.DateAttendanceOut.Value > a.DateAttendance.Value) ? "(+1 Day)" : ""),
                    EarlyIn = (a.TimeAttendance == null) ? "" : ((c.TimeIn > a.TimeAttendance) ? c.TimeIn.Value.Subtract(a.TimeAttendance.Value).ToString(@"hh\:mm") : ""),
                    EarlyOut = (a.TimeAttendanceOut == null) ? "" : ((c.TimeOut > a.TimeAttendanceOut) ? c.TimeOut.Value.Subtract(a.TimeAttendanceOut.Value).ToString(@"hh\:mm") : ""),
                    LateIn = (a.TimeAttendance > c.TimeIn) ? a.TimeAttendance.Value.Subtract(c.TimeIn.Value).ToString(@"hh\:mm") : "",
                    LateOut = (a.TimeAttendanceOut == null) ? "" : (a.TimeAttendanceOut > c.TimeOut) ? a.TimeAttendanceOut.Value.Subtract(c.TimeOut.Value).ToString(@"hh\:mm") : "",
                    //TotalHour = (a.TimeAttendanceOut == null) ? "" : a.TimeAttendanceOut.Value.Subtract(a.TimeAttendance.Value).ToString(@"hh\:mm")
                    TotalHour = (a.OverTime == null) ? "" : a.OverTime
                }).OrderByDescending(x => x.DateIn).AsNoTracking().ToListAsync();
            }

            return data;

            #region commented
            //return (from a in clockIn
            //        join b in clockOut on new { a.EmployeeBasicInfoId, a.DateAttendance } equals new { b.EmployeeBasicInfoId, b.DateAttendance } into g
            //        from c in g.DefaultIfEmpty()
            //        select new
            //        {
            //            a.EmployeeBasicInfoId,
            //            ClockIn = Convert.ToDateTime(a.DateAttendance).Add((TimeSpan)a.TimeAttendance),
            //            LocationIn = a.LocationName,
            //            Clockout = (c == null) ? (DateTime?)null : Convert.ToDateTime(c.DateAttendance).Add((TimeSpan)c.TimeAttendance),
            //            LocationOut = (c == null) ? "" : c.LocationName,
            //            a.Late,
            //            OverTime = (c == null) ? (TimeSpan?)null : c.OverTime
            //        }).ToList();
            #endregion commented			
        }

        public async Task<dynamic> ReportList(string month, string year, string legalEntityId = null)
        {
            DateTime dtMonth = DateTime.ParseExact(month, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
            DateTime dtYear = DateTime.ParseExact(year, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);

            var mt = dtMonth.Month;
            var yr = dtMonth.Year;

            var group = await userApp.GetByUserLogin();
            var HCAdmin = group.Where(x => x.GroupId == GroupAcess.HCAdmin || x.GroupId == GroupAcess.SuperAdmin).Count() > 0;

            //var groupAttandance = await (from q in context.Attendances.AsNoTracking()
            //							 join r in context.EmployeeBasicInfos.AsNoTracking() on q.EmployeeBasicInfoId equals r.Id
            //							 where q.IsDeleted != true && q.DateAttendance.Value.Month == dtMonth.Month && q.DateAttendance.Value.Year == dtYear.Year && r.    )

            var groupAttandance = await context.Attendances.AsNoTracking().Where(x => x.IsDeleted != true && x.DateAttendance.Value.Month == dtMonth.Month && x.DateAttendance.Value.Year == dtYear.Year)
                                    .GroupBy(x => new { x.EmployeeBasicInfoId, x.DateAttendance })
                                    .Select(x => new { x.Key.EmployeeBasicInfoId, x.Key.DateAttendance }).ToListAsync();

            if (!string.IsNullOrEmpty(legalEntityId))
            {
                groupAttandance = (from q in groupAttandance
                                   join r in context.EmployeeBasicInfos.AsNoTracking() on q.EmployeeBasicInfoId equals r.Id
                                   where r.LegalEntityId == legalEntityId
                                   select new
                                   {
                                       q.EmployeeBasicInfoId,
                                       q.DateAttendance
                                   }).ToList();
            }

            var groupAttandancePerMonth = groupAttandance.GroupBy(x => new
            {
                x.EmployeeBasicInfoId,
                Convert.ToDateTime(x.DateAttendance).Month,
                Convert.ToDateTime(x.DateAttendance).Year,
                MonthName = Convert.ToDateTime(x.DateAttendance).ToString("MMMM")
            }).Select(x => new
            {
                x.Key.EmployeeBasicInfoId,
                x.Key.Month,
                x.Key.Year,
                x.Key.MonthName
            }).ToList();

            if (!HCAdmin)
            {
                var subordinateforthisemployee = await GetSubordinate();

                var attandance = await (from a in context.EmployeeBasicInfos
                                        join b in groupAttandancePerMonth on a.Id equals b.EmployeeBasicInfoId
                                        join c in subordinateforthisemployee on a.BusinessUnitJobLevelId equals c
                                        where a.IsDeleted != true
                                        select new Attendance
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            EmployeeBasicInfoId = b.EmployeeBasicInfoId,
                                            TotalRecord = groupAttandance.Where(x => x.EmployeeBasicInfoId == b.EmployeeBasicInfoId &&
                                                                                     Convert.ToDateTime(x.DateAttendance).Month == b.Month &&
                                                                                     Convert.ToDateTime(x.DateAttendance).Year == b.Year).Count(),
                                            MonthName = b.MonthName,
                                            Month = b.Month,
                                            Year = b.Year,
                                            requestEmployee = new InfoEmployee
                                            {
                                                EmployeeId = a.Id,
                                                EmployeeName = a.NameEmployee,
                                                JobTitle = a.BusinessUnitJobLevelId == null ? "" :
                                                                                (from r in context.BusinessUnitJobLevels
                                                                                 join s in context.JobTitles on r.JobTitleId equals s.Id
                                                                                 where r.Id == a.BusinessUnitJobLevelId
                                                                                 select s.TitleName).FirstOrDefault(),
                                                Grade = a.BusinessUnitJobLevelId == null ? "" :
                                                                                (from r in context.BusinessUnitJobLevels
                                                                                 join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                                 where r.Id == a.BusinessUnitJobLevelId
                                                                                 select s.GradeName).FirstOrDefault(),
                                                LegalEntityId = a.LegalEntityId,
                                                LegalEntityName = (from r in context.LegalEntity
                                                                   where r.Id == a.LegalEntityId
                                                                   select r.LegalEntityName).FirstOrDefault()
                                            }
                                        }).ToListAsync();

                foreach (var item in attandance)
                {
                    var clockIn = await ListCheckIn(item.EmployeeBasicInfoId, mt, yr);
                    var clockOut = await ListCheckOut(item.EmployeeBasicInfoId, mt, yr);
                    var at = (from b in clockIn
                              join a in clockOut on new { b.EmployeeBasicInfoId, b.DateAttendance } equals new { a.EmployeeBasicInfoId, a.DateAttendance } into g
                              from c in g.DefaultIfEmpty()
                              select (c == null) ? "" : DateTime.Parse(((TimeSpan)c.TimeAttendanceOut).ToString(@"hh\:mm\:ss")).Subtract(DateTime.Parse(((TimeSpan)b.TimeAttendance).ToString(@"hh\:mm\:ss"))).ToString(@"hh\:mm")
                                      ).ToList();
                    TimeSpan t1 = TimeSpan.Parse("00:00:00");
                    for (int i = 0; i < at.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(at[i].ToString()))
                        {
                            TimeSpan t2 = TimeSpan.Parse(at[i].ToString());
                            t1 = t1.Add(t2);
                        }
                    }
                    item.TotalHour = t1.ToString(@"hh\:mm");
                }
                return attandance;
            }
            else
            {
                var attandance = await (from a in context.EmployeeBasicInfos
                                        join b in groupAttandancePerMonth on a.Id equals b.EmployeeBasicInfoId
                                        where a.IsDeleted != true
                                        select new Attendance
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            EmployeeBasicInfoId = b.EmployeeBasicInfoId,
                                            TotalRecord = groupAttandance.Where(x => x.EmployeeBasicInfoId == b.EmployeeBasicInfoId &&
                                                                                     Convert.ToDateTime(x.DateAttendance).Month == b.Month &&
                                                                                     Convert.ToDateTime(x.DateAttendance).Year == b.Year).Count(),

                                            MonthName = b.MonthName,
                                            Month = b.Month,
                                            Year = b.Year,
                                            requestEmployee = new InfoEmployee
                                            {
                                                EmployeeId = a.Id,
                                                EmployeeName = a.NameEmployee,
                                                JobTitle = a.BusinessUnitJobLevelId == null ? "" :
                                                                                (from r in context.BusinessUnitJobLevels
                                                                                 join s in context.JobTitles on r.JobTitleId equals s.Id
                                                                                 where r.Id == a.BusinessUnitJobLevelId
                                                                                 select s.TitleName).FirstOrDefault(),
                                                Grade = a.BusinessUnitJobLevelId == null ? "" :
                                                                                (from r in context.BusinessUnitJobLevels
                                                                                 join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                                 where r.Id == a.BusinessUnitJobLevelId
                                                                                 select s.GradeName).FirstOrDefault(),
                                                LegalEntityId = a.LegalEntityId,
                                                LegalEntityName = (from r in context.LegalEntity
                                                                   where r.Id == a.LegalEntityId
                                                                   select r.LegalEntityName).FirstOrDefault()
                                            }
                                        }).ToListAsync();

                foreach (var item in attandance)
                {
                    var clockIn = await ListCheckIn(item.EmployeeBasicInfoId, mt, yr);
                    var clockOut = await ListCheckOut(item.EmployeeBasicInfoId, mt, yr);
                    var at = (from b in clockIn
                              join a in clockOut on new { b.EmployeeBasicInfoId, b.DateAttendance } equals new { a.EmployeeBasicInfoId, a.DateAttendance } into g
                              from c in g.DefaultIfEmpty()
                              select (c == null) ? "" : DateTime.Parse(((TimeSpan)c.TimeAttendanceOut).ToString(@"hh\:mm\:ss")).Subtract(DateTime.Parse(((TimeSpan)b.TimeAttendance).ToString(@"hh\:mm\:ss"))).ToString(@"hh\:mm")
                                      ).ToList();
                    TimeSpan t1 = TimeSpan.Parse("00:00:00");
                    for (int i = 0; i < at.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(at[i].ToString()))
                        {
                            TimeSpan t2 = TimeSpan.Parse(at[i].ToString());
                            t1 = t1.Add(t2);
                        }

                    }
                    item.TotalHour = t1.ToString(@"hh\:mm");

                }

                return attandance;
            }
        }

        public async Task<dynamic> GetReportById(string employeeId, int month, int year)
        {
            var workingHour = await context.MasterWorkingTimes.AsNoTracking().Where(x => x.IsDefault == true).FirstOrDefaultAsync();
            var clockIn = await ListCheckIn(employeeId, month, year);
            var clockOut = await ListCheckOut(employeeId, month, year);

            return (from b in clockIn
                    join a in clockOut on new { b.EmployeeBasicInfoId, b.DateAttendance } equals new { a.EmployeeBasicInfoId, a.DateAttendance } into g
                    from c in g.DefaultIfEmpty()
                    select new
                    {
                        Id = Guid.NewGuid().ToString(),
                        DayIn = Convert.ToDateTime(b.DateAttendance).ToString("ddd"),
                        DateIn = b.DateAttendance,
                        WorkingHour = (c == null) ? ((TimeSpan)b.TimeAttendance).ToString(@"hh\:mm") + " - " : ((TimeSpan)b.TimeAttendance).ToString(@"hh\:mm\:ss") + " - " + ((TimeSpan)c.TimeAttendanceOut).ToString(@"hh\:mm\:ss"),
                        EarlyIn = (workingHour.TimeIn > b.TimeAttendance) ? DateTime.Parse(((TimeSpan)workingHour.TimeIn).ToString(@"hh\:mm\:ss")).Subtract(DateTime.Parse(((TimeSpan)b.TimeAttendance).ToString(@"hh\:mm\:ss"))).ToString(@"hh\:mm\:ss") : "",
                        EarlyOut = (c == null) ? "" : (workingHour.TimeOut > c.TimeAttendanceOut) ? DateTime.Parse(((TimeSpan)workingHour.TimeOut).ToString(@"hh\:mm\:ss")).Subtract(DateTime.Parse(((TimeSpan)c.TimeAttendanceOut).ToString(@"hh\:mm\:ss"))).ToString(@"hh\:mm\:ss") : "",
                        LateIn = (b.TimeAttendance > workingHour.TimeIn) ? DateTime.Parse(((TimeSpan)b.TimeAttendance).ToString(@"hh\:mm\:ss")).Subtract(DateTime.Parse(((TimeSpan)workingHour.TimeIn).ToString(@"hh\:mm\:ss"))).ToString(@"hh\:mm\:ss") : "",
                        LateOut = (c == null) ? "" : (c.TimeAttendanceOut > workingHour.TimeOut) ? DateTime.Parse(((TimeSpan)c.TimeAttendanceOut).ToString(@"hh\:mm\:ss")).Subtract(DateTime.Parse(((TimeSpan)workingHour.TimeOut).ToString(@"hh\:mm\:ss"))).ToString(@"hh\:mm\:ss") : "",
                        TotalHour = (c == null) ? "" : DateTime.Parse(((TimeSpan)c.TimeAttendanceOut).ToString(@"hh\:mm\:ss")).Subtract(DateTime.Parse(((TimeSpan)b.TimeAttendance).ToString(@"hh\:mm\:ss"))).ToString(@"hh\:mm\:ss")
                    }).ToList();
        }

        public async Task<Attendance> CheckStatus(string userid = null)
        {
            Attendance result = new Attendance();
            var employeeId = Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
            //var employeeId = context.GetEmployeeId();
            if (!string.IsNullOrEmpty(userid))
            {
                employeeId = userid;
            }
            string statusAttendance = string.Empty;
            DateTime attdDate = DateTime.Today;
            DateTime? latestAttendanceDate = await context.Attendances.AsNoTracking()
                .Where(x => x.DateAttendance.Value.Month == attdDate.Month && x.DateAttendance.Value.Year == attdDate.Year && x.EmployeeBasicInfoId == employeeId)
                .Select(x => x.DateAttendance).OrderByDescending(y => y).FirstOrDefaultAsync();

            if (latestAttendanceDate != null)
                attdDate = latestAttendanceDate.Value;

            Attendance lastData = await context.Attendances.AsNoTracking().Where(x => x.EmployeeBasicInfoId == employeeId && x.DateAttendance.Value.Date == attdDate.Date).FirstOrDefaultAsync();
            bool isClockInButNotClockOut = false;
            if (lastData != null)
            {
                isClockInButNotClockOut = lastData.DateAttendanceOut == null;
                result = new Attendance()
                {
                    EmployeeBasicInfoId = lastData.EmployeeBasicInfoId,
                    DateAttendance = lastData.DateAttendance,
                    TimeAttendance = lastData.TimeAttendance
                };

                if (isClockInButNotClockOut)
                {
                    result.Type = "ClockIn";
                }
                else
                {
                    result.Type = "ClockOut";
                    result.DateAttendanceOut = lastData.DateAttendanceOut;
                    result.TimeAttendanceOut = lastData.TimeAttendanceOut;
                }
            }



            //var data = await context.Attendances.Where(x => x.IsDeleted != true && x.EmployeeBasicInfoId == employeeId && x.DateAttendance.Value.Date == DateTime.Today.Date).ToListAsync();
            //if (data != null)
            //{
            //	if (data.Count > 0)
            //	{
            //		var clockIn = data.Where(x => x.Type == "ClockIn").FirstOrDefault();
            //		var clockOut = data.Where(x => x.Type == "ClockOut").FirstOrDefault();
            //		if (clockOut != null)
            //		{
            //			result = new Attendance()
            //			{
            //				EmployeeBasicInfoId = clockOut.EmployeeBasicInfoId,
            //				DateAttendance = clockOut.DateAttendance,
            //				TimeAttendance = clockOut.TimeAttendance,
            //				Type = clockOut.Type
            //			};
            //		}
            //		else
            //		{
            //			result = new Attendance()
            //			{
            //				EmployeeBasicInfoId = clockIn.EmployeeBasicInfoId,
            //				DateAttendance = clockIn.DateAttendance,
            //				TimeAttendance = clockIn.TimeAttendance,
            //				Type = clockIn.Type
            //			};
            //		}
            //	}
            //}

            if (result.DateAttendanceOut != null)
            {
                if (result.DateAttendance.Value < DateTime.Today && result.DateAttendanceOut.Value == DateTime.Today)
                {
                    result = new Attendance()
                    {
                        EmployeeBasicInfoId = employeeId,
                        Type = null
                    };
                }
            }

            return result;
        }

        private async Task<List<Attendance>> ListCheckIn(string employeeId, int month, int year)
        {
            var data = await context.Attendances.Where(x => x.IsDeleted != true && x.EmployeeBasicInfoId == employeeId
            //&& x.Type == "ClockIn" 
            && x.DateAttendance.Value.Month == month && x.DateAttendance.Value.Year == year)
                .GroupBy(x => new { x.EmployeeBasicInfoId, x.DateAttendance })
                .Select(x => new { x.Key.EmployeeBasicInfoId, x.Key.DateAttendance, TimeAttendance = x.Min(y => y.TimeAttendance) }).ToListAsync();

            var result = (from a in context.Attendances
                          join b in data on new { a.EmployeeBasicInfoId, a.DateAttendance, a.TimeAttendance } equals new { b.EmployeeBasicInfoId, b.DateAttendance, b.TimeAttendance }
                          select a).ToList();
            return result;
        }

        private async Task<List<Attendance>> ListCheckOut(string employeeId, int month, int year)
        {
            var data = await context.Attendances.Where(x => !(x.IsDeleted ?? false) && x.EmployeeBasicInfoId == employeeId
            //&& x.Type == "ClockOut"
            && x.DateAttendanceOut.Value.Month == month && x.DateAttendanceOut.Value.Year == year)
                .GroupBy(x => new { x.EmployeeBasicInfoId, x.DateAttendanceOut })
                .Select(x => new { x.Key.EmployeeBasicInfoId, x.Key.DateAttendanceOut, TimeAttendanceOut = x.Max(y => y.TimeAttendanceOut) }).ToListAsync();

            var result = (from a in context.Attendances
                          join b in data on new { a.EmployeeBasicInfoId, a.DateAttendanceOut, a.TimeAttendanceOut } equals new { b.EmployeeBasicInfoId, b.DateAttendanceOut, b.TimeAttendanceOut }
                          select a).ToList();
            return result;
        }

        private async Task<List<string>> GetSubordinate()
        {
            List<string> listContainer = new List<string>();

            //var employeeId = context.GetEmployeeId();
            var employeeId = Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
            var employee = await context.EmployeeBasicInfos.AsNoTracking().Where(x => x.Id == employeeId).FirstOrDefaultAsync();
            if (employee != null)
            {
                await GetSubordinateByParent(employee.BusinessUnitJobLevelId, listContainer);
                listContainer.Add(employee.BusinessUnitJobLevelId);
            }

            if (listContainer.Count > 0)
                return listContainer;
            else
                return new List<string>();
        }

        private async Task GetSubordinateByParent(string parentId, List<string> listToAdd)
        {
            var subordinates = await context.BusinessUnitJobLevels.AsNoTracking().Where(x => x.ParentId == parentId).ToListAsync();
            foreach (var item in subordinates)
            {
                listToAdd.Add(item.Id);
                await GetSubordinateByParent(item.Id, listToAdd);
            }
        }
    }
}