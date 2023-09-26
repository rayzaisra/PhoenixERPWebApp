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
    public partial interface IBussinessTypeServices : IBaseService<BussinessTypeDto, BussinessType, string>
    {
        GeneralResponseList<BussinessTypeDto> ListAll(BussinessTypeSearch filter);
    }
    public partial class BussinessTypeServices : BaseService<BussinessTypeDto, BussinessType, string>, IBussinessTypeServices
    {
        public BussinessTypeServices(IEFRepository<BussinessType, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<BussinessTypeDto> ListAll(BussinessTypeSearch filter)
        {

            GeneralResponseList<BussinessTypeDto> resp = new GeneralResponseList<BussinessTypeDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<BussinessTypeDto> que = from tb1 in _repo.GetContext().Set<BussinessType>()
                          select new BussinessTypeDto()
                          {
                              Id = tb1.Id,
                              propose_type = tb1.propose_type,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "propose_type", filter.Search_propose_type);

                

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

