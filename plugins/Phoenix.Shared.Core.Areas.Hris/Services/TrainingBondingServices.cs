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
    public partial interface ITrainingBondingServices : IBaseService<TrainingBondingDto, TrainingBonding, string>
    {
        GeneralResponseList<TrainingBondingDto> ListAll(TrainingBondingSearch filter);
    }
    public partial class TrainingBondingServices : BaseService<TrainingBondingDto, TrainingBonding, string>, ITrainingBondingServices
    {
        public TrainingBondingServices(IEFRepository<TrainingBonding, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<TrainingBondingDto> ListAll(TrainingBondingSearch filter)
        {

            GeneralResponseList<TrainingBondingDto> resp = new GeneralResponseList<TrainingBondingDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<TrainingBondingDto> que = from tb1 in _repo.GetContext().Set<TrainingBonding>()
                                                     join tb2 in _repo.GetContext().Set<TrainingRequisition>() on tb1.training_req_id equals tb2.Id
                                                     join tb3 in _repo.GetContext().Set<Training>() on tb2.training_id equals tb3.Id
                                                     join tb4 in _repo.GetContext().Set<EmployeeBasicInfo>() on tb1.employee_basic_info_id equals tb4.Id
                          select new TrainingBondingDto()
                          {
                              Id = tb1.Id,
                              employee_basic_info_id = tb1.employee_basic_info_id,
                              employee_full_name = tb4.name_employee,
                              training_category = tb3.category,
                              training_cost = tb3.training_cost,
                              training_name = tb3.name,
                              training_req_id = tb1.training_req_id,
                              start_date = tb1.start_date,
                              end_date = tb1.end_date,
                              created_on = tb1.created_on
                          };
                
                resp.RecordsTotal = que.Count();

                que = q.filterEquals(que, "name", filter.Search_name);
                que = q.filterEquals(que, "category", filter.Search_category);
                que = q.filterEquals(que, "employee_full_name", filter.Search_employee_full_name);

                

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

