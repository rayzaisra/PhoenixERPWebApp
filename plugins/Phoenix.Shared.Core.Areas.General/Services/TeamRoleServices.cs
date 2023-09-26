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
    public partial interface ITeamRoleServices : IBaseService<TeamRoleDto, TeamRole, string>
    {
        GeneralResponseList<TeamRoleDto> ListAll(TeamRoleSearch filter);
    }
    public partial class TeamRoleServices : BaseService<TeamRoleDto, TeamRole, string>, ITeamRoleServices
    {
        public TeamRoleServices(IEFRepository<TeamRole, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<TeamRoleDto> ListAll(TeamRoleSearch filter)
        {

            GeneralResponseList<TeamRoleDto> resp = new GeneralResponseList<TeamRoleDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<TeamRoleDto> que = from tb1 in _repo.GetContext().Set<TeamRole>()
                          select new TeamRoleDto()
                          {
                              Id = tb1.Id,
                              application_role_id = tb1.application_role_id,
                              team_id = tb1.team_id,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "application_role_id", filter.Search_application_role_id);
                que = q.filterEquals(que, "team_id", filter.Search_team_id);

                

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

