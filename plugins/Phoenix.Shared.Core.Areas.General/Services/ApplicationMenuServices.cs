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
    public partial interface IApplicationMenuServices : IBaseService<ApplicationMenuDto, ApplicationMenu, string>
    {
        GeneralResponseList<ApplicationMenuDto> ListAll(ApplicationMenuSearch filter);
    }
    public partial class ApplicationMenuServices : BaseService<ApplicationMenuDto, ApplicationMenu, string>, IApplicationMenuServices
    {
        public ApplicationMenuServices(IEFRepository<ApplicationMenu, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<ApplicationMenuDto> ListAll(ApplicationMenuSearch filter)
        {

            GeneralResponseList<ApplicationMenuDto> resp = new GeneralResponseList<ApplicationMenuDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<ApplicationMenuDto> que = from tb1 in _repo.GetContext().Set<ApplicationMenu>()
                          select new ApplicationMenuDto()
                          {
                              Id = tb1.Id,
                              action_url = tb1.action_url,
                              application_menu_category_id = tb1.application_menu_category_id,
                              application_menu_group_id = tb1.application_menu_group_id,
                              menu_name = tb1.menu_name,
                              organization_id = tb1.organization_id,
                              parent_menu_id = tb1.parent_menu_id,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "action_url", filter.Search_action_url);
                que = q.filterEquals(que, "application_menu_category_id", filter.Search_application_menu_category_id);
                que = q.filterEquals(que, "application_menu_group_id", filter.Search_application_menu_group_id);
                que = q.filterContains(que, "menu_name", filter.Search_menu_name);
                que = q.filterEquals(que, "organization_id", filter.Search_organization_id);
                que = q.filterEquals(que, "parent_menu_id", filter.Search_parent_menu_id);

                

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

