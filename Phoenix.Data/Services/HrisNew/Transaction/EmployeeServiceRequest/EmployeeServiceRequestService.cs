using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public class EmployeeServiceRequestService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage EmployeeServiceRequest
        /// </summary>
        /// <param name="context">Database context</param>
        public EmployeeServiceRequestService(DataContext context)
        {
            this.context = context;
        }

        public async Task<dynamic> GetList(int[] status)
        {
            var sql = await (from a in context.EmployeeServiceRequests
                             join b in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals b.Id
                             join c in context.MasterMappingRequests on a.MasterMappingRequestId equals c.Id
                             join d in context.MasterDivisionSelfservices on c.MasterDivisionSelfserviceId equals d.Id
                             where !(a.IsDeleted ?? false) && (status == null ? true : status.Any(y => y == a.Status))
                             select new
                             {
                                 a.Id,
                                 a.RequestCode,
                                 RequestBy = b.NameEmployee,
                                 c.ServiceName,
                                 a.Remarks,
                                 d.BusinessUnitName,
                                 Date = a.CreatedOn,
                                 DivisionName = d.BusinessUnitName,
                                 a.DueDate,
                                 a.Status,
                                 a.DateClose,
                                 a.Rating
                             }).OrderByDescending(x=>x.Date).ToListAsync();
            return sql;
        }

        public EmployeeServiceRequest GetById(string id)
        {
            RequestorService request = new RequestorService(context);
            var employeeId = context.GetEmployeeId();
            var employeeservicerequest = (from a in context.EmployeeServiceRequests
                                          join b in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals b.Id
                                          join c in context.MasterMappingRequests on a.MasterMappingRequestId equals c.Id
                                          join d in context.MasterDivisionSelfservices on c.MasterDivisionSelfserviceId equals d.Id
                                          where !(a.IsDeleted ?? false) && a.Id == id //&& b.Id == employeeId
                                          select new EmployeeServiceRequest
                                          {
                                              Id = a.Id,
                                              RequestCode = a.RequestCode,
                                              RequestBy = a.RequestBy,
                                              ServiceRequestCatalogue = c.ServiceName,
                                              EmployeeBasicInfoId = a.EmployeeBasicInfoId,
                                              Description = a.Remarks,
                                              MasterMappingRequestId = a.MasterMappingRequestId,
                                              PIC = d.BusinessUnitName,
                                              CreatedOn = a.CreatedOn,
                                              ModifiedBy = a.ModifiedBy,
                                              ModifiedOn = a.ModifiedOn,
                                              ApprovedBy = a.ApprovedBy,
                                              ApprovedOn = a.ApprovedOn,
                                              IsActive = a.IsActive,
                                              IsLocked = a.IsLocked,
                                              IsDefault = a.IsDefault,
                                              IsDeleted = a.IsDeleted,
                                              OwnerId = a.OwnerId,
                                              DeletedBy = a.DeletedBy,
                                              DeletedOn = a.DeletedOn,
                                              BusinessUnitDivisionId = a.BusinessUnitDivisionId,
                                              BusinessUnitDepartementId = a.BusinessUnitDepartementId,
                                              RequestDate = a.RequestDate,
                                              DueDate = a.DueDate,
                                              Status = a.Status,
                                              DateClose = a.DateClose,
                                              Fullfilment = a.Fullfilment,
                                              FollowUpRemarks = a.FollowUpRemarks,
                                              Remarks = a.Remarks,
                                              Comment = a.Comment,
                                              Rating = a.Rating,
                                              requestEmployee = new InfoEmployee
                                              {
                                                  EmployeeId = b.Id,
                                                  EmployeeName = b.NameEmployee,
                                                  DateEmployed = b.JoinDate,
                                                  Phone = b.PhoneNumber,
                                                  Email = b.Email
                                              }
                                          }).FirstOrDefault();

            employeeservicerequest.requestEmployee = request.GetEmployeeInfo(employeeservicerequest.requestEmployee);
            return employeeservicerequest;
        }
    }
}