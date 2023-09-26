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
    public partial interface IPoTypeServices : IBaseService<PoTypeDto, PoType, string>
    {
        GeneralResponseList<PoTypeDto> ListAll(PoTypeSearch filter);
    }
    public partial class PoTypeServices : BaseService<PoTypeDto, PoType, string>, IPoTypeServices
    {
        public PoTypeServices(IEFRepository<PoType, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<PoTypeDto> ListAll(PoTypeSearch filter)
        {

            GeneralResponseList<PoTypeDto> resp = new GeneralResponseList<PoTypeDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<PoTypeDto> que = from tb1 in _repo.GetContext().Set<PoType>()
                          select new PoTypeDto()
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

