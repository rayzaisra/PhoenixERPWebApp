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
    public partial interface ICandidateRecruitmentEmploymentHistoryServices : IBaseService<CandidateRecruitmentEmploymentHistoryDto, CandidateRecruitmentEmploymentHistory, string>
    {
        GeneralResponseList<CandidateRecruitmentEmploymentHistoryDto> ListAll(CandidateRecruitmentEmploymentHistorySearch filter);
        GeneralResponseList<CandidateRecruitmentEmploymentHistoryDto> GetListWithIDCandidate(string id);
    }
    public partial class CandidateRecruitmentEmploymentHistoryServices : BaseService<CandidateRecruitmentEmploymentHistoryDto, CandidateRecruitmentEmploymentHistory, string>, ICandidateRecruitmentEmploymentHistoryServices
    {
        public CandidateRecruitmentEmploymentHistoryServices(IEFRepository<CandidateRecruitmentEmploymentHistory, string> repo) : base(repo)
        {
            _repo = repo;
        }

        public GeneralResponseList<CandidateRecruitmentEmploymentHistoryDto> GetListWithIDCandidate(string id)
        {
            GeneralResponseList<CandidateRecruitmentEmploymentHistoryDto> resp = new GeneralResponseList<CandidateRecruitmentEmploymentHistoryDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<CandidateRecruitmentEmploymentHistoryDto> que = from tb1 in _repo.GetContext().Set<CandidateRecruitmentEmploymentHistory>()
                                                                       where tb1.candidate_recruitment_id == id
                                                                       select new CandidateRecruitmentEmploymentHistoryDto()
                                                                       {
                                                                           Id = tb1.Id,
                                                                           candidate_recruitment_id = tb1.candidate_recruitment_id,
                                                                           created_on = tb1.created_on,
                                                                           company_address = tb1.company_address,
                                                                           company_name = tb1.company_name,
                                                                           company_phone = tb1.company_phone,
                                                                           main_responsibilities = tb1.main_responsibilities,
                                                                           year_from = tb1.year_from,
                                                                           year_to = tb1.year_to,
                                                                           achievements = tb1.achievements,
                                                                           position = tb1.position
                                                                       };

                resp.RecordsTotal = que.Count();



                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;


                resp.Rows = que.ToList();
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

        public GeneralResponseList<CandidateRecruitmentEmploymentHistoryDto> ListAll(CandidateRecruitmentEmploymentHistorySearch filter)
        {

            GeneralResponseList<CandidateRecruitmentEmploymentHistoryDto> resp = new GeneralResponseList<CandidateRecruitmentEmploymentHistoryDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<CandidateRecruitmentEmploymentHistoryDto> que = from tb1 in _repo.GetContext().Set<CandidateRecruitmentEmploymentHistory>()
                          select new CandidateRecruitmentEmploymentHistoryDto()
                          {
                              Id = tb1.Id,
                              candidate_recruitment_id = tb1.candidate_recruitment_id,
                              company_address = tb1.company_address,
                              company_name = tb1.company_name,
                              company_phone = tb1.company_phone,
                              created_on = tb1.created_on,
                              position = tb1.position,
                              main_responsibilities = tb1.main_responsibilities,
                              year_from = tb1.year_from,
                              year_to = tb1.year_to,
                              achievements = tb1.achievements
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

