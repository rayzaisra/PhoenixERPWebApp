using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class JobPeDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(1)]
        public string approval_id { get; set; }
        [StringLength(50)]
        public string ca_number { get; set; }
        [StringLength(50)]
        public string child_pce_number { get; set; }
        [StringLength(50)]
        public string detail { get; set; }
        public DateTime? due_date { get; set; }
        [StringLength(50)]
        public string file_id { get; set; }
        [StringLength(50)]
        public string job_detail_id { get; set; }
        [StringLength(50)]
        public string job_pa_id { get; set; }
        public double? margin { get; set; }
        public decimal? pe_cost { get; set; }
        public DateTime? pe_date { get; set; }
        public int? pe_line_item { get; set; }
        public int? pe_number { get; set; }
        [StringLength(50)]
        public string proc_number { get; set; }
        [StringLength(50)]
        public string submitted_by { get; set; }
    }
    public class JobPeNew
    {
        [StringLength(1)]
        public string approval_id { get; set; }
        [StringLength(50)]
        public string ca_number { get; set; }
        [StringLength(50)]
        public string child_pce_number { get; set; }
        [StringLength(50)]
        public string detail { get; set; }
        public DateTime? due_date { get; set; }
        [StringLength(50)]
        public string file_id { get; set; }
        [StringLength(50)]
        public string job_detail_id { get; set; }
        [StringLength(50)]
        public string job_pa_id { get; set; }
        public double? margin { get; set; }
        public decimal? pe_cost { get; set; }
        public DateTime? pe_date { get; set; }
        public int? pe_line_item { get; set; }
        public int? pe_number { get; set; }
        [StringLength(50)]
        public string proc_number { get; set; }
        [StringLength(50)]
        public string submitted_by { get; set; }
    }            
    public class JobPeSearch : SearchParameter
    {
        public string Search_approval_id { get; set; }
        public string Search_ca_number { get; set; }
        public string Search_child_pce_number { get; set; }
        public string Search_detail { get; set; }
        public DateTime? Search_due_date { get; set; }
        public string Search_file_id { get; set; }
        public string Search_job_detail_id { get; set; }
        public string Search_job_pa_id { get; set; }
        public double? Search_margin { get; set; }
        public decimal? Search_pe_cost { get; set; }
        public DateTime? Search_pe_date { get; set; }
        public int? Search_pe_line_item { get; set; }
        public int? Search_pe_number { get; set; }
        public string Search_proc_number { get; set; }
        public string Search_submitted_by { get; set; }
        
    }
}