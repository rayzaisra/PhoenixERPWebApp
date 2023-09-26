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
    public partial interface IMedicalClaimServices : IBaseService<MedicalClaimDto, MedicalClaim, string>
    {
        GeneralResponseList<MedicalClaimDto> ListAll(MedicalClaimSearch filter);
    }
    public partial class MedicalClaimServices : BaseService<MedicalClaimDto, MedicalClaim, string>, IMedicalClaimServices
    {
        public MedicalClaimServices(IEFRepository<MedicalClaim, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<MedicalClaimDto> ListAll(MedicalClaimSearch filter)
        {

            GeneralResponseList<MedicalClaimDto> resp = new GeneralResponseList<MedicalClaimDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<MedicalClaimDto> que = from tb1 in _repo.GetContext().Set<MedicalClaim>()
                          select new MedicalClaimDto()
                          {
                              Id = tb1.Id,
                              date_claim = tb1.date_claim,
                              employee_basic_info_id = tb1.employee_basic_info_id,
                              reason = tb1.reason,
                              value = tb1.value,
                              year_periode = tb1.year_periode,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "date_claim", filter.Search_date_claim);
                que = q.filterEquals(que, "employee_basic_info_id", filter.Search_employee_basic_info_id);
                que = q.filterContains(que, "reason", filter.Search_reason);
                que = q.filterEquals(que, "value", filter.Search_value);
                que = q.filterContains(que, "year_periode", filter.Search_year_periode);

                

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

