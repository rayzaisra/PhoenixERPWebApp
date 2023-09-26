using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class CandidateRecruitmentDto
    {
        [Required]
        public string Id { get; set; }
        [StringLength(2147483647)]
        public string achievement { get; set; }
        [StringLength(2147483647)]

        public string address { get; set; }
        public bool alcohol_willing { get; set; }
        public bool out_of_town_willing { get; set; }
        [StringLength(10)]
        public string applicant_progress_id { get; set; }
        [StringLength(50)]
        public string approver_id { get; set; }
        [StringLength(50)]
        public string birth_place { get; set; }
        [StringLength(50)]
        public string certificate { get; set; }
        public bool cigaret_willing { get; set; }
        [StringLength(50)]
        public string city_id { get; set; }
        [StringLength(2147483647)]
        public string company_last_address { get; set; }
        public DateTime? company_last_from { get; set; }
        [StringLength(2147483647)]

        public string company_last_name { get; set; }
        [StringLength(50)]
        public string company_last_phone { get; set; }
        public DateTime? company_last_until { get; set; }
        [StringLength(100)]
        public string country { get; set; }
        [StringLength(80)]
        public string course_institution { get; set; }
        [StringLength(50)]
        public string course_tittle { get; set; }
        public DateTime? course_years { get; set; }

        public DateTime? date_birth { get; set; }
        [StringLength(50)]
        public string education_level_id { get; set; }
        public DateTime? education_year_from { get; set; }
        public DateTime? education_year_to { get; set; }
        public DateTime? contractSignDate { get; set; }
        [StringLength(50)]

        public string email { get; set; }
        [StringLength(50)]
        public string file_id { get; set; }
        [StringLength(50)]
        public string first_name { get; set; }
        [StringLength(50)]
        public string gender { get; set; }
        [StringLength(50)]
        public string identity_card { get; set; }
        [StringLength(50)]

        public string institusion { get; set; }
        [StringLength(2147483647)]
        public string interest { get; set; }
        [StringLength(50)]
        public string job_repository_id { get; set; }
        [StringLength(50)]

        public string last_education { get; set; }

        //[StringLength(200)]
        //public string last_education_name { get; set; }


        [StringLength(50)]
        public string last_name { get; set; }
        [StringLength(50)]
        public string last_position { get; set; }

        public int? looking_job { get; set; }
        [StringLength(100)]
        public string major { get; set; }
        [StringLength(50)]
        public string married_status_id { get; set; }
        [StringLength(50)]
        public string mobile_number { get; set; }
        [StringLength(50)]
        public string nationality_id { get; set; }
        [StringLength(50)]

        public string nick_name { get; set; }
        [StringLength(50)]
        public string pay_slip { get; set; }
        [StringLength(50)]

        public string phone_number { get; set; }
        [StringLength(50)]
        public string portofolio { get; set; }
        [StringLength(150)]

        public string profile_file { get; set; }
        [StringLength(100)]
        public string province { get; set; }
        [StringLength(50)]

        public string relation_people { get; set; }
        public long? religion_id { get; set; }

        public int? resume_screening { get; set; }
        [StringLength(50)]
        public string source_recruitment { get; set; }
        [StringLength(50)]
        public string work_number { get; set; }
        [StringLength(100)]
        public string zip_code { get; set; }
        public string status { get; set; }
        public string code { get; set; }
        public string contracttype { get; set; }
        public string job_vacancy_id { get; set; }
        public string year_experience { get; set; }
        public string source { get; set; }
        public string applicant_move_stage_id { get; set; }
        public string filemaster_id { get; set; }
        public string remarks { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }

        public float? thp { get; set; }

        public string ptpp { get; set; }
        public string last_education_name { get; set; }
        public string education_level_name { get; set; }

        public string groupId { get; set; }
        public string cv_file_master_id { get; set; }
        public string photo_file_master_id { get; set; }
        public string portofolio_file_master_id { get; set; }
        public string cv_file { get; set; }
        public string photo_file { get; set; }
        public string portofolio_file { get; set; }

        public string cv { get; set; }
        public string photo { get; set; }
        public string portfolio { get; set; }

        public DateTime? contract_date { get; set; }
        public string contract_number { get; set; }
        public DateTime? interview_date { get; set; }
        public DateTime? apply_date { get; set; }
        public DateTime? created_on { get; set; }
        public string job_title { get; set; }
        public string comment { get; set; }
        public string businessJobLevelId { get; set; }
    }
    public class CandidateRecruitmentSetDto
    {
        public string Id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }

    }
    public class CandidateRecruitmentNew
    {

        public string Id { get; set; }
        [StringLength(2147483647)]
        public string achievement { get; set; }
        [StringLength(2147483647)]
        [Required]
        public string address { get; set; }
        public bool alcohol_willing { get; set; }
        public bool out_of_town_willing { get; set; }
        [StringLength(10)]
        public string applicant_progress_id { get; set; }
        [StringLength(50)]
        public string approver_id { get; set; }
        [StringLength(50)]
        public string birth_place { get; set; }
        [StringLength(50)]
        public string certificate { get; set; }
        public bool cigaret_willing { get; set; }
        [StringLength(50)]
        public string city_id { get; set; }
        [StringLength(2147483647)]
        public string company_last_address { get; set; }
        public DateTime? company_last_from { get; set; }
        [StringLength(2147483647)]

        public string company_last_name { get; set; }
        [StringLength(50)]
        public string company_last_phone { get; set; }
        public DateTime? company_last_until { get; set; }
        [StringLength(100)]
        public string country { get; set; }
        [StringLength(80)]
        public string course_institution { get; set; }
        [StringLength(50)]
        public string course_tittle { get; set; }
        public DateTime? course_years { get; set; }

        public DateTime? date_birth { get; set; }
        [StringLength(50)]
        public string education_level_id { get; set; }
        public DateTime? education_year_from { get; set; }
        public DateTime? education_year_to { get; set; }
        [StringLength(50)]

        public string email { get; set; }
        [StringLength(50)]
        public string file_id { get; set; }
        [StringLength(50)]
        public string first_name { get; set; }
        [StringLength(50)]
        public string gender { get; set; }
        [StringLength(50)]
        public string identity_card { get; set; }
        [StringLength(50)]

        public string institusion { get; set; }
        [StringLength(2147483647)]
        public string interest { get; set; }
        [StringLength(50)]
        public string job_repository_id { get; set; }
        [StringLength(50)]

        public string last_education { get; set; }
        [StringLength(50)]
        public string last_name { get; set; }
        [StringLength(50)]
        public string last_position { get; set; }

        public int? looking_job { get; set; }
        [StringLength(100)]
        public string major { get; set; }
        [StringLength(50)]
        public string married_status_id { get; set; }
        [StringLength(50)]
        public string mobile_number { get; set; }
        [StringLength(50)]
        public string nationality_id { get; set; }
        [StringLength(50)]

        public string nick_name { get; set; }
        [StringLength(50)]
        public string pay_slip { get; set; }
        [StringLength(50)]

        public string phone_number { get; set; }
        [StringLength(50)]
        public string portofolio { get; set; }
        public string cv { get; set; }
        public string photo { get; set; }
        public string portfolio { get; set; }
        [StringLength(150)]

        public string profile_file { get; set; }
        [StringLength(100)]
        public string province { get; set; }
        [StringLength(50)]

        public string relation_people { get; set; }
        public long? religion_id { get; set; }

        public int? resume_screening { get; set; }
        [StringLength(50)]
        public string source_recruitment { get; set; }
        [StringLength(50)]
        public string work_number { get; set; }
        [StringLength(100)]
        public string zip_code { get; set; }
        public string status { get; set; }
        public string code { get; set; }
        public string job_vacancy_id { get; set; }
        public string year_experience { get; set; }
        public string source { get; set; }
        public string applicant_move_stage_id { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
    }
    public class CandidateRecruitmentSearch : SearchParameter
    {
        public string Search_achievement { get; set; }
        public string Search_address { get; set; }
        public bool Search_alcohol_willing { get; set; }
        public string Search_applicant_progress_id { get; set; }
        public string Search_approver_id { get; set; }
        public string Search_birth_place { get; set; }
        public string Search_certificate { get; set; }
        public bool Search_cigaret_willing { get; set; }
        public bool Search_out_of_town_willing { get; set; }
        public string Search_city_id { get; set; }
        public string Search_company_last_address { get; set; }
        public DateTime? Search_company_last_from { get; set; }
        public string Search_company_last_name { get; set; }
        public string Search_company_last_phone { get; set; }
        public DateTime? Search_company_last_until { get; set; }
        public string Search_country { get; set; }
        public string Search_course_institution { get; set; }
        public string Search_course_tittle { get; set; }
        public DateTime? Search_course_years { get; set; }
        public DateTime? Search_date_birth { get; set; }
        public string Search_education_level_id { get; set; }
        public DateTime? Search_education_year_from { get; set; }
        public DateTime? Search_education_year_to { get; set; }
        public string Search_email { get; set; }
        public string Search_file_id { get; set; }
        public string Search_first_name { get; set; }
        public string Search_gender { get; set; }
        public string Search_identity_card { get; set; }
        public string Search_institusion { get; set; }
        public string Search_interest { get; set; }
        public string Search_job_repository_id { get; set; }
        public string Search_last_education { get; set; }
        public string Search_last_name { get; set; }
        public string Search_last_position { get; set; }
        public int? Search_looking_job { get; set; }
        public string Search_major { get; set; }
        public string Search_married_status_id { get; set; }
        public string Search_mobile_number { get; set; }
        public string Search_nationality_id { get; set; }
        public string Search_nick_name { get; set; }
        public string Search_pay_slip { get; set; }
        public string Search_phone_number { get; set; }
        public string Search_portofolio { get; set; }
        public string Search_profile_file { get; set; }
        public string Search_province { get; set; }
        public string Search_relation_people { get; set; }
        public long? Search_religion_id { get; set; }
        public int? Search_resume_screening { get; set; }
        public string Search_source_recruitment { get; set; }
        public string Search_work_number { get; set; }
        public string Search_zip_code { get; set; }
        public string Search_job_vacancy_id { get; set; }
        public string Search_applicant_move_stage_id { get; set; }

        public string Search_title_name { get; set; }

    }
}