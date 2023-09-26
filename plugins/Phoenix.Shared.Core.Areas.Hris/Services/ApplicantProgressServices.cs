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
    public partial interface IApplicantProgressServices : IBaseService<ApplicantProgressDto, ApplicantProgress, string>
    {
        GeneralResponseList<ApplicantProgressDto> ListAll(ApplicantProgressSearch filter);
    }
    public partial class ApplicantProgressServices : BaseService<ApplicantProgressDto, ApplicantProgress, string>, IApplicantProgressServices
    {
        public ApplicantProgressServices(IEFRepository<ApplicantProgress, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<ApplicantProgressDto> ListAll(ApplicantProgressSearch filter)
        {

            GeneralResponseList<ApplicantProgressDto> resp = new GeneralResponseList<ApplicantProgressDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<ApplicantProgressDto> que = from tb1 in _repo.GetContext().Set<ApplicantProgress>()
                          select new ApplicantProgressDto()
                          {
                              Id = tb1.Id,
                              applicant_progress = tb1.applicant_progress,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "applicant_progress", filter.Search_applicant_progress);

                

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

