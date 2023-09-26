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
    public partial interface IEmployeeWarningServices : IBaseService<EmployeeWarningDto, EmployeeWarning, string>
    {
        GeneralResponseList<EmployeeWarningDto> ListAll(EmployeeWarningSearch filter);
    }
    public partial class EmployeeWarningServices : BaseService<EmployeeWarningDto, EmployeeWarning, string>, IEmployeeWarningServices
    {
        public EmployeeWarningServices(IEFRepository<EmployeeWarning, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<EmployeeWarningDto> ListAll(EmployeeWarningSearch filter)
        {

            GeneralResponseList<EmployeeWarningDto> resp = new GeneralResponseList<EmployeeWarningDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<EmployeeWarningDto> que = from tb1 in _repo.GetContext().Set<EmployeeWarning>()
                          select new EmployeeWarningDto()
                          {
                              Id = tb1.Id,
                              description = tb1.description,
                              notes = tb1.notes,
                              subject = tb1.subject,
                              warning_by = tb1.warning_by,
                              warning_date = tb1.warning_date,
                              warning_to = tb1.warning_to,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "description", filter.Search_description);
                que = q.filterContains(que, "notes", filter.Search_notes);
                que = q.filterContains(que, "subject", filter.Search_subject);
                que = q.filterContains(que, "warning_by", filter.Search_warning_by);
                que = q.filterEquals(que, "warning_date", filter.Search_warning_date);
                que = q.filterContains(que, "warning_to", filter.Search_warning_to);

                

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

