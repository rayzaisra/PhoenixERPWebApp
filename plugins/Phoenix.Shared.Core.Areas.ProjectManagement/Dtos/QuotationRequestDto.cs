using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class QuotationRequestDto
    {
        [Required]
            public string Id { get; set; }
        public DateTime? closing_date { get; set; }
        [StringLength(50)]
        public string company_id { get; set; }
        public int? days { get; set; }
        public DateTime? delivery_date { get; set; }
        [StringLength(50)]
        public string delivery_place { get; set; }
        public DateTime? fq_date { get; set; }
        [StringLength(50)]
        public string payment_id { get; set; }
        public int? remarks { get; set; }
        public int? shift { get; set; }
        [StringLength(50)]
        public string task_id { get; set; }
        [StringLength(50)]
        public string transaction_type_id { get; set; }
        public int? weeks { get; set; }
    }
    public class QuotationRequestNew
    {
        public DateTime? closing_date { get; set; }
        [StringLength(50)]
        public string company_id { get; set; }
        public int? days { get; set; }
        public DateTime? delivery_date { get; set; }
        [StringLength(50)]
        public string delivery_place { get; set; }
        public DateTime? fq_date { get; set; }
        [StringLength(50)]
        public string payment_id { get; set; }
        public int? remarks { get; set; }
        public int? shift { get; set; }
        [StringLength(50)]
        public string task_id { get; set; }
        [StringLength(50)]
        public string transaction_type_id { get; set; }
        public int? weeks { get; set; }
    }            
    public class QuotationRequestSearch : SearchParameter
    {
        public DateTime? Search_closing_date { get; set; }
        public string Search_company_id { get; set; }
        public int? Search_days { get; set; }
        public DateTime? Search_delivery_date { get; set; }
        public string Search_delivery_place { get; set; }
        public DateTime? Search_fq_date { get; set; }
        public string Search_payment_id { get; set; }
        public int? Search_remarks { get; set; }
        public int? Search_shift { get; set; }
        public string Search_task_id { get; set; }
        public string Search_transaction_type_id { get; set; }
        public int? Search_weeks { get; set; }
        
    }
}