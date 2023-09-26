using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class CompanyDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(400)]
        public string company_name { get; set; }
        [StringLength(50)]
        public string email { get; set; }
        [StringLength(50)]
        public string facebook { get; set; }
        [StringLength(50)]
        public string fax { get; set; }
        [StringLength(50)]
        public string instagram { get; set; }
        public bool is_client { get; set; }
        public bool is_vendor { get; set; }
        [StringLength(50)]
        public string mobile_phone { get; set; }
        [StringLength(50)]
        public string organization_id { get; set; }
        public int? payment_term_day { get; set; }
        [StringLength(50)]
        public string phone { get; set; }
        [StringLength(50)]
        public string registration_code { get; set; }
        public int? shipping_term_day { get; set; }
        [StringLength(50)]
        public string tax_number { get; set; }
        [StringLength(50)]
        public string twitter { get; set; }
        [StringLength(50)]
        public string website { get; set; }
    }
    public class CompanyNew
    {
        [StringLength(400)]
        public string company_name { get; set; }
        [StringLength(50)]
        public string email { get; set; }
        [StringLength(50)]
        public string facebook { get; set; }
        [StringLength(50)]
        public string fax { get; set; }
        [StringLength(50)]
        public string instagram { get; set; }
        public bool is_client { get; set; }
        public bool is_vendor { get; set; }
        [StringLength(50)]
        public string mobile_phone { get; set; }
        [StringLength(50)]
        public string organization_id { get; set; }
        public int? payment_term_day { get; set; }
        [StringLength(50)]
        public string phone { get; set; }
        [StringLength(50)]
        public string registration_code { get; set; }
        public int? shipping_term_day { get; set; }
        [StringLength(50)]
        public string tax_number { get; set; }
        [StringLength(50)]
        public string twitter { get; set; }
        [StringLength(50)]
        public string website { get; set; }
    }            
    public class CompanySearch : SearchParameter
    {
        public string Search_company_name { get; set; }
        public string Search_email { get; set; }
        public string Search_facebook { get; set; }
        public string Search_fax { get; set; }
        public string Search_instagram { get; set; }
        public bool Search_is_client { get; set; }
        public bool Search_is_vendor { get; set; }
        public string Search_mobile_phone { get; set; }
        public string Search_organization_id { get; set; }
        public int? Search_payment_term_day { get; set; }
        public string Search_phone { get; set; }
        public string Search_registration_code { get; set; }
        public int? Search_shipping_term_day { get; set; }
        public string Search_tax_number { get; set; }
        public string Search_twitter { get; set; }
        public string Search_website { get; set; }
        
    }
}