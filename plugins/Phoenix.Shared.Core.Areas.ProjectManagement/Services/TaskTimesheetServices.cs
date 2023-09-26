using Phoenix.Shared.Core.Areas.ProjectManagement.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Responses;
using System;
using System.Linq;


namespace Phoenix.Shared.Core.Areas.ProjectManagement.Services
{
    public partial interface ITaskTimesheetServices : IBaseService<TaskTimesheetDto, TaskTimesheet, string>
    {
        #region Timesheet
        GeneralResponseList<TaskTimesheetDto> ListAll(TaskTimesheetSearch filter);
        GeneralResponse<TimesheetDetailDto> GetSubTaskTimesheetDetail(string subTaskId);
        GeneralResponse<string> EntryTimesheetSingle(TimesheetDurationDto param);
        GeneralResponse<string> EntryTimesheetMultiple(MultipleTimesheetDurationDto param);
        GeneralResponseList<TimesheetDetailDto> ListSubTaskTimesheet(TimesheetParameterInputDto filter);
        GeneralResponse<string> StartTimeTimesheet(TimesheetPlayDto param);
        GeneralResponse<string> StopTimesheet(TimesheetPlayDto param);

        #endregion

        #region Timeline
        GeneralResponseList<MasterSearchContentDto> ListMasterSearchTimelineJob();
        GeneralResponseList<MasterSearchContentDto> ListMasterSearchTimelineUser();
        GeneralResponseList<TableTimelineJobDto> ListTimeline(TimelineParameterInputDto param);
        GeneralResponseList<TimelineTaskNameByUserDto> ListTaskByUserName(string usernameId);
        #endregion

    }
    public partial class TaskTimesheetServices : BaseService<TaskTimesheetDto, TaskTimesheet, string>, ITaskTimesheetServices
    {
        string username = "";
        public TaskTimesheetServices(IEFRepository<TaskTimesheet, string> repo) : base(repo)
        {
            _repo = repo;
            username = Helpers.Glosing.Instance.Username;
        }

        #region Timesheet
        public GeneralResponseList<TaskTimesheetDto> ListAll(TaskTimesheetSearch filter)
        {

            GeneralResponseList<TaskTimesheetDto> resp = new GeneralResponseList<TaskTimesheetDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<TaskTimesheetDto> que = from tb1 in _repo.GetContext().Set<TaskTimesheet>()
                                                   select new TaskTimesheetDto()
                                                   {
                                                       Id = tb1.Id,
                                                       date_worksheet = tb1.date_worksheet,
                                                       end_time = tb1.end_time,
                                                       notes = tb1.notes,
                                                       start_time = tb1.start_time,
                                                       task = tb1.task,
                                                       sub_task_id = tb1.sub_task_id,
                                                       worksheet_description = tb1.worksheet_description,
                                                       user_id = tb1.user_id,
                                                       working_hour = tb1.working_hour,
                                                       working_minutes = tb1.working_minutes
                                                   };

                resp.RecordsTotal = que.Count();

                que = q.filterEquals(que, "date_worksheet", filter.Search_date_worksheet);
                que = q.filterEquals(que, "end_time", filter.Search_end_time);
                que = q.filterContains(que, "notes", filter.Search_notes);
                que = q.filterEquals(que, "start_time", filter.Search_start_time);
                que = q.filterContains(que, "task", filter.Search_task);
                que = q.filterEquals(que, "sub_task_id", filter.Search_sub_task_id);
                que = q.filterContains(que, "worksheet_description", filter.Search_worksheet_description);
                que = q.filterContains(que, "user_id", filter.Search_worksheet_description);
                que = q.filterContains(que, "working_hour", filter.Search_worksheet_description);
                que = q.filterContains(que, "working_minutes", filter.Search_worksheet_description);

                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;

                que = q.sortir(que, "Id", filter.sortBy, filter.sortDir);
                que = q.limit(que, filter.GetLimit(), filter.GetOffset());

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

        public GeneralResponse<TimesheetDetailDto> GetSubTaskTimesheetDetail(string subTaskId)
        {
            GeneralResponse<TimesheetDetailDto> resp = new GeneralResponse<TimesheetDetailDto>();
            try
            {
                var query = (from tbl1 in _repo.GetContext().Set<TaskTimesheet>()
                             join tbl2 in _repo.GetContext().Set<SubTask>() on tbl1.sub_task_id equals tbl2.Id
                             join tbl3 in _repo.GetContext().Set<Task>() on tbl2.task_id equals tbl3.Id
                             join tbl4 in _repo.GetContext().Set<SubTaskUser>() on tbl2.Id equals tbl4.sub_task_id
                             join tbl5 in _repo.GetContext().Set<JobDetail>() on tbl3.job_number equals tbl5.job_number
                             join tbl6 in _repo.GetContext().Set<ClientBrief>() on tbl5.client_brief_id equals tbl6.Id
                             join tbl8 in _repo.GetContext().Set<CompanyBrand>() on tbl6.external_brand_id equals tbl8.Id
                             join tbl9 in _repo.GetContext().Set<Company>() on tbl8.company_id equals tbl9.Id
                             where tbl1.sub_task_id == subTaskId
                             select new TimesheetDetailDto()
                             {
                                 Brand = tbl8.brand_name,
                                 CampaignName = tbl6.campaign_name,
                                 ClientName = tbl9.company_name,
                                 dateWorksheet = tbl1.date_worksheet,
                                 DeliverableTask = tbl1.task,
                                 EndTime = tbl1.end_time,
                                 JobName = tbl5.job_name,
                                 JobNumber = tbl5.job_number,
                                 Notes = tbl1.notes,
                                 StartTime = tbl1.start_time,
                                 SubTaskId = tbl1.sub_task_id,
                                 SubTaskName = tbl2.sub_task,
                                 TaskId = tbl3.Id,
                                 TaskName = tbl3.task_name,

                                 workingHours = tbl1.working_hour,
                                 workingMinutes = tbl1.working_minutes,
                                 WorksheetDesc = tbl1.worksheet_description

                             }).FirstOrDefault();
                resp.Data = query;
                resp.Success = true;
                resp.Message = "Sukses";

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public GeneralResponse<string> EntryTimesheetSingle(TimesheetDurationDto param)
        {
            GeneralResponse<string> resp = new GeneralResponse<string>();
            string Response = "Data has been saved.";
            try
            {
                var query = _repo.GetContext().Set<TaskTimesheet>().Where(x => x.Id == param.TimesheetId).FirstOrDefault();
                if (param.WorkingHours != null)
                {
                    query.working_hour = query.working_hour + param.WorkingHours;
                    if (param.WorkingMinutes != null)
                    {
                        var time = TimeSpan.FromMinutes(Convert.ToDouble(param.WorkingMinutes));
                        query.working_hour = query.working_hour + (int)time.TotalHours;
                        query.working_minutes = query.working_minutes + time.Minutes;
                    }

                    if (param.dateWorksheet != null)
                    {
                        query.date_worksheet = param.dateWorksheet;
                    }

                    if (param.Remarks != null)
                    {
                        query.notes = param.Remarks;
                    }
                }
                else
                {
                    if (param.WorkingMinutes != null)
                    {
                        var time = TimeSpan.FromMinutes(Convert.ToDouble(param.WorkingMinutes));
                        query.working_hour = query.working_hour + (int)time.TotalHours;
                        query.working_minutes = query.working_minutes + time.Minutes;
                    }

                    if (param.dateWorksheet != null)
                    {
                        query.date_worksheet = param.dateWorksheet;
                    }

                    if (param.Remarks != null)
                    {
                        query.notes = param.Remarks;
                    }
                }
                _repo.Update(query);
                _repo.SaveChanges();

                resp.Data = Response;
                resp.Success = true;
                resp.Message = "Sukses";
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;

        }

        public GeneralResponse<string> EntryTimesheetMultiple(MultipleTimesheetDurationDto param)
        {
            GeneralResponse<string> resp = new GeneralResponse<string>();
            string Response = "Data has been saved.";
            try
            {
                for (int i = 0; i < param.Data.Count; i++)
                {
                    var query = _repo.GetContext().Set<TaskTimesheet>().Where(x => x.Id == param.Data[i].TimesheetId).FirstOrDefault();
                    if (param.Data[i].WorkingHours != null)
                    {
                        query.working_hour = query.working_hour + param.Data[i].WorkingHours;

                        if (param.Data[i].WorkingMinutes != null)
                        {
                            var time = TimeSpan.FromMinutes(Convert.ToDouble(param.Data[i].WorkingMinutes));
                            query.working_hour = query.working_hour + (int)time.TotalHours;
                            query.working_minutes = query.working_minutes + time.Minutes;
                        }

                        if (param.Data[i].dateWorksheet != null)
                        {
                            query.date_worksheet = param.Data[i].dateWorksheet;
                        }

                        if (param.Data[i].Remarks != null)
                        {
                            query.notes = param.Data[i].Remarks;
                        }
                    }
                    else
                    {
                        if (param.Data[i].WorkingMinutes != null)
                        {
                            var time = TimeSpan.FromMinutes(Convert.ToDouble(param.Data[i].WorkingMinutes));
                            query.working_hour = query.working_hour + (int)time.TotalHours;
                            query.working_minutes = query.working_minutes + time.Minutes;
                        }

                        if (param.Data[i].dateWorksheet != null)
                        {
                            query.date_worksheet = param.Data[i].dateWorksheet;
                        }

                        if (param.Data[i].Remarks != null)
                        {
                            query.notes = param.Data[i].Remarks;
                        }
                    }
                    _repo.Update(query);
                    _repo.SaveChanges();
                }

                resp.Data = Response;
                resp.Success = true;
                resp.Message = "Sukses";
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;


        }

        public GeneralResponseList<TimesheetDetailDto> ListSubTaskTimesheet(TimesheetParameterInputDto filter)
        {
            GeneralResponseList<TimesheetDetailDto> resp = new GeneralResponseList<TimesheetDetailDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<TimesheetDetailDto> que = from tbl1 in _repo.GetContext().Set<TaskTimesheet>()
                                                     join tbl2 in _repo.GetContext().Set<SubTask>() on tbl1.sub_task_id equals tbl2.Id
                                                     join tbl3 in _repo.GetContext().Set<Task>() on tbl2.task_id equals tbl3.Id
                                                     join tbl4 in _repo.GetContext().Set<SubTaskUser>() on tbl2.Id equals tbl4.sub_task_id
                                                     join tbl5 in _repo.GetContext().Set<JobDetail>() on tbl3.job_number equals tbl5.job_number
                                                     join tbl6 in _repo.GetContext().Set<ClientBrief>() on tbl5.client_brief_id equals tbl6.Id
                                                     join tbl8 in _repo.GetContext().Set<CompanyBrand>() on tbl6.external_brand_id equals tbl8.Id
                                                     join tbl9 in _repo.GetContext().Set<Company>() on tbl8.company_id equals tbl9.Id
                                                     select new TimesheetDetailDto()
                                                     {
                                                         Brand = tbl8.brand_name,
                                                         CampaignName = tbl6.campaign_name,
                                                         ClientName = tbl9.company_name,
                                                         dateWorksheet = tbl1.date_worksheet,
                                                         DeliverableTask = tbl1.task,
                                                         EndTime = tbl1.end_time,
                                                         JobName = tbl5.job_name,
                                                         JobNumber = tbl5.job_number,
                                                         Notes = tbl1.notes,
                                                         StartTime = tbl1.start_time,
                                                         SubTaskId = tbl1.sub_task_id,
                                                         SubTaskName = tbl2.sub_task,
                                                         TaskId = tbl3.Id,
                                                         TaskName = tbl3.task_name,
                                                         workingHours = tbl1.working_hour,
                                                         workingMinutes = tbl1.working_minutes,
                                                         WorksheetDesc = tbl1.worksheet_description
                                                     };

                resp.RecordsTotal = que.Count();

                que = q.filterGreaterEquals(que, "date_worksheet", filter.TimesheetStartDate);
                que = q.filterLessEquals(que, "date_worksheet", filter.TimesheetEndDate);
                que = q.filterEquals(que, "sub_task_id", filter.SubTaskID);

                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;

                que = q.sortir(que, "Id", filter.sortBy, filter.sortDir);
                que = q.limit(que, filter.GetLimit(), filter.GetOffset());

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

        public GeneralResponse<string> StartTimeTimesheet(TimesheetPlayDto param)
        {
            GeneralResponse<string> resp = new GeneralResponse<string>();
            string Response = "Data has been saved.";
            try
            {
                var query = _repo.GetContext().Set<TaskTimesheet>().Where(x => x.Id == param.TimesheetId && x.date_worksheet == DateTime.Now).FirstOrDefault();
                if (query.Id != null)
                {
                    query.start_time = DateTime.Now;
                    query.end_time = null;
                    query.date_worksheet = DateTime.Now;

                    _repo.Update(query);
                }
                else
                {
                    var dataTimesheet = new TaskTimesheet()
                    {
                        start_time = DateTime.Now,
                        end_time = null,
                        date_worksheet = DateTime.Now,
                        working_hour = null,
                        working_minutes = null,
                        sub_task_id = param.SubTaskId,
                        task = null,
                        user_id = username,
                        worksheet_description = null,
                        notes = null
                    };
                    _repo.Create(dataTimesheet);
                }
               
                _repo.SaveChanges();

                resp.Data = Response;
                resp.Success = true;
                resp.Message = "Sukses";
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public GeneralResponse<string> StopTimesheet(TimesheetPlayDto param)
        {
            GeneralResponse<string> resp = new GeneralResponse<string>();
            string Response = "Data has been saved.";
            try
            {
                var query = _repo.GetContext().Set<TaskTimesheet>().Where(x => x.Id == param.TimesheetId).FirstOrDefault();
                if (query.Id != null)
                {
                    query.end_time = DateTime.Now;
                    _repo.Update(query);
                }
                
                _repo.SaveChanges();

                resp.Data = Response;
                resp.Success = true;
                resp.Message = "Sukses";
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }


        #endregion
        #region Timeline
        public GeneralResponseList<MasterSearchContentDto> ListMasterSearchTimelineJob()
        {
            GeneralResponseList<MasterSearchContentDto> resp = new GeneralResponseList<MasterSearchContentDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<MasterSearchContentDto> que = from tb1 in _repo.GetContext().Set<MasterSearchContent>()
                                                         where tb1.isdeleted == false && tb1.screen == "Timeline Job"
                                                         select new MasterSearchContentDto()
                                                         {
                                                             Id = tb1.Id,
                                                             screen = tb1.screen,
                                                             search_content = tb1.search_content
                                                         };

                resp.RecordsTotal = que.Count();

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

        public GeneralResponseList<MasterSearchContentDto> ListMasterSearchTimelineUser()
        {
            GeneralResponseList<MasterSearchContentDto> resp = new GeneralResponseList<MasterSearchContentDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<MasterSearchContentDto> que = from tb1 in _repo.GetContext().Set<MasterSearchContent>()
                                                         where tb1.isdeleted == false && tb1.screen == "Timeline User"
                                                         select new MasterSearchContentDto()
                                                         {
                                                             Id = tb1.Id,
                                                             screen = tb1.screen,
                                                             search_content = tb1.search_content
                                                         };

                resp.RecordsTotal = que.Count();

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

        public GeneralResponseList<TableTimelineJobDto> ListTimeline(TimelineParameterInputDto param)
        {
            GeneralResponseList<TableTimelineJobDto> resp = new GeneralResponseList<TableTimelineJobDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<TableTimelineJobDto> que = from tb1 in _repo.GetContext().Set<JobDetail>()
                                                      join tb2 in _repo.GetContext().Set<Task>() on tb1.job_number equals tb2.job_number
                                                      join tb3 in _repo.GetContext().Set<SubTask>() on tb2.Id equals tb3.task_id
                                                      join tb4 in _repo.GetContext().Set<TaskStatus>() on tb2.task_status_id equals tb4.Id
                                                      where (tb3.start_date >= param.TimelineStartDate && tb3.due_date <= param.TimelineEndDate)
                                                      select new TableTimelineJobDto()
                                                      {
                                                          TaskEndDate = tb2.deadline_task,
                                                          TaskStartDate = tb2.start_task,
                                                          SubTaskEndDate = tb3.due_date,
                                                          SubTaskStartDate = tb3.start_date,
                                                          JobName = tb1.job_name,
                                                          JobNumber = tb1.job_number,
                                                          TaskName = tb2.task_name,
                                                          TaskStatus = tb4.name,
                                                          SubTaskName = tb3.sub_task,
                                                          SubTaskStatus = (from tb5 in _repo.GetContext().Set<TaskStatus>()
                                                                           where tb5.Id == tb3.task_status_id
                                                                           select tb5.name).FirstOrDefault(),
                                                          ListUserTask = (from tb5 in _repo.GetContext().Set<TaskUser>()
                                                                          join tb6 in _repo.GetContext().Set<ApplicationUser>() on tb5.user_id equals tb6.Id
                                                                          where tb5.task_id == tb2.Id
                                                                          select new TaskUserTimelineDto()
                                                                          {
                                                                              Name = tb6.app_fullname,
                                                                              UserId = tb5.user_id,
                                                                              CentralResourcesID = (from tb7 in _repo.GetContext().Set<CentralResourcesStaff>()
                                                                                                    join tb8 in _repo.GetContext().Set<CentralResource>() on tb7.central_resources_id equals tb8.Id
                                                                                                    join tb9 in _repo.GetContext().Set<BusinessUnit>() on tb8.business_unit_id equals tb9.Id
                                                                                                    where tb7.user_id == tb5.user_id
                                                                                                    select tb9.Id).FirstOrDefault(),
                                                                              CentralResourcesName = (from tb7 in _repo.GetContext().Set<CentralResourcesStaff>()
                                                                                                      join tb8 in _repo.GetContext().Set<CentralResource>() on tb7.central_resources_id equals tb8.Id
                                                                                                      join tb9 in _repo.GetContext().Set<BusinessUnit>() on tb8.business_unit_id equals tb9.Id
                                                                                                      where tb7.user_id == tb5.user_id
                                                                                                      select tb9.unit_name).FirstOrDefault()
                                                                          }).ToList()

                                                      };

                resp.RecordsTotal = que.Count();

                if (!string.IsNullOrEmpty(param.filterSearch))
                {
                    switch (param.filterCategory)
                    {
                        case "1":
                            que = from tb10 in que
                                  where tb10.TaskName.ToLower().Contains(param.filterSearch.ToLower())
                                  select tb10;
                            break;
                        case "2":
                            que = from tb10 in que
                                  where tb10.JobName.ToLower().Contains(param.filterSearch.ToLower())
                                  select tb10;
                            break;
                        case "3":
                            que = from tb10 in que
                                  where tb10.JobNumber.ToLower().Contains(param.filterSearch.ToLower())
                                  select tb10;
                            break;
                        default:
                            break;
                    }
                }


                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;

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

        public GeneralResponseList<TimelineTaskNameByUserDto> ListTaskByUserName(string usernameId)
        {
            GeneralResponseList<TimelineTaskNameByUserDto> resp = new GeneralResponseList<TimelineTaskNameByUserDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<TimelineTaskNameByUserDto> que = from tb1 in _repo.GetContext().Set<Task>()
                                                            join tb2 in _repo.GetContext().Set<ApplicationUser>() on tb1.central_resources_id equals tb2.Id
                                                            where tb1.central_resources_id == usernameId
                                                            select new TimelineTaskNameByUserDto()
                                                            {
                                                                Text = tb2.app_fullname,
                                                                Value = tb2.Id
                                                            };

                resp.RecordsTotal = que.Count();

                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;

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
        #endregion


    }
}

