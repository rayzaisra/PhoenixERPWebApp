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
    public partial interface ICandidateRecruitmentSignContractServices : IBaseService<CandidateRecruitmentSignContractDto, CandidateRecruitmentSignContract, string>
    {
        GeneralResponseList<CandidateRecruitmentSignContractDto> ListAll(CandidateRecruitmentSignContractSearch filter);
    }
    public partial class CandidateRecruitmentSignContractServices : BaseService<CandidateRecruitmentSignContractDto, CandidateRecruitmentSignContract, string>, ICandidateRecruitmentSignContractServices
    {
        public CandidateRecruitmentSignContractServices(IEFRepository<CandidateRecruitmentSignContract, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<CandidateRecruitmentSignContractDto> ListAll(CandidateRecruitmentSignContractSearch filter)
        {

            GeneralResponseList<CandidateRecruitmentSignContractDto> resp = new GeneralResponseList<CandidateRecruitmentSignContractDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<CandidateRecruitmentSignContractDto> que = from tb1 in _repo.GetContext().Set<CandidateRecruitmentSignContract>()
                          select new CandidateRecruitmentSignContractDto()
                          {
                              Id = tb1.Id,
                              candidate_recruitment_id = tb1.candidate_recruitment_id,
                              created_on = tb1.created_on,
                              filemaster_id = tb1.filemaster_id,
                              remarks = tb1.remarks
                          };
                
                resp.RecordsTotal = que.Count();



                

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

