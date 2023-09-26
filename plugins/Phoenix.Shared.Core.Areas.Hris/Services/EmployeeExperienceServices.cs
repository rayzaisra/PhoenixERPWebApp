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
    public partial interface IEmployeeExperienceServices : IBaseService<EmployeeExperienceDto, EmployeeExperience, string>
    {
        GeneralResponseList<EmployeeExperienceDto> ListAll(EmployeeExperienceSearch filter);
    }
    public partial class EmployeeExperienceServices : BaseService<EmployeeExperienceDto, EmployeeExperience, string>, IEmployeeExperienceServices
    {
        public EmployeeExperienceServices(IEFRepository<EmployeeExperience, string> repo) : base(repo)
        {
            _repo = repo;
        }

        public GeneralResponseList<EmployeeExperienceDto> ListAll(EmployeeExperienceSearch filter)
        {

            GeneralResponseList<EmployeeExperienceDto> resp = new GeneralResponseList<EmployeeExperienceDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<EmployeeExperienceDto> que = from tb1 in _repo.GetContext().Set<EmployeeExperience>()
                                                        select new EmployeeExperienceDto()
                                                        {
                                                            Id = tb1.Id,
                                                            address = tb1.address,
                                                            employee_basic_info_id = tb1.employee_basic_info_id,
                                                            end_date = tb1.end_date,
                                                            company_name = tb1.company_name,
                                                            phone = tb1.phone,
                                                            position = tb1.position,
                                                            start_date = tb1.start_date,
                                                            remark = tb1.remark,
                                                        };

                resp.RecordsTotal = que.Count();

                que = q.filterContains(que, "address", filter.Search_address);
                que = q.filterEquals(que, "employee_basic_info_id", filter.Search_employee_basic_info_id);
                que = q.filterContains(que, "end_date", filter.Search_end_date);
                que = q.filterEquals(que, "company_name", filter.Search_company_name);
                que = q.filterContains(que, "phone", filter.Search_phone);
                que = q.filterEquals(que, "position", filter.Search_position);
                que = q.filterContains(que, "start_date", filter.Search_start_date);


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

