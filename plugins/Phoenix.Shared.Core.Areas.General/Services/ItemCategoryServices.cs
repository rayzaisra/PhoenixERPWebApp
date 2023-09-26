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
    public partial interface IItemCategoryServices : IBaseService<ItemCategoryDto, ItemCategory, string>
    {
        GeneralResponseList<ItemCategoryDto> ListAll(ItemCategorySearch filter);
    }
    public partial class ItemCategoryServices : BaseService<ItemCategoryDto, ItemCategory, string>, IItemCategoryServices
    {
        public ItemCategoryServices(IEFRepository<ItemCategory, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<ItemCategoryDto> ListAll(ItemCategorySearch filter)
        {

            GeneralResponseList<ItemCategoryDto> resp = new GeneralResponseList<ItemCategoryDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<ItemCategoryDto> que = from tb1 in _repo.GetContext().Set<ItemCategory>()
                          select new ItemCategoryDto()
                          {
                              Id = tb1.Id,
                              category_name = tb1.category_name,
                              organization_id = tb1.organization_id,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "category_name", filter.Search_category_name);
                que = q.filterEquals(que, "organization_id", filter.Search_organization_id);

                

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

