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
    public partial interface IEmployeeAbsenceServices : IBaseService<EmployeeAbsenceDto, EmployeeAbsence, string>
    {
        GeneralResponseList<EmployeeAbsenceDto> ListAll(EmployeeAbsenceSearch filter);
    }
    public partial class EmployeeAbsenceServices : BaseService<EmployeeAbsenceDto, EmployeeAbsence, string>, IEmployeeAbsenceServices
    {
        public EmployeeAbsenceServices(IEFRepository<EmployeeAbsence, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<EmployeeAbsenceDto> ListAll(EmployeeAbsenceSearch filter)
        {

            GeneralResponseList<EmployeeAbsenceDto> resp = new GeneralResponseList<EmployeeAbsenceDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<EmployeeAbsenceDto> que = from tb1 in _repo.GetContext().Set<EmployeeAbsence>()
                          select new EmployeeAbsenceDto()
                          {
                              Id = tb1.Id,
                              date_absence = tb1.date_absence,
                              employee_basic_info_id = tb1.employee_basic_info_id,
                              note = tb1.note,
                              time_back = tb1.time_back,
                              time_come = tb1.time_come,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "date_absence", filter.Search_date_absence);
                que = q.filterEquals(que, "employee_basic_info_id", filter.Search_employee_basic_info_id);
                que = q.filterContains(que, "note", filter.Search_note);
                que = q.filterEquals(que, "time_back", filter.Search_time_back);
                que = q.filterEquals(que, "time_come", filter.Search_time_come);

                

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

