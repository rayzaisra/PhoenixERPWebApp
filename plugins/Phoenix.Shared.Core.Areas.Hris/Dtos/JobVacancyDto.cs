using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Phoenix.Shared.Core.Areas.General.Dtos;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class JobVacancyDto
    {
        [Required]
        public string Id { get; set; }
        [StringLength(50)]
        public string code { get; set; }
        [StringLength(100)]
        public string posting_status { get; set; }
        [StringLength(100)]
        public string vacancy_name { get; set; }
        public string master_location_id { get; set; }
        public string master_location_name { get; set; }
        public string hiring_request_id { get; set; }
        public DateTime? created_on { get; set; }
        public DateTime? closing_date { get; set; }
        public List<CandidateRecruitmentSetDto> Applicant { get; set; }
        public BusinessUnitParent BusinessUnit { get; set; }
        public string request_code { get; set; }
        public JobVacancyNameTitleDto VacancyTitleName { get; set; }
        public JobVacancyNameGradeDto VacancyGradeName { get; set; }
        public JobtitleRequesterDto JobTitleRequester { get; set; }
        public List<JobVacancyJobSouceInnerDto> JobVacancyJobSouceInnerDto { get; set; }
        public MasterLocationDto location { get; set; }
        public JobVacancyJobSourceDto JobVacancyJobSource { get; set; }

        public string template_approval_id { get; set; }
        public int CountHIringRequest { get; set; }


    }
    public class JobtitleRequesterDto{
        public string job_title_code { get; set; }
        public string job_title_name { get; set; }
    }
    public class JobVacancyJobSouceInnerDto
    {
        public string posting_code { get; set; }
        public string source { get; set; }
        public string files { get; set; }
        public string url { get; set; }
        public DateTime? date_posting { get; set; }
    }
    public class JobVacancyNameTitleDto
    {
        public string job_title_code { get; set; }
        public string job_title_name { get; set; }
    }

    public class JobVacancyNameGradeDto
    {
        public string job_grade_code { get; set; }
        public string job_grade_name { get; set; }
    }

    public class NewJobVacancyDto
    {
      
        public string Id { get; set; }
        [StringLength(50)]
        public string code { get; set; }
        [StringLength(100)]
        public string posting_status { get; set; }
        [StringLength(100)]
        public string vacancy_name { get; set; }
        public string master_location_id { get; set; }
        public string hiring_request_id { get; set; }
        public DateTime? created_on { get; set; }
        public DateTime? closing_date { get; set; }
        public List<CandidateRecruitmentSetDto> Applicant { get; set; }
        public BusinessUnitParent BusinessUnit { get; set; }
        public string request_code { get; set; }
        public JobVacancyNameTitleDto VacancyTitleName { get; set; }
        public JobVacancyNameGradeDto VacancyGradeName { get; set; }
        public JobtitleRequesterDto JobTitleRequester { get; set; }
        public List<JobVacancyJobSouceInnerDto> JobVacancyJobSouceInnerDto { get; set; }
        public string template_approval_id { get; set; }
    }

    public class JobVacancySearch : SearchParameter
    {
        public string Search_hiring_request_id { get; set; }
        public string Search_vacancy_name { get; set; }
        public string Search_posting_status { get; set; }

    }
}
