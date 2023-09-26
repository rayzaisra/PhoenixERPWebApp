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
    public partial interface IChannelServices : IBaseService<ChannelDto, Channel, string>
    {
        GeneralResponseList<ChannelDto> ListAll(ChannelSearch filter);
    }
    public partial class ChannelServices : BaseService<ChannelDto, Channel, string>, IChannelServices
    {
        public ChannelServices(IEFRepository<Channel, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<ChannelDto> ListAll(ChannelSearch filter)
        {

            GeneralResponseList<ChannelDto> resp = new GeneralResponseList<ChannelDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<ChannelDto> que = from tb1 in _repo.GetContext().Set<Channel>()
                          select new ChannelDto()
                          {
                              Id = tb1.Id,
                              central_resource_id = tb1.central_resource_id,
                              channel = tb1.channel,
                              note = tb1.note,
                              value = tb1.value,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "central_resource_id", filter.Search_central_resource_id);
                que = q.filterContains(que, "channel", filter.Search_channel);
                que = q.filterContains(que, "note", filter.Search_note);
                que = q.filterEquals(que, "value", filter.Search_value);

                

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

