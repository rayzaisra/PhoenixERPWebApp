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
    public partial interface ITaskCommentServices : IBaseService<TaskCommentDto, TaskComment, string>
    {
        GeneralResponseList<TaskCommentDto> ListAll(TaskCommentSearch filter);
    }
    public partial class TaskCommentServices : BaseService<TaskCommentDto, TaskComment, string>, ITaskCommentServices
    {
        public TaskCommentServices(IEFRepository<TaskComment, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<TaskCommentDto> ListAll(TaskCommentSearch filter)
        {

            GeneralResponseList<TaskCommentDto> resp = new GeneralResponseList<TaskCommentDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<TaskCommentDto> que = from tb1 in _repo.GetContext().Set<TaskComment>()
                          select new TaskCommentDto()
                          {
                              Id = tb1.Id,
                              comment = tb1.comment,
                              comment_date = tb1.comment_date,
                              file_id = tb1.file_id,
                              task_id = tb1.task_id,
                              user_id = tb1.user_id,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "comment", filter.Search_comment);
                que = q.filterEquals(que, "comment_date", filter.Search_comment_date);
                que = q.filterEquals(que, "file_id", filter.Search_file_id);
                que = q.filterEquals(que, "task_id", filter.Search_task_id);
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

