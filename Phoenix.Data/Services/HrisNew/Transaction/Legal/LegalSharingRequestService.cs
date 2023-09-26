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
    public interface ILegalSharingRequestService : IDataServiceHris<LegalSharingRequest>
    {
        Task<List<LegalSharingRequest>> GetApprovalList();
    }

    public class LegalSharingRequestService : ILegalSharingRequestService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly Services.Um.IManageMenuService menuService;
        string idTemplate = "";

        /// <summary>
        /// And endpoint to manage LegalSharingRequest
        /// </summary>
        /// <param name="context">Database context</param>
        public LegalSharingRequestService(DataContext context, GlobalFunctionApproval globalFcApproval, Services.Um.IManageMenuService menuService)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.menuService = menuService;
        }

        public async Task<int> AddAsync(LegalSharingRequest entity)
        {
            entity.Id = await TransID.GetTransId(context, Code.LegalSharing, DateTime.Today);
            var dataMenu = await menuService.GetByUniqueName(MenuUnique.LegalSharingRequest).ConfigureAwait(false);
            if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
            var dataTempate = context.TmTemplateApprovals.Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
            idTemplate = dataTempate.Id;
            return await globalFcApproval.SubmitApproval<LegalSharingRequest>(false, entity.Id, entity.Status, $"{ApprovalLink.LegalSharingRequest}?Id={entity.Id}&isApprove=true", idTemplate, MenuUnique.LegalSharingRequest, entity);
        }

        public async Task<int> EditAsync(LegalSharingRequest entity)
        {
            var dataMenu = await menuService.GetByUniqueName(MenuUnique.LegalSharingRequest).ConfigureAwait(false);
            if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
            var dataTempate = context.TmTemplateApprovals.Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
            idTemplate = dataTempate.Id;
            return await globalFcApproval.SubmitApproval<LegalSharingRequest>(true, entity.Id, entity.Status, $"{ApprovalLink.LegalSharingRequest}?Id={entity.Id}&isApprove=true", idTemplate, MenuUnique.LegalSharingRequest, entity);
        }

        public async Task<int> ApproveAsync(LegalSharingRequest entity)
        {
            return await globalFcApproval.ProcessApproval<LegalSharingRequest>(entity.Id, entity.Status, entity.RemarkRejected, entity);
        }

        public async Task<int> DeleteSoftAsync(LegalSharingRequest entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<List<LegalSharingRequest>> GetApprovalList()
        {
            var result = await (from a in context.LegalSharingRequests
                                join b in context.EmployeeBasicInfos on a.CreatedBy equals b.Id
                                join c in context.EmployeeBasicInfos on a.CounterpartId equals c.Id
                                join e in context.TrTemplateApprovals on a.Id equals e.RefId
                                join d in context.TrUserApprovals on e.Id equals d.TrTempApprovalId
                                where !(a.IsDeleted ?? false) && d.StatusApproved == Models.Um.StatusApproved.CurrentApproval && d.EmployeeBasicInfoId == context.GetEmployeeId() && a.Category == CategoryLegalDoc.Employee
                                select new LegalSharingRequest
                                {
                                    Id = a.Id,
                                    RequestBy = b.NameEmployee,
                                    Category = a.Category,
                                    CounterpartName = c.NameEmployee,
                                    StartSharing = a.StartSharing,
                                    EndSharing = a.EndSharing,
                                    Purpose = a.Purpose,
                                    StatusDesc = e.StatusApprovedDescription,
                                    CreatedOn = a.CreatedOn
                                }).ToListAsync();

            var result2 = await (from a in context.LegalSharingRequests
                                 join b in context.EmployeeBasicInfos on a.CreatedBy equals b.Id
                                 join c in context.Companys on a.CounterpartId equals c.Id
                                 join e in context.TrTemplateApprovals on a.Id equals e.RefId
                                 join d in context.TrUserApprovals on e.Id equals d.TrTempApprovalId
                                 where !(a.IsDeleted ?? false) && d.StatusApproved == Models.Um.StatusApproved.CurrentApproval && d.EmployeeBasicInfoId == context.GetEmployeeId() && a.Category != CategoryLegalDoc.Employee
                                 select new LegalSharingRequest
                                 {
                                     Id = a.Id,
                                     RequestBy = b.NameEmployee,
                                     Category = a.Category,
                                     CounterpartName = c.CompanyName,
                                     StartSharing = a.StartSharing,
                                     EndSharing = a.EndSharing,
                                     Purpose = a.Purpose,
                                     StatusDesc = e.StatusApprovedDescription,
                                     CreatedOn = a.CreatedOn
                                 }).ToListAsync();

            if (result == null) result = new List<LegalSharingRequest>();
            result.AddRange(result2);
            return result.OrderByDescending(x => x.CreatedOn).ToList();
        }

        public async Task<List<LegalSharingRequest>> Get()
        {
            var result = await (from a in context.LegalSharingRequests
                                join b in context.EmployeeBasicInfos on a.CreatedBy equals b.Id
                                join c in context.EmployeeBasicInfos on a.CounterpartId equals c.Id
                                join d in context.TrTemplateApprovals on a.Id equals d.RefId
                                where !(a.IsDeleted ?? false) && a.Category == CategoryLegalDoc.Employee
                                select new LegalSharingRequest
                                {
                                    Id = a.Id,
                                    RequestBy = b.NameEmployee,
                                    Category = a.Category,
                                    CounterpartName = c.NameEmployee,
                                    StartSharing = a.StartSharing,
                                    EndSharing = a.EndSharing,
                                    Purpose = a.Purpose,
                                    StatusDesc = d.StatusApprovedDescription,
                                    CreatedOn = a.CreatedOn
                                }).ToListAsync();


            var result3 = await (from a in context.LegalSharingRequests
                                 join b in context.EmployeeBasicInfos on a.CreatedBy equals b.Id
                                 join c in context.EmployeeBasicInfos on a.CounterpartId equals c.Id
                                 where !(a.IsDeleted ?? false) && !context.TrTemplateApprovals.Any(x => x.RefId == a.Id) && a.Category == CategoryLegalDoc.Employee
                                 select new LegalSharingRequest
                                 {
                                     Id = a.Id,
                                     RequestBy = b.NameEmployee,
                                     Category = a.Category,
                                     CounterpartName = c.NameEmployee,
                                     StartSharing = a.StartSharing,
                                     EndSharing = a.EndSharing,
                                     Purpose = a.Purpose,
                                     StatusDesc = StatusTransaction.StatusName(StatusTransaction.Draft),
                                     CreatedOn = a.CreatedOn
                                 }).ToListAsync();

            var result2 = await (from a in context.LegalSharingRequests
                                 join b in context.EmployeeBasicInfos on a.CreatedBy equals b.Id
                                 join c in context.Companys on a.CounterpartId equals c.Id
                                 join d in context.TrTemplateApprovals on a.Id equals d.RefId
                                 where !(a.IsDeleted ?? false) && a.Category != CategoryLegalDoc.Employee
                                 select new LegalSharingRequest
                                 {
                                     Id = a.Id,
                                     RequestBy = b.NameEmployee,
                                     Category = a.Category,
                                     CounterpartName = c.CompanyName,
                                     StartSharing = a.StartSharing,
                                     EndSharing = a.EndSharing,
                                     Purpose = a.Purpose,
                                     StatusDesc = d.StatusApprovedDescription,
                                     CreatedOn = a.CreatedOn
                                 }).ToListAsync();

            var result4 = await (from a in context.LegalSharingRequests
                                 join b in context.EmployeeBasicInfos on a.CreatedBy equals b.Id
                                 join c in context.Companys on a.CounterpartId equals c.Id
                                 where !(a.IsDeleted ?? false) && !context.TrTemplateApprovals.Any(x => x.RefId == a.Id) && a.Category != CategoryLegalDoc.Employee
                                 select new LegalSharingRequest
                                 {
                                     Id = a.Id,
                                     RequestBy = b.NameEmployee,
                                     Category = a.Category,
                                     CounterpartName = c.CompanyName,
                                     StartSharing = a.StartSharing,
                                     EndSharing = a.EndSharing,
                                     Purpose = a.Purpose,
                                     StatusDesc = StatusTransaction.StatusName(StatusTransaction.Draft),
                                     CreatedOn = a.CreatedOn
                                 }).ToListAsync();

            if (result == null) result = new List<LegalSharingRequest>();
            result.AddRange(result2);
            result.AddRange(result3);
            result.AddRange(result4);
            return result.OrderByDescending(x => x.CreatedOn).ToList();
        }

        public async Task<LegalSharingRequest> Get(string Id)
        {
            var result = await (from a in context.LegalSharingRequests
                                join d in context.TrTemplateApprovals on a.Id equals d.RefId
                                where !(a.IsDeleted ?? false) && a.Id == Id
                                select new LegalSharingRequest
                                {
                                    Id = a.Id,
                                    LegalDocumentId = a.LegalDocumentId,
                                    Category = a.Category,
                                    CounterpartId = a.CounterpartId,
                                    StartSharing = a.StartSharing,
                                    EndSharing = a.EndSharing,
                                    Purpose = a.Purpose,
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
                result = await (from a in context.LegalSharingRequests
                                where !(a.IsDeleted ?? false) && a.Id == Id && a.Id == Id && !context.TrTemplateApprovals.Any(x => x.RefId == a.Id)
                                select new LegalSharingRequest
                                {
                                    Id = a.Id,
                                    LegalDocumentId = a.LegalDocumentId,
                                    Category = a.Category,
                                    CounterpartId = a.CounterpartId,
                                    StartSharing = a.StartSharing,
                                    EndSharing = a.EndSharing,
                                    Purpose = a.Purpose,
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
            return result;
        }
    }
}