using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    public  class spClientBriefList: BaseEntity<string>
    {

        public string business_unit_id { get; set; }
        public string bussiness_type_id { get; set; }
        public string campaign_id { get; set; }
        public string campaign_type_id { get; set; }

        public string campaign_name { get; set; }
        public DateTime? deadline { get; set; }
        public string external_brand_id { get; set; }
        public string brand_name { get; set; }
        public DateTime? final_delivery { get; set; }
        public string job_number { get; set; }
        public string job_name { get; set; }
        public string account_manager { get; set; }

    }
}