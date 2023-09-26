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
    public partial interface IEducationLevelServices : IBaseService<EducationLevelDto, EducationLevel, string>
    {
        GeneralResponseList<EducationLevelDto> ListAll(EducationLevelSearch filter);
    }
    public partial class EducationLevelServices : BaseService<EducationLevelDto, EducationLevel, string>, IEducationLevelServices
    {
        public EducationLevelServices(IEFRepository<EducationLevel, string> repo) : base(repo)
        {
            _repo = repo;
        }

        public GeneralResponseList<EducationLevelDto> ListAll(EducationLevelSearch filter)
        {

            GeneralResponseList<EducationLevelDto> resp = new GeneralResponseList<EducationLevelDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<EducationLevelDto> que = from tb1 in _repo.GetContext().Set<EducationLevel>()
                                                    select new EducationLevelDto()
                                                    {
                                                        Id = tb1.Id,
                                                        education_name = tb1.education_name,
                                                        education_seq = tb1.education_seq
                                                    };

                resp.RecordsTotal = que.Count();
                que = que.OrderByDescending(x => x.education_seq);
                if (!string.IsNullOrEmpty(filter.Search_education_id))
                que = q.filterContains(que, "id", filter.Search_education_id);

                //if (!string.IsNullOrEmpty(filter.Search_education_name))
                //    que = q.filterContains(que, "education_name", filter.Search_education_name);



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

