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
    public interface IPDRObjectiveTargetService : IDataServiceHris<PdrObjectiveTarget>
    {
        Task<List<PdrObjectiveTargetDetail>> GetPdrObjectiveTargetDetailList(string PdrEmployeeId);
    }

    public class PDRObjectiveTargetService : IPDRObjectiveTargetService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly Services.Um.IManageMenuService menuService;

        string idTemplate = "";

        /// <summary>
        /// And endpoint to manage PdrObjectiveTarget
        /// </summary>
        /// <param name="context">Database context</param>
        public PDRObjectiveTargetService(DataContext context, GlobalFunctionApproval globalFcApproval, Services.Um.IManageMenuService menuService)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.menuService = menuService;
        }

        public async Task<int> AddAsync(PdrObjectiveTarget entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            return await AddEdit(entity, false);
        }

        public async Task<int> EditAsync(PdrObjectiveTarget entity)
        {
            return await AddEdit(entity, true);
        }

        private async Task<int> AddEdit(PdrObjectiveTarget entity, bool isEdit)
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

                        var PdrObjectiveTargetDetails = context.PdrObjectiveTargetDetails.AsNoTracking().Where(x => x.PdrObjectiveTargetId == entity.Id).ToList();
                        if (PdrObjectiveTargetDetails.Count > 0)
                        {
                            context.PdrObjectiveTargetDetails.RemoveRange(PdrObjectiveTargetDetails);
                        }
                    }

                    if (entity.PdrObjectiveTargetDetails != null)
                    {
                        Parallel.ForEach(entity.PdrObjectiveTargetDetails, (item) =>
                        {
                            item.Id = Guid.NewGuid().ToString();
                            item.PdrObjectiveTargetId = entity.Id;
                            context.PhoenixAddAsync(item);
                        });
                    }

                    var save = await context.SaveChangesAsync();
                    transaction.Commit();
                    return save;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<int> DeleteSoftAsync(PdrObjectiveTarget entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<List<PdrObjectiveTarget>> Get()
        {
            var result = await (from a in context.PdrObjectiveTargets
                                join b in context.EmployeeBasicInfos on a.EmployeeId equals b.Id
                                where !(a.IsDeleted ?? false) && !context.TrTemplateApprovals.Any(x => x.RefId == a.Id)
                                select new PdrObjectiveTarget
                                {
                                    Id = a.Id,
                                    Year = a.Year,
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

        public async Task<PdrObjectiveTarget> Get(string Id)
        {
            var result = await (from a in context.PdrObjectiveTargets
                                join b in context.EmployeeBasicInfos on a.EmployeeId equals b.Id
                                where !(a.IsDeleted ?? false) && a.Id == Id
                                select new PdrObjectiveTarget
                                {
                                    Id = a.Id,
                                    EmployeeId = a.EmployeeId,
                                    Year = a.Year,
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

            return result;
        }

        public async Task<List<PdrObjectiveTargetDetail>> GetPdrObjectiveTargetDetailList(string PdrEmployeeId)
        {
            return await (from a in context.PdrObjectiveTargetDetails
                          join b in context.PdrObjectiveSettings on a.PdrObjectiveSettingId equals b.Id
                          where a.PdrObjectiveTargetId == PdrEmployeeId
                          select new PdrObjectiveTargetDetail
                          {
                              Id = a.Id,
                              PdrObjectiveTargetId = a.PdrObjectiveTargetId,
                              PdrObjectiveSettingId = a.PdrObjectiveSettingId,
                              Target = a.Target,
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
                          }).ToListAsync();
        }

        public Task<int> ApproveAsync(PdrObjectiveTarget entity)
        {
            throw new NotImplementedException();
        }
    }
}