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
    public partial interface IItemServices : IBaseService<ItemDto, Item, string>
    {
        GeneralResponseList<ItemDto> ListAll(ItemSearch filter);
    }
    public partial class ItemServices : BaseService<ItemDto, Item, string>, IItemServices
    {
        public ItemServices(IEFRepository<Item, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<ItemDto> ListAll(ItemSearch filter)
        {

            GeneralResponseList<ItemDto> resp = new GeneralResponseList<ItemDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<ItemDto> que = from tb1 in _repo.GetContext().Set<Item>()
                          select new ItemDto()
                          {
                              Id = tb1.Id,
                              item_category_id = tb1.item_category_id,
                              item_code = tb1.item_code,
                              item_name = tb1.item_name,
                              organization_id = tb1.organization_id,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "item_category_id", filter.Search_item_category_id);
                que = q.filterContains(que, "item_code", filter.Search_item_code);
                que = q.filterContains(que, "item_name", filter.Search_item_name);
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

