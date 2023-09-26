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
    public partial interface ICandidateRecruitmentInterviewServices : IBaseService<CandidateRecruitmentInterviewDto, CandidateRecruitmentInterview, string>
    {
        GeneralResponseList<CandidateRecruitmentInterviewDto> ListAll(CandidateRecruitmentInterviewSearch filter);
    }
    public partial class CandidateRecruitmentInterviewServices : BaseService<CandidateRecruitmentInterviewDto, CandidateRecruitmentInterview, string>, ICandidateRecruitmentInterviewServices
    {
        public CandidateRecruitmentInterviewServices(IEFRepository<CandidateRecruitmentInterview, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<CandidateRecruitmentInterviewDto> ListAll(CandidateRecruitmentInterviewSearch filter)
        {

            GeneralResponseList<CandidateRecruitmentInterviewDto> resp = new GeneralResponseList<CandidateRecruitmentInterviewDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<CandidateRecruitmentInterviewDto> que = from tb1 in _repo.GetContext().Set<CandidateRecruitmentInterview>()
                                                                   join tb2 in _repo.GetContext().Set<MasterLocation>() on tb1.master_location_id equals tb2.Id
                          select new CandidateRecruitmentInterviewDto()
                          {
                              Id = tb1.Id,
                              candidate_recruitment_id = tb1.candidate_recruitment_id,
                              content = tb1.content,
                              date_interview = tb1.date_interview,
                              Interview_from = tb1.Interview_from,
                              Interview_to = tb1.Interview_to,
                              interviewer = tb1.interviewer,
                              job_vacancy_id = tb1.job_vacancy_id,
                              master_location_id = tb1.master_location_id,
                              time_from_hour = tb1.time_from_hour,
                              time_from_minutes = tb1.time_from_minutes,
                              time_to_hour = tb1.time_to_hour,
                              time_to_minutes = tb1.time_to_minutes,
                              location_name = tb2.location_name,
                              subject = tb1.subject

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

