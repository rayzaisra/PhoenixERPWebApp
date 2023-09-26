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
    public partial interface ISubSalaryComponentServices : IBaseService<SubSalaryComponentDto, SubSalaryComponent, string>
    {
        GeneralResponseList<SubSalaryComponentDto> ListAll(SubSalaryComponentSearch filter);
        GeneralResponse<SubSalaryComponentDto> GetWithName(string id);
    }
    public partial class SubSalaryComponentServices : BaseService<SubSalaryComponentDto, SubSalaryComponent, string>, ISubSalaryComponentServices
    {
        private IEFRepository<SalaryComponent, string> _repoSalarycomponent;
        public SubSalaryComponentServices(IEFRepository<SubSalaryComponent, string> repo, IEFRepository<SalaryComponent, string> repoSalarycomponent) : base(repo)
        {
            _repo = repo;
            _repoSalarycomponent = repoSalarycomponent;
        }

        public GeneralResponse<SubSalaryComponentDto> GetWithName(string id)
        {
            GeneralResponse<SubSalaryComponentDto> resp = new GeneralResponse<SubSalaryComponentDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<SubSalaryComponentDto> que = from tb1 in _repo.GetContext().Set<SubSalaryComponent>()
                                               join tb2 in _repo.GetContext().Set<SalaryComponent>() on tb1.salary_component_id equals tb2.Id
                                               where tb1.Id == id
                                               select new SubSalaryComponentDto()
                                               {
                                                   Id = tb1.Id,
                                                   name = tb1.name,
                                                   value = tb1.value,
                                                   description = tb1.description,
                                                   salary_component = tb2.salary_component,
                                                   salary_component_id = tb1.salary_component_id
                                                   //created_on = tb1.created_on
                                                   //mpp_id = tb1.mpp_id,
                                                   //ttf = tb1.ttf,
                                                   //job_grade_name = tb2.grade_name,
                                                   //job_title_name = tb3.title_name,
                                                   //desc = tb1.desc,
                                                   //porject_name = tb1.project_name,
                                                   //budget_souce = tb1.budget_souce,
                                                   //status = tb1.status,
                                                   //job_grade_id = tb1.job_grade_id
                                               };
                resp.Data = que.FirstOrDefault();
                resp.Success = true;
                resp.Code = "00";
                resp.Message = "sucess";

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public GeneralResponseList<SubSalaryComponentDto> ListAll(SubSalaryComponentSearch filter)
        {

            GeneralResponseList<SubSalaryComponentDto> resp = new GeneralResponseList<SubSalaryComponentDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<SubSalaryComponentDto> que = from tb1 in _repo.GetContext().Set<SubSalaryComponent>()
                                                        join tb2 in _repo.GetContext().Set<SalaryComponent>() on tb1.salary_component_id equals tb2.Id
                                               select new SubSalaryComponentDto()
                                               {
                                                   Id = tb1.Id,
                                                   name = tb1.name,
                                                   value = tb1.value,
                                                   description = tb1.description,
                                                   salary_component_id = tb1.salary_component_id,
                                                   salary_component = tb2.salary_component,
                                                   //employment_status = tb1.employment_status,
                                                   //job_tittle_id = tb1.job_tittle_id,
                                                   //mpp_budged = tb1.mpp_budged,
                                                   //mpp_id = tb1.mpp_id,
                                                   //ttf = tb1.ttf,
                                               };

                resp.RecordsTotal = que.Count();

                que = q.filterContains(que, "name", filter.Search_name);
                //que = q.filterEquals(que, "salary_component", filter.Search_salary_component);
                //que = q.filterEquals(que, "mpp_budged", filter.Search_mpp_budged);
                //que = q.filterEquals(que, "mpp_id", filter.Search_mpp_id);
                //que = q.filterContains(que, "ttf", filter.Search_ttf);



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

