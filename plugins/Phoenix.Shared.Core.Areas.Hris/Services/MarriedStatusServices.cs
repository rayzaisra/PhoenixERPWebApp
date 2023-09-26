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
    public partial interface IMarriedStatusServices : IBaseService<MarriedStatusDto, MarriedStatus, string>
    {
        GeneralResponseList<MarriedStatusDto> ListAll(MarriedStatusSearch filter);
    }
    public partial class MarriedStatusServices : BaseService<MarriedStatusDto, MarriedStatus, string>, IMarriedStatusServices
    {
        public MarriedStatusServices(IEFRepository<MarriedStatus, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<MarriedStatusDto> ListAll(MarriedStatusSearch filter)
        {

            GeneralResponseList<MarriedStatusDto> resp = new GeneralResponseList<MarriedStatusDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<MarriedStatusDto> que = from tb1 in _repo.GetContext().Set<MarriedStatus>()
                          select new MarriedStatusDto()
                          {
                              Id = tb1.Id,
                              married_name = tb1.married_name,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "married_name", filter.Search_married_name);

                

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

