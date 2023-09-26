using Phoenix.Shared.Core.Areas.Hris.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Responses;
using System;
using System.Linq;


namespace Phoenix.Shared.Core.Areas.Hris.Services
{
    public partial interface IMppApprovedStatusServices : IBaseService<MppApprovedStatusDto, MppApprovedStatus, string>
    {
        GeneralResponseList<MppApprovedStatusDto> ListAll(MppApprovedStatusSearch filter);
    }
    public partial class MppApprovedStatusServices : BaseService<MppApprovedStatusDto, MppApprovedStatus, string>, IMppApprovedStatusServices
    {
        public MppApprovedStatusServices(IEFRepository<MppApprovedStatus, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<MppApprovedStatusDto> ListAll(MppApprovedStatusSearch filter)
        {

            GeneralResponseList<MppApprovedStatusDto> resp = new GeneralResponseList<MppApprovedStatusDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<MppApprovedStatusDto> que = from tb1 in _repo.GetContext().Set<MppApprovedStatus>()
                          select new MppApprovedStatusDto()
                          {
                              Id = tb1.Id,
                              created_date = tb1.created_date,
                              id_mpp = tb1.id_mpp,
                              reason = tb1.reason,
                              updated_date = tb1.updated_date,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "created_date", filter.Search_created_date);
                que = q.filterContains(que, "id_mpp", filter.Search_id_mpp);
                que = q.filterContains(que, "reason", filter.Search_reason);
                que = q.filterEquals(que, "updated_date", filter.Search_updated_date);

                

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

