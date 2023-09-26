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
    public partial interface IPmServices : IBaseService<PmDto, Pm, string>
    {
        GeneralResponseList<PmDto> ListAll(PmSearch filter);
    }
    public partial class PmServices : BaseService<PmDto, Pm, string>, IPmServices
    {
        public PmServices(IEFRepository<Pm, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<PmDto> ListAll(PmSearch filter)
        {

            GeneralResponseList<PmDto> resp = new GeneralResponseList<PmDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<PmDto> que = from tb1 in _repo.GetContext().Set<Pm>()
                          select new PmDto()
                          {
                              Id = tb1.Id,
                              user_id = tb1.user_id,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "user_id", filter.Search_user_id);

                

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

