using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("cost_center_campaign",Schema ="hr")]
    public partial class CostCenterCampaign : GenericEntity<string>
    {
        public string address { get; set; }
        public int? cost { get; set; }
        public string employee_basic_info_id { get; set; }
        public DateTime? end_date { get; set; }
        public long? external_client_id { get; set; }
        public string notes { get; set; }
        public string phone_no { get; set; }
        public DateTime? start_date { get; set; }
    }
}