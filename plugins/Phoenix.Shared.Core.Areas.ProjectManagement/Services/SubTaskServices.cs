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
    public partial interface ISubTaskServices : IBaseService<SubTaskDto, SubTask, string>
    {
        GeneralResponseList<SubTaskDto> ListAll(SubTaskSearch filter);
    }
    public partial class SubTaskServices : BaseService<SubTaskDto, SubTask, string>, ISubTaskServices
    {
        public SubTaskServices(IEFRepository<SubTask, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<SubTaskDto> ListAll(SubTaskSearch filter)
        {

            GeneralResponseList<SubTaskDto> resp = new GeneralResponseList<SubTaskDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<SubTaskDto> que = from tb1 in _repo.GetContext().Set<SubTask>()
                          select new SubTaskDto()
                          {
                              Id = tb1.Id,
                              attachment = tb1.attachment,
                              description = tb1.description,
                              due_date = tb1.due_date,
                              hour = tb1.hour,
                              percentage = tb1.percentage,
                              start_date = tb1.start_date,
                              sub_task = tb1.sub_task,
                              task_id = tb1.task_id,
                              task_status_id = tb1.task_status_id,
     
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "attachment", filter.Search_attachment);
                que = q.filterContains(que, "description", filter.Search_description);
                que = q.filterEquals(que, "due_date", filter.Search_due_date);
                que = q.filterEquals(que, "hour", filter.Search_hour);
                que = q.filterEquals(que, "percentage", filter.Search_percentage);
                que = q.filterEquals(que, "start_date", filter.Search_start_date);
                que = q.filterContains(que, "sub_task", filter.Search_sub_task);
                que = q.filterEquals(que, "task_id", filter.Search_task_id);
                que = q.filterEquals(que, "task_status_id", filter.Search_task_status_id);
                que = q.filterEquals(que, "user_id", filter.Search_user_id);

                

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
    }
}

