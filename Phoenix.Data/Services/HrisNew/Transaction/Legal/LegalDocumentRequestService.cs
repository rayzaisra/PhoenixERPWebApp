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
    public interface ILegalDocumentRequestService : IDataServiceHris<LegalDocumentRequest>
    {
    }

    public class LegalDocumentRequestService : ILegalDocumentRequestService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;

        string idTemplate = "";

        /// <summary>
        /// And endpoint to manage LegalDocumentRequest
        /// </summary>
        /// <param name="context">Database context</param>
        public LegalDocumentRequestService(DataContext context, GlobalFunctionApproval globalFcApproval)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
        }

        public async Task<int> AddAsync(LegalDocumentRequest entity)
        {
            if (string.IsNullOrEmpty(entity.Id))
            {
                entity.Id = await TransID.GetTransId(context, Code.LegalDocument, DateTime.Today);
            }
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> EditAsync(LegalDocumentRequest entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteSoftAsync(LegalDocumentRequest entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<List<LegalDocumentRequest>> Get()
        {
            var employeeId = context.GetEmployeeId();
            var result = await (from a in context.LegalDocumentRequests.Include(x => x.LegalCategory)
                                join b in context.EmployeeBasicInfos on a.CreatedBy equals b.Id
                                join c in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals c.Id
                                join e in context.TrTemplateApprovals on a.Id equals e.RefId
                                where !(a.IsDeleted ?? false) 
                                select new LegalDocumentRequest
                                {
                                    Id = a.Id,
                                    StatusDesc = e.StatusApprovedDescription,
                                    DueDate = a.DueDate,
                                    LegalDocFile = a.LegalDocFile,
                                    Remarks = a.Remarks,
                                    SubmitUser = a.SubmitUser,
                                    RequestBy = b.NameEmployee,
                                    LegalCategory = a.LegalCategory,
                                    Counterpart = c.NameEmployee,
                                    CreatedOn = a.CreatedOn
                                }).OrderByDescending(x => x.CreatedOn).ToListAsync();


            var result2 = await (from a in context.LegalDocumentRequests.Include(x => x.LegalCategory)
                                 join b in context.EmployeeBasicInfos on a.CreatedBy equals b.Id
                                 join c in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals c.Id
                                 where !(a.IsDeleted ?? false) && !context.TrTemplateApprovals.Any(x => x.RefId == a.Id)
                                 
                                 select new LegalDocumentRequest
                                 {
                                     Id = a.Id,
                                     StatusDesc = StatusTransaction.StatusName(StatusTransaction.Draft),
                                     DueDate = a.DueDate,
                                     LegalDocFile = a.LegalDocFile,
                                     Remarks = a.Remarks,
                                     SubmitUser = a.SubmitUser,
                                     RequestBy = b.NameEmployee,
                                     LegalCategory = a.LegalCategory,
                                     Counterpart = c.NameEmployee,
                                     CreatedOn = a.CreatedOn
                                 }).OrderByDescending(x => x.CreatedOn).ToListAsync();

            if (result == null) result = new List<LegalDocumentRequest>();
            result.AddRange(result2);

            return result.OrderByDescending(x => x.CreatedOn).ToList();
        }

        public async Task<LegalDocumentRequest> Get(string Id)
        {
            var result = await (from a in context.LegalDocumentRequests
                                join d in context.TrTemplateApprovals on a.Id equals d.RefId
                                where !(a.IsDeleted ?? false) && a.Id == Id
                                select new LegalDocumentRequest
                                {
                                    Id = a.Id,
                                    EmployeeBasicInfoId = a.EmployeeBasicInfoId,
                                    LegalCategoryId = a.LegalCategoryId,
                                    DueDate = a.DueDate,
                                    LegalDocFile = a.LegalDocFile,
                                    Remarks = a.Remarks,
                                    SubmitUser = a.SubmitUser,
                                    CreatedBy = a.CreatedBy,
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
                                    Status = d.StatusApproved.GetHashCode()
                                }).FirstOrDefaultAsync();


            if (result == null)
            {
                result = await (from a in context.LegalDocumentRequests
                                where !(a.IsDeleted ?? false) && a.Id == Id
                                select new LegalDocumentRequest
                                {
                                    Id = a.Id,
                                    EmployeeBasicInfoId = a.EmployeeBasicInfoId,
                                    LegalCategoryId = a.LegalCategoryId,
                                    DueDate = a.DueDate,
                                    LegalDocFile = a.LegalDocFile,
                                    Remarks = a.Remarks,
                                    SubmitUser = a.SubmitUser,
                                    CreatedBy = a.CreatedBy,
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
                                    Status = StatusTransaction.Draft,//DRAFT
                                }).FirstOrDefaultAsync();
            }

            if (result != null)
            {
                if (result.LegalDocFile != null)
                {
                    var file = await context.Filemasters.Where(x => x.Id == result.LegalDocFile).FirstOrDefaultAsync();
                    if (file != null) result.LegalDocFileName = file.Name;
                }
            }
            return result;
        }

        public Task<int> ApproveAsync(LegalDocumentRequest entity)
        {
            throw new NotImplementedException();
        }
    }
}