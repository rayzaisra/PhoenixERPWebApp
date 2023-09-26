using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Areas.General.Dtos;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class MppDto
    {
        [Required]
        public string Id { get; set; }
        [StringLength(50)]
        public string approval_id { get; set; }
        [StringLength(50)]
        public string code { get; set; }
        public DateTime? created_date { get; set; }
        [StringLength(50)]
        public string fiscal_year { get; set; }
        [StringLength(50)]
        public string business_unit_id { get; set; }
        [StringLength(50)]
        public string status { get; set; }
        public decimal? total_budget { get; set; }
        public DateTime? updated_date { get; set; }
        public List<MppDetailDto> JobTitle { get; set; }
        public List<MppApprovedStatusDto> Approval { get; set; }
        public BusinessUnitDto BusinessUnit { get; set; }
        public List<MppDetailDto> mppDetail { get; set; }
        public string reason { get; set; }
        public string business_unit_name { get; set; }
        public List<MppNewRealData> dataDetailMpp { get; set; }
        public List<TrUserApprovalDto> ApprovalReal { get; set; }
        public string fiscal_year_name { get; set; }
        public string fiscal_year_number { get; set; }
        public string division_name { get; set; }
        public DateTime? created_on { get; set; }
        public string created_by { get; set; }
    }
    public class MppNew
    {
        [StringLength(50)]
        public string template_approval_id { get; set; }
        [StringLength(50)]
        public string code { get; set; }
        public DateTime? created_date { get; set; }
        [StringLength(50)]
        public string fiscal_year { get; set; }
        [StringLength(50)]
        public string business_unit_id { get; set; }
        [StringLength(50)]
        public string status { get; set; }
        public decimal? total_budget { get; set; }
        public DateTime? updated_date { get; set; }

        public string saveType { get; set; }
        public List<MppNewRealData> dataDetailMpp { get; set; }
    }

    public class MppNewRealData{
        public string Id { get; set; }
        public string jobtitle { get; set; }
        public string ttf { get; set; }
        public string status { get; set; }
        public decimal? mppbudget { get; set; }
        public string grade { get; set; }
        public string project { get; set; }
        public string Description { get; set; }
        public string descresponsibilites { get; set; }
        public string jobtitle_name { get; set; }
        public string budget_souce { get; set; }
        public DateTime? dff { get; set; }
    }
    public class MppSearch : SearchParameter
    {
        public string Search_approval_id { get; set; }
        public string Search_code { get; set; }
        public DateTime? Search_created_date { get; set; }
        public string Search_fiscal_year { get; set; }
        public string Search_business_unit_id { get; set; }
        public string Search_status { get; set; }
        public int? Search_total_budget { get; set; }
        public DateTime? Search_updated_date { get; set; }
        
    }
}