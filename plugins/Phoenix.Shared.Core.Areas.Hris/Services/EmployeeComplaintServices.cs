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
    public partial interface IEmployeeComplaintServices : IBaseService<EmployeeComplaintDto, EmployeeComplaint, string>
    {
        GeneralResponseList<EmployeeComplaintDto> ListAll(EmployeeComplaintSearch filter);
    }
    public partial class EmployeeComplaintServices : BaseService<EmployeeComplaintDto, EmployeeComplaint, string>, IEmployeeComplaintServices
    {
        public EmployeeComplaintServices(IEFRepository<EmployeeComplaint, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<EmployeeComplaintDto> ListAll(EmployeeComplaintSearch filter)
        {

            GeneralResponseList<EmployeeComplaintDto> resp = new GeneralResponseList<EmployeeComplaintDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<EmployeeComplaintDto> que = from tb1 in _repo.GetContext().Set<EmployeeComplaint>()
                          select new EmployeeComplaintDto()
                          {
                              Id = tb1.Id,
                              complaint_date = tb1.complaint_date,
                              complaint_description = tb1.complaint_description,
                              complaint_title = tb1.complaint_title,
                              employee_basic_info_id = tb1.employee_basic_info_id,
                              notes = tb1.notes,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "complaint_date", filter.Search_complaint_date);
                que = q.filterContains(que, "complaint_description", filter.Search_complaint_description);
                que = q.filterContains(que, "complaint_title", filter.Search_complaint_title);
                que = q.filterEquals(que, "employee_basic_info_id", filter.Search_employee_basic_info_id);
                que = q.filterContains(que, "notes", filter.Search_notes);

                

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

