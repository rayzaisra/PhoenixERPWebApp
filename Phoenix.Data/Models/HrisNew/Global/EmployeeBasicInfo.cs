using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("employee_basic_info", Schema = "hr")]
    public class EmployeeBasicInfo : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("employee_code"), StringLength(50)]
        public string EmployeeCode { get; set; }

        [MaxLength(255)]
        [Column("name_employee"), StringLength(255)]
        public string NameEmployee { get; set; }

        [MaxLength(50)]
        [Column("kitas_number"), StringLength(50)]
        public string KitasNumber { get; set; }

        [Column("kitas_expiry")]
        public DateTime? KitasExpiry { get; set; }

        [MaxLength(50)]
        [Column("religion_id"), StringLength(50)]
        public string ReligionId { get; set; }

        [MaxLength(50)]
        [Column("passport_number"), StringLength(50)]
        public string PassportNumber { get; set; }

        [Column("passport_expiry")]
        public DateTime? PassportExpiry { get; set; }

        [MaxLength(50)]
        [Column("npwp_number"), StringLength(50)]
        public string NpwpNumber { get; set; }

        [MaxLength(150)]
        [Column("npwp_expiry"), StringLength(150)]
        public DateTime? NpwpExpiry { get; set; }

        [MaxLength(50)]
        [Column(nameof(Blood)), StringLength(50)]
        public string Blood { get; set; }

        [MaxLength(50)]
        [Column("identity_card"), StringLength(50)]
        public string IdentityCard { get; set; }

        [MaxLength(500)]
        [Column("kk_no"), StringLength(500)]
        public string KkNo { get; set; }

        [Column("address_identity_card"), StringLength(2147483647)]
        public string AddressIdentityCard { get; set; }

        [MaxLength(50)]
        [Column(nameof(Photo)), StringLength(50)]
        public string Photo { get; set; }

        [Column("is_terminated")]
        public bool? IsTerminated { get; set; }

        [NotMapped]
		public IFormFile PhotoFile { get; set; }

		[NotMapped]
		public string PhotoFileName { get; set; }

		[Column("home_address"), StringLength(2147483647)]
        public string HomeAddress { get; set; }

        [Column("join_date")]
        public DateTime? JoinDate { get; set; }

        [MaxLength(50)]
        [Column("bpjs_healthy"), StringLength(50)]
        public string BpjsHealthy { get; set; }

        [MaxLength(50)]
        [Column("payroll_methods"), StringLength(50)]
        public string PayrollMethods { get; set; }

        [MaxLength(50)]
        [Column("central_resource_id"), StringLength(50)]
        public string CentralResourceId { get; set; }

        [MaxLength(50)]
        [Column("status_employee_id"), StringLength(50)]
        public string StatusEmployeeId { get; set; }

        [MaxLength(50)]
        [Column("business_unit_id"), StringLength(50)]
        public string BusinessUnitId { get; set; }

        [MaxLength(50)]
        [Column("employement_status_id"), StringLength(50)]
        public string EmployementStatusId { get; set; }

        [MaxLength(50)]
        [Column("city_id"), StringLength(50)]
        public string CityId { get; set; }

        [MaxLength(50)]
        [Column("account_no"), StringLength(50)]
        public string AccountNo { get; set; }

        [MaxLength(50)]
        [Column("bpjs_tk"), StringLength(50)]
        public string BpjsTk { get; set; }

        [MaxLength(50)]
        [Column("married_status_id"), StringLength(50)]
        public string MarriedStatusId { get; set; }

        [MaxLength(50)]
        [Column("last_education"), StringLength(50)]
        public string LastEducation { get; set; }

        [MaxLength(50)]
        [Column(nameof(Probation)), StringLength(50)]
        public string Probation { get; set; }

        [MaxLength(50)]
        [Column(nameof(Approval)), StringLength(50)]
        public string Approval { get; set; }

        [MaxLength(50)]
        [Column("process_salary"), StringLength(50)]
        public string ProcessSalary { get; set; }

        [MaxLength(50)]
        [Column("job_grade_id"), StringLength(50)]
        public string JobGradeId { get; set; }

        [MaxLength(50)]
        [Column("type_tax"), StringLength(50)]
        public string TypeTax { get; set; }

        [Column("warning_date")]
        public DateTime? WarningDate { get; set; }

        [MaxLength(50)]
        [Column("working_status"), StringLength(50)]
        public string WorkingStatus { get; set; }

        [Column("date_termination")]
        public DateTime? DateTermination { get; set; }

        [MaxLength(500)]
        [Column("termination_reason"), StringLength(500)]
        public string TerminationReason { get; set; }

        [MaxLength(50)]
        [Column("flag_diageo"), StringLength(50)]
        public string FlagDiageo { get; set; }

        [MaxLength(500)]
        [Column("personal_files"), StringLength(500)]
        public string PersonalFiles { get; set; }

        [MaxLength(50)]
        [Column("first_name"), StringLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        [Column("last_name"), StringLength(50)]
        public string LastName { get; set; }

        [MaxLength(50)]
        [Column("nick_name"), StringLength(50)]
        public string NickName { get; set; }

        [MaxLength(50)]
        [Column(nameof(Gender)), StringLength(50)]
        public string Gender { get; set; }

        [MaxLength(50)]
        [Column(nameof(Email)), StringLength(50)]
        public string Email { get; set; }

        [MaxLength(50)]
        [Column("birth_place"), StringLength(50)]
        public string BirthPlace { get; set; }

        [Column("date_birth")]
        public DateTime? DateBirth { get; set; }

        [MaxLength(50)]
        [Column("mobile_number"), StringLength(50)]
        public string MobileNumber { get; set; }

        [MaxLength(500)]
        [Column(nameof(Portfolio)), StringLength(500)]
        public string Portfolio { get; set; }

        [MaxLength(500)]
        [Column("pay_slip"), StringLength(500)]
        public string PaySlip { get; set; }

        [MaxLength(50)]
        [Column("location_id"), StringLength(50)]
        public string LocationId { get; set; }

        [MaxLength(50)]
        [Column("job_title_id"), StringLength(50)]
        public string JobTitleId { get; set; }

        [MaxLength(50)]
        [Column("bank_id"), StringLength(50)]
        public string BankId { get; set; }

        [MaxLength(50)]
        [Column("bank_branch"), StringLength(50)]
        public string BankBranch { get; set; }

        [MaxLength(2000)]
        [Column("bank_address"), StringLength(2000)]
        public string BankAddress { get; set; }

        [MaxLength(50)]
        [Column("account_name"), StringLength(50)]
        public string AccountName { get; set; }

        [MaxLength(50)]
        [Column("bank_city"), StringLength(50)]
        public string BankCity { get; set; }

        [MaxLength(255)]
        [Column("identity_address"), StringLength(255)]
        public string IdentityAddress { get; set; }

        [MaxLength(50)]
        [Column("post_code"), StringLength(50)]
        public string PostCode { get; set; }

        [MaxLength(50)]
        [Column("nationality_id"), StringLength(50)]
        public string NationalityId { get; set; }

        [MaxLength(50)]
        [Column("phone_number"), StringLength(50)]
        public string PhoneNumber { get; set; }

        [MaxLength(50)]
        [Column("legal_entity_id"), StringLength(50)]
        public string LegalEntityId { get; set; }

        [MaxLength(50)]
        [Column("ptkp_setting_id"), StringLength(50)]
        public string PtkpSettingId { get; set; }        

        [MaxLength(50)]
        [Column("reports_to"), StringLength(50)]
        public string ReportsTo { get; set; }

        [MaxLength(500)]
        [Column("position_id"), StringLength(500)]
        public string PositionId { get; set; }
		
        [MaxLength(50)]
        [Column("business_unit_job_level_id"), StringLength(50)]
        public string BusinessUnitJobLevelId { get; set; }

		[NotMapped]
		[DisplayName("Division")]
		public string BusinessUnitName { get; set; }
		[NotMapped]
		[DisplayName("Job Title")]
		public string JobTitle { get; set; }
		[NotMapped]
		[DisplayName("Grade")]
		public string JobGrade { get; set; }
		[NotMapped]
		[DisplayName("Emplyee Status")]
		public string EmployeeStatus { get; set; }

		[MaxLength(50)]
		[Column("master_working_time_id"), StringLength(50)]
		public string MasterWorkingTimeId { get; set; }

		[Column("employement_status_end_date")]
		[DisplayName("Contract/Probation End Date")]
		[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? EmployementStatusEndDate { get; set; }

		[NotMapped]
		public List<FormModelFamily> Families { get; set; }

		[NotMapped]
		public List<FormModelEmployeeEmergencyContact> EmployeeEmergencyContacts { get; set; }

		[NotMapped]
		public List<FormModelEducation> Educations { get; set; }

		[NotMapped]
		public List<FormModelEmployeeTraining> EmployeeTrainings { get; set; }

		[NotMapped]
		public List<FormModelEmployeeExperience> EmployeeExperiences { get; set; }

		[NotMapped]
		public List<FormModelEmployeeChecklist> EmployeeChecklists { get; set; }
        [NotMapped]
        public string EmployeeActiveStatus { get; set; }




        [NotMapped]
        public string NationalityLabel { get; set; }
        [NotMapped]
        public string ReligionLabel { get; set; }
        [NotMapped]
        public string MarriageLabel { get; set; }
        [NotMapped]
        public string LastEducationLabel { get; set; }

        [NotMapped]
        public string LocationLabel { get; set; }
        [NotMapped]
        public string EmploymentStatusLabel { get; set; }
            [NotMapped]
        public string LegalEntityLabel { get; set; }
        [NotMapped]
        public string GradeLabel { get; set; }
        [NotMapped]
        public string ReportToLabel { get; set; }
        [NotMapped]
        public string JobTitleLabel { get; set; }
    }
}