using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phoenix.ApiExtension.Extensions;
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public class TerminationService
    {
        readonly DataContext context;
        /// <summary>
        /// And endpoint to manage Termination
        /// </summary>
        /// <param name="context">Database context</param>
        public TerminationService(DataContext context)
        {
            this.context = context;
        }

        public async Task<List<Termination>> GetTerminationApproval(string[] terminationType, int[] status)
        {
            var termination = await (from a in context.Terminations
                                     join b in context.EmployeeBasicInfos on a.Requestor equals b.Id
                                     join q in context.TmTerminationTypes on a.TerminationType equals q.Id
                                     join w in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals w.Id
                                     join c in context.TrTemplateApprovals on a.Id equals c.RefId
                                     join d in context.TrUserApprovals on c.Id equals d.TrTempApprovalId
                                     where !(a.IsDeleted ?? false) && d.EmployeeBasicInfoId == context.GetEmployeeId() &&
                                     terminationType.Any(x => x == a.TerminationType) && (status == null ? true : status.Any(x => x == d.StatusApproved.GetHashCode()))
                                     select new Termination
                                     {
                                         Id = a.Id,
                                         EmployeeBasicInfoId = a.EmployeeBasicInfoId,
                                         LastDayDate = a.LastDayDate,
                                         EffectiveResignDate = a.EffectiveResignDate,
                                         ResignLetter = a.ResignLetter,
                                         EndOfContract = a.EndOfContract,
                                         Penalty = a.Penalty,
                                         TrainingBonding = a.TrainingBonding,
                                         Reason = a.Reason,
                                         TerminationType = a.TerminationType,
                                         TerminationDate = a.TerminationDate,
                                         MedicalCertificate = a.MedicalCertificate,
                                         DateOfSick = a.DateOfSick,
                                         SalaryCalculation = a.SalaryCalculation,
                                         FaulsNoticeDate = a.FaulsNoticeDate,
                                         InterviewWarningDate = a.InterviewWarningDate,
                                         FaultCategoryId = a.FaultCategoryId,
                                         InterviewWarningFile = a.InterviewWarningFile,
                                         StatementOfPoliceReport = a.StatementOfPoliceReport,
                                         WarningLetter = a.WarningLetter,
                                         WarningLetterDate = a.WarningLetterDate,
                                         IsHcProcess = a.IsHcProcess,
                                         IsHcApprove = a.IsHcApprove,
                                         Status = c.StatusApproved.GetHashCode(),
                                         StatusDesc = c.StatusApprovedDescription,
                                         StatusInterview = a.StatusInterview,
                                         StatusChecklist = a.StatusChecklist,
                                         RequestDate = a.RequestDate,
                                         Requestor = a.Requestor,
                                         TerminationInterviewDate = a.TerminationInterviewDate,
                                         Notes = a.Notes,
                                         ExitInterviewFiles = a.ExitInterviewFiles,
                                         ReplacementStatus = a.ReplacementStatus,
                                         IsChecklist = a.IsChecklist,
                                         IsInterview = a.IsInterview,
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
                                         TerminationTypeName = q.TypeName,
                                         requestEmployee = new InfoEmployee
                                         {
                                             EmployeeId = b.Id,
                                             EmployeeName = b.NameEmployee,
                                             DateEmployed = b.JoinDate,
                                             Phone = b.PhoneNumber,
                                             Email = b.Email,
                                             DateBirth = b.DateBirth,
                                             Age = Convert.ToDateTime(b.DateBirth).AddYears(DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) > DateTime.Now ?
                                             (DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) - 1 : DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year,
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
                                             EmployementStatusId = b.EmployementStatusId,
                                             EmployementStatus = context.EmployementStatuss.Where(x => x.Id == b.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault(),
                                             Division = b.BusinessUnitJobLevelId == null ? "" :
                                                                            (from r in context.BusinessUnitJobLevels
                                                                             join s in context.BusinessUnits on r.BusinessUnitDivisionId equals s.Id
                                                                             join t in context.BusinessUnitTypes on s.BusinessUnitTypeId equals t.Id
                                                                             where t.BusinessUnitLevel == BusinessUnitLevelCode.Division && r.Id == b.BusinessUnitJobLevelId
                                                                             select s.UnitName).FirstOrDefault(),
                                             LegalEntityId = b.LegalEntityId,
                                             LegalEntityName = (from h in context.LegalEntity
                                                                where h.Id == b.LegalEntityId
                                                                select h.LegalEntityName).FirstOrDefault()
                                         },
                                         infoEmployee = new InfoEmployee
                                         {
                                             EmployeeId = w.Id,
                                             EmployeeName = w.NameEmployee,
                                             DateEmployed = w.JoinDate,
                                             Phone = w.PhoneNumber,
                                             Email = w.Email,
                                             DateBirth = w.DateBirth,
                                             Age = Convert.ToDateTime(w.DateBirth).AddYears(DateTime.Now.Year - Convert.ToDateTime(w.DateBirth).Year) > DateTime.Now ?
                                             (DateTime.Now.Year - Convert.ToDateTime(w.DateBirth).Year) - 1 : DateTime.Now.Year - Convert.ToDateTime(w.DateBirth).Year,
                                             JobTitle = w.BusinessUnitJobLevelId == null ? "" :
                                                         (from r in context.BusinessUnitJobLevels
                                                          join s in context.JobTitles on r.JobTitleId equals s.Id
                                                          where r.Id == w.BusinessUnitJobLevelId
                                                          select s.TitleName).FirstOrDefault(),
                                             Grade = w.BusinessUnitJobLevelId == null ? "" :
                                                         (from r in context.BusinessUnitJobLevels
                                                          join s in context.JobGrades on r.JobGradeId equals s.Id
                                                          where r.Id == w.BusinessUnitJobLevelId
                                                          select s.GradeName).FirstOrDefault(),
                                             EmployementStatusId = b.EmployementStatusId,
                                             EmployementStatus = context.EmployementStatuss.Where(x => x.Id == w.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault(),
                                             Division = w.BusinessUnitJobLevelId == null ? "" :
                                                                            (from r in context.BusinessUnitJobLevels
                                                                             join s in context.BusinessUnits on r.BusinessUnitDivisionId equals s.Id
                                                                             join t in context.BusinessUnitTypes on s.BusinessUnitTypeId equals t.Id
                                                                             where t.BusinessUnitLevel == BusinessUnitLevelCode.Division && r.Id == w.BusinessUnitJobLevelId
                                                                             select s.UnitName).FirstOrDefault(),
                                             LegalEntityId = w.LegalEntityId,
                                             LegalEntityName = (from h in context.LegalEntity
                                                                where h.Id == w.LegalEntityId
                                                                select h.LegalEntityName).FirstOrDefault()
                                         }
                                     }).OrderByDescending(x => x.CreatedOn).ToListAsync();

            return termination;
        }

        public async Task<List<Termination>> GetTermination(string employeeId, string[] terminationType)
        {
            var termination = await (from a in context.Terminations
                                     join b in context.EmployeeBasicInfos on a.Requestor equals b.Id
                                     join q in context.TmTerminationTypes on a.TerminationType equals q.Id
                                     join w in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals w.Id
                                     join c in context.TrTemplateApprovals on a.Id equals c.RefId
                                     where !(a.IsDeleted ?? false) && ((string.IsNullOrEmpty(employeeId) ? true : b.Id == employeeId) || (string.IsNullOrEmpty(employeeId) ? true : w.Id == employeeId)) &&
                                     terminationType.Any(x => x == a.TerminationType)
                                     select new Termination
                                     {
                                         Id = a.Id,
                                         EmployeeBasicInfoId = a.EmployeeBasicInfoId,
                                         LastDayDate = a.LastDayDate,
                                         EffectiveResignDate = a.EffectiveResignDate,
                                         ResignLetter = a.ResignLetter,
                                         EndOfContract = a.EndOfContract,
                                         Penalty = a.Penalty,
                                         TrainingBonding = a.TrainingBonding,
                                         Reason = a.Reason,
                                         TerminationType = a.TerminationType,
                                         TerminationDate = a.TerminationDate,
                                         MedicalCertificate = a.MedicalCertificate,
                                         DateOfSick = a.DateOfSick,
                                         SalaryCalculation = a.SalaryCalculation,
                                         FaulsNoticeDate = a.FaulsNoticeDate,
                                         InterviewWarningDate = a.InterviewWarningDate,
                                         FaultCategoryId = a.FaultCategoryId,
                                         InterviewWarningFile = a.InterviewWarningFile,
                                         StatementOfPoliceReport = a.StatementOfPoliceReport,
                                         WarningLetter = a.WarningLetter,
                                         WarningLetterDate = a.WarningLetterDate,
                                         IsHcProcess = a.IsHcProcess,
                                         IsHcApprove = a.IsHcApprove,
                                         Status = c.StatusApproved.GetHashCode(),
                                         StatusDesc = c.StatusApprovedDescription,
                                         StatusInterview = a.StatusInterview,
                                         StatusChecklist = a.StatusChecklist,
                                         RequestDate = a.RequestDate,
                                         Requestor = a.Requestor,
                                         TerminationInterviewDate = a.TerminationInterviewDate,
                                         Notes = a.Notes,
                                         ExitInterviewFiles = a.ExitInterviewFiles,
                                         ReplacementStatus = a.ReplacementStatus,
                                         IsChecklist = a.IsChecklist,
                                         IsInterview = a.IsInterview,
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
                                         TerminationTypeName = q.TypeName,
                                         requestEmployee = new InfoEmployee
                                         {
                                             EmployeeId = b.Id,
                                             EmployeeName = b.NameEmployee,
                                             DateEmployed = b.JoinDate,
                                             Phone = b.PhoneNumber,
                                             Email = b.Email,
                                             DateBirth = b.DateBirth,
                                             Age = Convert.ToDateTime(b.DateBirth).AddYears(DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) > DateTime.Now ?
                                             (DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) - 1 : DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year,
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
                                             EmployementStatusId = b.EmployementStatusId,
                                             EmployementStatus = context.EmployementStatuss.Where(x => x.Id == b.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault(),
                                             Division = b.BusinessUnitJobLevelId == null ? "" :
                                                                            (from r in context.BusinessUnitJobLevels
                                                                             join s in context.BusinessUnits on r.BusinessUnitDivisionId equals s.Id
                                                                             join t in context.BusinessUnitTypes on s.BusinessUnitTypeId equals t.Id
                                                                             where t.BusinessUnitLevel == BusinessUnitLevelCode.Division && r.Id == b.BusinessUnitJobLevelId
                                                                             select s.UnitName).FirstOrDefault(),
                                             LegalEntityId = b.LegalEntityId,
                                             LegalEntityName = (from h in context.LegalEntity
                                                                where h.Id == b.LegalEntityId
                                                                select h.LegalEntityName).FirstOrDefault()
                                         },
                                         infoEmployee = new InfoEmployee
                                         {
                                             EmployeeId = w.Id,
                                             EmployeeName = w.NameEmployee,
                                             DateEmployed = w.JoinDate,
                                             Phone = w.PhoneNumber,
                                             Email = w.Email,
                                             DateBirth = w.DateBirth,
                                             Age = Convert.ToDateTime(w.DateBirth).AddYears(DateTime.Now.Year - Convert.ToDateTime(w.DateBirth).Year) > DateTime.Now ?
                                             (DateTime.Now.Year - Convert.ToDateTime(w.DateBirth).Year) - 1 : DateTime.Now.Year - Convert.ToDateTime(w.DateBirth).Year,
                                             JobTitle = w.BusinessUnitJobLevelId == null ? "" :
                                                         (from r in context.BusinessUnitJobLevels
                                                          join s in context.JobTitles on r.JobTitleId equals s.Id
                                                          where r.Id == w.BusinessUnitJobLevelId
                                                          select s.TitleName).FirstOrDefault(),
                                             Grade = w.BusinessUnitJobLevelId == null ? "" :
                                                         (from r in context.BusinessUnitJobLevels
                                                          join s in context.JobGrades on r.JobGradeId equals s.Id
                                                          where r.Id == w.BusinessUnitJobLevelId
                                                          select s.GradeName).FirstOrDefault(),
                                             EmployementStatusId = b.EmployementStatusId,
                                             EmployementStatus = context.EmployementStatuss.Where(x => x.Id == w.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault(),
                                             Division = w.BusinessUnitJobLevelId == null ? "" :
                                                                            (from r in context.BusinessUnitJobLevels
                                                                             join s in context.BusinessUnits on r.BusinessUnitDivisionId equals s.Id
                                                                             join t in context.BusinessUnitTypes on s.BusinessUnitTypeId equals t.Id
                                                                             where t.BusinessUnitLevel == BusinessUnitLevelCode.Division && r.Id == w.BusinessUnitJobLevelId
                                                                             select s.UnitName).FirstOrDefault(),
                                             LegalEntityId = w.LegalEntityId,
                                             LegalEntityName = (from h in context.LegalEntity
                                                                where h.Id == w.LegalEntityId
                                                                select h.LegalEntityName).FirstOrDefault()
                                         }
                                     }).OrderByDescending(x => x.CreatedOn).ToListAsync();

            var termination2 = await (from a in context.Terminations
                                      join b in context.EmployeeBasicInfos on a.Requestor equals b.Id
                                      join q in context.TmTerminationTypes on a.TerminationType equals q.Id
                                      join w in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals w.Id
                                      where !(a.IsDeleted ?? false) && ((string.IsNullOrEmpty(employeeId) ? true : b.Id == employeeId) || (string.IsNullOrEmpty(employeeId) ? true : w.Id == employeeId)) &&
                                      terminationType.Any(x => x == a.TerminationType) && !context.TrTemplateApprovals.Any(x => x.RefId == a.Id)
                                      select new Termination
                                      {
                                          Id = a.Id,
                                          EmployeeBasicInfoId = a.EmployeeBasicInfoId,
                                          LastDayDate = a.LastDayDate,
                                          EffectiveResignDate = a.EffectiveResignDate,
                                          ResignLetter = a.ResignLetter,
                                          EndOfContract = a.EndOfContract,
                                          Penalty = a.Penalty,
                                          TrainingBonding = a.TrainingBonding,
                                          Reason = a.Reason,
                                          TerminationType = a.TerminationType,
                                          TerminationDate = a.TerminationDate,
                                          MedicalCertificate = a.MedicalCertificate,
                                          DateOfSick = a.DateOfSick,
                                          SalaryCalculation = a.SalaryCalculation,
                                          FaulsNoticeDate = a.FaulsNoticeDate,
                                          InterviewWarningDate = a.InterviewWarningDate,
                                          FaultCategoryId = a.FaultCategoryId,
                                          InterviewWarningFile = a.InterviewWarningFile,
                                          StatementOfPoliceReport = a.StatementOfPoliceReport,
                                          WarningLetter = a.WarningLetter,
                                          WarningLetterDate = a.WarningLetterDate,
                                          IsHcProcess = a.IsHcProcess,
                                          IsHcApprove = a.IsHcApprove,
                                          Status = StatusTransaction.Draft,
                                          StatusDesc = StatusTransaction.StatusName(StatusTransaction.Draft),
                                          StatusInterview = a.StatusInterview,
                                          StatusChecklist = a.StatusChecklist,
                                          RequestDate = a.RequestDate,
                                          Requestor = a.Requestor,
                                          TerminationInterviewDate = a.TerminationInterviewDate,
                                          Notes = a.Notes,
                                          ExitInterviewFiles = a.ExitInterviewFiles,
                                          ReplacementStatus = a.ReplacementStatus,
                                          IsChecklist = a.IsChecklist,
                                          IsInterview = a.IsInterview,
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
                                          TerminationTypeName = q.TypeName,
                                          requestEmployee = new InfoEmployee
                                          {
                                              EmployeeId = b.Id,
                                              EmployeeName = b.NameEmployee,
                                              DateEmployed = b.JoinDate,
                                              Phone = b.PhoneNumber,
                                              Email = b.Email,
                                              DateBirth = b.DateBirth,
                                              Age = Convert.ToDateTime(b.DateBirth).AddYears(DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) > DateTime.Now ?
                                             (DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) - 1 : DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year,
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
                                              EmployementStatusId = b.EmployementStatusId,
                                              EmployementStatus = context.EmployementStatuss.Where(x => x.Id == b.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault(),
                                              Division = b.BusinessUnitJobLevelId == null ? "" :
                                                                            (from r in context.BusinessUnitJobLevels
                                                                             join s in context.BusinessUnits on r.BusinessUnitDivisionId equals s.Id
                                                                             join t in context.BusinessUnitTypes on s.BusinessUnitTypeId equals t.Id
                                                                             where t.BusinessUnitLevel == BusinessUnitLevelCode.Division && r.Id == b.BusinessUnitJobLevelId
                                                                             select s.UnitName).FirstOrDefault(),
                                              LegalEntityId = b.LegalEntityId,
                                              LegalEntityName = (from h in context.LegalEntity
                                                                 where h.Id == b.LegalEntityId
                                                                 select h.LegalEntityName).FirstOrDefault()
                                          },
                                          infoEmployee = new InfoEmployee
                                          {
                                              EmployeeId = w.Id,
                                              EmployeeName = w.NameEmployee,
                                              DateEmployed = w.JoinDate,
                                              Phone = w.PhoneNumber,
                                              Email = w.Email,
                                              DateBirth = w.DateBirth,
                                              Age = Convert.ToDateTime(w.DateBirth).AddYears(DateTime.Now.Year - Convert.ToDateTime(w.DateBirth).Year) > DateTime.Now ?
                                             (DateTime.Now.Year - Convert.ToDateTime(w.DateBirth).Year) - 1 : DateTime.Now.Year - Convert.ToDateTime(w.DateBirth).Year,
                                              JobTitle = w.BusinessUnitJobLevelId == null ? "" :
                                                         (from r in context.BusinessUnitJobLevels
                                                          join s in context.JobTitles on r.JobTitleId equals s.Id
                                                          where r.Id == w.BusinessUnitJobLevelId
                                                          select s.TitleName).FirstOrDefault(),
                                              Grade = w.BusinessUnitJobLevelId == null ? "" :
                                                         (from r in context.BusinessUnitJobLevels
                                                          join s in context.JobGrades on r.JobGradeId equals s.Id
                                                          where r.Id == w.BusinessUnitJobLevelId
                                                          select s.GradeName).FirstOrDefault(),
                                              EmployementStatusId = b.EmployementStatusId,
                                              EmployementStatus = context.EmployementStatuss.Where(x => x.Id == w.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault(),
                                              Division = w.BusinessUnitJobLevelId == null ? "" :
                                                                            (from r in context.BusinessUnitJobLevels
                                                                             join s in context.BusinessUnits on r.BusinessUnitDivisionId equals s.Id
                                                                             join t in context.BusinessUnitTypes on s.BusinessUnitTypeId equals t.Id
                                                                             where t.BusinessUnitLevel == BusinessUnitLevelCode.Division && r.Id == w.BusinessUnitJobLevelId
                                                                             select s.UnitName).FirstOrDefault(),
                                              LegalEntityId = w.LegalEntityId,
                                              LegalEntityName = (from h in context.LegalEntity
                                                                 where h.Id == w.LegalEntityId
                                                                 select h.LegalEntityName).FirstOrDefault()
                                          }
                                      }).OrderByDescending(x => x.CreatedOn).ToListAsync();

            if (termination == null) termination = new List<Termination>();
            termination.AddRange(termination2);
            return termination.OrderByDescending(x => x.CreatedOn).ToList();
        }

        public async Task<List<Termination>> GetTerminationHC(string[] terminationType, bool isHcProcess)
        {
            var termination = await (from a in context.Terminations
                                     join b in context.EmployeeBasicInfos on a.Requestor equals b.Id
                                     join q in context.TmTerminationTypes on a.TerminationType equals q.Id
                                     join w in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals w.Id
                                     where !(a.IsDeleted ?? false) &&
                                     terminationType.Any(x => x == a.TerminationType) && a.IsHcProcess == true && (isHcProcess ? true : a.IsHcApprove == true)
                                     select new Termination
                                     {
                                         Id = a.Id,
                                         EmployeeBasicInfoId = a.EmployeeBasicInfoId,
                                         LastDayDate = a.LastDayDate,
                                         EffectiveResignDate = a.EffectiveResignDate,
                                         ResignLetter = a.ResignLetter,
                                         EndOfContract = a.EndOfContract,
                                         Penalty = a.Penalty,
                                         TrainingBonding = a.TrainingBonding,
                                         Reason = a.Reason,
                                         TerminationType = a.TerminationType,
                                         TerminationDate = a.TerminationDate,
                                         MedicalCertificate = a.MedicalCertificate,
                                         DateOfSick = a.DateOfSick,
                                         SalaryCalculation = a.SalaryCalculation,
                                         FaulsNoticeDate = a.FaulsNoticeDate,
                                         InterviewWarningDate = a.InterviewWarningDate,
                                         FaultCategoryId = a.FaultCategoryId,
                                         InterviewWarningFile = a.InterviewWarningFile,
                                         StatementOfPoliceReport = a.StatementOfPoliceReport,
                                         WarningLetter = a.WarningLetter,
                                         WarningLetterDate = a.WarningLetterDate,
                                         IsHcProcess = a.IsHcProcess,
                                         IsHcApprove = a.IsHcApprove,
                                         //Status = c.StatusApproved.GetHashCode(),
                                         //StatusDesc = c.StatusApprovedDescription,
                                         StatusInterview = a.StatusInterview,
                                         StatusChecklist = a.StatusChecklist,
                                         RequestDate = a.RequestDate,
                                         Requestor = a.Requestor,
                                         TerminationInterviewDate = a.TerminationInterviewDate,
                                         Notes = a.Notes,
                                         ExitInterviewFiles = a.ExitInterviewFiles,
                                         ReplacementStatus = a.ReplacementStatus,
                                         IsChecklist = a.IsChecklist,
                                         IsInterview = a.IsInterview,
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
                                         TerminationTypeName = q.TypeName,
                                         requestEmployee = new InfoEmployee
                                         {
                                             EmployeeId = b.Id,
                                             EmployeeName = b.NameEmployee,
                                             DateEmployed = b.JoinDate,
                                             Phone = b.PhoneNumber,
                                             Email = b.Email,
                                             DateBirth = b.DateBirth,
                                             Age = Convert.ToDateTime(b.DateBirth).AddYears(DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) > DateTime.Now ?
                                             (DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) - 1 : DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year,
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
                                             EmployementStatusId = b.EmployementStatusId,
                                             EmployementStatus = context.EmployementStatuss.Where(x => x.Id == b.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault(),
                                             Division = b.BusinessUnitJobLevelId == null ? "" :
                                                                            (from r in context.BusinessUnitJobLevels
                                                                             join s in context.BusinessUnits on r.BusinessUnitDivisionId equals s.Id
                                                                             join t in context.BusinessUnitTypes on s.BusinessUnitTypeId equals t.Id
                                                                             where t.BusinessUnitLevel == BusinessUnitLevelCode.Division && r.Id == b.BusinessUnitJobLevelId
                                                                             select s.UnitName).FirstOrDefault(),
                                             LegalEntityId = b.LegalEntityId,
                                             LegalEntityName = (from h in context.LegalEntity
                                                                where h.Id == b.LegalEntityId
                                                                select h.LegalEntityName).FirstOrDefault()
                                         },
                                         infoEmployee = new InfoEmployee
                                         {
                                             EmployeeId = w.Id,
                                             EmployeeName = w.NameEmployee,
                                             DateEmployed = w.JoinDate,
                                             Phone = w.PhoneNumber,
                                             Email = w.Email,
                                             DateBirth = w.DateBirth,
                                             Age = Convert.ToDateTime(w.DateBirth).AddYears(DateTime.Now.Year - Convert.ToDateTime(w.DateBirth).Year) > DateTime.Now ?
                                             (DateTime.Now.Year - Convert.ToDateTime(w.DateBirth).Year) - 1 : DateTime.Now.Year - Convert.ToDateTime(w.DateBirth).Year,
                                             JobTitle = w.BusinessUnitJobLevelId == null ? "" :
                                                         (from r in context.BusinessUnitJobLevels
                                                          join s in context.JobTitles on r.JobTitleId equals s.Id
                                                          where r.Id == w.BusinessUnitJobLevelId
                                                          select s.TitleName).FirstOrDefault(),
                                             Grade = w.BusinessUnitJobLevelId == null ? "" :
                                                         (from r in context.BusinessUnitJobLevels
                                                          join s in context.JobGrades on r.JobGradeId equals s.Id
                                                          where r.Id == w.BusinessUnitJobLevelId
                                                          select s.GradeName).FirstOrDefault(),
                                             EmployementStatusId = b.EmployementStatusId,
                                             EmployementStatus = context.EmployementStatuss.Where(x => x.Id == w.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault(),
                                             Division = w.BusinessUnitJobLevelId == null ? "" :
                                                                            (from r in context.BusinessUnitJobLevels
                                                                             join s in context.BusinessUnits on r.BusinessUnitDivisionId equals s.Id
                                                                             join t in context.BusinessUnitTypes on s.BusinessUnitTypeId equals t.Id
                                                                             where t.BusinessUnitLevel == BusinessUnitLevelCode.Division && r.Id == w.BusinessUnitJobLevelId
                                                                             select s.UnitName).FirstOrDefault(),
                                             LegalEntityId = w.LegalEntityId,
                                             LegalEntityName = (from h in context.LegalEntity
                                                                where h.Id == w.LegalEntityId
                                                                select h.LegalEntityName).FirstOrDefault()
                                         }
                                     }).OrderByDescending(x => x.RequestDate).ToListAsync();

            return termination;
        }

        public async Task<Termination> GetTerminationById(string employeeId, string[] terminationType, string id)
        {
            var termination = await (from a in context.Terminations
                                     join b in context.EmployeeBasicInfos on a.Requestor equals b.Id
                                     join q in context.TmTerminationTypes on a.TerminationType equals q.Id
                                     join w in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals w.Id
                                     join d in context.TrTemplateApprovals on a.Id equals d.RefId
                                     where !(a.IsDeleted ?? false) && //&& (string.IsNullOrEmpty(employeeId) ? true : w.Id == employeeId)
									 terminationType.Any(x => x == a.TerminationType) && a.Id == id
                                     select new Termination
                                     {
                                         Id = a.Id,
                                         EmployeeBasicInfoId = a.EmployeeBasicInfoId,
                                         LastDayDate = a.LastDayDate,
                                         EffectiveResignDate = a.EffectiveResignDate,
                                         ResignLetter = a.ResignLetter,
                                         EndOfContract = a.EndOfContract,
                                         Penalty = a.Penalty,
                                         TrainingBonding = a.TrainingBonding,
                                         Reason = a.Reason,
                                         TerminationType = a.TerminationType,
                                         TerminationDate = a.TerminationDate,
                                         MedicalCertificate = a.MedicalCertificate,
                                         DateOfSick = a.DateOfSick,
                                         SalaryCalculation = a.SalaryCalculation,
                                         FaulsNoticeDate = a.FaulsNoticeDate,
                                         InterviewWarningDate = a.InterviewWarningDate,
                                         FaultCategoryId = a.FaultCategoryId,
                                         InterviewWarningFile = a.InterviewWarningFile,
                                         StatementOfPoliceReport = a.StatementOfPoliceReport,
                                         WarningLetter = a.WarningLetter,
                                         WarningLetterDate = a.WarningLetterDate,
                                         IsHcProcess = a.IsHcProcess,
                                         IsHcApprove = a.IsHcApprove,
                                         Status = d.StatusApproved.GetHashCode(),
                                         StatusInterview = a.StatusInterview,
                                         StatusChecklist = a.StatusChecklist,
                                         RequestDate = a.RequestDate,
                                         Requestor = a.Requestor,
                                         TerminationInterviewDate = a.TerminationInterviewDate,
                                         Notes = a.Notes,
                                         ExitInterviewFiles = a.ExitInterviewFiles,
                                         ReplacementStatus = a.ReplacementStatus,
                                         IsChecklist = a.IsChecklist,
                                         IsInterview = a.IsInterview,
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
                                         TerminationTypeName = q.TypeName,
                                         requestEmployee = new InfoEmployee
                                         {
                                             EmployeeId = b.Id,
                                             EmployeeName = b.NameEmployee,
                                             DateEmployed = b.JoinDate,
                                             Phone = b.PhoneNumber,
                                             Email = b.Email,
                                             DateBirth = b.DateBirth,
                                             Age = Convert.ToDateTime(b.DateBirth).AddYears(DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) > DateTime.Now ?
                                                   (DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) - 1 : DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year,
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
                                             EmployementStatusId = b.EmployementStatusId,
                                             EmployementStatus = context.EmployementStatuss.Where(x => x.Id == b.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault(),
                                             EmployeeContractDocument = context.EmployeeContracts.Where(x => x.EmployeeBasicInfoId == a.EmployeeBasicInfoId).Select(x => x.FilemasterId).FirstOrDefault(),
                                             EmployeeContractDocumentName = (from r in context.EmployeeContracts
                                                                             join s in context.Filemasters on r.FilemasterId equals s.Id
                                                                             where r.EmployeeBasicInfoId == a.EmployeeBasicInfoId
                                                                             select s.Name).FirstOrDefault(),
                                             Division = b.BusinessUnitJobLevelId == null ? "" :
                                                                            (from r in context.BusinessUnitJobLevels
                                                                             join s in context.BusinessUnits on r.BusinessUnitDivisionId equals s.Id
                                                                             join t in context.BusinessUnitTypes on s.BusinessUnitTypeId equals t.Id
                                                                             where t.BusinessUnitLevel == BusinessUnitLevelCode.Division && r.Id == b.BusinessUnitJobLevelId
                                                                             select s.UnitName).FirstOrDefault(),
                                             LegalEntityId = b.LegalEntityId,
                                             LegalEntityName = (from h in context.LegalEntity
                                                                where h.Id == b.LegalEntityId
                                                                select h.LegalEntityName).FirstOrDefault()
                                         },
                                         infoEmployee = new InfoEmployee
                                         {
                                             EmployeeId = w.Id,
                                             EmployeeName = w.NameEmployee,
                                             DateEmployed = w.JoinDate,
                                             Phone = w.PhoneNumber,
                                             Email = w.Email,
                                             DateBirth = w.DateBirth,
                                             Age = Convert.ToDateTime(w.DateBirth).AddYears(DateTime.Now.Year - Convert.ToDateTime(w.DateBirth).Year) > DateTime.Now ?
                                                   (DateTime.Now.Year - Convert.ToDateTime(w.DateBirth).Year) - 1 : DateTime.Now.Year - Convert.ToDateTime(w.DateBirth).Year,
                                             JobTitle = w.BusinessUnitJobLevelId == null ? "" :
                                                               (from r in context.BusinessUnitJobLevels
                                                                join s in context.JobTitles on r.JobTitleId equals s.Id
                                                                where r.Id == w.BusinessUnitJobLevelId
                                                                select s.TitleName).FirstOrDefault(),
                                             Grade = w.BusinessUnitJobLevelId == null ? "" :
                                                               (from r in context.BusinessUnitJobLevels
                                                                join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                where r.Id == w.BusinessUnitJobLevelId
                                                                select s.GradeName).FirstOrDefault(),
                                             EmployementStatusId = b.EmployementStatusId,
                                             EmployementStatus = context.EmployementStatuss.Where(x => x.Id == w.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault(),
                                             EmployeeContractDocument = context.EmployeeContracts.Where(x => x.EmployeeBasicInfoId == a.EmployeeBasicInfoId).Select(x => x.FilemasterId).FirstOrDefault(),
                                             EmployeeContractDocumentName = (from r in context.EmployeeContracts
                                                                             join s in context.Filemasters on r.FilemasterId equals s.Id
                                                                             where r.EmployeeBasicInfoId == a.EmployeeBasicInfoId
                                                                             select s.Name).FirstOrDefault(),
                                             Division = w.BusinessUnitJobLevelId == null ? "" :
                                                                            (from r in context.BusinessUnitJobLevels
                                                                             join s in context.BusinessUnits on r.BusinessUnitDivisionId equals s.Id
                                                                             join t in context.BusinessUnitTypes on s.BusinessUnitTypeId equals t.Id
                                                                             where t.BusinessUnitLevel == BusinessUnitLevelCode.Division && r.Id == w.BusinessUnitJobLevelId
                                                                             select s.UnitName).FirstOrDefault(),
                                             LegalEntityId = w.LegalEntityId,
                                             LegalEntityName = (from h in context.LegalEntity
                                                                where h.Id == w.LegalEntityId
                                                                select h.LegalEntityName).FirstOrDefault()
                                         }
                                     }).FirstOrDefaultAsync();

            if (termination == null)
            {
                termination = await (from a in context.Terminations
                                     join b in context.EmployeeBasicInfos on a.Requestor equals b.Id
                                     join q in context.TmTerminationTypes on a.TerminationType equals q.Id
                                     join w in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals w.Id
                                     where !(a.IsDeleted ?? false) && (string.IsNullOrEmpty(employeeId) ? true : b.Id == employeeId) &&
                                     terminationType.Any(x => x == a.TerminationType) && a.Id == id
                                     select new Termination
                                     {
                                         Id = a.Id,
                                         EmployeeBasicInfoId = a.EmployeeBasicInfoId,
                                         LastDayDate = a.LastDayDate,
                                         EffectiveResignDate = a.EffectiveResignDate,
                                         ResignLetter = a.ResignLetter,
                                         EndOfContract = a.EndOfContract,
                                         Penalty = a.Penalty,
                                         TrainingBonding = a.TrainingBonding,
                                         Reason = a.Reason,
                                         TerminationType = a.TerminationType,
                                         TerminationDate = a.TerminationDate,
                                         MedicalCertificate = a.MedicalCertificate,
                                         DateOfSick = a.DateOfSick,
                                         SalaryCalculation = a.SalaryCalculation,
                                         FaulsNoticeDate = a.FaulsNoticeDate,
                                         InterviewWarningDate = a.InterviewWarningDate,
                                         FaultCategoryId = a.FaultCategoryId,
                                         InterviewWarningFile = a.InterviewWarningFile,
                                         StatementOfPoliceReport = a.StatementOfPoliceReport,
                                         WarningLetter = a.WarningLetter,
                                         WarningLetterDate = a.WarningLetterDate,
                                         IsHcProcess = a.IsHcProcess,
                                         IsHcApprove = a.IsHcApprove,
                                         Status = StatusTransaction.Draft,//DRAFT
                                         StatusInterview = a.StatusInterview,
                                         StatusChecklist = a.StatusChecklist,
                                         RequestDate = a.RequestDate,
                                         Requestor = a.Requestor,
                                         TerminationInterviewDate = a.TerminationInterviewDate,
                                         Notes = a.Notes,
                                         ExitInterviewFiles = a.ExitInterviewFiles,
                                         ReplacementStatus = a.ReplacementStatus,
                                         IsChecklist = a.IsChecklist,
                                         IsInterview = a.IsInterview,
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
                                         TerminationTypeName = q.TypeName,
                                         requestEmployee = new InfoEmployee
                                         {
                                             EmployeeId = b.Id,
                                             EmployeeName = b.NameEmployee,
                                             DateEmployed = b.JoinDate,
                                             Phone = b.PhoneNumber,
                                             Email = b.Email,
                                             DateBirth = b.DateBirth,
                                             Age = Convert.ToDateTime(b.DateBirth).AddYears(DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) > DateTime.Now ?
                                                   (DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) - 1 : DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year,
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
                                             EmployementStatusId = b.EmployementStatusId,
                                             EmployementStatus = context.EmployementStatuss.Where(x => x.Id == b.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault(),
                                             EmployeeContractDocument = context.EmployeeContracts.Where(x => x.EmployeeBasicInfoId == a.EmployeeBasicInfoId).Select(x => x.FilemasterId).FirstOrDefault(),
                                             EmployeeContractDocumentName = (from r in context.EmployeeContracts
                                                                             join s in context.Filemasters on r.FilemasterId equals s.Id
                                                                             where r.EmployeeBasicInfoId == a.EmployeeBasicInfoId
                                                                             select s.Name).FirstOrDefault(),
                                             Division = b.BusinessUnitJobLevelId == null ? "" :
                                                                            (from r in context.BusinessUnitJobLevels
                                                                             join s in context.BusinessUnits on r.BusinessUnitDivisionId equals s.Id
                                                                             join t in context.BusinessUnitTypes on s.BusinessUnitTypeId equals t.Id
                                                                             where t.BusinessUnitLevel == BusinessUnitLevelCode.Division && r.Id == b.BusinessUnitJobLevelId
                                                                             select s.UnitName).FirstOrDefault(),
                                             LegalEntityId = b.LegalEntityId,
                                             LegalEntityName = (from h in context.LegalEntity
                                                                where h.Id == b.LegalEntityId
                                                                select h.LegalEntityName).FirstOrDefault()
                                         },
                                         infoEmployee = new InfoEmployee
                                         {
                                             EmployeeId = w.Id,
                                             EmployeeName = w.NameEmployee,
                                             DateEmployed = w.JoinDate,
                                             Phone = w.PhoneNumber,
                                             Email = w.Email,
                                             DateBirth = w.DateBirth,
                                             Age = Convert.ToDateTime(w.DateBirth).AddYears(DateTime.Now.Year - Convert.ToDateTime(w.DateBirth).Year) > DateTime.Now ?
                                                   (DateTime.Now.Year - Convert.ToDateTime(w.DateBirth).Year) - 1 : DateTime.Now.Year - Convert.ToDateTime(w.DateBirth).Year,
                                             JobTitle = w.BusinessUnitJobLevelId == null ? "" :
                                                               (from r in context.BusinessUnitJobLevels
                                                                join s in context.JobTitles on r.JobTitleId equals s.Id
                                                                where r.Id == w.BusinessUnitJobLevelId
                                                                select s.TitleName).FirstOrDefault(),
                                             Grade = w.BusinessUnitJobLevelId == null ? "" :
                                                               (from r in context.BusinessUnitJobLevels
                                                                join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                where r.Id == w.BusinessUnitJobLevelId
                                                                select s.GradeName).FirstOrDefault(),
                                             EmployementStatusId = b.EmployementStatusId,
                                             EmployementStatus = context.EmployementStatuss.Where(x => x.Id == w.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault(),
                                             EmployeeContractDocument = context.EmployeeContracts.Where(x => x.EmployeeBasicInfoId == a.EmployeeBasicInfoId).Select(x => x.FilemasterId).FirstOrDefault(),
                                             EmployeeContractDocumentName = (from r in context.EmployeeContracts
                                                                             join s in context.Filemasters on r.FilemasterId equals s.Id
                                                                             where r.EmployeeBasicInfoId == a.EmployeeBasicInfoId
                                                                             select s.Name).FirstOrDefault(),
                                             Division = w.BusinessUnitJobLevelId == null ? "" :
                                                                            (from r in context.BusinessUnitJobLevels
                                                                             join s in context.BusinessUnits on r.BusinessUnitDivisionId equals s.Id
                                                                             join t in context.BusinessUnitTypes on s.BusinessUnitTypeId equals t.Id
                                                                             where t.BusinessUnitLevel == BusinessUnitLevelCode.Division && r.Id == w.BusinessUnitJobLevelId
                                                                             select s.UnitName).FirstOrDefault(),
                                             LegalEntityId = w.LegalEntityId,
                                             LegalEntityName = (from h in context.LegalEntity
                                                                where h.Id == w.LegalEntityId
                                                                select h.LegalEntityName).FirstOrDefault()
                                         }
                                     }).FirstOrDefaultAsync();
            }

            return termination;
        }
    }
}