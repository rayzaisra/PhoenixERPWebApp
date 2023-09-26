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
    public partial interface ICandidateRecruitmentCommentServices : IBaseService<CandidateRecruitmentCommentDto, CandidateRecruitmentComment, string>
    {
        GeneralResponseList<CandidateRecruitmentCommentDto> ListAll(CandidateRecruitmentCommentSearch filter);
    }
    public partial class CandidateRecruitmentCommentServices : BaseService<CandidateRecruitmentCommentDto, CandidateRecruitmentComment, string>, ICandidateRecruitmentCommentServices
    {
        public CandidateRecruitmentCommentServices(IEFRepository<CandidateRecruitmentComment, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<CandidateRecruitmentCommentDto> ListAll(CandidateRecruitmentCommentSearch filter)
        {

            GeneralResponseList<CandidateRecruitmentCommentDto> resp = new GeneralResponseList<CandidateRecruitmentCommentDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<CandidateRecruitmentCommentDto> que = from tb1 in _repo.GetContext().Set<CandidateRecruitmentComment>()
                                                                 select new CandidateRecruitmentCommentDto()
                                                                 {
                                                                     Id = tb1.Id,
                                                                     candidate_recruitment_id = tb1.candidate_recruitment_id,
                                                                     comment_message = tb1.comment_message,
                                                                     employee_basic_info_id = tb1.employee_basic_info_id,
                                                                     name_employee = (from a in _repo.GetContext().Set<EmployeeBasicInfo>()
                                                                                      where a.Id == tb1.employee_basic_info_id
                                                                                      select a.name_employee).FirstOrDefault(),
                                                                     created_on = tb1.created_on
                                                                 };
                
                resp.RecordsTotal = que.Count();
                que = q.filterEquals(que, "Id", filter.search_id);
                que = q.filterEquals(que, "candidate_recruitment_id", filter.search_candidate_id);
                
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

