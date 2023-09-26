using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class QuotationVendorDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string delivery_place { get; set; }
        public DateTime? delivery_time { get; set; }
        public Single? downpayment { get; set; }
        public DateTime? expiry_date { get; set; }
        [StringLength(50)]
        public string external_id { get; set; }
        public DateTime? lead_time { get; set; }
        [StringLength(50)]
        public string noted { get; set; }
        [StringLength(50)]
        public string payment_id { get; set; }
        [StringLength(50)]
        public string pe_bidding_id { get; set; }
        public Single? price { get; set; }
        public int? qty { get; set; }
        public DateTime? quotation_date { get; set; }
        [StringLength(50)]
        public string rfq_number { get; set; }
        [StringLength(50)]
        public string task_id { get; set; }
        [StringLength(50)]
        public string transaction_type_id { get; set; }
    }
    public class QuotationVendorNew
    {
        [StringLength(50)]
        public string delivery_place { get; set; }
        public DateTime? delivery_time { get; set; }
        public Single? downpayment { get; set; }
        public DateTime? expiry_date { get; set; }
        [StringLength(50)]
        public string external_id { get; set; }
        public DateTime? lead_time { get; set; }
        [StringLength(50)]
        public string noted { get; set; }
        [StringLength(50)]
        public string payment_id { get; set; }
        [StringLength(50)]
        public string pe_bidding_id { get; set; }
        public Single? price { get; set; }
        public int? qty { get; set; }
        public DateTime? quotation_date { get; set; }
        [StringLength(50)]
        public string rfq_number { get; set; }
        [StringLength(50)]
        public string task_id { get; set; }
        [StringLength(50)]
        public string transaction_type_id { get; set; }
    }            
    public class QuotationVendorSearch : SearchParameter
    {
        public string Search_delivery_place { get; set; }
        public DateTime? Search_delivery_time { get; set; }
        public Single? Search_downpayment { get; set; }
        public DateTime? Search_expiry_date { get; set; }
        public string Search_external_id { get; set; }
        public DateTime? Search_lead_time { get; set; }
        public string Search_noted { get; set; }
        public string Search_payment_id { get; set; }
        public string Search_pe_bidding_id { get; set; }
        public Single? Search_price { get; set; }
        public int? Search_qty { get; set; }
        public DateTime? Search_quotation_date { get; set; }
        public string Search_rfq_number { get; set; }
        public string Search_task_id { get; set; }
        public string Search_transaction_type_id { get; set; }
        
    }
}