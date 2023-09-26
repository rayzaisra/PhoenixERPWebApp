using Phoenix.Shared.Core.Areas.General.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Helpers;
using Phoenix.Shared.Responses;
using server.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Phoenix.Shared.Core.Areas.General.Services
{
    public partial interface INotificationServices : IBaseService<NotificationDto, Notification, string>
    {
 
        GeneralResponseList<NotificationDto> ListAll(NotificationFilter filter);
        BaseGeneralResponse Seen(List<string> id);
        BaseGeneralResponse Unseen(List<string> id);
        GeneralResponse<NotificationDto> GetNotification(string id);
    }
    public partial class NotificationServices : BaseService<NotificationDto, Notification, string>, INotificationServices
    {
        public NotificationServices(IEFRepository<Notification, string> repo ) : base(repo)
        {
            _repo = repo;
        }

        public GeneralResponse<NotificationDto> GetNotification(string id)
        {
            GeneralResponse<NotificationDto> resp = new GeneralResponse<NotificationDto>();
            try
            {
                IQueryable<NotificationDto> que = from tb1 in _repo.GetContext().Set<Notification>()
                                              join tb2 in _repo.GetContext().Set<NotificationType>()
                                              on tb1.notification_type_id equals tb2.Id
                                              where tb1.Id == id
                                              select new NotificationDto()
                                              {
                                                  Id = tb1.Id,
                                                  href = tb2.href.Replace("{id}",tb1.item_id),
                                                  is_seen = tb1.is_seen,
                                                  item_id = tb1.item_id,
                                                  longtext = tb2.longtext,
                                                  shorttext = tb2.shorttext,
                                                  created_on = tb1.created_on
                                              };
                resp.Success = true;
                resp.Data = que.FirstOrDefault();
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;

        }

        public GeneralResponseList<NotificationDto> ListAll(NotificationFilter filter)
        {

            GeneralResponseList<NotificationDto> resp = new GeneralResponseList<NotificationDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<NotificationDto> que = from tb1 in _repo.GetContext().Set<Notification>()
                                                  join tb2 in _repo.GetContext().Set<NotificationType>()
                                                  on tb1.notification_type_id equals tb2.Id
                                                  join tb3 in _repo.GetContext().Set<TmUserApp>() on tb1.created_by equals tb3.EmployeeBasicInfoId
                                                  where tb1.user_id == Glosing.Instance.Username
                                                  select new NotificationDto()
                                                  {
                                                      Id = tb1.Id,
                                                      href = tb2.href.Replace("{id}", tb1.item_id),
                                                      is_seen = tb1.is_seen,
                                                      item_id = tb1.item_id,
                                                      longtext = tb2.longtext,
                                                      shorttext = tb2.shorttext,
                                                      date_on = tb1.created_on.Value.ToString("dd MMM yyyy"),
                                                      Module = tb1.module,
                                                      CreatedBy = tb3.AliasName
                                                  };
                
                resp.RecordsTotal = que.Count();

                que = q.filterEquals(que, "is_seen", filter.search_is_seen);
              

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

        public BaseGeneralResponse Seen(List<string> id)
        {
            throw new NotImplementedException();
        }

        public BaseGeneralResponse Unseen(List<string> id)
        {
            throw new NotImplementedException();
        }
    }
}

