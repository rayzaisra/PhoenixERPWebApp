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
    public partial interface IDivisionServices : IBaseService<DivisionDto, Division, string>
    {
        GeneralResponseList<DivisionDto> ListAll(DivisionSearch filter);
    }
    public partial class DivisionServices : BaseService<DivisionDto, Division, string>, IDivisionServices
    {
        public DivisionServices(IEFRepository<Division, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<DivisionDto> ListAll(DivisionSearch filter)
        {

            GeneralResponseList<DivisionDto> resp = new GeneralResponseList<DivisionDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<DivisionDto> que = from tb1 in _repo.GetContext().Set<Division>()
                          select new DivisionDto()
                          {
                              Id = tb1.Id,
                              division_code = tb1.division_code,
                              division_name = tb1.division_name,
                              subgroupid = tb1.subgroupid,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "division_code", filter.Search_division_code);
                que = q.filterContains(que, "division_name", filter.Search_division_name);
                que = q.filterContains(que, "subgroupid", filter.Search_subgroupid);

                

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

