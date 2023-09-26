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
    public partial interface IAffiliationServices : IBaseService<AffiliationDto, Affiliation, string>
    {
        GeneralResponseList<AffiliationDto> ListAll(AffiliationSearch filter);
    }
    public partial class AffiliationServices : BaseService<AffiliationDto, Affiliation, string>, IAffiliationServices
    {
        public AffiliationServices(IEFRepository<Affiliation, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<AffiliationDto> ListAll(AffiliationSearch filter)
        {

            GeneralResponseList<AffiliationDto> resp = new GeneralResponseList<AffiliationDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<AffiliationDto> que = from tb1 in _repo.GetContext().Set<Affiliation>()
                          select new AffiliationDto()
                          {
                              Id = tb1.Id,
                              affiliation_name = tb1.affiliation_name
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "affiliation_name", filter.Search_affiliation_name);
                que = q.filterEquals(que, "organization_id", filter.Search_organization_id);

                

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

