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
    public partial interface IHiringRequestDetailBudgetedServices : IBaseService<HiringRequestDetailBudgetedDto, HiringRequestDetailBudgeted, string>
    {
        GeneralResponseList<HiringRequestDetailBudgetedDto> ListAll(HiringRequestDetailBudgetedSearch filter);
    }
    public partial class HiringRequestDetailBudgetedServices : BaseService<HiringRequestDetailBudgetedDto, HiringRequestDetailBudgeted, string>, IHiringRequestDetailBudgetedServices
    {
        public HiringRequestDetailBudgetedServices(IEFRepository<HiringRequestDetailBudgeted, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<HiringRequestDetailBudgetedDto> ListAll(HiringRequestDetailBudgetedSearch filter)
        {

            GeneralResponseList<HiringRequestDetailBudgetedDto> resp = new GeneralResponseList<HiringRequestDetailBudgetedDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<HiringRequestDetailBudgetedDto> que = from tb1 in _repo.GetContext().Set<HiringRequestDetailBudgeted>()
                          select new HiringRequestDetailBudgetedDto()
                          {
                              Id = tb1.Id,
                              employment_status = tb1.employment_status,
                              mpp_budged_id = tb1.mpp_budged_id,
                              ttf = tb1.ttf,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "employment_status", filter.Search_employment_status);
                que = q.filterEquals(que, "mpp_budged_id", filter.Search_mpp_budged_id);
                que = q.filterContains(que, "ttf", filter.Search_ttf);

                

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

