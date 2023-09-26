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
    public partial interface IClientBriefCentralResourceServices : IBaseService<ClientBriefCentralResourceDto, ClientBriefCentralResource, string>
    {
        GeneralResponseList<ClientBriefCentralResourceDto> ListAll(ClientBriefCentralResourceSearch filter);
    }
    public partial class ClientBriefCentralResourceServices : BaseService<ClientBriefCentralResourceDto, ClientBriefCentralResource, string>, IClientBriefCentralResourceServices
    {
        public ClientBriefCentralResourceServices(IEFRepository<ClientBriefCentralResource, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<ClientBriefCentralResourceDto> ListAll(ClientBriefCentralResourceSearch filter)
        {

            GeneralResponseList<ClientBriefCentralResourceDto> resp = new GeneralResponseList<ClientBriefCentralResourceDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<ClientBriefCentralResourceDto> que = from tb1 in _repo.GetContext().Set<ClientBriefCentralResource>()
                          select new ClientBriefCentralResourceDto()
                          {
                              Id = tb1.Id,
                              central_resource_id = tb1.central_resource_id,
                              job_detail_id = tb1.job_detail_id,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "central_resource_id", filter.Search_central_resource_id);
                que = q.filterEquals(que, "job_detail_id", filter.Search_job_detail_id);

                

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

