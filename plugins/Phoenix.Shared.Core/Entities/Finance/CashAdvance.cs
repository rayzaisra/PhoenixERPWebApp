using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("cash_advance", Schema = "fn")]
    public partial class CashAdvance : GenericEntity<string>
    {
        public string ca_number { get; set; }
        public DateTime? ca_date { get; set; }
        public int revision { get; set; }
        public string requested_by { get; set; }
        public string requested_on { get; set; }
        public string legal_entity_id { get; set; }
        public string business_unit_id { get; set; }
        public bool is_project_cost { get; set; }
        public string purchase_order_id { get; set; }
        public string purchase_order_number { get; set; }
        public string job_id { get; set; }
        public string job_number { get; set; }
        public string client_name { get; set; }
        public string project_activity { get; set; }
        public string project_area { get; set; }
        public string vendor_id { get; set; }
        public string vendor_name { get; set; }
        public string employee_id { get; set; }
        public string notes { get; set; }
        public string payee_bank_id { get; set; }
        public string payee_account_number { get; set; }
        public string payee_account_name { get; set; }
        public string payee_bank_branch { get; set; }
        public DateTime? ca_period_from { get; set; }
        public DateTime? ca_period_to { get; set; }
        public string approval_id { get; set; }
        public string ca_status_id { get; set; }

    }
}
