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
    public partial interface ITaskPriorityServices : IBaseService<TaskPriorityDto, TaskPriority, string>
    {
        GeneralResponseList<TaskPriorityDto> ListAll(TaskPrioritySearch filter);
    }
    public partial class TaskPriorityServices : BaseService<TaskPriorityDto, TaskPriority, string>, ITaskPriorityServices
    {
        public TaskPriorityServices(IEFRepository<TaskPriority, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<TaskPriorityDto> ListAll(TaskPrioritySearch filter)
        {

            GeneralResponseList<TaskPriorityDto> resp = new GeneralResponseList<TaskPriorityDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<TaskPriorityDto> que = from tb1 in _repo.GetContext().Set<TaskPriority>()
                          select new TaskPriorityDto()
                          {
                              Id = tb1.Id,
                              name = tb1.name,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "name", filter.Search_name);

                

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

