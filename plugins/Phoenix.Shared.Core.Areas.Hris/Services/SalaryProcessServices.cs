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
    public partial interface ISalaryProcessServices : IBaseService<SalaryProcessDto, SalaryProcess, string>
    {
        GeneralResponseList<SalaryProcessDto> ListAll(SalaryProcessSearch filter);
    }
    public partial class SalaryProcessServices : BaseService<SalaryProcessDto, SalaryProcess, string>, ISalaryProcessServices
    {
        public SalaryProcessServices(IEFRepository<SalaryProcess, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<SalaryProcessDto> ListAll(SalaryProcessSearch filter)
        {

            GeneralResponseList<SalaryProcessDto> resp = new GeneralResponseList<SalaryProcessDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<SalaryProcessDto> que = from tb1 in _repo.GetContext().Set<SalaryProcess>()
                          select new SalaryProcessDto()
                          {
                              Id = tb1.Id,
                              basic_salary = tb1.basic_salary,
                              bonus = tb1.bonus,
                              bruto = tb1.bruto,
                              bruto_ofyears = tb1.bruto_ofyears,
                              employee_basic_info_id = tb1.employee_basic_info_id,
                              employement_status_id = tb1.employement_status_id,
                              loan = tb1.loan,
                              location = tb1.location,
                              month = tb1.month,
                              netto = tb1.netto,
                              netto_ofyears = tb1.netto_ofyears,
                              pkp_ofyears = tb1.pkp_ofyears,
                              position_cost = tb1.position_cost,
                              ptkp_ofmonths = tb1.ptkp_ofmonths,
                              ptkp_ofyears = tb1.ptkp_ofyears,
                              status_employee_id = tb1.status_employee_id,
                              tax_no_npwp = tb1.tax_no_npwp,
                              tax_ofmonths = tb1.tax_ofmonths,
                              tax_ofyears = tb1.tax_ofyears,
                              transport = tb1.transport,
                              year = tb1.year,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "basic_salary", filter.Search_basic_salary);
                que = q.filterEquals(que, "bonus", filter.Search_bonus);
                que = q.filterEquals(que, "bruto", filter.Search_bruto);
                que = q.filterEquals(que, "bruto_ofyears", filter.Search_bruto_ofyears);
                que = q.filterEquals(que, "employee_basic_info_id", filter.Search_employee_basic_info_id);
                que = q.filterEquals(que, "employement_status_id", filter.Search_employement_status_id);
                que = q.filterEquals(que, "loan", filter.Search_loan);
                que = q.filterEquals(que, "location", filter.Search_location);
                que = q.filterEquals(que, "month", filter.Search_month);
                que = q.filterEquals(que, "netto", filter.Search_netto);
                que = q.filterEquals(que, "netto_ofyears", filter.Search_netto_ofyears);
                que = q.filterEquals(que, "pkp_ofyears", filter.Search_pkp_ofyears);
                que = q.filterEquals(que, "position_cost", filter.Search_position_cost);
                que = q.filterEquals(que, "ptkp_ofmonths", filter.Search_ptkp_ofmonths);
                que = q.filterEquals(que, "ptkp_ofyears", filter.Search_ptkp_ofyears);
                que = q.filterEquals(que, "status_employee_id", filter.Search_status_employee_id);
                que = q.filterEquals(que, "tax_no_npwp", filter.Search_tax_no_npwp);
                que = q.filterEquals(que, "tax_ofmonths", filter.Search_tax_ofmonths);
                que = q.filterEquals(que, "tax_ofyears", filter.Search_tax_ofyears);
                que = q.filterEquals(que, "transport", filter.Search_transport);
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

