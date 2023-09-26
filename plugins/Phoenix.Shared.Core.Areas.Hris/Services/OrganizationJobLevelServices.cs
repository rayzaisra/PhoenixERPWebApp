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
    public partial interface IOrganizationJobLevelServices : IBaseService<OrganizationJobLevelDto, OrganizationJobLevel, string>
    {
        GeneralResponseList<OrganizationJobLevelDto> ListAll(OrganizationJobLevelSearch filter);
    }
    public partial class OrganizationJobLevelServices : BaseService<OrganizationJobLevelDto, OrganizationJobLevel, string>, IOrganizationJobLevelServices
    {
        public OrganizationJobLevelServices(IEFRepository<OrganizationJobLevel, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<OrganizationJobLevelDto> ListAll(OrganizationJobLevelSearch filter)
        {

            GeneralResponseList<OrganizationJobLevelDto> resp = new GeneralResponseList<OrganizationJobLevelDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<OrganizationJobLevelDto> que = from tb1 in _repo.GetContext().Set<OrganizationJobLevel>()
                          select new OrganizationJobLevelDto()
                          {
                              Id = tb1.Id,
                              business_unit_id = tb1.business_unit_id,
                              job_level_id = tb1.job_level_id,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "business_unit_id", filter.Search_business_unit_id);
                que = q.filterEquals(que, "job_level_id", filter.Search_job_level_id);

                

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

