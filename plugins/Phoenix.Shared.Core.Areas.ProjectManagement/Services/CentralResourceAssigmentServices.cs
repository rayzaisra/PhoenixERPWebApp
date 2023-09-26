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
    public partial interface ICentralResourceAssigmentServices : IBaseService<CentralResourceAssigmentDto, CentralResourceAssigment, string>
    {
        GeneralResponseList<CentralResourceAssigmentDto> ListAll(CentralResourceAssigmentSearch filter);
    }
    public partial class CentralResourceAssigmentServices : BaseService<CentralResourceAssigmentDto, CentralResourceAssigment, string>, ICentralResourceAssigmentServices
    {
        public CentralResourceAssigmentServices(IEFRepository<CentralResourceAssigment, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<CentralResourceAssigmentDto> ListAll(CentralResourceAssigmentSearch filter)
        {

            GeneralResponseList<CentralResourceAssigmentDto> resp = new GeneralResponseList<CentralResourceAssigmentDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<CentralResourceAssigmentDto> que = from tb1 in _repo.GetContext().Set<CentralResourceAssigment>()
                          select new CentralResourceAssigmentDto()
                          {
                              Id = tb1.Id,
                              approval_status_id = tb1.approval_status_id,
                              central_resource_id = tb1.central_resource_id,
                              client_brief_id = tb1.client_brief_id,
                              jobnumber = tb1.jobnumber,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "approval_status_id", filter.Search_approval_status_id);
                que = q.filterEquals(que, "central_resource_id", filter.Search_central_resource_id);
                que = q.filterEquals(que, "client_brief_id", filter.Search_client_brief_id);
                que = q.filterContains(que, "jobnumber", filter.Search_jobnumber);

                

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

