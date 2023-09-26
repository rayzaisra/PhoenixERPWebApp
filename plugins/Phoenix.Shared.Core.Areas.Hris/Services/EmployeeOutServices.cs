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
    public partial interface IEmployeeOutServices : IBaseService<EmployeeOutDto, EmployeeOut, string>
    {
        GeneralResponseList<EmployeeOutDto> ListAll(EmployeeOutSearch filter);
    }
    public partial class EmployeeOutServices : BaseService<EmployeeOutDto, EmployeeOut, string>, IEmployeeOutServices
    {
        public EmployeeOutServices(IEFRepository<EmployeeOut, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<EmployeeOutDto> ListAll(EmployeeOutSearch filter)
        {

            GeneralResponseList<EmployeeOutDto> resp = new GeneralResponseList<EmployeeOutDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<EmployeeOutDto> que = from tb1 in _repo.GetContext().Set<EmployeeOut>()
                          select new EmployeeOutDto()
                          {
                              Id = tb1.Id,
                              date_out = tb1.date_out,
                              employee_basic_info_id = tb1.employee_basic_info_id,
                              reason = tb1.reason,
                              time_comeback = tb1.time_comeback,
                              time_out = tb1.time_out,
                              type = tb1.type,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "date_out", filter.Search_date_out);
                que = q.filterEquals(que, "employee_basic_info_id", filter.Search_employee_basic_info_id);
                que = q.filterContains(que, "reason", filter.Search_reason);
                que = q.filterContains(que, "time_comeback", filter.Search_time_comeback);
                que = q.filterContains(que, "time_out", filter.Search_time_out);
                que = q.filterContains(que, "type", filter.Search_type);

                

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

