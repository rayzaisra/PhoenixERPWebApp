using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("candidate_recruitment",Schema ="hr")]
    public partial class CandidateRecruitment : GenericEntity<string>
    {
        public string achievement { get; set; }
        public string address { get; set; }
        public bool alcohol_willing { get; set; }
        public string applicant_progress_id { get; set; }
        public string approver_id { get; set; }
        public string birth_place { get; set; }
        public string certificate { get; set; }
        public bool cigaret_willing { get; set; }
        public bool out_of_town_willing { get; set; }
        public string city_id { get; set; }
        public string company_last_address { get; set; }
        public DateTime? company_last_from { get; set; }
        public string company_last_name { get; set; }
        public string company_last_phone { get; set; }
        public DateTime? company_last_until { get; set; }
        public string country { get; set; }
        public string course_institution { get; set; }
        public string course_tittle { get; set; }
        public DateTime? course_years { get; set; }
        public DateTime? date_birth { get; set; }
        public string education_level_id { get; set; }
        public DateTime? education_year_from { get; set; }
        public DateTime? education_year_to { get; set; }
        public string email { get; set; }
        public string file_id { get; set; }
        public string first_name { get; set; }
        public string gender { get; set; }
        public string identity_card { get; set; }
        public string institusion { get; set; }
        public string interest { get; set; }
        public string job_repository_id { get; set; }
        public string last_education { get; set; }
        public string last_name { get; set; }
        public string last_position { get; set; }
        public int? looking_job { get; set; }
        public string major { get; set; }
        public string married_status_id { get; set; }
        public string mobile_number { get; set; }
        public string nationality_id { get; set; }
        public string nick_name { get; set; }
        public string pay_slip { get; set; }
        public string phone_number { get; set; }
        public string portofolio { get; set; }
        public string profile_file { get; set; }
        public string province { get; set; }
        public string relation_people { get; set; }
        public long? religion_id { get; set; }
        public int? resume_screening { get; set; }
        public string source_recruitment { get; set; }
        public string work_number { get; set; }
        public string zip_code { get; set; }
        public string code { get; set; }
        public string year_experience { get; set; }
        public string source { get; set; }
        public string applicant_move_stage_id { get; set; }
        public string job_vacancy_id { get; set; }
        public string cv_file_master_id { get; set; }
        public string photo_file_master_id { get; set; }
        public string portofolio_file_master_id { get; set; }
        public string comment { get; set; }
    }
}