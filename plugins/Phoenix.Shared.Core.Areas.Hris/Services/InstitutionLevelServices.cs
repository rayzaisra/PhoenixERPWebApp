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
    public partial interface IInstitutionLevelServices : IBaseService<InstitutionLevelDto, InstitutionLevel, string>
    {
        GeneralResponseList<InstitutionLevelDto> ListAll(InstitutionLevelSearch filter);
    }
    public partial class InstitutionLevelServices : BaseService<InstitutionLevelDto, InstitutionLevel, string>, IInstitutionLevelServices
    {
        public InstitutionLevelServices(IEFRepository<InstitutionLevel, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<InstitutionLevelDto> ListAll(InstitutionLevelSearch filter)
        {

            GeneralResponseList<InstitutionLevelDto> resp = new GeneralResponseList<InstitutionLevelDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<InstitutionLevelDto> que = from tb1 in _repo.GetContext().Set<InstitutionLevel>()
                          select new InstitutionLevelDto()
                          {
                              Id = tb1.Id,
                              competence_id = tb1.competence_id,
                              level_position_id = tb1.level_position_id,
                              level_req = tb1.level_req,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "competence_id", filter.Search_competence_id);
                que = q.filterEquals(que, "level_position_id", filter.Search_level_position_id);
                que = q.filterContains(que, "level_req", filter.Search_level_req);

                

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

