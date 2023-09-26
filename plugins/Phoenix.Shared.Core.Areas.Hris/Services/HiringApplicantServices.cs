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
    public partial interface IHiringApplicantServices : IBaseService<HiringApplicantDto, HiringApplicant, string>
    {
        GeneralResponseList<HiringApplicantDto> ListAll(HiringApplicantSearch filter);
    }
    public partial class HiringApplicantServices : BaseService<HiringApplicantDto, HiringApplicant, string>, IHiringApplicantServices
    {
        public HiringApplicantServices(IEFRepository<HiringApplicant, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<HiringApplicantDto> ListAll(HiringApplicantSearch filter)
        {

            GeneralResponseList<HiringApplicantDto> resp = new GeneralResponseList<HiringApplicantDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<HiringApplicantDto> que = from tb1 in _repo.GetContext().Set<HiringApplicant>()
                          select new HiringApplicantDto()
                          {
                              Id = tb1.Id,
                              candidate_recruitment_id = tb1.candidate_recruitment_id,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "hiring_request_id", filter.Search_hiring_request_id);

                

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

