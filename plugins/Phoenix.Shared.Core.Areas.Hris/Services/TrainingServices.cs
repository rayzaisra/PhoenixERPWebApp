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
    public partial interface ITrainingServices : IBaseService<TrainingDto, Training, string>
    {
        GeneralResponseList<TrainingDto> ListAll(TrainingSearch filter);
    }
    public partial class TrainingServices : BaseService<TrainingDto, Training, string>, ITrainingServices
    {
        public TrainingServices(IEFRepository<Training, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<TrainingDto> ListAll(TrainingSearch filter)
        {

            GeneralResponseList<TrainingDto> resp = new GeneralResponseList<TrainingDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<TrainingDto> que = from tb1 in _repo.GetContext().Set<Training>()
                          select new TrainingDto()
                          {
                              Id = tb1.Id,
                              name = tb1.name,
                              category = tb1.category,
                              bonding_detail = tb1.bonding_detail,
                              description = tb1.description,
                              recruitment = tb1.recruitment,
                              subject = tb1.subject,
                              target_audiance = tb1.target_audiance,
                              training_cost = tb1.training_cost,
                              created_on = tb1.created_on
                          };
                
                resp.RecordsTotal = que.Count();

                que = q.filterEquals(que, "name", filter.Search_name);
                que = q.filterEquals(que, "category", filter.Search_category);
                que = q.filterEquals(que, "target_audience", filter.Search_target_audience);

                

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

