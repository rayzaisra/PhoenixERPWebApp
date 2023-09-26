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
    public partial interface IStatusServices : IBaseService<StatusDto, Status, string>
    {
        GeneralResponseList<StatusDto> ListAll(StatusSearch filter);
    }
    public partial class StatusServices : BaseService<StatusDto, Status, string>, IStatusServices
    {
        public StatusServices(IEFRepository<Status, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<StatusDto> ListAll(StatusSearch filter)
        {

            GeneralResponseList<StatusDto> resp = new GeneralResponseList<StatusDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<StatusDto> que = from tb1 in _repo.GetContext().Set<Status>()
                          select new StatusDto()
                          {
                              Id = tb1.Id,
                              status_name = tb1.status_name,
                              code = tb1.code,
                              description = tb1.description,
                              value = tb1.value,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "status_name", filter.Search_status_name);
                que = q.filterEquals(que, "value", filter.Search_value);

                

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

