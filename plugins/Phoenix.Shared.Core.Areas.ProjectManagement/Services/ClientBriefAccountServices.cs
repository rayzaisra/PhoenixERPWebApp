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
    public partial interface IClientBriefAccountServices : IBaseService<ClientBriefAccountDto, ClientBriefAccount, string>
    {
        GeneralResponseList<ClientBriefAccountDto> ListAll(ClientBriefAccountSearch filter);
    }
    public partial class ClientBriefAccountServices : BaseService<ClientBriefAccountDto, ClientBriefAccount, string>, IClientBriefAccountServices
    {
        public ClientBriefAccountServices(IEFRepository<ClientBriefAccount, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<ClientBriefAccountDto> ListAll(ClientBriefAccountSearch filter)
        {

            GeneralResponseList<ClientBriefAccountDto> resp = new GeneralResponseList<ClientBriefAccountDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<ClientBriefAccountDto> que = from tb1 in _repo.GetContext().Set<ClientBriefAccount>()
                          select new ClientBriefAccountDto()
                          {
                              Id = tb1.Id,
                              account_management_id = tb1.account_management_id,
                              approval_status_id = tb1.approval_status_id,
                              client_brief_id = tb1.client_brief_id,
                              job_posting_requisition_id = tb1.job_posting_requisition_id,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "account_management_id", filter.Search_account_management_id);
                que = q.filterEquals(que, "approval_status_id", filter.Search_approval_status_id);
                que = q.filterEquals(que, "client_brief_id", filter.Search_client_brief_id);
                que = q.filterEquals(que, "job_posting_requisition_id", filter.Search_job_posting_requisition_id);

                

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

