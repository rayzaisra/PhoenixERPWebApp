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
    public partial interface ITrUserApprovalServices : IBaseService<TrUserApprovalDto, TrUserApproval, string>
    {
        GeneralResponseList<TrUserApprovalDto> ListAll(SearchTrUserApproval filter);
    }
    public partial class TrUserApprovalServices : BaseService<TrUserApprovalDto, TrUserApproval, string>, ITrUserApprovalServices
    {
        public TrUserApprovalServices(IEFRepository<TrUserApproval, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<TrUserApprovalDto> ListAll(SearchTrUserApproval filter)
        {

            GeneralResponseList<TrUserApprovalDto> resp = new GeneralResponseList<TrUserApprovalDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<TrUserApprovalDto> que = from tb1 in _repo.GetContext().Set<TrainingRequisition>()
                          select new TrUserApprovalDto()
                          {
                              Id = tb1.Id,
                             
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

