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
    public partial interface IEducationServices : IBaseService<EducationDto, Education, string>
    {
        GeneralResponseList<EducationDto> ListAll(EducationSearch filter);
    }
    public partial class EducationServices : BaseService<EducationDto, Education, string>, IEducationServices
    {
        public EducationServices(IEFRepository<Education, string> repo) : base(repo)
        {
            _repo = repo;
        }

        public GeneralResponseList<EducationDto> ListAll(EducationSearch filter)
        {

            GeneralResponseList<EducationDto> resp = new GeneralResponseList<EducationDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<EducationDto> que = from tb1 in _repo.GetContext().Set<Education>()
                                               join tb2 in _repo.GetContext().Set<Institution>() on tb1.institution_id equals tb2.Id
                                               join tb3 in _repo.GetContext().Set<EducationLevel>() on tb1.education_level_id equals tb3.Id
                                               select new EducationDto()
                                               {
                                                   Id = tb1.Id,
                                                   address = tb1.address,
                                                   education_level_id = tb1.education_level_id,
                                                   employee_basic_info_id = tb1.employee_basic_info_id,
                                                   end_education = tb1.end_education,
                                                   institution_id = tb1.institution_id,
                                                   majors = tb1.majors,
                                                   start_education = tb1.start_education,
                                                   institution_name = tb2.institution_name,
                                                   qualification = tb1.qualification,
                                                   education_level_name = tb3.education_name
                                               };

                resp.RecordsTotal = que.Count();

                que = q.filterContains(que, "address", filter.Search_address);
                que = q.filterEquals(que, "education_level_id", filter.Search_education_level_id);
                que = q.filterEquals(que, "employee_basic_info_id", filter.Search_employee_basic_info_id);
                que = q.filterContains(que, "end_education", filter.Search_end_education);
                que = q.filterEquals(que, "institution_id", filter.Search_institution_id);
                que = q.filterContains(que, "majors", filter.Search_majors);
                que = q.filterContains(que, "start_education", filter.Search_start_education);



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

