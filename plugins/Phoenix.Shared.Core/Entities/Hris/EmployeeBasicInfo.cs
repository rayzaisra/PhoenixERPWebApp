using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("employee_basic_info", Schema ="hr")]
    public partial class EmployeeBasicInfo : GenericEntity<string>
    {
        public string account_name { get; set; }
        public string account_no { get; set; }
        public string address_identity_card { get; set; }
        public string approval { get; set; }
        public string bank_address { get; set; }
        public string bank_branch { get; set; }
        public string bank_city { get; set; }
        public string bank_id { get; set; }
        public string birth_place { get; set; }
        public string blood { get; set; }
        public string bpjs_healthy { get; set; }
        public string bpjs_tk { get; set; }
        public string business_unit_id { get; set; }
        public string central_resource_id { get; set; }
        public string city_id { get; set; }
        public DateTime? date_birth { get; set; }
        public DateTime? date_termination { get; set; }
        public string email { get; set; }
        public string employee_code { get; set; }
        public string employement_status_id { get; set; }
        public string first_name { get; set; }
        public string flag_diageo { get; set; }
        public string gender { get; set; }
        public string home_address { get; set; }
        public string identity_address { get; set; }
        public string identity_card { get; set; }
        public string job_title_id { get; set; }
        public string job_grade_id { get; set; }
        public DateTime? join_date { get; set; }
        public DateTime? kitas_expiry { get; set; }
        public string kitas_number { get; set; }
        public string kk_no { get; set; }
        public string last_education { get; set; }
        public string last_name { get; set; }
        public string married_status_id { get; set; }
        public string mobile_number { get; set; }
        public string name_employee { get; set; }
        public string nick_name { get; set; }
        public string npwp_expiry { get; set; }
        public string npwp_number { get; set; }
        public DateTime? passport_expiry { get; set; }
        public string passport_number { get; set; }
        public string payroll_methods { get; set; }
        public string pay_slip { get; set; }
        public string personal_files { get; set; }
        public string photo { get; set; }
        public string phone_number { get; set; }
        public string portfolio { get; set; }
        public string position_id { get; set; }
        public string probation { get; set; }
        public string process_salary { get; set; }
        public string religion_id { get; set; }
        public string reports_to { get; set; }
        public string status_employee_id { get; set; }
        public string termination_reason { get; set; }
        public string type_tax { get; set; }
        public DateTime? warning_date { get; set; }
        public string working_status { get; set; }
        public string business_unit_job_level_id { get; set; }
        public string legal_entity_id { get; set; }
        public string location_id { get; set; }
        public string nationality_id { get; set; }
        public string post_code { get; set; }

		public string ptkp_setting_id { get; set; }

	}
}