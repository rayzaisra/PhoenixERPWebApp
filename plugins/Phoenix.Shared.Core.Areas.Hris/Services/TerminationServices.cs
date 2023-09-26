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
    public partial interface ITerminationServices : IBaseService<TerminationDto, Termination, string>
    {
        GeneralResponseList<TerminationDto> ListAll(TerminationSearch filter);
    }
    public partial class TerminationServices : BaseService<TerminationDto, Termination, string>, ITerminationServices
    {
        public TerminationServices(IEFRepository<Termination, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<TerminationDto> ListAll(TerminationSearch filter)
        {

            GeneralResponseList<TerminationDto> resp = new GeneralResponseList<TerminationDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<TerminationDto> que = from tb1 in _repo.GetContext().Set<Termination>()
                          select new TerminationDto()
                          {
                              Id = tb1.Id,
                              employee_basic_info_id = tb1.employee_basic_info_id,
                              reason = tb1.reason,
                              termination_date = tb1.termination_date,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "employee_basic_info_id", filter.Search_employee_basic_info_id);
                que = q.filterContains(que, "reason", filter.Search_reason);
                que = q.filterEquals(que, "termination_date", filter.Search_termination_date);

                

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

