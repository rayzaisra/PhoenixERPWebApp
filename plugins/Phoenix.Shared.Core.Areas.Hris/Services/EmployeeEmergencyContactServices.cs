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
    public partial interface IEmployeeEmergencyContactServices : IBaseService<EmployeeEmergencyContactDto, EmployeeEmergencyContact, string>
    {
        GeneralResponseList<EmployeeEmergencyContactDto> ListAll(EmployeeEmergencyContactSearch filter);
    }
    public partial class EmployeeEmergencyContactServices : BaseService<EmployeeEmergencyContactDto, EmployeeEmergencyContact, string>, IEmployeeEmergencyContactServices
    {
        public EmployeeEmergencyContactServices(IEFRepository<EmployeeEmergencyContact, string> repo) : base(repo)
        {
            _repo = repo;
        }

        public GeneralResponseList<EmployeeEmergencyContactDto> ListAll(EmployeeEmergencyContactSearch filter)
        {

            GeneralResponseList<EmployeeEmergencyContactDto> resp = new GeneralResponseList<EmployeeEmergencyContactDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<EmployeeEmergencyContactDto> que = from tb1 in _repo.GetContext().Set<EmployeeEmergencyContact>()
                                                              join tb2 in _repo.GetContext().Set<FamilyStatus>() on tb1.family_status_id equals tb2.Id
                                                              select new EmployeeEmergencyContactDto()
                                                              {
                                                                  Id = tb1.Id,
                                                                  address = tb1.address,
                                                                  employee_basic_info_id = tb1.employee_basic_info_id,
                                                                  full_name = tb1.full_name,
                                                                  other_no = tb1.other_no,
                                                                  phone_no = tb1.phone_no,
                                                                  relation_name = tb2.family_status_name,
                                                              };

                resp.RecordsTotal = que.Count();

                que = q.filterContains(que, "address", filter.Search_address);
                que = q.filterEquals(que, "employee_basic_info_id", filter.Search_employee_basic_info_id);
                que = q.filterContains(que, "full_name", filter.Search_full_name);
                que = q.filterContains(que, "other_no", filter.Search_other_no);
                que = q.filterContains(que, "phone_no", filter.Search_phone_no);
                que = q.filterContains(que, "realation", filter.Search_relation);



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

