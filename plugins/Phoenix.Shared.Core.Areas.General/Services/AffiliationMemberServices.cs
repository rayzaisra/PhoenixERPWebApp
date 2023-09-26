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
    public partial interface IAffiliationMemberServices : IBaseService<AffiliationMemberDto, AffiliationMember, string>
    {
        GeneralResponseList<AffiliationMemberDto> ListAll(AffiliationMemberSearch filter);
    }
    public partial class AffiliationMemberServices : BaseService<AffiliationMemberDto, AffiliationMember, string>, IAffiliationMemberServices
    {
        public AffiliationMemberServices(IEFRepository<AffiliationMember, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<AffiliationMemberDto> ListAll(AffiliationMemberSearch filter)
        {

            GeneralResponseList<AffiliationMemberDto> resp = new GeneralResponseList<AffiliationMemberDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<AffiliationMemberDto> que = from tb1 in _repo.GetContext().Set<AffiliationMember>()
                          select new AffiliationMemberDto()
                          {
                              Id = tb1.Id,
                              affiliation_id = tb1.affiliation_id,
                              business_unit_id = tb1.business_unit_id,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "affiliation_id", filter.Search_affiliation_id);
                que = q.filterEquals(que, "business_unit_id", filter.Search_business_unit_id);

                

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

