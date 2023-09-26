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
    public partial interface IResignationServices : IBaseService<ResignationDto, Resignation, string>
    {
        GeneralResponseList<ResignationDto> ListAll(ResignationSearch filter);
    }
    public partial class ResignationServices : BaseService<ResignationDto, Resignation, string>, IResignationServices
    {
        public ResignationServices(IEFRepository<Resignation, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<ResignationDto> ListAll(ResignationSearch filter)
        {

            GeneralResponseList<ResignationDto> resp = new GeneralResponseList<ResignationDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<ResignationDto> que = from tb1 in _repo.GetContext().Set<Resignation>()
                          select new ResignationDto()
                          {
                              Id = tb1.Id,
                              employee_basic_info_id = tb1.employee_basic_info_id,
                              notes = tb1.notes,
                              notice_date = tb1.notice_date,
                              resignation_date = tb1.resignation_date,
                              resignation_reason = tb1.resignation_reason,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "employee_basic_info_id", filter.Search_employee_basic_info_id);
                que = q.filterContains(que, "notes", filter.Search_notes);
                que = q.filterEquals(que, "notice_date", filter.Search_notice_date);
                que = q.filterEquals(que, "resignation_date", filter.Search_resignation_date);
                que = q.filterContains(que, "resignation_reason", filter.Search_resignation_reason);

                

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

