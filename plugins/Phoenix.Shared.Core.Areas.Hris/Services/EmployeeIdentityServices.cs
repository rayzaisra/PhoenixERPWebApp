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
    public partial interface IEmployeeIdentityServices : IBaseService<EmployeeIdentityDto, EmployeeIdentity, string>
    {
        GeneralResponseList<EmployeeIdentityDto> ListAll(EmployeeIdentitySearch filter);
    }
    public partial class EmployeeIdentityServices : BaseService<EmployeeIdentityDto, EmployeeIdentity, string>, IEmployeeIdentityServices
    {
        public EmployeeIdentityServices(IEFRepository<EmployeeIdentity, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<EmployeeIdentityDto> ListAll(EmployeeIdentitySearch filter)
        {

            GeneralResponseList<EmployeeIdentityDto> resp = new GeneralResponseList<EmployeeIdentityDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<EmployeeIdentityDto> que = from tb1 in _repo.GetContext().Set<EmployeeIdentity>()
                          select new EmployeeIdentityDto()
                          {
                              Id = tb1.Id,
                              employee_basic_info_id = tb1.employee_basic_info_id,
                              expired = tb1.expired,
                              file_upload = tb1.file_upload,
                              name_card = tb1.name_card,
                              no_card = tb1.no_card,
                              published = tb1.published,
                              published_by = tb1.published_by,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "employee_basic_info_id", filter.Search_employee_basic_info_id);
                que = q.filterEquals(que, "expired", filter.Search_expired);
                que = q.filterContains(que, "file_upload", filter.Search_file_upload);
                que = q.filterContains(que, "name_card", filter.Search_name_card);
                que = q.filterContains(que, "no_card", filter.Search_no_card);
                que = q.filterEquals(que, "published", filter.Search_published);
                que = q.filterContains(que, "published_by", filter.Search_published_by);

                

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

