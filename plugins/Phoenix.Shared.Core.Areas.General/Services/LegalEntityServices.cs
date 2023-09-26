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
    public partial interface ILegalEntityServices : IBaseService<LegalEntityDto, LegalEntity, string>
    {
        GeneralResponseList<LegalEntityDto> ListAll(LegalEntitySearch filter);
    }
    public partial class LegalEntityServices : BaseService<LegalEntityDto, LegalEntity, string>, ILegalEntityServices
    {
        public LegalEntityServices(IEFRepository<LegalEntity, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<LegalEntityDto> ListAll(LegalEntitySearch filter)
        {

            GeneralResponseList<LegalEntityDto> resp = new GeneralResponseList<LegalEntityDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<LegalEntityDto> que = from tb1 in _repo.GetContext().Set<LegalEntity>()
                          select new LegalEntityDto()
                          {
                              Id = tb1.Id,
                              legal_entity_name = tb1.legal_entity_name
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "legal_entity_name", filter.Search_legal_entity_name);
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

