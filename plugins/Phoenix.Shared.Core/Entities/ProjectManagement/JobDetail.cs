using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("job_detail",Schema ="pm")]
    public partial class JobDetail : GenericEntity<string>
    {
        public double? cash_advance { get; set; }
        public string client_brief_id { get; set; }
        public string team_id { get; set; }
        public string company_id { get; set; }
        public DateTime? deadline { get; set; }
        public DateTime? start_job { get; set; }
        public DateTime? finish_job { get; set; }
        public DateTime? final_delivery { get; set; }
        public string job_description { get; set; }
        public string job_name { get; set; }
        public string job_number { get; set; }

        public string job_status_detail { get; set; }
        public string job_status_id { get; set; }
        public double? pa { get; set; }
        public double? pe { get; set; }
        public string pm_id { get; set; }
        public string job_type { get; set; }

        public string legal_entity_id { get; set; }
        public string business_unit_division_id { get; set; }
        public string business_unit_departement_id { get; set; }

        public string main_service_category { get; set; }
        public string brand_id { get; set; }

        public string affiliation_id { get; set; }
    }
}