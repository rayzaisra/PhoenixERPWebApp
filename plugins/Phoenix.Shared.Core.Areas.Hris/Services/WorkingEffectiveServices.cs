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
    public partial interface IWorkingEffectiveServices : IBaseService<WorkingEffectiveDto, WorkingEffective, string>
    {
        GeneralResponseList<WorkingEffectiveDto> ListAll(WorkingEffectiveSearch filter);
    }
    public partial class WorkingEffectiveServices : BaseService<WorkingEffectiveDto, WorkingEffective, string>, IWorkingEffectiveServices
    {
        public WorkingEffectiveServices(IEFRepository<WorkingEffective, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<WorkingEffectiveDto> ListAll(WorkingEffectiveSearch filter)
        {

            GeneralResponseList<WorkingEffectiveDto> resp = new GeneralResponseList<WorkingEffectiveDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<WorkingEffectiveDto> que = from tb1 in _repo.GetContext().Set<WorkingEffective>()
                          select new WorkingEffectiveDto()
                          {
                              Id = tb1.Id,
                              from_time = tb1.from_time,
                              to_time = tb1.to_time,
                          };
                
                resp.RecordsTotal = que.Count();

                que = q.filterContains(que, "from_time", filter.Search_from_time);
                que = q.filterContains(que, "to_time", filter.Search_to_time);

                

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

