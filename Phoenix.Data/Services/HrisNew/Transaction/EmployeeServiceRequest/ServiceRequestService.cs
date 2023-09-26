using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface IServiceRequestService : IDataServiceHris<EmployeeServiceRequest>
    {
        Task<dynamic> GetList();
        EmployeeServiceRequest GetById(string id);
    }

    public class ServiceRequestService : IServiceRequestService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        /// <summary>
        /// And endpoint to manage ServiceRequest
        /// </summary>
        /// <param name="context">Database context</param>
        public ServiceRequestService(DataContext context, GlobalFunctionApproval globalFcApproval)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
        }

        public Task<int> ApproveAsync(EmployeeServiceRequest entity)
        {
            throw new NotImplementedException();
        }

        public async Task<int> AddAsync(EmployeeServiceRequest entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            entity.RequestCode = await TransID.GetTransId(context, Code.ServiceRequest, DateTime.Today);
            entity.EmployeeBasicInfoId = context.GetEmployeeId();
            var requestBy = context.GetUserId();
            await context.PhoenixAddAsync(entity);
            var result =  await context.SaveChangesAsync();
            await globalFcApproval.AddNotification(NotificationTypeCode.ServiceRequestNotification, requestBy, NotificationModuleType.ModuleHRIS, entity.RequestCode, true, true,null );

            var userHcAdmin = await context.TmUserApps.Where(x => x.GroupId == GroupAcess.HCAdmin).AsNoTracking().ToListAsync();

            foreach (var item in userHcAdmin)
            {
                await globalFcApproval.AddNotification(NotificationTypeCode.ServiceRequestNotification, item.Id, NotificationModuleType.ModuleHRIS, entity.RequestCode, true, true, null);
            }

            var userAdmin = await context.TmUserApps.Where(x => x.GroupId == GroupAcess.SuperAdmin).AsNoTracking().ToListAsync();

            foreach (var item in userAdmin)
            {
                await globalFcApproval.AddNotification(NotificationTypeCode.ServiceRequestNotification, item.Id, NotificationModuleType.ModuleHRIS, entity.RequestCode, true, true, null);
            }
            return result;
        }

        public async Task<int> DeleteSoftAsync(EmployeeServiceRequest entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> EditAsync(EmployeeServiceRequest entity)
        {
            if (string.IsNullOrEmpty(entity.RequestCode))
            {
                entity.RequestCode = (context.EmployeeServiceRequests.Where(x => x.Id == entity.Id).FirstOrDefault() ?? new EmployeeServiceRequest()).RequestCode;
            }
            if (string.IsNullOrEmpty(entity.EmployeeBasicInfoId))
            {
                entity.EmployeeBasicInfoId = context.GetEmployeeId();
            }
            context.PhoenixEdit(entity);
            var requestBy = context.GetUserId();
            var result = await context.SaveChangesAsync();
            await globalFcApproval.AddNotification(NotificationTypeCode.ServiceRequestNotificationEdit, requestBy, NotificationModuleType.ModuleHRIS, entity.RequestCode, true, true, null);

            var userHcAdmin = await context.TmUserApps.Where(x => x.GroupId == GroupAcess.HCAdmin).AsNoTracking().ToListAsync();

            foreach (var item in userHcAdmin)
            {
                await globalFcApproval.AddNotification(NotificationTypeCode.ServiceRequestNotificationEdit, item.Id, NotificationModuleType.ModuleHRIS, entity.RequestCode, true, true, null);
            }

            var userAdmin = await context.TmUserApps.Where(x => x.GroupId == GroupAcess.SuperAdmin).AsNoTracking().ToListAsync();

            foreach (var item in userAdmin)
            {
                await globalFcApproval.AddNotification(NotificationTypeCode.ServiceRequestNotificationEdit, item.Id, NotificationModuleType.ModuleHRIS, entity.RequestCode, true, true, null);
            }
            return result;
        }
        public async Task<List<EmployeeServiceRequest>> Get() => await context.EmployeeServiceRequests.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).OrderByDescending(x=>x.RequestDate).ToListAsync();

        public async Task<EmployeeServiceRequest> Get(string Id)
        {
            return await context.EmployeeServiceRequests.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).OrderByDescending(x => x.RequestDate).FirstOrDefaultAsync();
        }

        public async Task<dynamic> GetList()
        {
            EmployeeServiceRequestService fullfillment = new EmployeeServiceRequestService(context);
            int[] status = new[] {   EmployeeServiceRequestStatus.Submit, EmployeeServiceRequestStatus.Requested,
                                     EmployeeServiceRequestStatus.Prosess, EmployeeServiceRequestStatus.Procurement,
                                     EmployeeServiceRequestStatus.Fullfillment, EmployeeServiceRequestStatus.Closed };
            return await fullfillment.GetList(status);

            //var sql = await (from a in context.EmployeeServiceRequests
            //                 join b in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals b.Id
            //                 join c in context.MasterMappingRequests on a.MasterMappingRequestId equals c.Id
            //                 join d in context.MasterDivisionSelfservices on c.MasterDivisionSelfserviceId equals d.Id
            //                 where a.IsDeleted == false
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
            //                     a.DateClose,
            //                 }).ToListAsync();
            //return sql;
        }

        public EmployeeServiceRequest GetById(string id)
        {
            EmployeeServiceRequestService servicerequest = new EmployeeServiceRequestService(context);
            return servicerequest.GetById(id);
        }
    }
}