using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table(nameof(Termination), Schema = "hr")]
    public class Termination : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("employee_basic_info_id"), StringLength(50)]
        public string EmployeeBasicInfoId { get; set; }

        [Column("last_day_date")]
        public DateTime? LastDayDate { get; set; }

        [Column("effective_resign_date")]
        public DateTime? EffectiveResignDate { get; set; }

        [Column("resign_letter")]
        public string ResignLetter { get; set; }

        [Column("end_of_contract")]
        public DateTime? EndOfContract { get; set; }

        [Column(nameof(Penalty))]
        public long? Penalty { get; set; }

        [Column("training_bonding")]
        public long? TrainingBonding { get; set; }

        [Required]
        [Column(nameof(Reason)), StringLength(2147483647)]
        public string Reason { get; set; }

        [MaxLength(50)]
        [Column("termination_type"), StringLength(50)]
        public string TerminationType { get; set; }

        [Column("termination_date")]
        public DateTime? TerminationDate { get; set; }

        [MaxLength(100)]
        [Column("medical_certificate"), StringLength(100)]
        public string MedicalCertificate { get; set; }

        [Column("date_of_sick")]
        public DateTime? DateOfSick { get; set; }

        [Column("salary_calculation")]
        public long? SalaryCalculation { get; set; }

        [Column("fauls_notice_date")]
        public DateTime? FaulsNoticeDate { get; set; }

        [Column("interview_warning_date")]
        public DateTime? InterviewWarningDate { get; set; }

        [MaxLength(50)]
        [Column("fault_category_id"), StringLength(50)]
        public string FaultCategoryId { get; set; }

        [MaxLength(150)]
        [Column("interview_warning_file"), StringLength(150)]
        public string InterviewWarningFile { get; set; }

        [MaxLength(150)]
        [Column("statement_of_police_report"), StringLength(150)]
        public string StatementOfPoliceReport { get; set; }

        [MaxLength(30)]
        [Column("warning_letter"), StringLength(30)]
        public string WarningLetter { get; set; }

        [Column("warning_letter_date")]
        public DateTime? WarningLetterDate { get; set; }

        [Column("is_hc_process")]
        public bool? IsHcProcess { get; set; }

        [Column("is_hc_approve")]
        public bool? IsHcApprove { get; set; }

        [Column("status_interview")]
        public int? StatusInterview { get; set; }

        [Column("status_checklist")]
        public int? StatusChecklist { get; set; }

        [Column("request_date")]
        public DateTime? RequestDate { get; set; }

        [MaxLength(50)]
        [Column(nameof(Requestor)), StringLength(50)]
        public string Requestor { get; set; }

        [Column("termination_interview_date")]
        public DateTime? TerminationInterviewDate { get; set; }

        [Column(nameof(Notes)), StringLength(2147483647)]
        public string Notes { get; set; }

        [MaxLength(50)]
        [Column("exit_interview_files"), StringLength(50)]
        public string ExitInterviewFiles { get; set; }

        [Column("replacement_status")]
        public int? ReplacementStatus { get; set; }

        [Column("is_interview")]
        public bool? IsInterview { get; set; }

        [Column("is_checklist")]
        public bool? IsChecklist { get; set; }

		[NotMapped]
		public List<ExitInterview> ExitInterviews { get; set; }

        [JsonConverter(typeof(ConcreteConverter<FileDetail>))] 
        [NotMapped]
        public IFormFile file { get; set; }

        [NotMapped]
        public string TerminationTypeName { get; set; }
        [NotMapped]
        public string ResignLetterName { get; set; }
        [NotMapped]
        public string MedicalCertificateName { get; set; }

        [NotMapped]
        public InfoEmployee requestEmployee { get; set; }
        [NotMapped]
        public InfoEmployee infoEmployee { get; set; }
        [NotMapped]
        public IFormFile fileInterviewWarning { get; set; }
        [NotMapped]
        public IFormFile fileStatementOfPolice { get; set; }
        [NotMapped]
        public string InterviewWarningFileName { get; set; }
        [NotMapped]
        public string StatementOfPoliceReportName { get; set; }
        [NotMapped]
        public string ExitInterviewFilesName { get; set; }
        [NotMapped]
        public List<CategoryExitInterview> CategoryExitInterviews { get; set; }

        [NotMapped]
        public List<ExitChecklist> JobHandover { get; set; }
        [NotMapped]
        public List<ExitChecklist> OfficeManagement { get; set; }
        [NotMapped]
        public List<ExitChecklist> InformationTechnology { get; set; }
        [NotMapped]
        public List<ExitChecklist> Finance { get; set; }
        [NotMapped]
        public List<ExitChecklist> HumanResource { get; set; }
        [NotMapped]
        public List<ExitChecklist> Other { get; set; }
        [NotMapped]
        public List<ExitChecklist> FinalHandover { get; set; }


        [NotMapped]
        public int? Status { get; set; }

        [NotMapped]
        public string StatusDesc { get; set; }

        [NotMapped]
        public string RemarkRejected { get; set; }

    }

    public class FileDetail {
        public string ContentType { get; set; }
        public string ContentDisposition { get; set; }
        public long Length { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
    }


    public class ConcreteConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType) => true;

        public override object ReadJson(JsonReader reader,
         Type objectType, object existingValue, JsonSerializer serializer)
        {
            return serializer.Deserialize<T>(reader);
        }

        public override void WriteJson(JsonWriter writer,
            object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}