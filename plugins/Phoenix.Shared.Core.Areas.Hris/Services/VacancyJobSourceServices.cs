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
    public partial interface IVacancyJobSourceServices : IBaseService<VacancyJobSourceDto, VacancyJobSource, string>
    {
        GeneralResponseList<VacancyJobSourceDto> ListAll(VacancyJobSourceSearch filter);
    }
    public partial class VacancyJobSourceServices : BaseService<VacancyJobSourceDto, VacancyJobSource, string>, IVacancyJobSourceServices
    {
        public VacancyJobSourceServices(IEFRepository<VacancyJobSource, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<VacancyJobSourceDto> ListAll(VacancyJobSourceSearch filter)
        {

            GeneralResponseList<VacancyJobSourceDto> resp = new GeneralResponseList<VacancyJobSourceDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<VacancyJobSourceDto> que = from tb1 in _repo.GetContext().Set<VacancyJobSource>()
                          select new VacancyJobSourceDto()
                          {
                              Id = tb1.Id,
                              file_id = tb1.file_id,
                              links = tb1.links,
                              source_vacancy = tb1.source_vacancy,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "file_id", filter.Search_file_id);
                que = q.filterContains(que, "links", filter.Search_links);
                que = q.filterContains(que, "source_vacancy", filter.Search_source_vacancy);

                

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

