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
    public partial interface ICandidateRecruitmentEducationInfoServices : IBaseService<CandidateRecruitmentEducationInfoDto, CandidateRecruitmentEducationInfo, string>
    {
        GeneralResponseList<CandidateRecruitmentEducationInfoDto> ListAll(CandidateRecruitmentEducationInfoSearch filter);
        GeneralResponseList<CandidateRecruitmentEducationInfoDto> GetListWithIDCandidate(string id);
        GeneralResponse<CandidateRecruitmentEducationInfoDto> GetDetail(string id);
    }
    public partial class CandidateRecruitmentEducationInfoServices : BaseService<CandidateRecruitmentEducationInfoDto, CandidateRecruitmentEducationInfo, string>, ICandidateRecruitmentEducationInfoServices
    {
        public CandidateRecruitmentEducationInfoServices(IEFRepository<CandidateRecruitmentEducationInfo, string> repo) : base(repo)
        {
            _repo = repo;
        }

        public GeneralResponse<CandidateRecruitmentEducationInfoDto> GetDetail(string id)
        {
            GeneralResponse<CandidateRecruitmentEducationInfoDto> resp = new GeneralResponse<CandidateRecruitmentEducationInfoDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<CandidateRecruitmentEducationInfoDto> que = from tb1 in _repo.GetContext().Set<CandidateRecruitmentEducationInfo>()
                                                                       join tb2 in _repo.GetContext().Set<EducationLevel>() on tb1.education_level_id equals tb2.Id
                                                                       join tb3 in _repo.GetContext().Set<Institution>() on tb1.institution_id equals tb3.Id
                                                                       where tb1.Id == id
                                                                       select new CandidateRecruitmentEducationInfoDto()
                                                                       {
                                                                           Id = tb1.Id,
                                                                           candidate_recruitment_id = tb1.candidate_recruitment_id,
                                                                           created_on = tb1.created_on,
                                                                           institution_name = tb3.institution_name,
                                                                           last_education = tb2.education_name,
                                                                           education_level_id = tb1.education_level_id,
                                                                           institution_id = tb1.institution_id,
                                                                           location = tb1.location,
                                                                           major = tb1.major,
                                                                           year_from = tb1.year_from,
                                                                           year_to = tb1.year_to
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

        public GeneralResponseList<CandidateRecruitmentEducationInfoDto> GetListWithIDCandidate(string id)
        {
            GeneralResponseList<CandidateRecruitmentEducationInfoDto> resp = new GeneralResponseList<CandidateRecruitmentEducationInfoDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<CandidateRecruitmentEducationInfoDto> que = from tb1 in _repo.GetContext().Set<CandidateRecruitmentEducationInfo>()
                                                                       join tb2 in _repo.GetContext().Set<EducationLevel>() on tb1.education_level_id equals tb2.Id
                                                                       join tb3 in _repo.GetContext().Set<Institution>() on tb1.institution_id equals tb3.Id
                                                                       where tb1.candidate_recruitment_id == id
                                                                       select new CandidateRecruitmentEducationInfoDto()
                                                                       {
                                                                           Id = tb1.Id,
                                                                           candidate_recruitment_id = tb1.candidate_recruitment_id,
                                                                           created_on = tb1.created_on,
                                                                           institution_name = tb3.institution_name,
                                                                           last_education = tb2.education_name,
                                                                           education_level_id = tb1.education_level_id,
                                                                           institution_id = tb1.institution_id,
                                                                           location = tb1.location,
                                                                           major = tb1.major,
                                                                           year_from = tb1.year_from,
                                                                           year_to = tb1.year_to
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

        public GeneralResponseList<CandidateRecruitmentEducationInfoDto> ListAll(CandidateRecruitmentEducationInfoSearch filter)
        {

            GeneralResponseList<CandidateRecruitmentEducationInfoDto> resp = new GeneralResponseList<CandidateRecruitmentEducationInfoDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<CandidateRecruitmentEducationInfoDto> que = from tb1 in _repo.GetContext().Set<CandidateRecruitmentEducationInfo>()
                                                                       join tb2 in _repo.GetContext().Set<EducationLevel>() on tb1.education_level_id equals tb2.Id
                                                                       join tb3 in _repo.GetContext().Set<Institution>() on tb1.institution_id equals tb3.Id
                                                                        select new CandidateRecruitmentEducationInfoDto()
                                                                          {
                                                                              Id = tb1.Id,
                                                                              candidate_recruitment_id = tb1.candidate_recruitment_id,
                                                                              created_on = tb1.created_on,
                                                                              institution_name = tb3.institution_name,
                                                                              education_level_id = tb1.education_level_id,
                                                                              institution_id = tb1.institution_id,
                                                                              last_education = tb2.education_name,
                                                                              location = tb1.location,
                                                                              major = tb1.major,
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

