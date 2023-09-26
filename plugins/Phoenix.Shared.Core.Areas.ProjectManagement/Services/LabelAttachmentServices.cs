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
    public partial interface ILabelAttachmentServices : IBaseService<LabelAttachmentDto, LabelAttachment, string>
    {
        GeneralResponseList<LabelAttachmentDto> ListAll(LabelAttachmentSearch filter);
    }
    public partial class LabelAttachmentServices : BaseService<LabelAttachmentDto, LabelAttachment, string>, ILabelAttachmentServices
    {
        public LabelAttachmentServices(IEFRepository<LabelAttachment, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<LabelAttachmentDto> ListAll(LabelAttachmentSearch filter)
        {

            GeneralResponseList<LabelAttachmentDto> resp = new GeneralResponseList<LabelAttachmentDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<LabelAttachmentDto> que = from tb1 in _repo.GetContext().Set<LabelAttachment>()
                          select new LabelAttachmentDto()
                          {
                              Id = tb1.Id,
                              attachement = tb1.attachement,
                              client_brief_id = tb1.client_brief_id,
                              description = tb1.description,
                              file_id = tb1.file_id,
                              label = tb1.label,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "attachement", filter.Search_attachement);
                que = q.filterEquals(que, "client_brief_id", filter.Search_client_brief_id);
                que = q.filterContains(que, "description", filter.Search_description);
                que = q.filterEquals(que, "file_id", filter.Search_file_id);
                que = q.filterContains(que, "label", filter.Search_label);

                

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

