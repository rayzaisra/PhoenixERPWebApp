using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Phoenix.Data;
using Phoenix.Data.Attributes;
using Phoenix.Data.Services.Um;
using Phoenix.Shared.Core.Areas.ProjectManagement.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Helpers;
using Phoenix.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Services
{


    public partial interface ITimesheetUserServices : IBaseService<TimesheetUserDto, TimesheetUser, string>
    {
        GeneralResponseList<TimesheetUserDto> ListAll(TimesheetUserDto data);
        GeneralResponse<TimesheetUserDtoNew> CreateTimesheetUser(TimesheetUserDtoNew data);
        GeneralResponse<TimeSheetCreateDto> CreateTimeSheet(TimeSheetCreateDto data);
        GeneralResponse<TimeSheetRow> CreateTableSheet(List<TimeSheetCreateDto> data);
        GeneralResponseList<TimeSheetCreateDto> ListAllTimeSheet(string id);
        Task<DataSet> ListTimeSheetUser(string id, string userid = null);
        Task<GeneralResponse<TimeSheetDto>> UpdateTimeSheet(TimeSheetDto data);
        Task<BaseGeneralResponse> SubmitTimeSheet(SaveTimeSheetDto dto);
        Task<BaseGeneralResponse> Approval(SaveTimeSheetDto dto);
        Task<BaseGeneralResponse> ActivatedTimeSheet(SaveTimeSheetDto dto);
        GeneralResponseList<JobResultDto> ListAllJob(JobResultDto data);
        Task<GeneralResponse<string>> SaveTimeSheet(SaveTimeSheetDto dto);
        Task<DataSet> GetTimeSheetDataById(string id);
        Task<GeneralResponse<string>> DeleteNonJob(TimeSheetDto dto);
        Task<GeneralResponse<string>> CheckIsUserLocked(string userid);
        Task<DataTable> GetApprovalList();
    }

    //public enum DayOfWeek
    //{
    //    Sunday = 0,
    //    Monday = 1,
    //    Tuesday = 2,
    //    Wednesday = 3,
    //    Thursday = 4,
    //    Friday = 5,
    //    Saturday = 6,
    //}

    public partial class TimesheetUserServices : BaseService<TimesheetUserDto, TimesheetUser, string>, ITimesheetUserServices
    {
        IEFRepository<TimesheetNonJob, string> _repononjob;
        IEFRepository<TimesheetUserData, string> _repouserdata;
        readonly DataContext context;
        readonly IManageMenuService menuService;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly StatusLogService log;
        public TimesheetUserServices(IEFRepository<TimesheetUser, string> repo,
                                     IEFRepository<TimesheetNonJob, string> repononjob,
                                     IEFRepository<TimesheetUserData, string> repouserdata,
                                     IManageMenuService menuService,
                                     GlobalFunctionApproval globalFcApproval,
                                     StatusLogService log,
            DataContext context) : base(repo)
        {
            _repo = repo;
            _repononjob = repononjob;
            _repouserdata = repouserdata;
            this.context = context;
            this.menuService = menuService;
            this.globalFcApproval = globalFcApproval;
            this.log = log;

        }


        public GeneralResponseList<TimeSheetCreateDto> ListAllTimeSheet(string id)
        {

            GeneralResponseList<TimeSheetCreateDto> resp = new GeneralResponseList<TimeSheetCreateDto>();
            try
            {
                DateTime dt = Convert.ToDateTime(id);


                //DayOfWeek CurrentDay = DateTime.Now.DayOfWeek;
                //int daysTillCurenntDay = CurrentDay - DayOfWeek.Monday;
                //int daysLastWeek = CurrentDay - DayOfWeek.Sunday;
                //DayOfWeek CurrentDay = DateTime.Now.DayOfWeek;
                //DayOfWeek CurrentDay = new DayOfWeek();
                DayOfWeek CurrentDay = dt.DayOfWeek;
                //if ( dt != null  )
                //{
                //     CurrentDay = dt.DayOfWeek;
                //}
                //else
                //{
                //     CurrentDay = DateTime.Now.DayOfWeek;
                //}


                int daysTillCurenntDay = CurrentDay - DayOfWeek.Monday;
                int daysLastWeek = CurrentDay - DayOfWeek.Sunday;
                //DateTime currentWeekStartDate = DateTime.Now.AddDays(-daysTillCurenntDay);
                DateTime currentWeekStartDate = dt.AddDays(-daysTillCurenntDay);
                DateTime currentWeekEndDate = currentWeekStartDate.AddDays(6);

                IQueryable<TimeSheetCreateDto> que = from tb1 in _repo.GetContext().Set<TimesheetUser>().Where(u => u.user_id == Glosing.Instance.Username).Where(t => dt >= t.start_date_weekly && dt <= t.end_date_weekly)
                                                     join tb2 in _repo.GetContext().Set<JobDetail>() on tb1.job_id equals tb2.Id
                                                     select new TimeSheetCreateDto()
                                                     {
                                                         id = tb1.Id,
                                                         mon = currentWeekEndDate,
                                                         job_id = tb1.job_id,
                                                         job_name = tb2.job_name,
                                                         remark = tb1.remark,
                                                         time = _repo.GetContext().Set<TimesheetUserData>()
                                                       .Where(f => f.timesheet_userid == tb1.Id && f.timesheet_date >= currentWeekStartDate && f.timesheet_date <= currentWeekEndDate).Select(i => new TimeSheetUserDataDto()
                                                       {
                                                           id = i.Id,
                                                           timesheet_date = i.timesheet_date,
                                                           //timesheet_date = currentWeekEndDate,
                                                           timesheet_hour = i.timesheet_hour,
                                                           remarks = i.remarks
                                                       }).ToList()
                                                     };
                resp.Rows = que.ToList();
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public async Task<DataSet> GetTimeSheetDataById(string id)
        {
            SqlParameter[] parameter = {
            new SqlParameter("@id",id)
            };
            DataSet result = await context.ExecuteStoredProcedureDataSet("dbo.uspGetTimesheetDataId", parameter);
            return result;
        }

        public async Task<DataSet> ListTimeSheetUser(string id, string userid = null)
        {
            var employeeID = Glosing.Instance.Username;

            SqlParameter[] parameter = {
            new SqlParameter("@Date",id),
            new SqlParameter("@UserId", employeeID)
            };
            DataSet result = await context.ExecuteStoredProcedureDataSet("dbo.uspGetTimeSheetUser", parameter);
            return result;
        }

        public async Task<GeneralResponse<string>> CheckIsUserLocked(string userid)
        {
            GeneralResponse<string> resp = new GeneralResponse<string>();

            SqlParameter[] parameter = {
            new SqlParameter("@UserId", userid)
            };
            DataTable result = await context.ExecuteStoredProcedure("dbo.uspCheckIsUserLocked", parameter);
            var hasil = result.Rows[0]["result"].ToString();
           
            resp.Code = "200";
            resp.Message = "Data has been saved successfully";
            resp.Success = true;
            resp.Data = hasil;
            
            return resp;
        }

        public async Task<GeneralResponse<string>> SaveTimeSheet(SaveTimeSheetDto dto)
        {
            var userid = Glosing.Instance.Username;

            GeneralResponse<string> resp = new GeneralResponse<string>();
            SqlParameter[] parameter = {
            new SqlParameter("@Date",dto.ChoosenDate),
            new SqlParameter("@BusinessUnitId",dto.BusinessUnitId),
            new SqlParameter("@UserId", userid)
            };
            DataTable result = await context.ExecuteStoredProcedure("dbo.uspSaveTimeSheetUser", parameter);
            var id = result.Rows[0]["id"].ToString();
            resp.Code = "200";
            resp.Message = "Data has been saved successfully";
            resp.Success = true;
            resp.Data = id;

            return resp;
        }

        public async Task<BaseGeneralResponse> ActivatedTimeSheet(SaveTimeSheetDto dto)
        {
            BaseGeneralResponse resp = new BaseGeneralResponse();
            var saveTimesheet = SaveTimeSheet(dto);
            if (saveTimesheet.Result.Success)
            {
                var userid = Glosing.Instance.Username;

                SqlParameter[] parameter = {
                    new SqlParameter("@UserId", userid)
                };
                DataTable result = await context.ExecuteStoredProcedure("dbo.uspActivatedTimeSheetUser", parameter);
                var res = result.Rows[0]["result"].ToString();
                if (res == "true")
                {
                    resp.Code = "200";
                    resp.Message = "Data has been successfully activated";
                    resp.Success = true;
                }
                else
                {
                    resp.Code = "404";
                    resp.Message = "data cannot be activated";
                    resp.Success = false;
                }
                
            }
           
            return resp;
        }

        public async Task<GeneralResponse<string>> DeleteNonJob(TimeSheetDto dto)
        {
            GeneralResponse<string> resp = new GeneralResponse<string>();
            SqlParameter[] parameter = {
            new SqlParameter("@NonJobId",dto.NonJobId)
            };
            DataTable result = await context.ExecuteStoredProcedure("dbo.uspDeleteNonJobTimeSheetUser", parameter);
            resp.Code = "200";
            resp.Message = "Data has been deleted successfully";
            resp.Success = true;
            resp.Data = dto.id;

            return resp;
        }

        public async Task<GeneralResponse<TimeSheetDto>> UpdateTimeSheet(TimeSheetDto data)
        {
            GeneralResponse<TimeSheetDto> resp = new GeneralResponse<TimeSheetDto>();

            try
            {
                var nonJob = "";
                if (string.IsNullOrEmpty(data.non_job_name))
                {
                    nonJob = data.NonJobId;
                }
                else
                {
                    nonJob = data.non_job_name;
                }

                var userid = Glosing.Instance.Username;

                SqlParameter[] parameter = {
                new SqlParameter("@id",data.id),
                new SqlParameter("@job_id", data.job_id),
                new SqlParameter("@company_id", data.company_id),
                new SqlParameter("@job_name", data.job_name),
                new SqlParameter("@non_job_name", nonJob),
                new SqlParameter("@start_date", SqlDbType.DateTime),
                new SqlParameter("@end_date", SqlDbType.DateTime),
                new SqlParameter("@Sunday", SqlDbType.DateTime),
                new SqlParameter("@Monday", SqlDbType.DateTime),
                new SqlParameter("@Tuesday", SqlDbType.DateTime),
                new SqlParameter("@Wednesday", SqlDbType.DateTime),
                new SqlParameter("@Thursday", SqlDbType.DateTime),
                new SqlParameter("@Friday", SqlDbType.DateTime),
                new SqlParameter("@Saturday", SqlDbType.DateTime),
                new SqlParameter("@UserId", userid),
                new SqlParameter("@BusinessUnitId", data.BusinessUnitId)
                };
                parameter[5].Value = data.start_date;
                parameter[6].Value = data.end_date;
                parameter[7].Value = data.Sunday;
                parameter[8].Value = data.Monday;
                parameter[9].Value = data.Tuesday;
                parameter[10].Value = data.Wednesday;
                parameter[11].Value = data.Thursday;
                parameter[12].Value = data.Friday;
                parameter[13].Value = data.Saturday;
                await context.ExecuteStoredProcedure("dbo.uspUpdateTimeSheetUser", parameter);
                resp.Success = true;
                resp.Data = data;
            }
            catch (Exception ex)
            {
                resp.Success = false;

                resp.Message = ex.Message;
            }
            return resp;
        }

        public async Task<BaseGeneralResponse> Approval(SaveTimeSheetDto dto) 
        {
            BaseGeneralResponse resp = new BaseGeneralResponse();
            Phoenix.Data.Models.TimeSheetHeader entity = new Phoenix.Data.Models.TimeSheetHeader();
            var status = dto.Status;
            var isApprove = true;
            var userid = Glosing.Instance.Username;

            entity = await context.TimeSheetHeaders.Where(x => x.Id == dto.Id).DefaultIfEmpty(new Data.Models.TimeSheetHeader()).FirstOrDefaultAsync();
            var remarks = "";
            if (status == 3)
            {
                remarks = "Data has been agreed to be accepted.";
            }
            else
            {
                remarks = "Data has been agreed to be rejected.";
            }
            await globalFcApproval.ProcessApproval<Phoenix.Data.Models.TimeSheetHeader>(dto.Id, status, remarks, entity);
            var modeltrapp = await context.TrTemplateApprovals.Where(x => x.RefId == dto.Id).FirstOrDefaultAsync();
            if (modeltrapp != null)
            {
                if (modeltrapp.StatusApprovedDescription == Phoenix.Data.Models.StatusTransaction.StatusName(status))
                {
                    entity.Status = Phoenix.Data.Models.StatusTransaction.StatusName(status);

                }
                else 
                {
                    entity.Status = Phoenix.Data.Models.StatusTransaction.StatusName(Phoenix.Data.Models.StatusTransaction.WaitingApproval);
                    status = 2;
                }
            }
            else
            {
                entity.Status = Phoenix.Data.Models.StatusTransaction.StatusName(Phoenix.Data.Models.StatusTransaction.WaitingApproval);
                status = 2;
            }

            await log.AddAsync(new Phoenix.Data.Models.StatusLog()
            {
                TransactionId = dto.Id,
                Status = Phoenix.Data.Models.StatusTransaction.StatusName(status),
                Description = "Process "+ Phoenix.Data.Models.StatusTransaction.StatusName(status)  + " timesheet"
            });

            context.PhoenixEdit(entity);
            var save = context.SaveChanges();
            if (save > 0)
            {
                isApprove = true;
            }
            else
            {
                isApprove = false;
            }

            if (isApprove)
            {
                resp.Code = "200";
                if (status == 3)
                {
                    resp.Message = "Data has been approved successfully";
                }
                else
                {
                    resp.Message = "Data has been rejected successfully";
                }

                resp.Success = true;

            }
            else
            {

                resp.Code = "404";
                if (string.IsNullOrEmpty(resp.Message))
                {
                    resp.Message = "please check the approval template that will be processed";
                }

                resp.Success = false;
            }

            try
            {
                if (resp.Success)
                {
                    SqlParameter[] parameter = {
                        new SqlParameter("@id",dto.Id),
                        new SqlParameter("@status",status),
                        new SqlParameter("@modifiedby",userid)
                    };
                    await context.ExecuteStoredProcedure("dbo.uspUpdateStatusTimesheetHeader", parameter);
                }

                return resp;
            }
            catch (Exception ex)
            {
                resp.Message = ex.Message;
                resp.Success = false;
                resp.Code = "404";
                return resp;
            }

        }
        public async Task<BaseGeneralResponse> SubmitTimeSheet(SaveTimeSheetDto dto)
        {
            BaseGeneralResponse resp = new BaseGeneralResponse();
            Phoenix.Data.Models.TimeSheetHeader entity = new Phoenix.Data.Models.TimeSheetHeader();
            var status = dto.Status;
            var isApprove = true;
            var userid = Glosing.Instance.Username;
 
            //using (var transaction = await context.Database.BeginTransactionAsync())
            //{
            //if (status != Data.Models.StatusTransactionName.Draft)
            //{
            try
            {
                if (status == 2)
                {
                    var approval = await context.TrTemplateApprovals.AsNoTracking().Where(x => x.RefId == entity.Id).ToListAsync();
                    if (approval.Count > 0)
                    {
                        var userApproval = await context.TrUserApprovals.AsNoTracking().Where(x => approval.Any(y => y.Id == x.TrTempApprovalId)).ToListAsync();
                        var conditionUserApproval = await context.TrConditionUserApprovals.AsNoTracking().Where(x => userApproval.Any(y => y.Id == x.TrUserApprovalId)).ToListAsync();

                        context.TrConditionUserApprovals.RemoveRange(conditionUserApproval);
                        context.TrUserApprovals.RemoveRange(userApproval);
                        context.TrTemplateApprovals.RemoveRange(approval);
                    }

                    var dataMenu = await menuService.GetByUniqueName(Data.Models.MenuUnique.TimeSheet).ConfigureAwait(false);
                    var dataTempate = context.TmTemplateApprovals.Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
                    var vm = new Data.Models.ViewModel.TransApprovalHrisVm()
                    {
                        IdTemplate = dataTempate.Id,
                        MenuId = dataMenu.Id,
                        RefId = dto.Id,
                        DetailLink = $"{Data.Models.ApprovalLink.TimeSheet}?Id={dto.Id}",
                        NotificationType = Phoenix.Data.Models.NotificationTypeCode.TimesheetNotification,
                        Module = Phoenix.Data.Models.NotificationModuleType.ModulePM
                    };

                    var subGroupId = PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(context.HttpContext);
                    var divisionId = PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(context.HttpContext);
                    var employeeId = PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);

                    isApprove = await globalFcApproval.Save(vm, subGroupId, divisionId, employeeId);
                    await log.AddAsync(new Phoenix.Data.Models.StatusLog()
                    {
                        TransactionId = dto.Id,
                        Status = Phoenix.Data.Models.StatusTransaction.StatusName(Phoenix.Data.Models.StatusTransaction.WaitingApproval),
                        Description = "Process submit timesheet"
                    });

                }

            }
            catch (Exception ex)
            {
                resp.Message = ex.Message;
                isApprove = false;
            }


            if (isApprove)
            {
                var save = await context.SaveChangesAsync();
                resp.Code = "200";
                if (status== 2)
                {
                    resp.Message = "Data has been submit successfully";
                }
                else
                {
                    resp.Message = "Data has been approve successfully";
                }
              
                resp.Success = true;

            }
            else
            {
                resp.Code = "404";
                if (string.IsNullOrEmpty(resp.Message))
                {
                    resp.Message = "please check the approval template that will be processed";
                }

                resp.Success = false;

            }

            try
            {
                if (resp.Success)
                {
                    SqlParameter[] parameter = {
                        new SqlParameter("@id",dto.Id),
                        new SqlParameter("@status",status),
                        new SqlParameter("@modifiedby",userid)
                    };
                    await context.ExecuteStoredProcedure("dbo.uspUpdateStatusTimesheetHeader", parameter);
                }

                return resp;
            }
            catch (Exception ex)
            {
                resp.Message = ex.Message;
                resp.Success = false;
                resp.Code = "404";
                return resp;
            }
        }


        public GeneralResponseList<JobResultDto> ListAllJob(JobResultDto data)
        {
            GeneralResponseList<JobResultDto> resp = new GeneralResponseList<JobResultDto>();
            try
            {
                //var filter = _repo.GetContext().Set<TeamMember>().Where(p => p.application_user_id == Glosing.Instance.Username);

                IQueryable<JobResultDto> que = from tb1 in _repo.GetContext().Set<TeamMember>().Where(p => p.application_user_id == Glosing.Instance.Username)

                                               join tb2 in _repo.GetContext().Set<JobDetail>() on tb1.team_id equals tb2.team_id

                                               select new JobResultDto()
                                               {
                                                   Id = tb2.Id,
                                                   job_name = tb2.job_name,

                                               };
                resp.Rows = que.Distinct().ToList();
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public GeneralResponseList<TimesheetUserDto> ListAll(TimesheetUserDto filter)
        {
            GeneralResponseList<TimesheetUserDto> resp = new GeneralResponseList<TimesheetUserDto>();
            try
            {
                IQueryable<TimesheetUserDto> que = from tb1 in _repo.GetContext().Set<TimesheetUser>()

                                                   join tb3 in _repo.GetContext().Set<JobDetail>() on tb1.job_id equals tb3.Id
                                                   where tb1.user_id == Glosing.Instance.Username

                                                   select new TimesheetUserDto()
                                                   {
                                                       id = tb1.Id,
                                                       Non_job = tb1.Non_job,

                                                       job_number = tb3.job_number,
                                                       job_name = tb3.job_name

                                                   };
                resp.Rows = que.ToList();
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }
        public GeneralResponse<TimesheetUserDtoNew> CreateTimesheetUser(TimesheetUserDtoNew data)
        {
            GeneralResponse<TimesheetUserDtoNew> resp = new GeneralResponse<TimesheetUserDtoNew>();

            try
            {
                TimesheetUser Tsu = new TimesheetUser();

                TimesheetUserData Tsud = new TimesheetUserData();



                Tsu.job_id = data.job_id;
                Tsu.Non_job = data.Non_job;
                Tsu.autocount_time = data.autocount_time;
                Tsu.user_id = Glosing.Instance.Username;

                _repo.Create(Tsu);


                Tsud.remarks = data.remarks;
                Tsud.timesheet_userid = Tsu.Id;
                Tsud.user_id = Tsu.user_id;
                Tsud.timesheet_date = Convert.ToDateTime(data.timesheet_date);




                _repouserdata.Create(Tsud);

                //_repo.SaveChanges();
                //_repononjob.SaveChanges();
                //_repouserdata.SaveChanges();
                //_repo.Dispose();
                resp.Data = data;

                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;

            }
            return resp;
        }

        public GeneralResponse<TimeSheetRow> CreateTableSheet(List<TimeSheetCreateDto> data)
        {
            GeneralResponse<TimeSheetRow> resp = new GeneralResponse<TimeSheetRow>();
            try
            {
                var name = Glosing.Instance.Username;

                for (int i1 = 0; i1 < data.Count(); i1++)
                {
                    TimeSheetCreateDto tsc = data[i1];

                    var cek = _repo.GetContext().Set<TimesheetUser>().Where(p => p.Id == data[i1].id).FirstOrDefault();
                    if (cek == null)
                    {
                        TimesheetUser tu = new TimesheetUser();
                        tu.job_id = data[i1].job_id;
                        tu.user_id = name;
                        tu.remark = data[i1].remark;
                        tu.start_date_weekly = data[i1].start_date_weekly;
                        tu.end_date_weekly = data[i1].end_date_weekly;
                        _repo.Create(tu);

                        for (int i2 = 0; i2 < data[i1].time.Count(); i2++)
                        {
                            var iu = _repouserdata.GetContext().Set<TimesheetUserData>().Where(i => i.Id == data[i1].time[i2].id).FirstOrDefault();
                            if (iu == null)
                            {
                                TimesheetUserData tud = new TimesheetUserData();
                                //_repo.GetContext().Set<TimesheetUserData>().Where(i => i.Id == data[i1].time[i2].id).FirstOrDefault();
                                tud.timesheet_hour = data[i1].time[i2].timesheet_hour;
                                tud.timesheet_date = data[i1].time[i2].timesheet_date;
                                tud.user_id = name;
                                tud.timesheet_userid = tu.Id;
                                _repouserdata.Create(tud);
                            }
                            else
                            {
                                var u = _repouserdata.GetContext().Set<TimesheetUserData>().Where(i => i.Id == data[i1].time[i2].id).FirstOrDefault();


                                u.timesheet_hour = data[i1].time[i2].timesheet_hour;

                                _repouserdata.UpdateAll(u);
                            }

                        }



                        //foreach(var TudDto in data[i1].time )
                        //{
                        //    TimesheetUserData tud = new TimesheetUserData();
                        //    _repo.GetContext().Set<TimesheetUserData>().Where(i => i.Id == TudDto.id).FirstOrDefault();
                        //    tud.timesheet_hour = TudDto.timesheet_hour;
                        //    tud.timesheet_date = TudDto.timesheet_date;
                        //    tud.timesheet_userid = tu.Id;
                        //    tud.user_id = tu.user_id;
                        //    tud.remarks = TudDto.remarks;
                        //    _repouserdata.Create(tud);
                        //}
                    }
                    else
                    {




                        //_repo.GetContext().Set<TimesheetUser>().Where(p => p.Id == data[i1].id).FirstOrDefault();
                        _repo.GetContext().Set<TimesheetUser>().Where(p => p.Id == data[i1].id).FirstOrDefault();


                        for (int i2 = 0; i2 < data[i1].time.Count(); i2++)
                        {
                            var u = _repouserdata.GetContext().Set<TimesheetUserData>().Where(i => i.Id == data[i1].time[i2].id).FirstOrDefault();
                            if (u != null)
                            {



                                u.timesheet_hour = data[i1].time[i2].timesheet_hour;

                                _repouserdata.UpdateAll(u);
                            }
                            else
                            {
                                TimesheetUserData tud = new TimesheetUserData();
                                tud.timesheet_hour = data[i1].time[i2].timesheet_hour;
                                tud.timesheet_date = data[i1].time[i2].timesheet_date;
                                tud.user_id = name;
                                tud.timesheet_userid = data[i1].id;
                                _repouserdata.Create(tud);
                            }

                        };



                        //for (int i2 = 0; i2 < data[i1].time.Count(); i2++)
                        //{
                        //    var u = _repouserdata.GetContext().Set<TimesheetUserData>().Where(i => i.Id == data[i1].time[i2].id).FirstOrDefault();


                        //    u.timesheet_hour = data[i1].time[i2].timesheet_hour;

                        //    _repouserdata.UpdateAll(u);
                        //};

                        //var job = _repo.GetContext().Set<TimesheetUser>().Where(p => p.Id == data[i1].id).FirstOrDefault();


                        //for (int i2 = 0; i2 < data[i1].time.Count(); i2++)
                        //{
                        //    var iu = _repouserdata.GetContext().Set<TimesheetUserData>().Where(i => i.Id == data[i1].time[i2].id).FirstOrDefault();
                        //    if (iu == null)
                        //    {
                        //        TimesheetUserData tud = new TimesheetUserData();
                        //        //_repo.GetContext().Set<TimesheetUserData>().Where(i => i.Id == data[i1].time[i2].id).FirstOrDefault();
                        //        tud.timesheet_hour = data[i1].time[i2].timesheet_hour;
                        //        tud.timesheet_date = data[i1].time[i2].timesheet_date;
                        //        tud.user_id = name;
                        //        tud.timesheet_userid = job.Id;
                        //        _repouserdata.Create(tud);
                        //    }
                        //    else
                        //    {
                        //        var u = _repouserdata.GetContext().Set<TimesheetUserData>().Where(i => i.Id == data[i1].time[i2].id).FirstOrDefault();


                        //        u.timesheet_hour = data[i1].time[i2].timesheet_hour;

                        //        _repouserdata.UpdateAll(u);
                        //    }


                        //};
                    }

                }


                resp.Success = true;

            }
            catch (Exception ex)
            {
                resp.Success = false;

                resp.Message = ex.Message;
            }
            return resp;
        }

        
        public GeneralResponse<TimeSheetCreateDto> CreateTimeSheet(TimeSheetCreateDto data)
        {
            GeneralResponse<TimeSheetCreateDto> resp = new GeneralResponse<TimeSheetCreateDto>();

            try
            {


                var cek = _repo.GetContext().Set<TimesheetUser>().Where(p => p.Id == data.id).FirstOrDefault();

                if (cek == null)
                {
                    TimesheetUser tu = new TimesheetUser();

                    tu.job_id = data.job_id;
                    tu.user_id = Glosing.Instance.Username;
                    tu.remark = data.remark;
                    _repo.Create(tu);

                    foreach (var TudDto in data.time)
                    {
                        TimesheetUserData tud = new TimesheetUserData();

                        _repo.GetContext().Set<TimesheetUserData>().Where(i => i.Id == TudDto.id).FirstOrDefault();
                        tud.timesheet_hour = TudDto.timesheet_hour;
                        tud.timesheet_date = TudDto.timesheet_date;
                        tud.timesheet_userid = tu.Id;
                        tud.user_id = tu.user_id;
                        if (tud.remarks == null)
                        {
                            tud.remarks = string.Empty;
                        }
                        tud.remarks = TudDto.remarks;
                        _repouserdata.Create(tud);
                    };

                }
                else
                {
                    foreach (var TdDto in data.time)
                    {
                        var u = _repo.GetContext().Set<TimesheetUserData>().Where(i => i.Id == TdDto.id).FirstOrDefault();


                        u.timesheet_hour = TdDto.timesheet_hour;

                        _repouserdata.UpdateAll(u);
                    };
                }


                resp.Data = data;

                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public async Task<DataTable> GetApprovalList()
        {
            var currentUserId = context.GetEmployeeId();

            SqlParameter[] parameter = {
                        new SqlParameter("@currentUserId",currentUserId)
                    };
            DataTable result = await context.ExecuteStoredProcedure("dbo.uspGetListTimesheetApproval", parameter);
            return result;
        }
    }
}
