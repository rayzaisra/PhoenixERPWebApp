using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Pm.MyDam
{
    public class MyDam :  PmEntityBase
    {
        public string Id { get; set; }
        [DisplayName("File Name")]
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public string MimeType { get; set; }
        public int? Status { get; set; }
        [DisplayName("Request By")]
        public string RequestBy { get; set; }
        public string LogDescription { get; set; }
        [DisplayName("Job Id")]
        public string ParentId { get; set; }
        [DisplayName("Request Date")]
        public DateTime? RequestDate { get; set; }
        [DisplayName("Status")]
        public string StatusDesc { get; set; }
        [DisplayName("Job Name")]
        public string JobName { get; set; }
        [DisplayName("Job Id")]
        public string JobId { get; set; }
        [DisplayName("Client Brief")]
        public string ClientBriefId { get; set; }
        [DisplayName("Client Name")]
        public string ClientName { get; set; }
        [DisplayName("Campaign Name")]
        public string CampaignName { get; set; }
        [DisplayName("Job Created On")]
        public DateTime? JobCreateOn { get; set; }
        public string ClientId { get; set; }
        public string RequestByName { get; set; }

    }

    public class RequestMyDam
    {
        public string SelectedId { get; set; }
        public int? Status { get; set; }
        public DateTime? RequestDate { get; set; }
        public string RequestBy { get; set; }
        public string FolderId { get; set; }
    }

  
}
