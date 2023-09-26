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
    public partial interface ILeaveServices : IBaseService<LeaveDto, Leave, string>
    {
        GeneralResponseList<LeaveDto> ListAll(LeaveSearch filter);
    }
    public partial class LeaveServices : BaseService<LeaveDto, Leave, string>, ILeaveServices
    {
        public LeaveServices(IEFRepository<Leave, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<LeaveDto> ListAll(LeaveSearch filter)
        {

            GeneralResponseList<LeaveDto> resp = new GeneralResponseList<LeaveDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<LeaveDto> que = from tb1 in _repo.GetContext().Set<Leave>()
                          select new LeaveDto()
                          {
                              Id = tb1.Id,
                              employee_basic_info_id = tb1.employee_basic_info_id,
                              end_date = tb1.end_date,
                              reason = tb1.reason,
                              start_date = tb1.start_date,
                              status = tb1.status,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "employee_basic_info_id", filter.Search_employee_basic_info_id);
                que = q.filterEquals(que, "end_date", filter.Search_end_date);
                que = q.filterContains(que, "reason", filter.Search_reason);
                que = q.filterEquals(que, "start_date", filter.Search_start_date);
                que = q.filterContains(que, "status", filter.Search_status);

                

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

