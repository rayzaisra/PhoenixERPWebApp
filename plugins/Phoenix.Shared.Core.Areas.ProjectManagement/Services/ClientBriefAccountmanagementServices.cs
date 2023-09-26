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
    public partial interface IClientBriefAccountmanagementServices : IBaseService<ClientBriefAccountmanagementDto, ClientBriefAccountmanagement, string>
    {
        GeneralResponseList<ClientBriefAccountmanagementDto> ListAll(ClientBriefAccountmanagementSearch filter);
    }
    public partial class ClientBriefAccountmanagementServices : BaseService<ClientBriefAccountmanagementDto, ClientBriefAccountmanagement, string>, IClientBriefAccountmanagementServices
    {
        public ClientBriefAccountmanagementServices(IEFRepository<ClientBriefAccountmanagement, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<ClientBriefAccountmanagementDto> ListAll(ClientBriefAccountmanagementSearch filter)
        {

            GeneralResponseList<ClientBriefAccountmanagementDto> resp = new GeneralResponseList<ClientBriefAccountmanagementDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<ClientBriefAccountmanagementDto> que = from tb1 in _repo.GetContext().Set<ClientBriefAccountmanagement>()
                          select new ClientBriefAccountmanagementDto()
                          {
                              Id = tb1.Id,
                              User_id = tb1.user_id,
                              account_management_seq = tb1.account_management_seq,
                              client_brief_id = tb1.client_brief_id,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "account_management_id", filter.Search_User_id);
                que = q.filterEquals(que, "account_management_seq", filter.Search_account_management_seq);
                que = q.filterEquals(que, "client_brief_id", filter.Search_client_brief_id);

                

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

