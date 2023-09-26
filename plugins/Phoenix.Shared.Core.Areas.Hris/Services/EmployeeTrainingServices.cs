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
    public partial interface IEmployeeTrainingServices : IBaseService<EmployeeTrainingDto, EmployeeTraining, string>
    {
        GeneralResponseList<EmployeeTrainingDto> ListAll(EmployeeTrainingSearch filter);
    }
    public partial class EmployeeTrainingServices : BaseService<EmployeeTrainingDto, EmployeeTraining, string>, IEmployeeTrainingServices
    {
        public EmployeeTrainingServices(IEFRepository<EmployeeTraining, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<EmployeeTrainingDto> ListAll(EmployeeTrainingSearch filter)
        {

            GeneralResponseList<EmployeeTrainingDto> resp = new GeneralResponseList<EmployeeTrainingDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<EmployeeTrainingDto> que = from tb1 in _repo.GetContext().Set<EmployeeTraining>()
                                                      join tb2 in _repo.GetContext().Set<Institution>() on tb1.institution_id equals tb2.Id
                          select new EmployeeTrainingDto()
                          {
                              Id = tb1.Id,
                              address = tb1.address,
                              description = tb1.description,
                              employee_basic_info_id = tb1.employee_basic_info_id,
                              end_training = tb1.end_training,
                              expired_certificate = tb1.expired_certificate.ToString(),
                              filemaster_id = tb1.filemaster_id,
                              institution_name = tb2.institution_name,
                              name_training = tb1.name_training,
                              no_certification = tb1.no_certification,
                              start_training = tb1.start_training,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "address", filter.Search_address);
                que = q.filterContains(que, "description", filter.Search_description);
                que = q.filterEquals(que, "employee_basic_info_id", filter.Search_employee_basic_info_id);
                que = q.filterContains(que, "end_training", filter.Search_end_training);
                que = q.filterEquals(que, "expired_certificate", filter.Search_expired_certificate);
                que = q.filterContains(que, "file_upload", filter.Search_file_upload);
                que = q.filterEquals(que, "institusion_id", filter.Search_institusion_id);
                que = q.filterContains(que, "name_training", filter.Search_name_training);
                que = q.filterContains(que, "no_certification", filter.Search_no_certification);
                que = q.filterContains(que, "start_training", filter.Search_start_training);

                

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

