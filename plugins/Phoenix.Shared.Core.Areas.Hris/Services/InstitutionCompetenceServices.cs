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
    public partial interface IInstitutionCompetenceServices : IBaseService<InstitutionCompetenceDto, InstitutionCompetence, string>
    {
        GeneralResponseList<InstitutionCompetenceDto> ListAll(InstitutionCompetenceSearch filter);
    }
    public partial class InstitutionCompetenceServices : BaseService<InstitutionCompetenceDto, InstitutionCompetence, string>, IInstitutionCompetenceServices
    {
        public InstitutionCompetenceServices(IEFRepository<InstitutionCompetence, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<InstitutionCompetenceDto> ListAll(InstitutionCompetenceSearch filter)
        {

            GeneralResponseList<InstitutionCompetenceDto> resp = new GeneralResponseList<InstitutionCompetenceDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<InstitutionCompetenceDto> que = from tb1 in _repo.GetContext().Set<InstitutionCompetence>()
                          select new InstitutionCompetenceDto()
                          {
                              Id = tb1.Id,
                              competence_id = tb1.competence_id,
                              institution_id = tb1.institution_id,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "competence_id", filter.Search_competence_id);
                que = q.filterEquals(que, "institution_id", filter.Search_institution_id);

                

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

