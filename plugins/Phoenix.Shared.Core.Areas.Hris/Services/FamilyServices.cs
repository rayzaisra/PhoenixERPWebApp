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
    public partial interface IFamilyServices : IBaseService<FamilyDto, Family, string>
    {
        GeneralResponseList<FamilyDto> ListAll(FamilySearch filter);
    }
    public partial class FamilyServices : BaseService<FamilyDto, Family, string>, IFamilyServices
    {
        public FamilyServices(IEFRepository<Family, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<FamilyDto> ListAll(FamilySearch filter)
        {

            GeneralResponseList<FamilyDto> resp = new GeneralResponseList<FamilyDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<FamilyDto> que = from tb1 in _repo.GetContext().Set<Family>()
                                            join tb2 in _repo.GetContext().Set<FamilyStatus>() on tb1.family_status_id equals tb2.Id
                          select new FamilyDto()
                          {
                              Id = tb1.Id,
                              address = tb1.address,
                              birthday = tb1.birthday.ToString(),
                              description = tb1.description,
                              employee_basic_info_id = tb1.employee_basic_info_id,
                              family_status_id = tb1.family_status_id,
                              gender = tb1.gender,
                              institution_id = tb1.institution_id,
                              married_status_id = tb1.married_status_id,
                              name = tb1.name,
                              other_no = tb1.other_no,
                              phone_no = tb1.phone_no,
                              place_birth = tb1.place_birth,
                              religion_id = tb1.religion_id,
                              family_status_name = tb2.family_status_name
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "address", filter.Search_address);
                que = q.filterEquals(que, "birthday", filter.Search_birthday);
                que = q.filterContains(que, "description", filter.Search_description);
                que = q.filterEquals(que, "education_id", filter.Search_education_id);
                que = q.filterEquals(que, "employee_basic_info_id", filter.Search_employee_basic_info_id);
                que = q.filterEquals(que, "family_status_id", filter.Search_family_status_id);
                que = q.filterContains(que, "gender", filter.Search_gender);
                que = q.filterEquals(que, "institution_id", filter.Search_institution_id);
                que = q.filterEquals(que, "married_status_id", filter.Search_married_status_id);
                que = q.filterContains(que, "name", filter.Search_name);
                que = q.filterContains(que, "other_no", filter.Search_other_no);
                que = q.filterContains(que, "phone_no", filter.Search_phone_no);
                que = q.filterContains(que, "place_birth", filter.Search_place_birth);
                que = q.filterEquals(que, "religion_id", filter.Search_religion_id);

                

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

