using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("employee_campaign",Schema ="hr")]
    public partial class EmployeeCampaign : GenericEntity<string>
    {
        public string client_brief_id { get; set; }
        public string employee_basic_info_id { get; set; }
        public DateTime? end_date { get; set; }
        public string file_upload { get; set; }
        public string notes { get; set; }
        public DateTime? start_date { get; set; }
        public string subcampaign_id { get; set; }
    }
}