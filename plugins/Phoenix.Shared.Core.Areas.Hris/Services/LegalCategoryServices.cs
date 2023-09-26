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
    public partial interface ILegalCategoryServices : IBaseService<LegalCategoryDto, LegalCategory, string>
    {
        GeneralResponseList<LegalCategoryDto> ListAll(LegalCategorySearch filter);
    }
    public partial class LegalCategoryServices : BaseService<LegalCategoryDto, LegalCategory, string>, ILegalCategoryServices
    {
        public LegalCategoryServices(IEFRepository<LegalCategory, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<LegalCategoryDto> ListAll(LegalCategorySearch filter)
        {

            GeneralResponseList<LegalCategoryDto> resp = new GeneralResponseList<LegalCategoryDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<LegalCategoryDto> que = from tb1 in _repo.GetContext().Set<LegalCategory>()
                          select new LegalCategoryDto()
                          {
                              Id = tb1.Id,
                              category = tb1.category,
                              description = tb1.description,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "category", filter.Search_category);
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

