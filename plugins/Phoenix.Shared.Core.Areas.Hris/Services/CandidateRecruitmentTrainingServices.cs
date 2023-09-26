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
    public partial interface ICandidateRecruitmentTrainingServices : IBaseService<CandidateRecruitmentTrainingDto, CandidateRecruitmentTraining, string>
    {
        GeneralResponseList<CandidateRecruitmentTrainingDto> ListAll(CandidateRecruitmentTrainingSearch filter);
    }
    public partial class CandidateRecruitmentTrainingServices : BaseService<CandidateRecruitmentTrainingDto, CandidateRecruitmentTraining, string>, ICandidateRecruitmentTrainingServices
    {
        public CandidateRecruitmentTrainingServices(IEFRepository<CandidateRecruitmentTraining, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<CandidateRecruitmentTrainingDto> ListAll(CandidateRecruitmentTrainingSearch filter)
        {

            GeneralResponseList<CandidateRecruitmentTrainingDto> resp = new GeneralResponseList<CandidateRecruitmentTrainingDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<CandidateRecruitmentTrainingDto> que = from tb1 in _repo.GetContext().Set<CandidateRecruitmentTraining>()
                          select new CandidateRecruitmentTrainingDto()
                          {
                              Id = tb1.Id,
                              candidate_recruitment_id = tb1.candidate_recruitment_id,
                              score = tb1.score,
                              status = tb1.status,
                              training_name = tb1.training_name,
                              job_vacancy_id = tb1.job_vacancy_id

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

