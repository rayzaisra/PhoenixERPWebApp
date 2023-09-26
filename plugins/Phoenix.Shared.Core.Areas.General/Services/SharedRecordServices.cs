using Phoenix.Shared.Core.Areas.General.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Responses;
using System;
using System.Linq;


namespace Phoenix.Shared.Core.Areas.General.Services
{
    public partial interface ISharedRecordServices : IBaseService<SharedRecordDto, SharedRecord, string>
    {
        GeneralResponseList<SharedRecordDto> ListAll(SharedRecordSearch filter);
    }
    public partial class SharedRecordServices : BaseService<SharedRecordDto, SharedRecord, string>, ISharedRecordServices
    {
        public SharedRecordServices(IEFRepository<SharedRecord, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<SharedRecordDto> ListAll(SharedRecordSearch filter)
        {

            GeneralResponseList<SharedRecordDto> resp = new GeneralResponseList<SharedRecordDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<SharedRecordDto> que = from tb1 in _repo.GetContext().Set<SharedRecord>()
                          select new SharedRecordDto()
                          {
                              Id = tb1.Id,
                              application_entity_id = tb1.application_entity_id,
                              can_read = tb1.can_read,
                              can_write = tb1.can_write,
                              record_id = tb1.record_id,
                              shared_to = tb1.shared_to,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "application_entity_id", filter.Search_application_entity_id);
                que = q.filterEquals(que, "can_read", filter.Search_can_read);
                que = q.filterEquals(que, "can_write", filter.Search_can_write);
                que = q.filterEquals(que, "record_id", filter.Search_record_id);
                que = q.filterContains(que, "shared_to", filter.Search_shared_to);

                

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

