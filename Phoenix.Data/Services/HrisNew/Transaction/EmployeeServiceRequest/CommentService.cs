using Microsoft.EntityFrameworkCore;

using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Data
{
    public interface ICommentService : IDataServiceHris<EmployeeServiceRequest>
    {
        Task<dynamic> GetList();
        EmployeeServiceRequest GetById(string id);
    }
    public class CommentService : ICommentService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        /// <summary>
        /// And endpoint to manage Fullfillment
        /// </summary>
        /// <param name="context">Database context</param>
        public CommentService(DataContext context, GlobalFunctionApproval globalFcApproval)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
        }

        public Task<int> ApproveAsync(EmployeeServiceRequest entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddAsync(EmployeeServiceRequest entity)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(EmployeeServiceRequest entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> EditAsync(EmployeeServiceRequest entity)
        {
            context.PhoenixEdit(entity);
            var requestBy = context.GetUserId();
            var result = await context.SaveChangesAsync();
            await globalFcApproval.AddNotification(NotificationTypeCode.CommentNotificationEdit, requestBy, NotificationModuleType.ModuleHRIS, entity.RequestCode, true, true, null);

            var userHcAdmin = await context.TmUserApps.Where(x => x.GroupId == GroupAcess.HCAdmin).AsNoTracking().ToListAsync();

            foreach (var item in userHcAdmin)
            {
                await globalFcApproval.AddNotification(NotificationTypeCode.CommentNotificationEdit, item.Id, NotificationModuleType.ModuleHRIS, entity.RequestCode, true, true, null);
            }

            var userAdmin = await context.TmUserApps.Where(x => x.GroupId == GroupAcess.SuperAdmin).AsNoTracking().ToListAsync();

            foreach (var item in userAdmin)
            {
                await globalFcApproval.AddNotification(NotificationTypeCode.CommentNotificationEdit, item.Id, NotificationModuleType.ModuleHRIS, entity.RequestCode, true, true, null);
            }
            return result;
        }

        public async Task<List<EmployeeServiceRequest>> Get() => await context.EmployeeServiceRequests.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).OrderByDescending(x=>x.RequestDate).ToListAsync();

        public async Task<EmployeeServiceRequest> Get(string Id) => await context.EmployeeServiceRequests.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).OrderByDescending(x => x.RequestDate).FirstOrDefaultAsync();

        public EmployeeServiceRequest GetById(string id)
        {
            EmployeeServiceRequestService fullfillment = new EmployeeServiceRequestService(context);
            return fullfillment.GetById(id);
        }

        public async Task<dynamic> GetList()
        {
            EmployeeServiceRequestService fullfillment = new EmployeeServiceRequestService(context);
            int[] status = new[] { EmployeeServiceRequestStatus.Fullfillment, EmployeeServiceRequestStatus.Closed };
            return await fullfillment.GetList(status);

            //var sql = await (from a in context.EmployeeServiceRequests
            //                 join b in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals b.Id
            //                 join c in context.MasterMappingRequests on a.MasterMappingRequestId equals c.Id
            //                 join d in context.MasterDivisionSelfservices on c.MasterDivisionSelfserviceId equals d.Id
            //                 where a.IsDeleted == false && a.Status == 4 || a.Status == 5
            //                 select new
            //                 {
            //                     a.Id,
            //                     a.RequestCode,
            //                     RequestBy = b.NameEmployee,
            //                     c.ServiceName,
            //                     a.Remarks,
            //                     d.DivisionName,
            //                     Date = a.CreatedOn,
            //                     a.DueDate,
            //                     a.Status,
            //                     a.DateClose
            //                 }).ToListAsync();
            //return sql;
        }
    }
}