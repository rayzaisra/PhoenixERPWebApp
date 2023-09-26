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
    public partial interface IFamilyStatusServices : IBaseService<FamilyStatusDto, FamilyStatus, string>
    {
        GeneralResponseList<FamilyStatusDto> ListAll(FamilyStatusSearch filter);
    }
    public partial class FamilyStatusServices : BaseService<FamilyStatusDto, FamilyStatus, string>, IFamilyStatusServices
    {
        public FamilyStatusServices(IEFRepository<FamilyStatus, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<FamilyStatusDto> ListAll(FamilyStatusSearch filter)
        {

            GeneralResponseList<FamilyStatusDto> resp = new GeneralResponseList<FamilyStatusDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<FamilyStatusDto> que = from tb1 in _repo.GetContext().Set<FamilyStatus>()
                          select new FamilyStatusDto()
                          {
                              Id = tb1.Id,
                              family_status_name = tb1.family_status_name,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "family_status_name", filter.Search_family_status_name);

                

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

