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
    public interface IPDRApprailsalService : IDataServiceHris<PdrApprailsalM1Header>
    {
        Task<List<PdrApprailsalM1Header>> GetApprovalList();
        Task<List<PdrApprailsalM1>> GetPdrApprailsalM1List(string PdrEmployeeId, string employeeId, DateTime? startReview);
    }

    public class PDRApprailsalService : IPDRApprailsalService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly Services.Um.IManageMenuService menuService;

        /// <summary>
        /// And endpoint to manage PdrApprailsalM1Header
        /// </summary>
        /// <param name="context">Database context</param>
        public PDRApprailsalService(DataContext context, GlobalFunctionApproval globalFcApproval, Services.Um.IManageMenuService menuService)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.menuService = menuService;
        }

        public async Task<int> AddAsync(PdrApprailsalM1Header entity)
        {
            entity.Id = await TransID.GetTransId(context, Code.PDRApprailsal, DateTime.Today);
            return await AddEdit(entity, false);
        }

        public async Task<int> EditAsync(PdrApprailsalM1Header entity)
        {
            return await AddEdit(entity, true);
        }

        private async Task<int> AddEdit(PdrApprailsalM1Header entity, bool isEdit)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (isEdit)
                    {
                        context.PhoenixEdit(entity);
                    }
                    else
                    {
                        await context.PhoenixAddAsync(entity);
                        await context.SaveChangesAsync();
                    }

                    if (!string.IsNullOrEmpty(entity.Id))
                    {

                        var PdrApprailsalM1s = context.PdrApprailsalM1s.AsNoTracking().Where(x => x.PdrApprailsalM1HeaderId == entity.Id).ToList();
                        if (PdrApprailsalM1s.Count > 0)
                        {
                            context.PdrApprailsalM1s.RemoveRange(PdrApprailsalM1s);
                        }
                    }

                    if (entity.PdrApprailsalM1s != null)
                    {
                        Parallel.ForEach(entity.PdrApprailsalM1s, (item) =>
                        {
                            item.Id = Guid.NewGuid().ToString();
                            item.PdrApprailsalM1HeaderId = entity.Id;
                            context.PhoenixAddAsync(item);
                        });
                    }

                    var isApprove = true;
                    if (entity.Status != 0)//0: Draft
                    {

                        var dataMenu = await menuService.GetByUniqueName(MenuUnique.PdrApprailsal).ConfigureAwait(false);
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            MenuId = dataMenu.Id,
                            RefId = entity.Id,
                            DetailLink = $"{ApprovalLink.PdrApprailsal}?Id={entity.Id}&isApprove=true"
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
        }

        public async Task<int> ApproveAsync(PdrApprailsalM1Header entity)
        {
            return await globalFcApproval.ProcessApproval<PdrApprailsalM1Header>(entity.Id, entity.Status, entity.RemarkRejected, entity);
        }

        public async Task<int> DeleteSoftAsync(PdrApprailsalM1Header entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<List<PdrApprailsalM1Header>> GetApprovalList()
        {
            var result = await (from a in context.PdrApprailsalM1Headers
                                join b in context.EmployeeBasicInfos on a.EmployeeId equals b.Id
                                join c in context.TrTemplateApprovals on a.Id equals c.RefId
                                join d in context.TrUserApprovals on c.Id equals d.TrTempApprovalId
                                where !(a.IsDeleted ?? false) && d.StatusApproved == Models.Um.StatusApproved.CurrentApproval && d.EmployeeBasicInfoId == context.GetEmployeeId()
                                select new PdrApprailsalM1Header
                                {
                                    Id = a.Id,
                                    Status = c.StatusApproved.GetHashCode(),
                                    StatusDesc = c.StatusApprovedDescription,
                                    ReviewStart = a.ReviewStart,
                                    ReviewEnd = a.ReviewEnd,
                                    CreatedOn = a.CreatedOn,
                                    infoEmployee = new InfoEmployee
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
                                        //Grade = b.BusinessUnitJobLevelId == null ? "" :
                                        //                                  (from r in context.BusinessUnitJobLevels
                                        //                                   join s in context.JobGrades on r.JobGradeId equals s.Id
                                        //                                   where r.Id == b.BusinessUnitJobLevelId
                                        //                                   select s.GradeName).FirstOrDefault(),
                                        EmployementStatus = context.EmployementStatuss.Where(x => x.Id == b.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault(),
                                        //Division = b.BusinessUnitJobLevelId == null ? "" :
                                        //                                (from r in context.BusinessUnitJobLevels
                                        //                                 join s in context.BusinessUnits on r.BusinessUnitDivisionId equals s.Id
                                        //                                 join t in context.BusinessUnitTypes on s.BusinessUnitTypeId equals t.Id
                                        //                                 where t.BusinessUnitLevel == BusinessUnitLevelCode.Division && r.Id == b.BusinessUnitJobLevelId
                                        //                                 select s.UnitName).FirstOrDefault()
                                    }
                                }).OrderByDescending(x => x.CreatedOn).ToListAsync();
            return result;
        }

        public async Task<List<PdrApprailsalM1Header>> Get()
        {
            var result = await (from a in context.PdrApprailsalM1Headers
                                join b in context.EmployeeBasicInfos on a.EmployeeId equals b.Id
                                join c in context.TrTemplateApprovals on a.Id equals c.RefId
                                where !(a.IsDeleted ?? false)
                                select new PdrApprailsalM1Header
                                {
                                    Id = a.Id,
                                    Status = c.StatusApproved.GetHashCode(),
                                    StatusDesc = c.StatusApprovedDescription,
                                    ReviewStart = a.ReviewStart,
                                    ReviewEnd = a.ReviewEnd,
                                    CreatedOn = a.CreatedOn,
                                    infoEmployee = new InfoEmployee
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
                                        //Grade = b.BusinessUnitJobLevelId == null ? "" :
                                        //                                  (from r in context.BusinessUnitJobLevels
                                        //                                   join s in context.JobGrades on r.JobGradeId equals s.Id
                                        //                                   where r.Id == b.BusinessUnitJobLevelId
                                        //                                   select s.GradeName).FirstOrDefault(),
                                        EmployementStatus = context.EmployementStatuss.Where(x => x.Id == b.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault(),
                                        //Division = b.BusinessUnitJobLevelId == null ? "" :
                                        //                                (from r in context.BusinessUnitJobLevels
                                        //                                 join s in context.BusinessUnits on r.BusinessUnitDivisionId equals s.Id
                                        //                                 join t in context.BusinessUnitTypes on s.BusinessUnitTypeId equals t.Id
                                        //                                 where t.BusinessUnitLevel == BusinessUnitLevelCode.Division && r.Id == b.BusinessUnitJobLevelId
                                        //                                 select s.UnitName).FirstOrDefault()
                                    }
                                }).OrderByDescending(x => x.CreatedOn).ToListAsync();

            var result2 = await (from a in context.PdrApprailsalM1Headers
                                 join b in context.EmployeeBasicInfos on a.EmployeeId equals b.Id
                                 where !(a.IsDeleted ?? false) && !context.TrTemplateApprovals.Any(x => x.RefId == a.Id)
                                 select new PdrApprailsalM1Header
                                 {
                                     Id = a.Id,
                                     Status = StatusTransaction.Draft,
                                     StatusDesc = StatusTransaction.StatusName(StatusTransaction.Draft),
                                     ReviewStart = a.ReviewStart,
                                     ReviewEnd = a.ReviewEnd,
                                     CreatedOn = a.CreatedOn,
                                     infoEmployee = new InfoEmployee
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
                                         //Grade = b.BusinessUnitJobLevelId == null ? "" :
                                         //                                  (from r in context.BusinessUnitJobLevels
                                         //                                   join s in context.JobGrades on r.JobGradeId equals s.Id
                                         //                                   where r.Id == b.BusinessUnitJobLevelId
                                         //                                   select s.GradeName).FirstOrDefault(),
                                         EmployementStatus = context.EmployementStatuss.Where(x => x.Id == b.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault(),
                                         //Division = b.BusinessUnitJobLevelId == null ? "" :
                                         //                                (from r in context.BusinessUnitJobLevels
                                         //                                 join s in context.BusinessUnits on r.BusinessUnitDivisionId equals s.Id
                                         //                                 join t in context.BusinessUnitTypes on s.BusinessUnitTypeId equals t.Id
                                         //                                 where t.BusinessUnitLevel == BusinessUnitLevelCode.Division && r.Id == b.BusinessUnitJobLevelId
                                         //                                 select s.UnitName).FirstOrDefault()
                                     }
                                 }).OrderByDescending(x => x.CreatedOn).ToListAsync();

            if (result == null) result = new List<PdrApprailsalM1Header>();
            result.AddRange(result2);
            return result.OrderByDescending(x => x.CreatedOn).ToList();
        }

        public async Task<PdrApprailsalM1Header> Get(string Id)
        {
            var result = await (from a in context.PdrApprailsalM1Headers
                                join b in context.EmployeeBasicInfos on a.EmployeeId equals b.Id
                                join c in context.TrTemplateApprovals on a.Id equals c.RefId
                                where !(a.IsDeleted ?? false) && a.Id == Id
                                select new PdrApprailsalM1Header
                                {
                                    Id = a.Id,
                                    EmployeeId = a.EmployeeId,
                                    Status = c.StatusApproved.GetHashCode(),
                                    StatusDesc = c.StatusApprovedDescription,
                                    ReviewStart = a.ReviewStart,
                                    ReviewEnd = a.ReviewEnd,
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
                                    infoEmployee = new InfoEmployee
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
                                        EmployementStatus = context.EmployementStatuss.Where(x => x.Id == b.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault(),
                                        //Division = b.BusinessUnitJobLevelId == null ? "" :
                                        //                                (from r in context.BusinessUnitJobLevels
                                        //                                 join s in context.BusinessUnits on r.BusinessUnitDivisionId equals s.Id
                                        //                                 join t in context.BusinessUnitTypes on s.BusinessUnitTypeId equals t.Id
                                        //                                 where t.BusinessUnitLevel == BusinessUnitLevelCode.Division && r.Id == b.BusinessUnitJobLevelId
                                        //                                 select s.UnitName).FirstOrDefault()
                                    }
                                }).FirstOrDefaultAsync();


            if (result == null)
            {
                result = await (from a in context.PdrApprailsalM1Headers
                                join b in context.EmployeeBasicInfos on a.EmployeeId equals b.Id
                                where !(a.IsDeleted ?? false) && a.Id == Id && !context.TrTemplateApprovals.Any(x => x.RefId == a.Id)
                                select new PdrApprailsalM1Header
                                {
                                    Id = a.Id,
                                    EmployeeId = a.EmployeeId,
                                    Status = StatusTransaction.Draft,
                                    StatusDesc = StatusTransaction.StatusName(StatusTransaction.Draft),
                                    ReviewStart = a.ReviewStart,
                                    ReviewEnd = a.ReviewEnd,
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
                                    infoEmployee = new InfoEmployee
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
                                        EmployementStatus = context.EmployementStatuss.Where(x => x.Id == b.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault(),
                                        //Division = b.BusinessUnitJobLevelId == null ? "" :
                                        //                                (from r in context.BusinessUnitJobLevels
                                        //                                 join s in context.BusinessUnits on r.BusinessUnitDivisionId equals s.Id
                                        //                                 join t in context.BusinessUnitTypes on s.BusinessUnitTypeId equals t.Id
                                        //                                 where t.BusinessUnitLevel == BusinessUnitLevelCode.Division && r.Id == b.BusinessUnitJobLevelId
                                        //                                 select s.UnitName).FirstOrDefault()
                                    }
                                }).FirstOrDefaultAsync();
            }

            return result;
        }

        public async Task<List<PdrApprailsalM1>> GetPdrApprailsalM1List(string PdrEmployeeId, string employeeId, DateTime? startReview)
        {
            var result = await (from a in context.PdrApprailsalM1s
                                join b in context.PdrObjectiveSettings on a.PdrObjectiveSettingId equals b.Id
                                where a.PdrApprailsalM1HeaderId == PdrEmployeeId
                                select new PdrApprailsalM1
                                {
                                    Id = a.Id,
                                    PdrApprailsalM1HeaderId = a.PdrApprailsalM1HeaderId,
                                    PdrObjectiveSettingId = a.PdrObjectiveSettingId,
                                    Target = a.Target,
                                    Achievement = a.Achievement,
                                    Weight = a.Weight,
                                    RatingScore = a.RatingScore,
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
                                    PdrObjectiveSettingItem = b.ObjectiveItem,
                                    WeightedScore = a.RatingScore * (a.Weight / 100)
                                }).ToListAsync();

            if (result.Count == 0)
            {
                result = await (from a in context.PdrObjectiveTargets
                                join b in context.PdrObjectiveTargetDetails on a.Id equals b.PdrObjectiveTargetId
                                join c in context.PdrObjectiveSettings on b.PdrObjectiveSettingId equals c.Id
                                where a.EmployeeId == employeeId && a.Year == Convert.ToDateTime(startReview).Year
                                select new PdrApprailsalM1
                                {
                                    Id = a.Id,
                                    PdrObjectiveSettingId = b.PdrObjectiveSettingId,
                                    Target = b.Target,
                                    Achievement = 0,
                                    Weight = 0,
                                    RatingScore = 0,
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
                                    PdrObjectiveSettingItem = c.ObjectiveItem,
                                    WeightedScore = 0
                                }).ToListAsync();
            }
            return result;
        }
    }
}