using Phoenix.Shared.Core.Areas.ProjectManagement.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Responses;
using System;
using System.Linq;


namespace Phoenix.Shared.Core.Areas.ProjectManagement.Services
{
    public partial interface IReportFileServices : IBaseService<ReportFileDto, ReportFile, string>
    {
        GeneralResponseList<ReportFileDto> ListAll(ReportFileSearch filter);
    }
    public partial class ReportFileServices : BaseService<ReportFileDto, ReportFile, string>, IReportFileServices
    {
        public ReportFileServices(IEFRepository<ReportFile, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<ReportFileDto> ListAll(ReportFileSearch filter)
        {

            GeneralResponseList<ReportFileDto> resp = new GeneralResponseList<ReportFileDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<ReportFileDto> que = from tb1 in _repo.GetContext().Set<ReportFile>()
                          select new ReportFileDto()
                          {
                              Id = tb1.Id,
                              client_brief_id = tb1.client_brief_id,
                              file_id = tb1.file_id,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "client_brief_id", filter.Search_client_brief_id);
                que = q.filterEquals(que, "file_id", filter.Search_file_id);

                

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

