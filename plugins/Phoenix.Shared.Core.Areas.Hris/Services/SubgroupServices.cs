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
    public partial interface ISubgroupServices : IBaseService<SubgroupDto, Subgroup, string>
    {
        GeneralResponseList<SubgroupDto> ListAll(SubgroupSearch filter);
    }
    public partial class SubgroupServices : BaseService<SubgroupDto, Subgroup, string>, ISubgroupServices
    {
        public SubgroupServices(IEFRepository<Subgroup, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<SubgroupDto> ListAll(SubgroupSearch filter)
        {

            GeneralResponseList<SubgroupDto> resp = new GeneralResponseList<SubgroupDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<SubgroupDto> que = from tb1 in _repo.GetContext().Set<Subgroup>()
                          select new SubgroupDto()
                          {
                              Id = tb1.Id,
                              group_id = tb1.group_id,
                              subgroup_code = tb1.subgroup_code,
                              subgroup_name = tb1.subgroup_name,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "group_id", filter.Search_group_id);
                que = q.filterContains(que, "subgroup_code", filter.Search_subgroup_code);
                que = q.filterContains(que, "subgroup_name", filter.Search_subgroup_name);

                

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

