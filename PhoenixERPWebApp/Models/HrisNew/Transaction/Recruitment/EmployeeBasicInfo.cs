using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using PhoenixERPWebApp.Models.HrisNew.Transaction.Recruitment;

namespace PhoenixERPWebApp.Models
{
	public class EmployeeBasicInfo : HrisEntityBase
	{
		[Key]
		[MaxLength(50)]
		[Column(nameof(Id)), StringLength(50)]
		public string Id { get; set; }

		[MaxLength(50)]
		[Column("employee_code"), StringLength(50)]
		[DisplayName("NIP (No Induk Pegawai)")]
		public string EmployeeCode { get; set; }

		[MaxLength(255)]
		[Column("name_employee"), StringLength(255)]
		[DisplayName("Name Employee")]
		public string NameEmployee { get; set; }

		[MaxLength(50)]
		[Column("kitas_number"), StringLength(50)]
		[DisplayName("KITAS No")]
		public string KitasNumber { get; set; }

		[Column("kitas_expiry")]
		[DisplayName("KITAS Exipired Date")]
		public DateTime? KitasExpiry { get; set; }

		[MaxLength(50)]
		[Column("religion_id"), StringLength(50)]
		[DisplayName("Religion")]
		public string ReligionId { get; set; }

		[MaxLength(50)]
		[Column("passport_number"), StringLength(50)]
		[DisplayName("Passport Number")]
		public string PassportNumber { get; set; }

		[Column("passport_expiry")]
		[DisplayName("Passport Expired Date")]
		[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? PassportExpiry { get; set; }

		[MaxLength(50)]
		[Column("npwp_number"), StringLength(50)]
		[DisplayName("NPWP Number")]
		public string NpwpNumber { get; set; }
		
		[Column("npwp_expiry")]
		[DisplayName("NPWP Expired Date")]
		[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? NpwpExpiry { get; set; }

		[MaxLength(50)]
		[Column(nameof(Blood)), StringLength(50)]
		[DisplayName("Blood Type")]
		public string Blood { get; set; }

		[MaxLength(50)]
		[Column("identity_card"), StringLength(50)]
		[DisplayName("Identity Card Number")]
		public string IdentityCard { get; set; }

		[MaxLength(500)]
		[Column("kk_no"), StringLength(500)]
		[DisplayName("KK Number")]
		public string KkNo { get; set; }

		[MaxLength(2000)]
		[Column("address_identity_card"), StringLength(2000)]
		[DisplayName("Identity Address")]
		public string AddressIdentityCard { get; set; }

		[MaxLength(50)]
		[Column(nameof(Photo)), StringLength(50)]
		public string Photo { get; set; }

		[MaxLength(2000)]
		[Column("home_address"), StringLength(2000)]
		[DisplayName("Home Address")]
		public string HomeAddress { get; set; }

		[Column("join_date")]
		[DisplayName("Join Date")]
		[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? JoinDate { get; set; }

		[MaxLength(50)]
		[Column("bpjs_healthy"), StringLength(50)]
		[DisplayName("BPJS KES")]
		public string BpjsHealthy { get; set; }

		[MaxLength(50)]
		[Column("payroll_methods"), StringLength(50)]
		[DisplayName("Payroll Method")]
		public string PayrollMethods { get; set; }

		[MaxLength(50)]
		[Column("central_resource_id"), StringLength(50)]
		public string CentralResourceId { get; set; }

		[MaxLength(50)]
		[Column("status_employee_id"), StringLength(50)]
		[DisplayName("Status Employee")]
		public string StatusEmployeeId { get; set; }

		[MaxLength(50)]
		[Column("business_unit_id"), StringLength(50)]
		[DisplayName("Business Unit")]
		public string BusinessUnitId { get; set; }

		[MaxLength(50)]
		[Column("employement_status_id"), StringLength(50)]
		[DisplayName("Employement Status")]
		public string EmployementStatusId { get; set; }				

		[MaxLength(50)]
		[Column("city_id"), StringLength(50)]
		[DisplayName("City")]
		public string CityId { get; set; }

		[MaxLength(50)]
		[Column("account_no"), StringLength(50)]
		[DisplayName("Account No")]
		public string AccountNo { get; set; }

		[MaxLength(50)]
		[Column("bpjs_tk"), StringLength(50)]
		[DisplayName("BPJS TK")]
		public string BpjsTk { get; set; }

		[MaxLength(50)]
		[Column("married_status_id"), StringLength(50)]
		[DisplayName("Married Status")]
		public string MarriedStatusId { get; set; }

		[MaxLength(50)]
		[Column("last_education"), StringLength(50)]
		[DisplayName("Last Education")]
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
		[DisplayName("Grade")]
		public string JobGradeId { get; set; }

		[MaxLength(50)]
		[Column("type_tax"), StringLength(50)]
		public string TypeTax { get; set; }

		[Column("warning_date")]
		[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? WarningDate { get; set; }

		[MaxLength(50)]
		[Column("working_status"), StringLength(50)]
		[DisplayName("Working Status")]
		public string WorkingStatus { get; set; }

		[Column("date_termination")]
		[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
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
		[DisplayName("First Name")]
		public string FirstName { get; set; }

		[MaxLength(50)]
		[Column("last_name"), StringLength(50)]
		[DisplayName("Last Name")]
		public string LastName { get; set; }

		[MaxLength(50)]
		[Column("nick_name"), StringLength(50)]
		public string NickName { get; set; }

		[MaxLength(50)]
		[Column(nameof(Gender)), StringLength(50)]
		public string Gender { get; set; }

		[MaxLength(50)]
		[Column(nameof(Email)), StringLength(50)]
		[DisplayName("Email")]
		public string Email { get; set; }

		[MaxLength(50)]
		[Column("birth_place"), StringLength(50)]
		[DisplayName("Place Birth")]
		public string BirthPlace { get; set; }

		[Column("date_birth")]
		[DisplayName("Date Birth")]
		public DateTime? DateBirth { get; set; }

		[MaxLength(50)]
		[Column("mobile_number"), StringLength(50)]
		[DisplayName("Mobile Number")]
		public string MobileNumber { get; set; }

		[MaxLength(500)]
		[Column(nameof(Portfolio)), StringLength(500)]
		public string Portfolio { get; set; }

		[MaxLength(500)]
		[Column("pay_slip"), StringLength(500)]
		public string PaySlip { get; set; }

		[MaxLength(50)]
		[Column("location_id"), StringLength(50)]
		[DisplayName("Location")]
		public string LocationId { get; set; }

		[MaxLength(50)]
		[Column("job_title_id"), StringLength(50)]
		[DisplayName("Job Title")]
		public string JobTitleId { get; set; }

		[MaxLength(50)]
		[Column("bank_id"), StringLength(50)]
		[DisplayName("Bank")]
		public string BankId { get; set; }

		[MaxLength(50)]
		[Column("bank_branch"), StringLength(50)]
		public string BankBranch { get; set; }

		[MaxLength(2000)]
		[Column("bank_address"), StringLength(2000)]
		[DisplayName("Bank Address")]
		public string BankAddress { get; set; }

		[MaxLength(50)]
		[Column("account_name"), StringLength(50)]
		[DisplayName("Account Name")]
		public string AccountName { get; set; }

		[MaxLength(50)]
		[Column("bank_city"), StringLength(50)]
		[DisplayName("Bank City")]
		public string BankCity { get; set; }

		[MaxLength(255)]
		[Column("identity_address"), StringLength(255)]
		[DisplayName("Identity Address")]
		public string IdentityAddress { get; set; }

		[MaxLength(50)]
		[Column("post_code"), StringLength(50)]
		[DisplayName("Post Code")]
		public string PostCode { get; set; }

		[MaxLength(50)]
		[Column("nationality_id"), StringLength(50)]
		[DisplayName("Nationality")]
		public string NationalityId { get; set; }

		[MaxLength(50)]
		[Column("phone_number"), StringLength(50)]
		[DisplayName("Home Number (Residence)")]
		public string PhoneNumber { get; set; }

		[MaxLength(50)]
		[Column("legal_entity_id"), StringLength(50)]
		[DisplayName("Legal Entity")]
		public string LegalEntityId { get; set; }

		[MaxLength(50)]
		[Column("ptkp_setting_id"), StringLength(50)]
		[DisplayName("PTKP")]
		public string PtkpSettingId { get; set; }

		[MaxLength(50)]
		[Column("reports_to"), StringLength(50)]
		[DisplayName("Report to")]
		public string ReportsTo { get; set; }

		[MaxLength(500)]
		[Column("position_id"), StringLength(500)]
		[DisplayName("Position")]
		public string PositionId { get; set; }		

		[MaxLength(50)]
		[Column("business_unit_job_level_id"), StringLength(50)]
		[DisplayName("Job Title")]
		public string BusinessUnitJobLevelId { get; set; }
		
		[DisplayName("Division")]
		public string BusinessUnitName { get; set; }
		[DisplayName("Job Title")]
		public string JobTitle { get; set; }
		[DisplayName("Grade")]
		public string JobGrade { get; set; }
		[DisplayName("Emplyee Status")]
		public string EmployeeStatus { get; set; }

		[MaxLength(50)]
		[Column("master_working_time_id"), StringLength(50)]
		[DisplayName("Working Hour")]
		public string MasterWorkingTimeId { get; set; }

		[Column("employement_status_end_date")]
		[DisplayName("Contract/Probation End Date")]
		[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? EmployementStatusEndDate { get; set; }

		[Column("is_terminated")]
		[DisplayName("Is Terminated")]
		public bool? IsTerminated { get; set; }

		[NotMapped]
		public string PhotoFileName { get; set; }

		[NotMapped]
		public string JobGradeName { get; set; }

		[NotMapped]
		public string JobLevel { get; set; }

		[NotMapped]
		public string Supervisor { get; set; }

		[NotMapped]
		public string Remarks { get; set; }

		[NotMapped]
		public string ParamColumn { get; set; }

		[NotMapped]
		public string ParamValue { get; set; }

		[NotMapped]
		public string IsReadOnly { get; set; }

		public List<Family> Families { get; set; }
		public List<EmployeeEmergencyContact> EmployeeEmergencyContacts { get; set; }
		public List<Education> Educations { get; set; }
		public List<EmployeeExperience> EmployeeExperiences { get; set; }
		public List<EmployeeTraining> EmployeeTrainings { get; set; }
		public List<EmployeeChecklist> EmployeeChecklists { get; set; }
		public List<EmployeeHistory> EmployeeHistorys { get; set; }
		public List<EmployeeContractSign> EmployeeContractSigns { get; set; }

		public string EmployeeActiveStatus { get; set; }
    }

	public class EmployeeContractSign
	{
        public string ContractName { get; set; }
        public string ContractFile { get; set; }
		[DisplayName("Date")]
		[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? StartDate { get; set; }
		[DisplayName("End Date")]
		[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? EndDate { get; set; }
		[DisplayName("File")]
		public string FileId { get; set; }
        public string Ptpp { get; set; }
		public string ContractType { get; set; }
	}
}
