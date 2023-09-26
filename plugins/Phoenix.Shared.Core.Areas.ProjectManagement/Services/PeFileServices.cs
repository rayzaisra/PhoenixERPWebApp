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
    public partial interface IPeFileServices : IBaseService<PeFileDto, PeFile, string>
    {
        GeneralResponseList<PeFileDto> ListAll(PeFileSearch filter);
    }
    public partial class PeFileServices : BaseService<PeFileDto, PeFile, string>, IPeFileServices
    {
        public PeFileServices(IEFRepository<PeFile, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<PeFileDto> ListAll(PeFileSearch filter)
        {

            GeneralResponseList<PeFileDto> resp = new GeneralResponseList<PeFileDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<PeFileDto> que = from tb1 in _repo.GetContext().Set<PeFile>()
                          select new PeFileDto()
                          {
                              Id = tb1.Id,
                              file_name = tb1.file_name,
                              file_path = tb1.file_path,
                              file_size = tb1.file_size,
                              file_type = tb1.file_type,
                              flag = tb1.flag,
                              upload_date = tb1.upload_date,
                          };
                
                resp.RecordsTotal = que.Count();

                que = q.filterContains(que, "file_name", filter.Search_file_name);
                que = q.filterContains(que, "file_path", filter.Search_file_path);
                que = q.filterEquals(que, "file_size", filter.Search_file_size);
                que = q.filterContains(que, "file_type", filter.Search_file_type);
                que = q.filterEquals(que, "flag", filter.Search_flag);
                que = q.filterEquals(que, "upload_date", filter.Search_upload_date);

                

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

