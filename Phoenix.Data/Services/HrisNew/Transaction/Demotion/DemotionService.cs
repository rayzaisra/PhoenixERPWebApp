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
    public interface IDemotionService : IDataServiceHris<Demotion>
    {
        Task<List<Demotion>> GetApprovalList();
    }

    public class DemotionService : IDemotionService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly FileService uploadFile;
        readonly Services.Um.IManageMenuService menuService;
        private readonly Services.Um.IManageUserAppService userApp;
        string idTemplate = "";

        /// <summary>
        /// And endpoint to manage Demotion
        /// </summary>
        /// <param name="context">Database context</param>
        public DemotionService(DataContext context, GlobalFunctionApproval globalFcApproval, FileService uploadFile, Services.Um.IManageMenuService menuService, Services.Um.IManageUserAppService userApp)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.uploadFile = uploadFile;
            this.menuService = menuService;
            this.userApp = userApp;
        }

        public async Task<int> AddAsync(Demotion entity)
        {
            var id = await TransID.GetTransId(context, Code.Demotion, DateTime.Today);
            entity.Id = id;
            entity.RequestCode = id;
            var warningLetter = await ProcessUpload(entity.fileWarningLetter);
            if (!string.IsNullOrEmpty(warningLetter) || string.IsNullOrEmpty(entity.WarningLetter)) entity.WarningLetter = warningLetter;
            await context.PhoenixAddAsync(entity);

            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var isApprove = true;
                    if (entity.Status != StatusTransaction.Draft)
                    {
                        var dataMenu = await menuService.GetByUniqueName(MenuUnique.Demotion).ConfigureAwait(false);
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            MenuId = dataMenu.Id,
                            RefId = entity.Id,
                            DetailLink = $"{ApprovalLink.Demotion}?Id={entity.Id}&isApprove=true",
                            Module = NotificationModuleType.ModuleHRIS,
                            NotificationType = NotificationTypeCode.DemotionNotification
                        };
                        var subGroupId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(context.HttpContext);
                        var divisionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(context.HttpContext);
                        var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
                        isApprove = await globalFcApproval.Save(vm, subGroupId, divisionId, employeeId);
                        if (isApprove)
                        {
                            var save = await context.SaveChangesAsync();
                            transaction.Commit();
                            return save;
                        }
                        else
                        {
                            transaction.Rollback();
                            throw new Exception("please check the approval template that will be processed");
                        }
                    }
                    else
                    {
                        var save = await context.SaveChangesAsync();
                        transaction.Commit();
                        return save;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }

            //return await globalFcApproval.SubmitApproval<Demotion>(false, entity.Id, entity.Status, $"{ApprovalLink.Demotion}?Id={entity.Id}&isApprove=true", idTemplate, MenuUnique.Demotion, entity);
        }

        public async Task<int> EditAsync(Demotion entity)
        {
            if (string.IsNullOrEmpty(entity.RequestCode))
            {
                entity.RequestCode = (context.Demotions.Where(x => x.Id == entity.Id).AsNoTracking().FirstOrDefault() ?? new Demotion()).RequestCode;
            }
            var warningLetter = await ProcessUpload(entity.fileWarningLetter);
            
            if (!string.IsNullOrEmpty(warningLetter) || string.IsNullOrEmpty(entity.WarningLetter)) entity.WarningLetter = warningLetter;
            context.PhoenixEdit(entity);
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var isApprove = true;
                    if (entity.Status != StatusTransaction.Draft)
                    {
                        var dataMenu = await menuService.GetByUniqueName(MenuUnique.Demotion).ConfigureAwait(false);
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            MenuId = dataMenu.Id,
                            RefId = entity.Id,
                            DetailLink = $"{ApprovalLink.Demotion}?Id={entity.Id}&isApprove=true",
                            Module = NotificationModuleType.ModuleHRIS,
                            NotificationType = NotificationTypeCode.DemotionNotification
                        };
                        var subGroupId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(context.HttpContext);
                        var divisionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(context.HttpContext);
                        var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
                        isApprove = await globalFcApproval.Save(vm, subGroupId, divisionId, employeeId);
                        if (isApprove)
                        {
                            var save = await context.SaveChangesAsync();
                            transaction.Commit();
                            return save;
                        }
                        else
                        {
                            transaction.Rollback();
                            throw new Exception("please check the approval template that will be processed");
                        }
                    }
                    else
                    {
                        var save = await context.SaveChangesAsync();
                        transaction.Commit();
                        return save;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
            //return await globalFcApproval.SubmitApproval<Demotion>(true, entity.Id, entity.Status, $"{ApprovalLink.Demotion}?Id={entity.Id}&isApprove=true", idTemplate, MenuUnique.Demotion, entity);
        }

        private async Task<string> ProcessUpload(IFormFile fileWarningLetter)
        {
            string warningLetter = ""; 
            var file = await uploadFile.Upload(fileWarningLetter, null);
            if (!string.IsNullOrEmpty(file))
            {
                warningLetter = file;
            }

            return warningLetter;
        }

        public async Task<int> ApproveAsync(Demotion entity)
        {
            return await globalFcApproval.ProcessApproval<Demotion>(entity.Id, entity.Status, entity.RemarkRejected, entity);
        }

        public async Task<int> DeleteSoftAsync(Demotion entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<List<Demotion>> GetApprovalList()
        {
            var result = await (from a in context.Demotions
                                join b in context.EmployeeBasicInfos on a.Requester equals b.Id
                                join c in context.TrTemplateApprovals on a.Id equals c.RefId
                                join d in context.TrUserApprovals on c.Id equals d.TrTempApprovalId
                                where !(a.IsDeleted ?? false) && d.StatusApproved == Models.Um.StatusApproved.CurrentApproval && d.EmployeeBasicInfoId == context.GetEmployeeId()
                                select new Demotion
                                {
                                    Id = a.Id,
                                    RequestCode = a.RequestCode,
                                    RequestDate = a.RequestDate,
                                    StatusDesc = c.StatusApprovedDescription,
                                    CreatedOn = a.CreatedOn,
                                    requestEmployee = new InfoEmployee
                                    {
                                        //EmployeeId = b.Id,
                                        EmployeeName = b.NameEmployee,
                                        //DateEmployed = b.JoinDate,
                                        //Phone = b.PhoneNumber,
                                        //Email = b.Email,
                                        //DateBirth = b.DateBirth,
                                        //Age = Convert.ToDateTime(b.DateBirth).AddYears(DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) > DateTime.Now ?
                                        //                      (DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) - 1 : DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year,
                                        JobTitle = b.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobTitles on r.JobTitleId equals s.Id
                                                                           where r.Id == b.BusinessUnitJobLevelId
                                                                           select s.TitleName).FirstOrDefault(),
                                        Grade = b.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                           where r.Id == b.BusinessUnitJobLevelId
                                                                           select s.GradeName).FirstOrDefault(),
                                        //EmployementStatus = context.EmployementStatuss.Where(x => x.Id == b.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault(),
                                        Division = b.BusinessUnitJobLevelId == null ? "" :
                                                                        (from r in context.BusinessUnitJobLevels
                                                                         join s in context.BusinessUnits on r.BusinessUnitDivisionId equals s.Id
                                                                         join t in context.BusinessUnitTypes on s.BusinessUnitTypeId equals t.Id
                                                                         where t.BusinessUnitLevel == BusinessUnitLevelCode.Division && r.Id == b.BusinessUnitJobLevelId
                                                                         select s.UnitName).FirstOrDefault()
                                    }
                                }).OrderByDescending(x => x.CreatedOn).ToListAsync();
            return result;
        }

        public async Task<List<Demotion>> Get()
        {
            var group = await userApp.GetByUserLogin();
            var HCAdmin = group.Where(x => x.GroupId == GroupAcess.HCAdmin).Count() > 0;
            var result = new List<Demotion>();
            if (HCAdmin)
            {
                result = await (from a in context.Demotions
                                    join b in context.EmployeeBasicInfos on a.Requester equals b.Id
                                    join c in context.TrTemplateApprovals on a.Id equals c.RefId
                                    join d in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals d.Id
                                    where !(a.IsDeleted ?? false)
                                    select new Demotion
                                    {
                                        Id = a.Id,
                                        RequestCode = a.RequestCode,
                                        RequestDate = a.RequestDate,
                                        StatusDesc = c.StatusApprovedDescription,
                                        CreatedOn = a.CreatedOn,
                                        requestEmployee = new InfoEmployee
                                        {
                                            //EmployeeId = b.Id,
                                            EmployeeName = b.NameEmployee,
                                            //DateEmployed = b.JoinDate,
                                            //Phone = b.PhoneNumber,
                                            //Email = b.Email,
                                            //DateBirth = b.DateBirth,
                                            //Age = Convert.ToDateTime(b.DateBirth).AddYears(DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) > DateTime.Now ?
                                            //                      (DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) - 1 : DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year,
                                            JobTitle = b.BusinessUnitJobLevelId == null ? "" :
                                                                              (from r in context.BusinessUnitJobLevels
                                                                               join s in context.JobTitles on r.JobTitleId equals s.Id
                                                                               where r.Id == b.BusinessUnitJobLevelId
                                                                               select s.TitleName).FirstOrDefault(),
                                            Grade = b.BusinessUnitJobLevelId == null ? "" :
                                                                              (from r in context.BusinessUnitJobLevels
                                                                               join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                               where r.Id == b.BusinessUnitJobLevelId
                                                                               select s.GradeName).FirstOrDefault(),
                                            //EmployementStatus = context.EmployementStatuss.Where(x => x.Id == b.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault(),
                                            Division = b.BusinessUnitJobLevelId == null ? "" :
                                                                            (from r in context.BusinessUnitJobLevels
                                                                             join s in context.BusinessUnits on r.BusinessUnitDivisionId equals s.Id
                                                                             join t in context.BusinessUnitTypes on s.BusinessUnitTypeId equals t.Id
                                                                             where t.BusinessUnitLevel == BusinessUnitLevelCode.Division && r.Id == b.BusinessUnitJobLevelId
                                                                             select s.UnitName).FirstOrDefault()
                                        },
                                        infoEmployee = new InfoEmployee
                                        {
                                            EmployeeName = d.NameEmployee,
                                            JobTitle = d.BusinessUnitJobLevelId == null ? "" :
                                                                              (from r in context.BusinessUnitJobLevels
                                                                               join s in context.JobTitles on r.JobTitleId equals s.Id
                                                                               where r.Id == d.BusinessUnitJobLevelId
                                                                               select s.TitleName).FirstOrDefault(),
                                            Grade = d.BusinessUnitJobLevelId == null ? "" :
                                                                              (from r in context.BusinessUnitJobLevels
                                                                               join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                               where r.Id == d.BusinessUnitJobLevelId
                                                                               select s.GradeName).FirstOrDefault(),
                                            Division = d.BusinessUnitJobLevelId == null ? "" :
                                                                            (from x in context.BusinessUnits
                                                                             where x.Id == ((from r in context.BusinessUnitJobLevels
                                                                                             where r.Id == d.BusinessUnitJobLevelId
                                                                                             select r.BusinessUnitDivisionId).FirstOrDefault())
                                                                             select x.UnitName).FirstOrDefault()
                                        }
                                    }).OrderByDescending(x => x.CreatedOn).ToListAsync();

                var result2 = await (from a in context.Demotions
                                     join b in context.EmployeeBasicInfos on a.Requester equals b.Id
                                     join d in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals d.Id
                                     where !(a.IsDeleted ?? false) && !context.TrTemplateApprovals.Any(x => x.RefId == a.Id)
                                     select new Demotion
                                     {
                                         Id = a.Id,
                                         RequestCode = a.RequestCode,
                                         RequestDate = a.RequestDate,
                                         StatusDesc = StatusTransaction.StatusName(StatusTransaction.Draft),
                                         CreatedOn = a.CreatedOn,
                                         requestEmployee = new InfoEmployee
                                         {
                                             //EmployeeId = b.Id,
                                             EmployeeName = b.NameEmployee,
                                             //DateEmployed = b.JoinDate,
                                             //Phone = b.PhoneNumber,
                                             //Email = b.Email,
                                             //DateBirth = b.DateBirth,
                                             //Age = Convert.ToDateTime(b.DateBirth).AddYears(DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) > DateTime.Now ?
                                             //                      (DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) - 1 : DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year,
                                             JobTitle = b.BusinessUnitJobLevelId == null ? "" :
                                                                              (from r in context.BusinessUnitJobLevels
                                                                               join s in context.JobTitles on r.JobTitleId equals s.Id
                                                                               where r.Id == b.BusinessUnitJobLevelId
                                                                               select s.TitleName).FirstOrDefault(),
                                             Grade = b.BusinessUnitJobLevelId == null ? "" :
                                                                              (from r in context.BusinessUnitJobLevels
                                                                               join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                               where r.Id == b.BusinessUnitJobLevelId
                                                                               select s.GradeName).FirstOrDefault(),
                                             //EmployementStatus = context.EmployementStatuss.Where(x => x.Id == b.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault(),
                                             Division = b.BusinessUnitJobLevelId == null ? "" :
                                                                            (from r in context.BusinessUnitJobLevels
                                                                             join s in context.BusinessUnits on r.BusinessUnitDivisionId equals s.Id
                                                                             join t in context.BusinessUnitTypes on s.BusinessUnitTypeId equals t.Id
                                                                             where t.BusinessUnitLevel == BusinessUnitLevelCode.Division && r.Id == b.BusinessUnitJobLevelId
                                                                             select s.UnitName).FirstOrDefault()
                                         },
                                         infoEmployee = new InfoEmployee
                                         {
                                             EmployeeName = d.NameEmployee,
                                             JobTitle = d.BusinessUnitJobLevelId == null ? "" :
                                                                              (from r in context.BusinessUnitJobLevels
                                                                               join s in context.JobTitles on r.JobTitleId equals s.Id
                                                                               where r.Id == d.BusinessUnitJobLevelId
                                                                               select s.TitleName).FirstOrDefault(),
                                             Grade = d.BusinessUnitJobLevelId == null ? "" :
                                                                              (from r in context.BusinessUnitJobLevels
                                                                               join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                               where r.Id == d.BusinessUnitJobLevelId
                                                                               select s.GradeName).FirstOrDefault(),
                                             Division = d.BusinessUnitJobLevelId == null ? "" :
                                                                           (from x in context.BusinessUnits
                                                                            where x.Id == ((from r in context.BusinessUnitJobLevels
                                                                                            where r.Id == d.BusinessUnitJobLevelId
                                                                                            select r.BusinessUnitDivisionId).FirstOrDefault())
                                                                            select x.UnitName).FirstOrDefault()
                                         }
                                     }).OrderByDescending(x => x.CreatedOn).ToListAsync();

                if (result == null) result = new List<Demotion>();
                result.AddRange(result2);
                result.OrderByDescending(x => x.CreatedOn).ToList();
            }


            return result;
        }

        public async Task<Demotion> Get(string Id)
        {
            var result = await (from a in context.Demotions
                                join b in context.EmployeeBasicInfos on a.Requester equals b.Id
                                join c in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals c.Id
                                join d in context.TrTemplateApprovals on a.Id equals d.RefId
                                where !(a.IsDeleted ?? false) && a.Id == Id
                                select new Demotion
                                {
                                    Id = a.Id,
                                    EmployeeBasicInfoId = a.EmployeeBasicInfoId,
                                    EffectiveDate = a.EffectiveDate,
                                    JobTitleId = a.JobTitleId,
                                    JobGradeId = a.JobGradeId,
                                    JobLevelId = a.JobLevelId,
                                    Notes = a.Notes,
                                    WarningLetter = a.WarningLetter,
                                    Status = d.StatusApproved.GetHashCode(),
                                    Requester = a.Requester,
                                    RequestCode = a.RequestCode,
                                    RequestDate = a.RequestDate,
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
                                    WarningLetterName = context.Filemasters.Where(x => x.Id == a.WarningLetter).Select(x => x.Name).FirstOrDefault(),
                                    requestEmployee = new InfoEmployee
                                    {
                                        EmployeeId = b.Id,
                                        EmployeeName = b.NameEmployee,
                                        //DateEmployed = b.JoinDate,
                                        //Phone = b.PhoneNumber,
                                        //Email = b.Email,
                                        //DateBirth = b.DateBirth,
                                        //Age = Convert.ToDateTime(b.DateBirth).AddYears(DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) > DateTime.Now ?
                                        //                      (DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) - 1 : DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year,
                                        JobTitle = b.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobTitles on r.JobTitleId equals s.Id
                                                                           where r.Id == b.BusinessUnitJobLevelId
                                                                           select s.TitleName).FirstOrDefault(),
                                        Grade = b.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                           where r.Id == b.BusinessUnitJobLevelId
                                                                           select s.GradeName).FirstOrDefault(),
                                        GradeId = b.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                           where r.Id == b.BusinessUnitJobLevelId
                                                                           select s.Id).FirstOrDefault(),
                                        //EmployementStatus = context.EmployementStatuss.Where(x => x.Id == b.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault()
                                    },
                                    infoEmployee = new InfoEmployee
                                    {
                                        EmployeeId = c.Id,
                                        EmployeeName = c.NameEmployee,
                                        DateEmployed = c.JoinDate,
                                        Phone = c.PhoneNumber,
                                        Email = c.Email,
                                        DateBirth = c.DateBirth,
                                        Age = Convert.ToDateTime(c.DateBirth).AddYears(DateTime.Now.Year - Convert.ToDateTime(c.DateBirth).Year) > DateTime.Now ?
                                                              (DateTime.Now.Year - Convert.ToDateTime(c.DateBirth).Year) - 1 : DateTime.Now.Year - Convert.ToDateTime(c.DateBirth).Year,
                                        JobTitle = c.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobTitles on r.JobTitleId equals s.Id
                                                                           where r.Id == c.BusinessUnitJobLevelId
                                                                           select s.TitleName).FirstOrDefault(),
                                        Grade = c.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                           where r.Id == c.BusinessUnitJobLevelId
                                                                           select s.GradeName).FirstOrDefault(),
                                        EmployementStatus = context.EmployementStatuss.Where(x => x.Id == c.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault(),
                                        EmployeeContractDocument = context.EmployeeContracts.Where(x => x.EmployeeBasicInfoId == a.EmployeeBasicInfoId).Select(x => x.FilemasterId).FirstOrDefault(),
                                        EmployeeContractDocumentName = (from r in context.EmployeeContracts
                                                                        join s in context.Filemasters on r.FilemasterId equals s.Id
                                                                        where r.EmployeeBasicInfoId == a.EmployeeBasicInfoId
                                                                        select s.Name).FirstOrDefault(),
                                        EmployeeJobDescription = "",
                                        EmployeeJobDescriptionName = "",
                                        GradeId = c.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                           where r.Id == c.BusinessUnitJobLevelId
                                                                           select s.Id).FirstOrDefault()
                                    }
                                }).FirstOrDefaultAsync();



            if (result == null)
            {
                result = await (from a in context.Demotions
                                join b in context.EmployeeBasicInfos on a.Requester equals b.Id
                                join c in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals c.Id
                                where !(a.IsDeleted ?? false) && a.Id == Id
                                select new Demotion
                                {
                                    Id = a.Id,
                                    EmployeeBasicInfoId = a.EmployeeBasicInfoId,
                                    EffectiveDate = a.EffectiveDate,
                                    JobTitleId = a.JobTitleId,
                                    JobGradeId = a.JobGradeId,
                                    JobLevelId = a.JobLevelId,
                                    Notes = a.Notes,
                                    WarningLetter = a.WarningLetter,
                                    Status = StatusTransaction.Draft,//DRAFT
                                    Requester = a.Requester,
                                    RequestCode = a.RequestCode,
                                    RequestDate = a.RequestDate,
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
                                    WarningLetterName = context.Filemasters.Where(x => x.Id == a.WarningLetter).Select(x => x.Name).FirstOrDefault(),
                                    requestEmployee = new InfoEmployee
                                    {
                                        EmployeeId = b.Id,
                                        EmployeeName = b.NameEmployee,
                                        //DateEmployed = b.JoinDate,
                                        //Phone = b.PhoneNumber,
                                        //Email = b.Email,
                                        //DateBirth = b.DateBirth,
                                        //Age = Convert.ToDateTime(b.DateBirth).AddYears(DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) > DateTime.Now ?
                                        //                      (DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) - 1 : DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year,
                                        JobTitle = b.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobTitles on r.JobTitleId equals s.Id
                                                                           where r.Id == b.BusinessUnitJobLevelId
                                                                           select s.TitleName).FirstOrDefault(),
                                        Grade = b.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                           where r.Id == b.BusinessUnitJobLevelId
                                                                           select s.GradeName).FirstOrDefault(),
                                        GradeId = b.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                           where r.Id == b.BusinessUnitJobLevelId
                                                                           select s.Id).FirstOrDefault(),
                                        //EmployementStatus = context.EmployementStatuss.Where(x => x.Id == b.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault()
                                    },
                                    infoEmployee = new InfoEmployee
                                    {
                                        EmployeeId = c.Id,
                                        EmployeeName = c.NameEmployee,
                                        DateEmployed = c.JoinDate,
                                        Phone = c.PhoneNumber,
                                        Email = c.Email,
                                        DateBirth = c.DateBirth,
                                        Age = Convert.ToDateTime(c.DateBirth).AddYears(DateTime.Now.Year - Convert.ToDateTime(c.DateBirth).Year) > DateTime.Now ?
                                                              (DateTime.Now.Year - Convert.ToDateTime(c.DateBirth).Year) - 1 : DateTime.Now.Year - Convert.ToDateTime(c.DateBirth).Year,
                                        JobTitle = c.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobTitles on r.JobTitleId equals s.Id
                                                                           where r.Id == c.BusinessUnitJobLevelId
                                                                           select s.TitleName).FirstOrDefault(),
                                        Grade = c.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                           where r.Id == c.BusinessUnitJobLevelId
                                                                           select s.GradeName).FirstOrDefault(),
                                        GradeId = c.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                           where r.Id == c.BusinessUnitJobLevelId
                                                                           select s.Id).FirstOrDefault(),
                                        EmployementStatus = context.EmployementStatuss.Where(x => x.Id == c.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault(),
                                        EmployeeContractDocument = context.EmployeeContracts.Where(x => x.EmployeeBasicInfoId == a.EmployeeBasicInfoId).Select(x => x.FilemasterId).FirstOrDefault(),
                                        EmployeeContractDocumentName = (from r in context.EmployeeContracts
                                                                        join s in context.Filemasters on r.FilemasterId equals s.Id
                                                                        where r.EmployeeBasicInfoId == a.EmployeeBasicInfoId
                                                                        select s.Name).FirstOrDefault(),
                                        EmployeeJobDescription = "",
                                        EmployeeJobDescriptionName = ""
                                    }
                                }).FirstOrDefaultAsync();
            }
            return result;
        }
    }
}