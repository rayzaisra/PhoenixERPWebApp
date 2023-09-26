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
    public partial interface IMasterSearchContentServices : IBaseService<MasterSearchContentDto, MasterSearchContent, string>
    {
        GeneralResponseList<MasterSearchContentDto> ListAll(MasterSearchContentSearch filter);
    }
    public partial class MasterSearchContentServices : BaseService<MasterSearchContentDto, MasterSearchContent, string>, IMasterSearchContentServices
    {
        public MasterSearchContentServices(IEFRepository<MasterSearchContent, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<MasterSearchContentDto> ListAll(MasterSearchContentSearch filter)
        {

            GeneralResponseList<MasterSearchContentDto> resp = new GeneralResponseList<MasterSearchContentDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<MasterSearchContentDto> que = from tb1 in _repo.GetContext().Set<MasterSearchContent>()
                          select new MasterSearchContentDto()
                          {
                              Id = tb1.Id,
                              created_date = tb1.created_date,
                              isdeleted = tb1.isdeleted,
                              screen = tb1.screen,
                              search_content = tb1.search_content,
                              updated_by = tb1.updated_by,
                              updated_date = tb1.updated_date,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "created_date", filter.Search_created_date);
                //que = q.filterEquals(que, "isdeleted", filter.Search_isdeleted);
                que = q.filterContains(que, "screen", filter.Search_screen);
                que = q.filterContains(que, "search_content", filter.Search_search_content);
                que = q.filterContains(que, "updated_by", filter.Search_updated_by);
                que = q.filterEquals(que, "updated_date", filter.Search_updated_date);

                

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

