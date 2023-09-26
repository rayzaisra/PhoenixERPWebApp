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
    public partial interface IClientBriefContentRefServices : IBaseService<ClientBriefContentRefDto, ClientBriefContentRef, string>
    {
        GeneralResponseList<ClientBriefContentRefDto> ListAll(ClientBriefContentRefSearch filter);
    }
    public partial class ClientBriefContentRefServices : BaseService<ClientBriefContentRefDto, ClientBriefContentRef, string>, IClientBriefContentRefServices
    {
        public ClientBriefContentRefServices(IEFRepository<ClientBriefContentRef, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<ClientBriefContentRefDto> ListAll(ClientBriefContentRefSearch filter)
        {

            GeneralResponseList<ClientBriefContentRefDto> resp = new GeneralResponseList<ClientBriefContentRefDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<ClientBriefContentRefDto> que = from tb1 in _repo.GetContext().Set<ClientBriefContentRef>()
                          select new ClientBriefContentRefDto()
                          {
                              Id = tb1.Id,
                              description = tb1.description,
                              name = tb1.name,
                              seq = tb1.seq,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "description", filter.Search_description);
                que = q.filterContains(que, "name", filter.Search_name);
                que = q.filterEquals(que, "seq", filter.Search_seq);

                

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

