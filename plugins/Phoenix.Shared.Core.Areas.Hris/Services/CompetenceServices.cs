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
    public partial interface ICompetenceServices : IBaseService<CompetenceDto, Competence, string>
    {
        GeneralResponseList<CompetenceDto> ListAll(CompetenceSearch filter);
    }
    public partial class CompetenceServices : BaseService<CompetenceDto, Competence, string>, ICompetenceServices
    {
        public CompetenceServices(IEFRepository<Competence, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<CompetenceDto> ListAll(CompetenceSearch filter)
        {

            GeneralResponseList<CompetenceDto> resp = new GeneralResponseList<CompetenceDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<CompetenceDto> que = from tb1 in _repo.GetContext().Set<Competence>()
                          select new CompetenceDto()
                          {
                              Id = tb1.Id,
                              competence_name = tb1.competence_name,
                              level1 = tb1.level1,
                              level2 = tb1.level2,
                              level3 = tb1.level3,
                              level4 = tb1.level4,
                              level5 = tb1.level5,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "competence_name", filter.Search_competence_name);
                que = q.filterContains(que, "level1", filter.Search_level1);
                que = q.filterContains(que, "level2", filter.Search_level2);
                que = q.filterContains(que, "level3", filter.Search_level3);
                que = q.filterContains(que, "level4", filter.Search_level4);
                que = q.filterContains(que, "level5", filter.Search_level5);

                

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

