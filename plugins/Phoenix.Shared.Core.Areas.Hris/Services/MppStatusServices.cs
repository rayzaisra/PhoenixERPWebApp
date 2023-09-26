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
    public partial interface IMppStatusServices : IBaseService<MppStatusDto, MppStatus, string>
    {
        GeneralResponseList<MppStatusDto> ListAll(MppStatusSearch filter);
    }
    public partial class MppStatusServices : BaseService<MppStatusDto, MppStatus, string>, IMppStatusServices
    {
        public MppStatusServices(IEFRepository<MppStatus, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<MppStatusDto> ListAll(MppStatusSearch filter)
        {

            GeneralResponseList<MppStatusDto> resp = new GeneralResponseList<MppStatusDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<MppStatusDto> que = from tb1 in _repo.GetContext().Set<MppStatus>()
                          select new MppStatusDto()
                          {
                              Id = tb1.Id,
                              name = tb1.name,
                          };
                
                resp.RecordsTotal = que.Count();

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

