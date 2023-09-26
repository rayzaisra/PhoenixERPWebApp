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
    public partial interface IDepartementServices : IBaseService<DepartementDto, Departement, string>
    {
        GeneralResponseList<DepartementDto> ListAll(DepartementSearch filter);
    }
    public partial class DepartementServices : BaseService<DepartementDto, Departement, string>, IDepartementServices
    {
        public DepartementServices(IEFRepository<Departement, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<DepartementDto> ListAll(DepartementSearch filter)
        {

            GeneralResponseList<DepartementDto> resp = new GeneralResponseList<DepartementDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<DepartementDto> que = from tb1 in _repo.GetContext().Set<Departement>()
                          select new DepartementDto()
                          {
                              Id = tb1.Id,
                              departement_code = tb1.departement_code,
                              departement_name = tb1.departement_name,
                              divisionid = tb1.divisionid,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "departement_code", filter.Search_departement_code);
                que = q.filterContains(que, "departement_name", filter.Search_departement_name);
                que = q.filterContains(que, "divisionid", filter.Search_divisionid);

                

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

