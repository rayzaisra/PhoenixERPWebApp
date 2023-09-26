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
    public partial interface IInstitutionServices : IBaseService<InstitutionDto, Institution, string>
    {
        GeneralResponseList<InstitutionDto> ListAll(InstitutionSearch filter);
    }
    public partial class InstitutionServices : BaseService<InstitutionDto, Institution, string>, IInstitutionServices
    {
        public InstitutionServices(IEFRepository<Institution, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<InstitutionDto> ListAll(InstitutionSearch filter)
        {

            GeneralResponseList<InstitutionDto> resp = new GeneralResponseList<InstitutionDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<InstitutionDto> que = from tb1 in _repo.GetContext().Set<Institution>()
                          select new InstitutionDto()
                          {
                              Id = tb1.Id,
                              institution_address = tb1.institution_address,
                              institution_name = tb1.institution_name,
                              institution_type = tb1.institution_type
                              
                          };
                
                resp.RecordsTotal = que.Count();
                
                que = q.filterEquals(que, "institution_type", filter.Search_institution_type);
                que = q.filterContains(que, "institution_address", filter.Search_institution_address);
                que = q.filterContains(que, "institution_name", filter.Search_institution_name);

                

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

