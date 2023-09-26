using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.ViewModel
{
    [Table("vw_exec_journal_ap", Schema = "fn")]
    public class ExecJournalAPView
    {
        public string invoice_number { get; set; }
        public string invoice_receivable_id { get; set; }

        public string purchase_order_number { get; set; }
 
        public string purchase_order_id { get; set; }
        
        public string invoice_vendor { get; set; }
        public DateTime? invoice_vendor_date { get; set; }
        [Column("description")]
        public string DescriptionJournal { get; set; }
        public string service_name { get; set; }
        public string method_name { get; set; }
        public string tax_number { get; set; }
        public string currency_code { get; set; }
        public string code_rec { get; set; }
        [Key]
        public string coa_id { get; set; }
        public string name5 { get; set; }
        public string condition { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public string line_code { get; set; }
        public string ap_level { get; set; }


        //tampilan ke journal tap
        //[NotMapped]
        //public string AccountId { get; set; }
        //[NotMapped]
        //public string AccountCode { get; set; }
        //[NotMapped]
        //public string AccountName { get; set; }
        //[NotMapped]
        //public string Description { get; set; }
        public string job_id { get; set; }
        //[NotMapped]
        //public string JobIdName { get; set; }
        public string business_unit_id { get; set; }
        //[NotMapped]
        //public string DivisiName { get; set; }
        public string purchase_request_id { get; set; }
    }
}
