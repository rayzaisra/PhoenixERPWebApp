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
    public interface ILegalDocumentService : IDataService<LegalDocument>
    {
        Task<List<LegalDocument>> GetDocumentList(LegalDocumentSearch search);
        Task<List<LegalDocument>> GetDashboardList(DashboardSearch search);
        bool GetUserAcess();
    }

    public class LegalDocumentService : ILegalDocumentService
    {
        readonly DataContext context;
        readonly FileService uploadFile;

        /// <summary>
        /// And endpoint to manage LegalDocument
        /// </summary>
        /// <param name="context">Database context</param>
        public LegalDocumentService(DataContext context, FileService uploadFile)
        {
            this.context = context;
            this.uploadFile = uploadFile;
        }

        public async Task<int> AddAsync(LegalDocument entity)
        {
            var model = await ProcessUpload(entity);
            model.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(model);

            if (entity.LegalDocumentCounterpart != null)
            {
                entity.LegalDocumentCounterpart.Id = Guid.NewGuid().ToString();
                entity.LegalDocumentCounterpart.LegalDocumentId = model.Id;
                await context.PhoenixAddAsync(entity.LegalDocumentCounterpart);
            }
            return await context.SaveChangesAsync();
        }

        private async Task<LegalDocument> ProcessUpload(LegalDocument entity)
        {
            var file1 = await uploadFile.Upload(entity.file, null);
            if (!string.IsNullOrEmpty(file1))
            {
                entity.FileUpload = file1;
            }

            return entity;
        }

        public Task<int> AddRangeAsync(params LegalDocument[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(LegalDocument entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params LegalDocument[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(LegalDocument entity)
        {
            context.PhoenixDelete(entity);
            context.PhoenixDelete(await context.LegalDocumentCounterparts.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.LegalDocumentId == entity.Id).FirstOrDefaultAsync());
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params LegalDocument[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(LegalDocument entity)
        {
            var model = await ProcessUpload(entity);
            context.PhoenixEdit(model);

            if (entity.LegalDocumentCounterpart != null)
            {
                var modelDetail = await context.LegalDocumentCounterparts.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.LegalDocumentId == entity.Id).FirstOrDefaultAsync();
                modelDetail.CounterpartId = entity.LegalDocumentCounterpart.CounterpartId;
                context.PhoenixEdit(modelDetail);
            }
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(LegalDocument entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<LegalDocument>> Get() => await context.LegalDocuments.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).ToListAsync();

        public Task<List<LegalDocument>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<LegalDocument> Get(string Id)
        {
            return await (from a in context.LegalDocuments
                          join b in context.LegalDocumentCounterparts on a.Id equals b.LegalDocumentId
                          join e in context.Filemasters on a.FileUpload equals e.Id
                          where !(a.IsDeleted ?? false) && !(b.IsDeleted ?? false) && a.Id == Id
                          select new LegalDocument
                          {
                              Id = a.Id,
                              Category = a.Category,
                              LegalCategoryId = a.LegalCategoryId,
                              DocumentDate = a.DocumentDate,
                              ExpiredDate = a.ExpiredDate,
                              FileUpload = a.FileUpload,
                              Label = a.Label,
                              Remarks = a.Remarks,
                              LegalDocumentCounterpart = b,
                              FileUploadName = e.Name,
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
                          }).FirstOrDefaultAsync();
        }

        public async Task<List<LegalDocument>> GetDocumentList(LegalDocumentSearch search)
        {
            var employeeId = context.GetEmployeeId();
            var documentSharing = await (from a in context.LegalSharingRequests
                                         join b in context.TrTemplateApprovals on a.Id equals b.RefId
                                         where (a.CounterpartId == employeeId || a.CreatedBy == employeeId) &&
                                         DateTime.Now.Date >= Convert.ToDateTime(a.StartSharing).Date &&
                                         DateTime.Now.Date <= Convert.ToDateTime(a.EndSharing).Date &&
                                         b.StatusApproved.GetHashCode() == StatusTransaction.Approved
                                         select a).ToListAsync();

            return await (from a in context.LegalDocuments
                          join b in context.LegalDocumentCounterparts on a.Id equals b.LegalDocumentId
                          join c in context.LegalCategorys on a.LegalCategoryId equals c.Id
                          join d in context.EmployeeBasicInfos on b.CounterpartId equals d.Id
                          join e in context.Filemasters on a.FileUpload equals e.Id
                          where !(a.IsDeleted ?? false) && !(b.IsDeleted ?? false) && a.Category == search.Category &&
                          (string.IsNullOrEmpty(search.CounterpartId) ? true : b.CounterpartId == search.CounterpartId) &&
                          (string.IsNullOrEmpty(search.LegalCategoryId) ? true : a.LegalCategoryId == search.LegalCategoryId)
                          && (a.CreatedBy == employeeId || documentSharing.Any(y => y.LegalDocumentId == a.Id))
                          select new LegalDocument
                          {
                              Id = a.Id,
                              Category = a.Category,
                              LegalCategoryName = c.Category,
                              DocumentDate = a.DocumentDate,
                              ExpiredDate = a.ExpiredDate,
                              FileUploadName = e.Name,
                              FileUpload = a.FileUpload,
                              Label = a.Label,
                              Remarks = a.Remarks,
                              LocationName = context.Locations.Where(x => x.Id == d.LocationId).Select(x => x.LocationName).FirstOrDefault(),
                              LegalDocumentCounterpart = new LegalDocumentCounterpart()
                              {
                                  CounterpartName = d.NameEmployee
                              }
                          }).ToListAsync();
        }

        public async Task<List<LegalDocument>> GetDashboardList(DashboardSearch search)
        {
            var employeeId = context.GetEmployeeId();
            var documentSharing = await (from a in context.LegalSharingRequests
                                         join b in context.TrTemplateApprovals on a.Id equals b.RefId
                                         where (a.CounterpartId == employeeId || a.CreatedBy == employeeId) &&
                                         DateTime.Now.Date >= Convert.ToDateTime(a.StartSharing).Date &&
                                         DateTime.Now.Date <= Convert.ToDateTime(a.EndSharing).Date &&
                                         b.StatusApproved.GetHashCode() == StatusTransaction.Approved
                                         select a).ToListAsync();

            var result = await (from a in context.LegalDocuments
                                join b in context.LegalDocumentCounterparts on a.Id equals b.LegalDocumentId
                                join c in context.LegalCategorys on a.LegalCategoryId equals c.Id
                                join d in context.EmployeeBasicInfos on b.CounterpartId equals d.Id
                                join e in context.Filemasters on a.FileUpload equals e.Id
                                where !(a.IsDeleted ?? false) && !(b.IsDeleted ?? false) && a.Category == search.Category &&
                                (string.IsNullOrEmpty(search.CounterpartId) ? true : b.CounterpartId == search.CounterpartId) &&
                                (string.IsNullOrEmpty(search.LegalCategoryId) ? true : a.LegalCategoryId == search.LegalCategoryId) &&
                                (string.IsNullOrEmpty(search.LocationId) ? true : d.LocationId == search.LocationId) &&
                                (search.ExpiredDate == null ? true : a.ExpiredDate == search.ExpiredDate)
                               && (a.CreatedBy == employeeId || documentSharing.Any(y => y.LegalDocumentId == a.Id))
                                select new LegalDocument
                                {
                                    Id = a.Id,
                                    Category = a.Category,
                                    LegalCategoryName = c.Category,
                                    DocumentDate = a.DocumentDate,
                                    ExpiredDate = a.ExpiredDate,
                                    FileUploadName = e.Name,
                                    FileUpload = a.FileUpload,
                                    Label = a.Label,
                                    Remarks = a.Remarks,
                                    LocationName = context.Locations.Where(x => x.Id == d.LocationId).Select(x => x.LocationName).FirstOrDefault(),
                                    DivisionId = d.BusinessUnitJobLevelId == null ? "" :
                                                    (from r in context.BusinessUnitJobLevels
                                                     join s in context.BusinessUnits on r.BusinessUnitDivisionId equals s.Id
                                                     join t in context.BusinessUnitTypes on s.BusinessUnitTypeId equals t.Id
                                                     where t.BusinessUnitLevel == BusinessUnitLevelCode.Division && r.Id == d.BusinessUnitJobLevelId
                                                     select s.Id).FirstOrDefault(),
                                    DivisionName = d.BusinessUnitJobLevelId == null ? "" :
                                                    (from r in context.BusinessUnitJobLevels
                                                     join s in context.BusinessUnits on r.BusinessUnitDivisionId equals s.Id
                                                     join t in context.BusinessUnitTypes on s.BusinessUnitTypeId equals t.Id
                                                     where t.BusinessUnitLevel == BusinessUnitLevelCode.Division && r.Id == d.BusinessUnitJobLevelId
                                                     select s.UnitName).FirstOrDefault(),
                                    LegalDocumentCounterpart = new LegalDocumentCounterpart()
                                    {
                                        CounterpartName = d.NameEmployee
                                    }
                                }).ToListAsync();

            return result.Where(x => (string.IsNullOrEmpty(search.DivisionId) ? true : x.DivisionId == search.DivisionId)).ToList();
        }

        public bool GetUserAcess() => context.TmUserApps.Where(x => x.EmployeeBasicInfoId == context.GetEmployeeId() && (x.GroupId == GroupAcess.HCAdmin || x.GroupId == GroupAcess.SuperAdmin)).Count() > 0;
    }
}