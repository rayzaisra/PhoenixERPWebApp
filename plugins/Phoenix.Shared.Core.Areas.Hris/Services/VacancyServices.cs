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
    public partial interface IVacancyServices : IBaseService<VacancyDto, Vacancy, string>
    {
        GeneralResponseList<VacancyDto> ListAll(VacancySearch filter);
    }
    public partial class VacancyServices : BaseService<VacancyDto, Vacancy, string>, IVacancyServices
    {
        public VacancyServices(IEFRepository<Vacancy, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<VacancyDto> ListAll(VacancySearch filter)
        {

            GeneralResponseList<VacancyDto> resp = new GeneralResponseList<VacancyDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<VacancyDto> que = from tb1 in _repo.GetContext().Set<Vacancy>()
                          select new VacancyDto()
                          {
                              Id = tb1.Id,
                              applicant_total = tb1.applicant_total,
                              approval_id = tb1.approval_id,
                              business_unit_id = tb1.business_unit_id,
                              hiring_request_id = tb1.hiring_request_id,
                              reg = tb1.reg,
                              vacancy_name = tb1.vacancy_name,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "applicant_total", filter.Search_applicant_total);
                que = q.filterEquals(que, "approval_id", filter.Search_approval_id);
                que = q.filterEquals(que, "business_unit_id", filter.Search_business_unit_id);
                que = q.filterEquals(que, "hiring_request_id", filter.Search_hiring_request_id);
                que = q.filterContains(que, "reg", filter.Search_reg);
                que = q.filterContains(que, "vacancy_name", filter.Search_vacancy_name);

                

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

