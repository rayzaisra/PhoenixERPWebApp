using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class Termination : HrisEntityBase
    {
        [DisplayName("Request Code")]
        public string Id { get; set; }

        [DisplayName("Employee ID")]
        public string EmployeeBasicInfoId { get; set; }

        [DisplayName("Last Day Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? LastDayDate { get; set; }

        [DisplayName("Effective Resign Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EffectiveResignDate { get; set; }

        [DisplayName("Resign Letter")]
        public string ResignLetter { get; set; }

        [DisplayName("Resign Letter")]
        public string ResignLetterName { get; set; }

        [DisplayName("End of Contract")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndOfContract { get; set; }

        [DisplayName("Penalty")]
        public long? Penalty { get; set; }

        [DisplayName("Training Bonding")]
        public long? TrainingBonding { get; set; }

        [DisplayName("Notes / Reason")]
        public string Reason { get; set; }

        [DisplayName("Termination Type")]
        public string TerminationType { get; set; }

        [DisplayName("Termination Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? TerminationDate { get; set; }

        [DisplayName("Death / Medical Certificate")]
        public string MedicalCertificate { get; set; }

        [DisplayName("Date Of Sick")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfSick { get; set; }

        [DisplayName("Salary Calculation")]
        public long? SalaryCalculation { get; set; }

        [DisplayName("Fauls Notice Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FaulsNoticeDate { get; set; }

        [DisplayName("Interview Warning Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? InterviewWarningDate { get; set; }

        [DisplayName("Fault Category")]
        public string FaultCategoryId { get; set; }

        [DisplayName("Interview Warning File")]
        public string InterviewWarningFile { get; set; }

        [DisplayName("Statement Of Police Report")]
        public string StatementOfPoliceReport { get; set; }

        [DisplayName("Warning Letter")]
        public string WarningLetter { get; set; }

        [DisplayName("Warning Letter Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? WarningLetterDate { get; set; }

        [DisplayName("Status")]
        public int? Status { get; set; }

        public bool? IsHcProcess { get; set; }
        public bool? IsHcApprove { get; set; }

        [DisplayName("Status")]
        public int? StatusInterview { get; set; }

        [DisplayName("Status")]
        public int? StatusChecklist { get; set; }

        [DisplayName("Request Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RequestDate { get; set; }

        [DisplayName("Requestor")]
        public string Requestor { get; set; }

        [DisplayName("Exit Interview Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? TerminationInterviewDate { get; set; }

        [DisplayName("Exit Interview Note")]
        public string Notes { get; set; }

        [DisplayName("Exit Interview Files")]
        public string ExitInterviewFiles { get; set; }

        [DisplayName("Replacement Status")]
        public int? ReplacementStatus { get; set; }

        [DisplayName("Interview")]
        public bool? IsInterview { get; set; }

        [DisplayName("Checklist")]
        public bool? IsChecklist { get; set; }
        //public EmployeeBasicInfo EmployeeBasicInfo { get; set; }

        [DisplayName("Status")]
        public string StatusDesc { get; set; }

        public string RemarkRejected { get; set; }

        [DisplayName("Status")]
        public string StatusHCDesc { get { return (IsHcApprove ?? false) ? "Terminate" : "Process"; } }

        [DisplayName("Status")]
        public string StatusInterviewDesc { get { return TerminationInterviewStatus.StatusName(StatusInterview); } }

        [DisplayName("Status")]
        public string StatusChecklistDesc { get { return TerminationExitChecklistStatus.StatusName(StatusChecklist); } }


        [DisplayName("Status")]
        public string StatusSDeathOrContractDesc { get { return (IsHcProcess ?? false) ? "Submit to HC" : "Draft"; } }


        [DisplayName("Termination Type")]
        public string TerminationTypeName { get; set; }

        [DisplayName("Interview")]
        public string Interview { get { return (IsInterview ?? false) ? "Yes" : "No"; } }

        [DisplayName("Checklist")]
        public string Checklist { get { return (IsChecklist ?? false) ? "Yes" : "No"; } }

        public List<ExitInterview> ExitInterviews { get; set; }

        //Not Mapped
        public InfoEmployee requestEmployee { get; set; }
        public InfoEmployee infoEmployee { get; set; }
        public string MedicalCertificateName { get; set; }
        public string ExitInterviewFilesName { get; set; }
        public bool IsApprove { get; set; }

        public string InterviewWarningFileName { get; set; }
        public string StatementOfPoliceReportName { get; set; }

        public List<CategoryExitInterview> CategoryExitInterviews { get; set; }
        //public ViewExitChecklist detail { get; set; }

        public List<ExitChecklist> JobHandover { get; set; }
        public List<ExitChecklist> OfficeManagement { get; set; }
        public List<ExitChecklist> InformationTechnology { get; set; }
        public List<ExitChecklist> Finance { get; set; }
        public List<ExitChecklist> HumanResource { get; set; }
        public List<ExitChecklist> Other { get; set; }
        public List<ExitChecklist> FinalHandover { get; set; }

        public string SearchEmployee { get; set; }
        public string SearchEmployeeId { get; set; }

    }
}