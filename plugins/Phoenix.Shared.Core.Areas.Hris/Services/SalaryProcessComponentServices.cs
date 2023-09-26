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
    public partial interface ISalaryProcessComponentServices : IBaseService<SalaryProcessComponentDto, SalaryProcessComponent, string>
    {
        GeneralResponseList<SalaryProcessComponentDto> ListAll(SalaryProcessComponentSearch filter);
    }
    public partial class SalaryProcessComponentServices : BaseService<SalaryProcessComponentDto, SalaryProcessComponent, string>, ISalaryProcessComponentServices
    {
        public SalaryProcessComponentServices(IEFRepository<SalaryProcessComponent, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<SalaryProcessComponentDto> ListAll(SalaryProcessComponentSearch filter)
        {

            GeneralResponseList<SalaryProcessComponentDto> resp = new GeneralResponseList<SalaryProcessComponentDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<SalaryProcessComponentDto> que = from tb1 in _repo.GetContext().Set<SalaryProcessComponent>()
                          select new SalaryProcessComponentDto()
                          {
                              Id = tb1.Id,
                              component_name = tb1.component_name,
                              employee_basic_info_id = tb1.employee_basic_info_id,
                              month = tb1.month,
                              tax_id = tb1.tax_id,
                              type = tb1.type,
                              value = tb1.value,
                              value_year = tb1.value_year,
                              year = tb1.year,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "component_name", filter.Search_component_name);
                que = q.filterEquals(que, "employee_basic_info_id", filter.Search_employee_basic_info_id);
                que = q.filterEquals(que, "month", filter.Search_month);
                que = q.filterEquals(que, "tax_id", filter.Search_tax_id);
                que = q.filterContains(que, "type", filter.Search_type);
                que = q.filterEquals(que, "value", filter.Search_value);
                que = q.filterEquals(que, "value_year", filter.Search_value_year);
                que = q.filterEquals(que, "year", filter.Search_year);

                

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

