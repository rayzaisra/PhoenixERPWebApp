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
    public partial interface ITaskUserServices : IBaseService<TaskUserDto, TaskUser, string>
    {
        GeneralResponseList<TaskUserDto> ListAll(TaskUserSearch filter);
    }
    public partial class TaskUserServices : BaseService<TaskUserDto, TaskUser, string>, ITaskUserServices
    {
        public TaskUserServices(IEFRepository<TaskUser, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<TaskUserDto> ListAll(TaskUserSearch filter)
        {

            GeneralResponseList<TaskUserDto> resp = new GeneralResponseList<TaskUserDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<TaskUserDto> que = from tb1 in _repo.GetContext().Set<TaskUser>()
                          select new TaskUserDto()
                          {
                              Id = tb1.Id,
                              job_posting_requisition_id = tb1.job_posting_requisition_id,
                              task_id = tb1.task_id,
                              task_user = tb1.task_user,
                              user_id = tb1.user_id,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "job_posting_requisition_id", filter.Search_job_posting_requisition_id);
                que = q.filterEquals(que, "task_id", filter.Search_task_id);
                que = q.filterContains(que, "task_user", filter.Search_task_user);
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

