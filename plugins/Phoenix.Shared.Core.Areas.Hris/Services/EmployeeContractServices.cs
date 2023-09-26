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
    public partial interface IEmployeeContractServices : IBaseService<EmployeeContractDto, EmployeeContract, string>
    {
        GeneralResponseList<EmployeeContractDto> ListAll(EmployeeContractSearch filter);
    }
    public partial class EmployeeContractServices : BaseService<EmployeeContractDto, EmployeeContract, string>, IEmployeeContractServices
    {
        public EmployeeContractServices(IEFRepository<EmployeeContract, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<EmployeeContractDto> ListAll(EmployeeContractSearch filter)
        {

            GeneralResponseList<EmployeeContractDto> resp = new GeneralResponseList<EmployeeContractDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<EmployeeContractDto> que = from tb1 in _repo.GetContext().Set<EmployeeContract>()
                          select new EmployeeContractDto()
                          {
                              Id = tb1.Id,
                              active = tb1.active,
                              candidate_recruitment_id = tb1.candidate_recruitment_id,
                              contract_upload = tb1.contract_upload,
                              employee_basic_info_id = tb1.employee_basic_info_id,
                              end_date = tb1.end_date,
                              notes = tb1.notes,
                              start_date = tb1.start_date,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "active", filter.Search_active);
                que = q.filterEquals(que, "candidate_recruitment_id", filter.Search_candidate_recruitment_id);
                que = q.filterContains(que, "contract_upload", filter.Search_contract_upload);
                que = q.filterEquals(que, "employee_basic_info_id", filter.Search_employee_basic_info_id);
                que = q.filterEquals(que, "end_date", filter.Search_end_date);
                que = q.filterContains(que, "notes", filter.Search_notes);
                que = q.filterEquals(que, "start_date", filter.Search_start_date);

                

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

