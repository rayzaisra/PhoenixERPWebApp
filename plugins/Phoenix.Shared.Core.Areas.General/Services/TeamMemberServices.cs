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
    public partial interface ITeamMemberServices : IBaseService<TeamMemberDto, TeamMember, string>
    {
        GeneralResponseList<TeamMemberDto> ListAll(TeamMemberSearch filter);
    }
    public partial class TeamMemberServices : BaseService<TeamMemberDto, TeamMember, string>, ITeamMemberServices
    {
        public TeamMemberServices(IEFRepository<TeamMember, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<TeamMemberDto> ListAll(TeamMemberSearch filter)
        {

            GeneralResponseList<TeamMemberDto> resp = new GeneralResponseList<TeamMemberDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<TeamMemberDto> que = from tb1 in _repo.GetContext().Set<TeamMember>()
                          select new TeamMemberDto()
                          {
                              Id = tb1.Id,
                              application_user_id = tb1.application_user_id,
                              team_id = tb1.team_id,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "application_user_id", filter.Search_application_user_id);
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

