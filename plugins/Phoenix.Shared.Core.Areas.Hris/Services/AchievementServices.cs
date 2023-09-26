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
    public partial interface IAchievementServices : IBaseService<AchievementDto, Achievement, string>
    {
        GeneralResponseList<AchievementDto> ListAll(AchievementSearch filter);
    }
    public partial class AchievementServices : BaseService<AchievementDto, Achievement, string>, IAchievementServices
    {
        public AchievementServices(IEFRepository<Achievement, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<AchievementDto> ListAll(AchievementSearch filter)
        {

            GeneralResponseList<AchievementDto> resp = new GeneralResponseList<AchievementDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<AchievementDto> que = from tb1 in _repo.GetContext().Set<Achievement>()
                          select new AchievementDto()
                          {
                              Id = tb1.Id,
                              achievement_date = tb1.achievement_date,
                              achievement_description = tb1.achievement_description,
                              achievement_title = tb1.achievement_title,
                              employee_basic_info_id = tb1.employee_basic_info_id,
                              notes = tb1.notes,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "achievement_date", filter.Search_achievement_date);
                que = q.filterContains(que, "achievement_description", filter.Search_achievement_description);
                que = q.filterContains(que, "achievement_title", filter.Search_achievement_title);
                que = q.filterEquals(que, "employee_basic_info_id", filter.Search_employee_basic_info_id);
                que = q.filterContains(que, "notes", filter.Search_notes);

                

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

