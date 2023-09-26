using Phoenix.Shared.Core.Areas.ProjectManagement.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Helpers;
using Phoenix.Shared.Responses;
using System;
using System.Linq;


namespace Phoenix.Shared.Core.Areas.ProjectManagement.Services
{
    public partial interface ITimelineServices : IBaseService<TaskTimesheetDto, TaskTimesheet, string>
    {
 
        #region Timeline
        GeneralResponseList<MasterSearchContentDto> ListMasterSearchTimelineJob();
        GeneralResponseList<MasterSearchContentDto> ListMasterSearchTimelineUser();
        GeneralResponseList<TableTimelineJobDto> ListTimeline(TimelineParameterInputDto param);
        GeneralResponseList<TableTimelineJobDto> ListTimelineByJobNumber(string jobnumber);
        
        GeneralResponseList<TimelineTaskNameByUserDto> ListTaskByUserName(string usernameId);
        #endregion

    }
    public partial class TimelineServices : BaseService<TaskTimesheetDto, TaskTimesheet, string>, ITimelineServices
    {
        string username = "";
        public TimelineServices(IEFRepository<TaskTimesheet, string> repo) : base(repo)
        {
            _repo = repo;
            username = Glosing.Instance.Username;
        }

 
        #region Timeline
        public GeneralResponseList<MasterSearchContentDto> ListMasterSearchTimelineJob()
        {
            GeneralResponseList<MasterSearchContentDto> resp = new GeneralResponseList<MasterSearchContentDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<MasterSearchContentDto> que = from tb1 in _repo.GetContext().Set<MasterSearchContent>()
                                                         where tb1.screen == "Timeline Job"
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
                                                         where  tb1.screen == "Timeline User"
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
                                                      join tb5 in _repo.GetContext().Set<CentralResource>() on tb2.central_resources_id equals tb5.Id
                                                      where  tb3.start_date >= param.TimelineStartDate && tb3.due_date <= param.TimelineEndDate
                                                      select new TableTimelineJobDto()
                                                      {
                                                          TaskEndDate = tb2.deadline_task,
                                                          TaskStartDate = tb2.start_task,
                                                          SubTaskEndDate = tb3.due_date,
                                                          SubTaskStartDate = tb3.start_date,
                                                          JobName = tb1.job_name,
                                                          JobNumber = tb1.job_number,
                                                          TaskId=tb2.Id,
                                                          TaskName = tb2.task_name,
                                                          TaskStatus = tb4.name,
                                                          SubTaskName = tb3.sub_task,
                                                          SubTaskId=tb3.Id,
                                                          SubTaskStatus = tb3.task_status_id,
                                                          Central_resource_id = tb5.Id,
                                                          Central_resource_name = tb5.name
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
        public GeneralResponseList<TableTimelineJobDto> ListTimelineByJobNumber(string jobnumber)
        {
            GeneralResponseList<TableTimelineJobDto> resp = new GeneralResponseList<TableTimelineJobDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<TableTimelineJobDto> que = from tb1 in _repo.GetContext().Set<JobDetail>()
                                                      join tb2 in _repo.GetContext().Set<Task>() on tb1.job_number equals tb2.job_number
                                                      join tb3 in _repo.GetContext().Set<SubTask>() on tb2.Id equals tb3.task_id
                                                      join tb4 in _repo.GetContext().Set<TaskStatus>() on tb2.task_status_id equals tb4.Id
                                                      join tb5 in _repo.GetContext().Set<CentralResource>() on tb2.central_resources_id equals tb5.Id
                                                      where tb1.job_number == jobnumber
                                                      select new TableTimelineJobDto()
                                                      {
                                                          TaskEndDate = tb2.deadline_task,
                                                          TaskStartDate = tb2.start_task,
                                                          SubTaskEndDate = tb3.due_date,
                                                          SubTaskStartDate = tb3.start_date,
                                                          JobName = tb1.job_name,
                                                          JobNumber = tb1.job_number,
                                                          TaskId = tb2.Id,
                                                          TaskName = tb2.task_name,
                                                          TaskStatus = tb4.name,
                                                          SubTaskName = tb3.sub_task,
                                                          SubTaskId = tb3.Id,
                                                          SubTaskStatus = tb3.task_status_id,
                                                          Central_resource_id=tb5.Id,
                                                          Central_resource_name=tb5.name
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

        public GeneralResponseList<TimelineTaskNameByUserDto> ListTaskByUserName(string usernameId)
        {
            GeneralResponseList<TimelineTaskNameByUserDto> resp = new GeneralResponseList<TimelineTaskNameByUserDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<TimelineTaskNameByUserDto> que = from tb1 in _repo.GetContext().Set<Task>()
                                                            join tb2 in _repo.GetContext().Set<ApplicationUser>() on tb1.central_resources_id equals tb2.Id
                                                            where tb1.is_deleted == false && tb1.central_resources_id == usernameId
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

