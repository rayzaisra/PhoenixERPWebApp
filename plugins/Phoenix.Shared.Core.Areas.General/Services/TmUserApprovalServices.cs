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
    public partial interface ITmUserApprovalServices : IBaseService<TmUserApprovalDto, TmUserApproval, string>
    {
        GeneralResponseList<TmUserApprovalDto> ListAll(SearchTmUserApproval filter);
    }
    public partial class TmUserApprovalServices : BaseService<TmUserApprovalDto, TmUserApproval, string>, ITmUserApprovalServices
    {
        public TmUserApprovalServices(IEFRepository<TmUserApproval, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<TmUserApprovalDto> ListAll(SearchTmUserApproval filter)
        {

            GeneralResponseList<TmUserApprovalDto> resp = new GeneralResponseList<TmUserApprovalDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<TmUserApprovalDto> que = from tb1 in _repo.GetContext().Set<TmUserApproval>()
                          select new TmUserApprovalDto()
                          {
                              Id = tb1.Id
                          };
                
                resp.RecordsTotal = que.Count();

                //que = q.filterEquals(que, "end_date", filter.Search_end_date);
                

                

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

