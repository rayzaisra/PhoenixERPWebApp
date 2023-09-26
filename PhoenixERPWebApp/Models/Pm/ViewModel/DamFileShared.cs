using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Pm.ViewModel
{
    public class DamFileShared : PmEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }
        [DisplayName("Job Number")]
        public string job_id { get; set; }
        [DisplayName("Job Name")]
        public string job_Name { get; set; }
        [DisplayName("File Id")]
        public string file_id { get; set; }
        [DisplayName("File Name")]
        public string file_name { get; set; }
        [DisplayName("Share To Id")]
        public string share_to { get; set; }
        [DisplayName("Share To")]
        public string share_to_name { get; set; }
        public string status { get; set; }
        [DisplayName("Status")]
        public string statusApproval { get; set; }
        [DisplayName("File Path")]
        public string file_path { get; set; }
        [DisplayName("Requestor Id")]
        public string requester_id { get; set; }
        [DisplayName("Requestor")]
        public string requester_name { get; set; }
        [DisplayName("Request on")]
        public DateTime? requester_on { get; set; }
    }
}
