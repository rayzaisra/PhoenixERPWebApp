using Phoenix.Shared.Core.Areas.ProjectManagement.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Responses;
using System;
using System.Linq;


namespace Phoenix.Shared.Core.Areas.ProjectManagement.Services
{
    public partial interface IRateCardCodeServices : IBaseService<RateCardCodeDto, RateCardCode, string>
    {
        GeneralResponseList<RateCardCodeDto> ListAll(RateCardCodeSearch filter);
    }
    public partial class RateCardCodeServices : BaseService<RateCardCodeDto, RateCardCode, string>, IRateCardCodeServices
    {
        public RateCardCodeServices(IEFRepository<RateCardCode, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<RateCardCodeDto> ListAll(RateCardCodeSearch filter)
        {

            GeneralResponseList<RateCardCodeDto> resp = new GeneralResponseList<RateCardCodeDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<RateCardCodeDto> que = from tb1 in _repo.GetContext().Set<RateCardCode>()
                          select new RateCardCodeDto()
                          {
                              Id = tb1.Id,
                              code = tb1.code,
                              description = tb1.description,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "code", filter.Search_code);
                que = q.filterContains(que, "description", filter.Search_description);

                

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

