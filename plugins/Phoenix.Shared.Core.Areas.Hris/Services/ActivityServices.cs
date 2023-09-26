using Phoenix.Shared.Core.Areas.Hris.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Responses;
using System;
using System.Linq;


namespace Phoenix.Shared.Core.Areas.Hris.Services
{
    public partial interface IActivityServices : IBaseService<ActivityDto, Activity, string>
    {
        GeneralResponseList<ActivityDto> ListAll(ActivitySearch filter);
    }
    public partial class ActivityServices : BaseService<ActivityDto, Activity, string>, IActivityServices
    {
        public ActivityServices(IEFRepository<Activity, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<ActivityDto> ListAll(ActivitySearch filter)
        {

            GeneralResponseList<ActivityDto> resp = new GeneralResponseList<ActivityDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<ActivityDto> que = from tb1 in _repo.GetContext().Set<Activity>()
                          select new ActivityDto()
                          {
                              Id = tb1.Id,
                              code = tb1.code,
                              description = tb1.description,
                              type = tb1.type,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "code", filter.Search_code);
                que = q.filterContains(que, "description", filter.Search_description);
                que = q.filterContains(que, "type", filter.Search_type);

                

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

