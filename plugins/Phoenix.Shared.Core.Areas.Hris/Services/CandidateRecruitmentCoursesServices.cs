using Phoenix.Shared.Core.Areas.General.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Responses;
using System;
using System.Linq;


namespace Phoenix.Shared.Core.Areas.General.Services
{
    public partial interface ICandidateRecruitmentCoursesServices : IBaseService<CandidateRecruitmentCoursesDto, CandidateRecruitmentCourses, string>
    {
        GeneralResponseList<CandidateRecruitmentCoursesDto> ListAll(CandidateRecruitmentCoursesSearch filter);
    }
    public partial class CandidateRecruitmentCoursesServices : BaseService<CandidateRecruitmentCoursesDto, CandidateRecruitmentCourses, string>, ICandidateRecruitmentCoursesServices
    {
        public CandidateRecruitmentCoursesServices(IEFRepository<CandidateRecruitmentCourses, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<CandidateRecruitmentCoursesDto> ListAll(CandidateRecruitmentCoursesSearch filter)
        {

            GeneralResponseList<CandidateRecruitmentCoursesDto> resp = new GeneralResponseList<CandidateRecruitmentCoursesDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<CandidateRecruitmentCoursesDto> que = from tb1 in _repo.GetContext().Set<CandidateRecruitmentCourses>()
                          select new CandidateRecruitmentCoursesDto()
                          {
                              Id = tb1.Id,
                              candidate_recruitment_id = tb1.candidate_recruitment_id,
                              course_name = tb1.course_name,
                              remarks = tb1.remarks,
                              year_from = tb1.year_from,
                              year_to = tb1.year_to
                          };
                
                resp.RecordsTotal = que.Count();
                que = q.filterEquals(que, "candidate_recruitment_id", filter.Search_candidate_recruitment_id);
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

