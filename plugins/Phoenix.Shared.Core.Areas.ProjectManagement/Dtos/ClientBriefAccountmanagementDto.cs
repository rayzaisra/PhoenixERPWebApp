using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class ClientBriefAccountmanagementDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string User_id { get; set; }
        public int? account_management_seq { get; set; }
        [StringLength(50)]
        public string client_brief_id { get; set; }
    }
    public class ClientBriefAccountmanagementNew
    {
        [StringLength(50)]
        public string User_id { get; set; }
        public int? account_management_seq { get; set; }
        [StringLength(50)]
        public string client_brief_id { get; set; }
    }            
    public class ClientBriefAccountmanagementSearch : SearchParameter
    {
        public string Search_User_id { get; set; }
        public int? Search_account_management_seq { get; set; }
        public string Search_client_brief_id { get; set; }
        
    }
}