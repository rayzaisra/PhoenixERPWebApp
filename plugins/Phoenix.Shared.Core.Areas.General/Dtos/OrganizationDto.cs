using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class OrganizationDto
    {
        [Required]
            public string Id { get; set; }
        public bool is_default_organization { get; set; }
        [StringLength(50)]
        [Required]
        public string organization_name { get; set; }
        [StringLength(50)]
        public string parent_organization_id { get; set; }
        [StringLength(450)]
        public string smtp_from_address { get; set; }
        [StringLength(450)]
        public string smtp_password { get; set; }
        public int? smtp_port { get; set; }
        [StringLength(450)]
        public string smtp_server { get; set; }
        [StringLength(450)]
        public string smtp_username { get; set; }
    }
    public class OrganizationNew
    {
        public bool is_default_organization { get; set; }
        [StringLength(50)]
        [Required]
        public string organization_name { get; set; }
        [StringLength(50)]
        public string parent_organization_id { get; set; }
        [StringLength(450)]
        public string smtp_from_address { get; set; }
        [StringLength(450)]
        public string smtp_password { get; set; }
        public int? smtp_port { get; set; }
        [StringLength(450)]
        public string smtp_server { get; set; }
        [StringLength(450)]
        public string smtp_username { get; set; }
    }            
    public class OrganizationSearch : SearchParameter
    {
        public bool Search_is_default_organization { get; set; }
        public string Search_organization_name { get; set; }
        public string Search_parent_organization_id { get; set; }
        public string Search_smtp_from_address { get; set; }
        public string Search_smtp_password { get; set; }
        public int? Search_smtp_port { get; set; }
        public string Search_smtp_server { get; set; }
        public string Search_smtp_username { get; set; }
        
    }
}