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
    public partial interface IEmployementStatusServices : IBaseService<EmployementStatusDto, EmployementStatus, string>
    {
        GeneralResponseList<EmployementStatusDto> ListAll(EmployementStatusSearch filter);
    }
    public partial class EmployementStatusServices : BaseService<EmployementStatusDto, EmployementStatus, string>, IEmployementStatusServices
    {
        public EmployementStatusServices(IEFRepository<EmployementStatus, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<EmployementStatusDto> ListAll(EmployementStatusSearch filter)
        {

            GeneralResponseList<EmployementStatusDto> resp = new GeneralResponseList<EmployementStatusDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<EmployementStatusDto> que = from tb1 in _repo.GetContext().Set<EmployementStatus>()
                          select new EmployementStatusDto()
                          {
                              Id = tb1.Id,
                              employment_status_name = tb1.employment_status_name,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "employment_status_name", filter.Search_employment_status_name);

                

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

