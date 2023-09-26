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
    public partial interface ILevelPositionServices : IBaseService<LevelPositionDto, LevelPosition, string>
    {
        GeneralResponseList<LevelPositionDto> ListAll(LevelPositionSearch filter);
    }
    public partial class LevelPositionServices : BaseService<LevelPositionDto, LevelPosition, string>, ILevelPositionServices
    {
        public LevelPositionServices(IEFRepository<LevelPosition, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<LevelPositionDto> ListAll(LevelPositionSearch filter)
        {

            GeneralResponseList<LevelPositionDto> resp = new GeneralResponseList<LevelPositionDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<LevelPositionDto> que = from tb1 in _repo.GetContext().Set<LevelPosition>()
                          select new LevelPositionDto()
                          {
                              Id = tb1.Id,
                              position_level = tb1.position_level,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "position_level", filter.Search_position_level);

                

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

