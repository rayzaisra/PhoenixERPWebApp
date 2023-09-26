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
    public partial interface IApplicationEntityServices : IBaseService<ApplicationEntityDto, ApplicationEntity, string>
    {
        GeneralResponseList<ApplicationEntityDto> ListAll(ApplicationEntitySearch filter);
    }
    public partial class ApplicationEntityServices : BaseService<ApplicationEntityDto, ApplicationEntity, string>, IApplicationEntityServices
    {
        public ApplicationEntityServices(IEFRepository<ApplicationEntity, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<ApplicationEntityDto> ListAll(ApplicationEntitySearch filter)
        {

            GeneralResponseList<ApplicationEntityDto> resp = new GeneralResponseList<ApplicationEntityDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<ApplicationEntityDto> que = from tb1 in _repo.GetContext().Set<ApplicationEntity>()
                          select new ApplicationEntityDto()
                          {
                              Id = tb1.Id,
                              display_name = tb1.display_name,
                              entity_name = tb1.entity_name,
                              organization_id = tb1.organization_id,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "display_name", filter.Search_display_name);
                que = q.filterContains(que, "entity_name", filter.Search_entity_name);
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

