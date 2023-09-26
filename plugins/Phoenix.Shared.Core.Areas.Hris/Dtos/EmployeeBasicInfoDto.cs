using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Phoenix.Data.Models;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class EmployeeBasicInfoDto
    {
       [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string name_employee { get; set; }
        [StringLength(50)]
        public string division { get; set; }
        [StringLength(50)]
        public string job_title { get; set; }
        [StringLength(50)]
        public string grade { get; set; }
        [StringLength(50)]
        public string employee_status { get; set; }


        [StringLength(50)]
        public string account_name { get; set; }
        [StringLength(50)]
        public string account_no { get; set; }
        [StringLength(500)]
        public string address_identity_card { get; set; }
        [StringLength(50)]
        public string approval { get; set; }
        [StringLength(150)]
        public string bank_address { get; set; }
        [StringLength(50)]
        public string bank_branch { get; set; }
        [StringLength(50)]
        public string bank_city { get; set; }
        [StringLength(50)]
        public string bank_id { get; set; }
        [StringLength(50)]
        public string birth_place { get; set; }
        [StringLength(50)]
        public string blood { get; set; }
        [StringLength(50)]
        public string bpjs_healthy { get; set; }
        [StringLength(50)]
        public string bpjs_tk { get; set; }
        [StringLength(50)]
        public string business_unit_id { get; set; }
        [StringLength(50)]
        public string central_resource_id { get; set; }
        [StringLength(50)]
        public string city_id { get; set; }
        public DateTime? date_birth { get; set; }
        public DateTime? date_termination { get; set; }
        [StringLength(50)]
        public string email { get; set; }
        [StringLength(50)]
        public string employee_code { get; set; }
        [StringLength(50)]
        public string employement_status_id { get; set; }
        [StringLength(50)]
        public string first_name { get; set; }
        [StringLength(50)]
        public string flag_diageo { get; set; }
        [StringLength(50)]
        public string gender { get; set; }
        [StringLength(50)]
        public string group { get; set; }
        [StringLength(2000)]
        public string home_address { get; set; }
        [StringLength(2000)]
        public string identity_address { get; set; }
        [StringLength(50)]
        public string identity_card { get; set; }
        [StringLength(50)]
        public string job_grade { get; set; }
        [StringLength(50)]
        public string job_title_id { get; set; }
        public string join_date { get; set; }
        public string kitas_expiry { get; set; }
        [StringLength(50)]
        public string kitas_number { get; set; }
        [StringLength(500)]
        public string kk_no { get; set; }
        [StringLength(50)]
        public string last_education { get; set; }
        [StringLength(50)]
        public string last_name { get; set; }
        [StringLength(50)]
        public string location { get; set; }
        [StringLength(50)]
        public string married_status_id { get; set; }
        [StringLength(50)]
        public string mobile_number { get; set; }
        [StringLength(255)]
        public string phone_number { get; set; }
        [StringLength(50)]
        public string nick_name { get; set; }
        [StringLength(150)]
        public string npwp_expiry { get; set; }
        [StringLength(50)]
        public string npwp_number { get; set; }
        public string passport_expiry { get; set; }
        [StringLength(50)]
        public string passport_number { get; set; }
        [StringLength(50)]
        public string payroll_methods { get; set; }
        [StringLength(500)]
        public string pay_slip { get; set; }
        [StringLength(500)]
        public string personal_files { get; set; }
        [StringLength(50)]
        public string photo { get; set; }
        [StringLength(500)]
        public string portfolio { get; set; }
        [StringLength(500)]
        public string position_id { get; set; }
        [StringLength(50)]
        public string probation { get; set; }
        [StringLength(50)]
        public string process_salary { get; set; }
        [StringLength(50)]
        public string religion_id { get; set; }
        [StringLength(50)]
        public string reports_to { get; set; }
        [StringLength(50)]
        public string status_employee_id { get; set; }
        [StringLength(500)]
        public string termination_reason { get; set; }
        [StringLength(50)]
        public string type_tax { get; set; }
        public DateTime? warning_date { get; set; }
        [StringLength(50)]
        public string working_status { get; set; }
        public List<FamilyNew> listFamily { get; set; }
        public List<EmployeeEmergencyContactNew> listEmergency { get; set; }
        public List<EducationNew> listEducation { get; set; }
        public List<EmployeeTrainingNew> listTraining { get; set; }
        public List<EmployeeExperienceNew> listExperience{ get; set; }
        public string application_user_id { get; set; }
        public List<dataNewEmployeeChecklist> listDataNewEmployeeChecklist { get; set; }
        public string business_unit_job_level_id { get; set; }
        public string candicate_recruitment_id { get; set; }
        public string legal_entity_id { get; set; }
        public string location_id { get; set; }
        public string nationality_id { get; set; }
        public string post_code { get; set; }
        public string nationality_name { get; set; }
        public string last_education_name { get; set; }
        public string religion_name { get; set; }
        public string married_status_name { get; set; }
        public string employement_status_name { get; set; }
        public string legal_entity_name { get; set; }
        public string location_name { get; set; }
        public BussinesUnitJobLevelDto bussinesUnitJobLevel { get; set; }
		public string ptkp_setting_id { get; set; }
		public string ptkp_setting_name { get; set; }

	}
    public class EmployeeBasicInfoNew
    {
        public string Id { get; set; }
        [StringLength(50)]
        public string account_name { get; set; }
        [StringLength(50)]
        public string account_no { get; set; }
        [StringLength(500)]
        public string address_identity_card { get; set; }
        [StringLength(50)]
        public string approval { get; set; }
        [StringLength(150)]
        public string bank_address { get; set; }
        [StringLength(50)]
        public string bank_branch { get; set; }
        [StringLength(50)]
        public string bank_city { get; set; }
        [StringLength(50)]
        public string bank_id { get; set; }
        [StringLength(50)]
        public string birth_place { get; set; }
        [StringLength(50)]
        public string blood { get; set; }
        [StringLength(50)]
        public string bpjs_healthy { get; set; }
        [StringLength(50)]
        public string bpjs_tk { get; set; }
        [StringLength(50)]
        public string business_unit_id { get; set; }
        [StringLength(50)]
        public string central_resource_id { get; set; }
        [StringLength(50)]
        public string city_id { get; set; }
        public DateTime? date_birth { get; set; }
        public DateTime? date_termination { get; set; }
        [StringLength(50)]
        public string email { get; set; }
        [StringLength(50)]
        public string employee_code { get; set; }
        [StringLength(50)]
        public string employement_status_id { get; set; }
        [StringLength(50)]
        public string first_name { get; set; }
        [StringLength(50)]
        public string flag_diageo { get; set; }
        [StringLength(50)]
        public string gender { get; set; }
        [StringLength(50)]
        public string group { get; set; }
        [StringLength(2000)]
        public string home_address { get; set; }
        [StringLength(2000)]
        public string identity_address { get; set; }
        [StringLength(50)]
        public string identity_card { get; set; }
        [StringLength(50)]
        public string job_grade_id { get; set; }
        [StringLength(50)]
        public string job_title_id { get; set; }
        public string join_date { get; set; }
        public string kitas_expiry { get; set; }
        [StringLength(50)]
        public string kitas_number { get; set; }
        [StringLength(500)]
        public string kk_no { get; set; }
        [StringLength(50)]
        public string last_education { get; set; }
        [StringLength(50)]
        public string last_name { get; set; }
        [StringLength(50)]
        public string location { get; set; }
        [StringLength(50)]
        public string married_status_id { get; set; }
        [StringLength(50)]
        public string mobile_number { get; set; }
        [StringLength(255)]
        public string phone_number { get; set; }
        [StringLength(255)]
        public string name_employee { get; set; }
        [StringLength(50)]
        public string nick_name { get; set; }
        [StringLength(150)]
        public string npwp_expiry { get; set; }
        [StringLength(50)]
        public string npwp_number { get; set; }
        public string passport_expiry { get; set; }
        [StringLength(50)]
        public string passport_number { get; set; }
        [StringLength(50)]
        public string payroll_methods { get; set; }
        [StringLength(500)]
        public string pay_slip { get; set; }
        [StringLength(500)]
        public string personal_files { get; set; }
        [StringLength(50)]
        public string photo { get; set; }
        [StringLength(500)]
        public string portfolio { get; set; }
        public string cv { get; set; }
        [StringLength(500)]
        public string position_id { get; set; }
        [StringLength(50)]
        public string probation { get; set; }
        [StringLength(50)]
        public string process_salary { get; set; }
        [StringLength(50)]
        public string religion_id { get; set; }
        [StringLength(50)]
        public string reports_to { get; set; }
        [StringLength(50)]
        public string status_employee_id { get; set; }
        [StringLength(500)]
        public string termination_reason { get; set; }
        [StringLength(50)]
        public string type_tax { get; set; }
        public DateTime? warning_date { get; set; }
        [StringLength(50)]
        public string working_status { get; set; }
        public List<FamilyNew> listFamily { get; set; }
        public List<FamilyNew> dataListEmployeBasicFamily { get; set; }
        public List<EmployeeEmergencyContactNew> listEmergency { get; set; }
        public List<EmployeeEmergencyContactNew> dataListEmployeBasicEmegencyContact { get; set; }  
        public List<EducationNew> listEducation { get; set; }
        public List<EducationNew> dataListEmployeBasicEducationInfo { get; set; }     
        public List<EmployeeTrainingNew> listTraining { get; set; }
        public List<EmployeeTrainingNew> dataListEmployeBasicCourseSeminar { get; set; }
        public List<EmployeeExperienceNew> listExperience { get; set; }
        public List<EmployeeExperienceNew> dataListEmployeBasicEmploymentHistory { get; set; }
        public List<dataNewEmployeeChecklist> listDataNewEmployeeChecklist { get; set; }
        public List<EmployeeContractSign> EmployeeContractSigns { get; set; }
        public string business_unit_job_level_id { get; set; }
        public string candicate_recruitment_id { get; set; }
        public string legal_entity_id { get; set; }
        public string location_id { get; set; }
        public string nationality_id { get; set; }
        public string post_code { get; set; }
		public string ptkp_setting_id { get; set; }
        public string comment { get; set; }
        public string address { get; set; }
        public DateTime? start_date { get; set; }
    }

    public class dataNewEmployeeChecklist {
        public string checklist_category { get; set; }
        public string checklistitem { get; set; }
        public string description { get; set; }
        public DateTime? whentoalert { get; set; }

    }
    public class EmployeeBasicInfoSearch : SearchParameter
    {
        public string Search_division { get; set; }
        public string Search_job_title { get; set; }
        public string Search_grade { get; set; }
        public string Search_employee_status { get; set; }
        public string Search_name { get; set; }

    }
}