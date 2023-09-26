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
    public partial interface IClientBriefContentServices : IBaseService<ClientBriefContentDto, ClientBriefContent, string>
    {
        GeneralResponseList<ClientBriefContentDto> ListAll(ClientBriefContentSearch filter);
    }
    public partial class ClientBriefContentServices : BaseService<ClientBriefContentDto, ClientBriefContent, string>, IClientBriefContentServices
    {
        public ClientBriefContentServices(IEFRepository<ClientBriefContent, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<ClientBriefContentDto> ListAll(ClientBriefContentSearch filter)
        {

            GeneralResponseList<ClientBriefContentDto> resp = new GeneralResponseList<ClientBriefContentDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<ClientBriefContentDto> que = from tb1 in _repo.GetContext().Set<ClientBriefContent>()
                          select new ClientBriefContentDto()
                          {
                              Id = tb1.Id,
                              client_brief_id = tb1.client_brief_id,
                              file = tb1.file,
                              name = tb1.name,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "client_brief_id", filter.Search_client_brief_id);
                que = q.filterContains(que, "file", filter.Search_file);
                que = q.filterContains(que, "name", filter.Search_name);

                

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

