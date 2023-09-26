using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("client_brief",Schema ="pm")]
    public partial class ClientBrief : GenericEntity<string>
    {
        public string assigment_id { get; set; }
        public string business_unit_id { get; set; }
        public string bussiness_type_id { get; set; }
        public string campaign_name { get; set; }
        public long? campaign_progress_id { get; set; }
        public long? campaign_status_id { get; set; }
        public string campaign_type_id { get; set; }
        public string client_brief_file_id { get; set; }
        public DateTime? deadline { get; set; }
        public string external_brand_id { get; set; }
        public DateTime? final_delivery { get; set; }
        public DateTime? finish_campaign { get; set; }
        public string job_category_id { get; set; }
        public long? job_pe_id { get; set; }
        public string nationality_id { get; set; }
        public long? proposal_file_id { get; set; }
        public long? responsible_person { get; set; }
        public DateTime? start_campaign { get; set; }

        public string client_id { get; set; }
    }
}