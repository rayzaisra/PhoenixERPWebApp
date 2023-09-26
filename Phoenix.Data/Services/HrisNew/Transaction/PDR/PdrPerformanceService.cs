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
    public interface IPdrPerformanceService : IDataServiceHris<PdrEmployee>
    {
        Task<List<PdrEmployee>> GetApprovalList();
        Task<List<PdrDevelopmentPlan>> GetPdrDevelopmentPlanList(string PdrEmployeeId);
    }

    public class PdrPerformanceService : IPdrPerformanceService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly Services.Um.IManageMenuService menuService;

        /// <summary>
        /// And endpoint to manage PdrEmployee
        /// </summary>
        /// <param name="context">Database context</param>
        public PdrPerformanceService(DataContext context, GlobalFunctionApproval globalFcApproval, Services.Um.IManageMenuService menuService)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.menuService = menuService;
        }

        public async Task<int> AddAsync(PdrEmployee entity)
        {
            entity.Id = await TransID.GetTransId(context, Code.PDRPerformance, DateTime.Today);
            return await AddEdit(entity, false);
        }

        public async Task<int> EditAsync(PdrEmployee entity)
        {
            return await AddEdit(entity, true);
        }

        private async Task<int> AddEdit(PdrEmployee entity, bool isEdit)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (isEdit)
                    {
                        context.PhoenixEdit(entity);

                        if (entity.PdrRecomendation != null)
                        {
                            entity.PdrRecomendation.PdrEmployeeId = entity.Id;
                            context.PhoenixEdit(entity.PdrRecomendation);
                        }
                    }
                    else
                    {
                        await context.PhoenixAddAsync(entity);
                        await context.SaveChangesAsync();

                        if (entity.PdrRecomendation != null)
                        {
                            entity.PdrRecomendation.Id = Guid.NewGuid().ToString();
                            entity.PdrRecomendation.PdrEmployeeId = entity.Id;
                            await context.PhoenixAddAsync(entity.PdrRecomendation);
                        }
                    }

                    if (!string.IsNullOrEmpty(entity.Id))
                    {
                        var m2 = context.PdrDetailM2belows.AsNoTracking().Where(x => x.PdrEmployeeId == entity.Id).ToList();
                        if (m2.Count > 0)
                        {
                            context.PdrDetailM2belows.RemoveRange(m2);
                        }

                        var pdrOverallScores = context.PdrOverallScores.AsNoTracking().Where(x => x.PdrEmployeeId == entity.Id).ToList();
                        if (pdrOverallScores.Count > 0)
                        {
                            context.PdrOverallScores.RemoveRange(pdrOverallScores);
                        }

                        var pdrDevelopmentPlans = context.PdrDevelopmentPlans.AsNoTracking().Where(x => x.PdrEmployeeId == entity.Id).ToList();
                        if (pdrDevelopmentPlans.Count > 0)
                        {
                            context.PdrDevelopmentPlans.RemoveRange(pdrDevelopmentPlans);
                        }
                    }

                    if (entity.PdrDetailM2belows != null)
                    {
                        Parallel.ForEach(entity.PdrDetailM2belows, (item) =>
                        {
                            item.Id = Guid.NewGuid().ToString();
                            item.PdrEmployeeId = entity.Id;
                            context.PhoenixAddAsync(item);
                        });
                    }

                    if (entity.PdrOverallScores != null)
                    {
                        Parallel.ForEach(entity.PdrOverallScores, (item) =>
                        {
                            item.Id = Guid.NewGuid().ToString();
                            item.PdrEmployeeId = entity.Id;
                            context.PhoenixAddAsync(item);
                        });
                    }

                    if (entity.PdrDevelopmentPlans != null)
                    {
                        Parallel.ForEach(entity.PdrDevelopmentPlans, (item) =>
                        {
                            item.Id = Guid.NewGuid().ToString();
                            item.PdrEmployeeId = entity.Id;
                            context.PhoenixAddAsync(item);
                        });
                    }

                    var isApprove = true;
                    if (entity.Status != 0)//0: Draft
                    {

                        var dataMenu = await menuService.GetByUniqueName(MenuUnique.PdrPerformance).ConfigureAwait(false);
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            MenuId = dataMenu.Id,
                            RefId = entity.Id,
                            DetailLink = $"{ApprovalLink.PdrPerformance}?Id={entity.Id}&isApprove=true",
                            NotificationType = Phoenix.Data.Models.NotificationTypeCode.PdrPerformance,
                            Module = Phoenix.Data.Models.NotificationModuleType.ModulePM
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

        public async Task<int> ApproveAsync(PdrEmployee entity)
        {
            return await globalFcApproval.ProcessApproval<PdrEmployee>(entity.Id, entity.Status, entity.RemarkRejected, entity);
        }

        public async Task<int> DeleteSoftAsync(PdrEmployee entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<List<PdrEmployee>> GetApprovalList()
        {
            var result = await (from a in context.PdrEmployees
                                join b in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals b.Id
                                join c in context.TrTemplateApprovals on a.Id equals c.RefId
                                join d in context.TrUserApprovals on c.Id equals d.TrTempApprovalId
                                where !(a.IsDeleted ?? false) && d.StatusApproved == Models.Um.StatusApproved.CurrentApproval && d.EmployeeBasicInfoId == context.GetEmployeeId()
                                select new PdrEmployee
                                {
                                    Id = a.Id,
                                    Status = c.StatusApproved.GetHashCode(),
                                    StatusDesc = c.StatusApprovedDescription,
                                    ReviewStart = a.ReviewStart,
                                    ReviewEnd = a.ReviewEnd,
                                    FinalScore = a.FinalScore,
                                    Grade = a.Grade,
                                    DateAppraisal = a.DateAppraisal,
                                    DateSubmit = a.DateSubmit,
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

        public async Task<List<PdrEmployee>> Get()
        {
            var result = await (from a in context.PdrEmployees
                                join b in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals b.Id
                                join c in context.TrTemplateApprovals on a.Id equals c.RefId
                                where !(a.IsDeleted ?? false)
                                select new PdrEmployee
                                {
                                    Id = a.Id,
                                    Status = c.StatusApproved.GetHashCode(),
                                    StatusDesc = c.StatusApprovedDescription,
                                    ReviewStart = a.ReviewStart,
                                    ReviewEnd = a.ReviewEnd,
                                    FinalScore = a.FinalScore,
                                    Grade = a.Grade,
                                    DateAppraisal = a.DateAppraisal,
                                    DateSubmit = a.DateSubmit,
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

            var result2 = await (from a in context.PdrEmployees
                                 join b in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals b.Id
                                 where !(a.IsDeleted ?? false) && !context.TrTemplateApprovals.Any(x => x.RefId == a.Id)
                                 select new PdrEmployee
                                 {
                                     Id = a.Id,
                                     Status = StatusTransaction.Draft,
                                     StatusDesc = StatusTransaction.StatusName(StatusTransaction.Draft),
                                     ReviewStart = a.ReviewStart,
                                     ReviewEnd = a.ReviewEnd,
                                     FinalScore = a.FinalScore,
                                     Grade = a.Grade,
                                     DateAppraisal = a.DateAppraisal,
                                     DateSubmit = a.DateSubmit,
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

            if (result == null) result = new List<PdrEmployee>();
            result.AddRange(result2);
            return result.OrderByDescending(x => x.CreatedOn).ToList();
        }

        public async Task<PdrEmployee> Get(string Id)
        {
            var result = new PdrEmployee();
            if (!string.IsNullOrEmpty(Id))
            {
                result = await (from a in context.PdrEmployees
                                join b in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals b.Id
                                join c in context.TrTemplateApprovals on a.Id equals c.RefId
                                where !(a.IsDeleted ?? false) && a.Id == Id
                                select new PdrEmployee
                                {
                                    Id = a.Id,
                                    EmployeeBasicInfoId = a.EmployeeBasicInfoId,
                                    Status = c.StatusApproved.GetHashCode(),
                                    StatusDesc = c.StatusApprovedDescription,
                                    ReviewStart = a.ReviewStart,
                                    ReviewEnd = a.ReviewEnd,
                                    DateAppraisal = a.DateAppraisal,
                                    FinalScore = a.FinalScore,
                                    Grade = a.Grade,
                                    DateSubmit = a.DateSubmit,
                                    FileResult = a.FileResult,
                                    SalaryIncrease = a.SalaryIncrease,
                                    JobGradeId = a.JobGradeId,
                                    Notes = a.Notes,
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
                                    PdrCategorys = context.PdrCategorys.Include(x => x.PdrParameters).Where(x => !(x.IsDeleted ?? false))
                                                    .Select(x => new PdrCategory
                                                    {
                                                        Id = x.Id,
                                                        No = x.No,
                                                        Name = x.Name,
                                                        Description = x.Description,
                                                        Percentage = x.Percentage,
                                                        PdrParameters = x.PdrParameters.Where(y => !(y.IsDeleted ?? false)).ToList()
                                                    }).ToList(),
                                    PdrGrades = context.PdrGrades.Where(x => !(x.IsDeleted ?? false)).ToList(),
                                    PdrRecomendation = context.PdrRecomendations.Where(x => x.PdrEmployeeId == a.Id).FirstOrDefault(),
                                    PdrDetailM2belows = context.PdrDetailM2belows.Where(x => x.PdrEmployeeId == a.Id).ToList(),
                                    PdrDevelopmentPlans = context.PdrDevelopmentPlans.Where(x => x.PdrEmployeeId == a.Id).ToList(),
                                    PdrOverallScores = context.PdrOverallScores.Where(x => x.PdrEmployeeId == a.Id).ToList(),
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
                    result = await (from a in context.PdrEmployees
                                    join b in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals b.Id
                                    where !(a.IsDeleted ?? false) && a.Id == Id && !context.TrTemplateApprovals.Any(x => x.RefId == a.Id)
                                    select new PdrEmployee
                                    {
                                        Id = a.Id,
                                        EmployeeBasicInfoId = a.EmployeeBasicInfoId,
                                        Status = StatusTransaction.Draft,
                                        StatusDesc = StatusTransaction.StatusName(StatusTransaction.Draft),
                                        ReviewStart = a.ReviewStart,
                                        ReviewEnd = a.ReviewEnd,
                                        DateAppraisal = a.DateAppraisal,
                                        FinalScore = a.FinalScore,
                                        Grade = a.Grade,
                                        DateSubmit = a.DateSubmit,
                                        FileResult = a.FileResult,
                                        SalaryIncrease = a.SalaryIncrease,
                                        JobGradeId = a.JobGradeId,
                                        Notes = a.Notes,
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
                                        PdrCategorys = context.PdrCategorys.Include(x => x.PdrParameters).Where(x => !(x.IsDeleted ?? false))
                                                    .Select(x => new PdrCategory
                                                    {
                                                        Id = x.Id,
                                                        No = x.No,
                                                        Name = x.Name,
                                                        Description = x.Description,
                                                        Percentage = x.Percentage,
                                                        PdrParameters = x.PdrParameters.Where(y => !(y.IsDeleted ?? false)).ToList()
                                                    }).ToList(),
                                        PdrGrades = context.PdrGrades.Where(x => !(x.IsDeleted ?? false)).ToList(),
                                        PdrRecomendation = context.PdrRecomendations.Where(x => x.PdrEmployeeId == a.Id).FirstOrDefault(),
                                        PdrDetailM2belows = context.PdrDetailM2belows.Where(x => x.PdrEmployeeId == a.Id).ToList(),
                                        PdrDevelopmentPlans = context.PdrDevelopmentPlans.Where(x => x.PdrEmployeeId == a.Id).ToList(),
                                        PdrOverallScores = context.PdrOverallScores.Where(x => x.PdrEmployeeId == a.Id).ToList(),
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
            }
            else
            {
                result.PdrCategorys = await context.PdrCategorys.Include(x => x.PdrParameters).Where(x => !(x.IsDeleted ?? false))
                    .Select(x => new PdrCategory
                    {
                        Id = x.Id,
                        No = x.No,
                        Name = x.Name,
                        Description = x.Description,
                        Percentage = x.Percentage,
                        PdrParameters = x.PdrParameters.Where(y => !(y.IsDeleted ?? false)).ToList()
                    }).ToListAsync();
                result.PdrGrades = await context.PdrGrades.Where(x => !(x.IsDeleted ?? false)).ToListAsync();
            }
            return result;
        }

        public async Task<List<PdrDevelopmentPlan>> GetPdrDevelopmentPlanList(string PdrEmployeeId)
        {
            return await context.PdrDevelopmentPlans.Where(x => x.PdrEmployeeId == PdrEmployeeId).ToListAsync();
        }
    }
}