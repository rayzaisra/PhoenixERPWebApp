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
    public partial interface IBscCategoryServices : IBaseService<BscCategoryDto, BscCategory, string>
    {
        GeneralResponseList<BscCategoryDto> ListAll(BscCategorySearch filter);
    }
    public partial class BscCategoryServices : BaseService<BscCategoryDto, BscCategory, string>, IBscCategoryServices
    {
        public BscCategoryServices(IEFRepository<BscCategory, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<BscCategoryDto> ListAll(BscCategorySearch filter)
        {

            GeneralResponseList<BscCategoryDto> resp = new GeneralResponseList<BscCategoryDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<BscCategoryDto> que = from tb1 in _repo.GetContext().Set<BscCategory>()
                          select new BscCategoryDto()
                          {
                              Id = tb1.Id,
                              bsc_category = tb1.bsc_category,
                              bsc_description = tb1.bsc_description,
                              status_id = tb1.status_id,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "bsc_category", filter.Search_bsc_category);
                que = q.filterContains(que, "bsc_description", filter.Search_bsc_description);
                que = q.filterEquals(que, "status_id", filter.Search_status_id);

                

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

