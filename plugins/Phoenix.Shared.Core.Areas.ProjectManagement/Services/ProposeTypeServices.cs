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
    public partial interface IProposeTypeServices : IBaseService<ProposeTypeDto, ProposeType, string>
    {
        GeneralResponseList<ProposeTypeDto> ListAll(ProposeTypeSearch filter);
    }
    public partial class ProposeTypeServices : BaseService<ProposeTypeDto, ProposeType, string>, IProposeTypeServices
    {
        public ProposeTypeServices(IEFRepository<ProposeType, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<ProposeTypeDto> ListAll(ProposeTypeSearch filter)
        {

            GeneralResponseList<ProposeTypeDto> resp = new GeneralResponseList<ProposeTypeDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<ProposeTypeDto> que = from tb1 in _repo.GetContext().Set<ProposeType>()
                          select new ProposeTypeDto()
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

