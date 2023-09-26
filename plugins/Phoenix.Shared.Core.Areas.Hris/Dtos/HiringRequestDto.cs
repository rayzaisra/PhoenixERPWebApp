using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Areas.General.Dtos;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class HiringRequestDto
    {
        [Required]
        public string Id { get; set; }
        public DateTime? created_on { get; set; }
        [StringLength(50)]
        public string approval_id { get; set; }
        public string budget_souce { get; set; }
        [StringLength(50)]
        public string basic_info_grade_tittle { get; set; }
        public string detail_candidate_qualification { get; set; }
        [StringLength(50)]
        public string code { get; set; }
        public DateTime? date_fulfilment { get; set; }
        [StringLength(50)]
        public string requester_employee_basic_info_id { get; set; }
        [StringLength(255)]
        public string employment_status { get; set; }
        public decimal? mpp_budged { get; set; }
        [StringLength(255)]
        public string mpp_id { get; set; }
        [StringLength(255)]
        public string ttf { get; set; }
        public string requester_name { get; set; }
        public string replacement_employee_basic_info_id { get; set; }
        public string business_unit_name { get; set; }
        public string job_title_name { get; set; }
        public string job_grade_name { get; set; }
        public string hiring_request_type_id { get; set; }
        public string job_grade_id { get; set; }
        public string job_title_id { get; set; }
        public string detail_candidate_responsibilites { get; set; }
        public string status { get; set; }
        public List<MppNewRealData> dataDetailMpp { get; set; }
        public JobTitleDto Jobtitile { get; set; }
        public List<HiringRequestApprovalDto> Approval { get; set; }
        public BusinessUnitDto Division { get; set; }
        public string job_title_requester { get; set; }
        public JobGradeDto JobGrade { get; set; }
        public string StatusApproved { get; set; }
        public ReplaceEmployee replacedEmployee { get; set; }
        public string business_unit_id { get; set; }
        public string departement_id { get; set; }
        public string departement_name { get; set; }

        public string created_by { get; set; }

        public string groupId { get; set; }
    }

    public class ReplaceEmployee {
         public string employeebasicinfoId { get; set; }
        public string employee_replaced_name { get; set; }
    }
    public class HiringRequestGetMppDetail {
        public string business_unit_id { get; set; }
        public string job_tittle_id { get; set; }

    }

    public class HiringRequestNew
    {
        [StringLength(50)]
        public string approval_id { get; set; }
        [StringLength(50)]
        public string basic_info_grade_tittle { get; set; }
        public string detail_candidate_qualification { get; set; }
        [StringLength(50)]
        public string code { get; set; }
        public DateTime? date_fulfilment { get; set; }
        [StringLength(50)]
        public string requester_employee_basic_info_id { get; set; }
        [StringLength(255)]
        public string employment_status { get; set; }
        public decimal? mpp_budged { get; set; }
        [StringLength(255)]
        public string mpp_id { get; set; }
        [StringLength(255)]
        public string ttf { get; set; }
        public string requester_name { get; set; }
        public string business_unit_name { get; set; }
        public string job_title_name { get; set; }
        public string hiring_request_type_id { get; set; }
        public string job_grade_id { get; set; }
        public string job_title_id { get; set; }
        public string replacement_employee_basic_info_id { get; set; }
        public string detail_candidate_responsibilites { get; set; }
        [Required]
        public List<MppNewRealData> dataDetailMpp { get; set; }
        public string status { get; set; }
        public string id { get; set; }
    }            
    public class HiringRequestSearch : SearchParameter
    {
        public string Search_approval_id { get; set; }
        public string Search_basic_info_grade_tittle { get; set; }
        public string Search_candidate_qualification { get; set; }
        public string Search_code { get; set; }
        public DateTime? Search_date_fulfilment { get; set; }
        public string Search_employee_basic_info_id { get; set; }
        public string Search_employment_status { get; set; }
        public string Search_hiring_type { get; set; }
        public decimal? Search_mpp_budged { get; set; }
        public string Search_mpp_id { get; set; }
        public string Search_ttf { get; set; }
        public string Search_status { get; set; }
        public string Search_business_unit_division_id { get; set; }

    }
}