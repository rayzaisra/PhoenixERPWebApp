using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class BusinessUnitDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string business_unit_type_id { get; set; }
        [StringLength(50)]
        [Required]
        public string default_team { get; set; }
        public decimal latitude { get; set; }
        public decimal longitude { get; set; }
        [StringLength(50)]
        [Required]
        public string organization_id { get; set; }
        [StringLength(50)]
        public string parent_unit { get; set; }
        [StringLength(150)]
        public string unit_address { get; set; }
        [StringLength(50)]
        public string unit_code { get; set; }
        [StringLength(255)]
        public string unit_description { get; set; }
        [StringLength(50)]
        public string unit_email { get; set; }
        [StringLength(50)]
        public string unit_fax { get; set; }
        [StringLength(350)]
        [Required]
        public string unit_name { get; set; }
        [StringLength(50)]
        public string unit_phone { get; set; }

        public DateTime? created_on { get; set; }
        public string job_title_name { get; set; }
        public Int32 business_unit_level { get; set; }
        public string business_unit_type_name { get; set; }

        public bool? isfinance { get; set; }
        public bool? isHris { get; set; }

    }
    public class BusinessUnitNew
    {
        [StringLength(50)]
        public string business_unit_type_id { get; set; }
        [StringLength(50)]
    
        public string default_team { get; set; }
        public decimal latitude { get; set; }
        public decimal longitude { get; set; }
        [StringLength(50)]
        [Required]
        public string organization_id { get; set; }
        [StringLength(50)]
        public string parent_unit { get; set; }
        [StringLength(150)]
        public string unit_address { get; set; }
        [StringLength(50)]
        public string unit_code { get; set; }
        [StringLength(255)]
        public string unit_description { get; set; }
        [StringLength(50)]
        public string unit_email { get; set; }
        [StringLength(50)]
        public string unit_fax { get; set; }
        [StringLength(350)]
        [Required]
        public string unit_name { get; set; }
        [StringLength(50)]
        public string unit_phone { get; set; }
        public DateTime? created_on { get; set; }
        public string job_grade_name { get; set; }
        public string job_title_name { get; set; }
        public Int32 business_unit_level { get; set; }
    }

    public class BusinessUnitKendoUI {
        public Boolean expanded { get; set; }
        public string id { get; set; }
        public string text { get; set; }
    }
    public class BusinessUnitKendoUIParent : BusinessUnitKendoUI
    {
        public string parent_id { get; set; }
        public List<BusinessUnitKendoUIParent> items { get; set; }
    }
    public class BusinessUnitKendoUIChild : BusinessUnitKendoUI
    {
        public string parent_id { get; set; }
        public List<BusinessUnitKendoUIChild> items { get; set; }
    }
    public class BusinessUnitKendoUIGrandChild : BusinessUnitKendoUI
    {
        public string parent_id { get; set; }
        public List<BusinessUnitKenoUiItems> items { get; set; }
    }
    public class BusinessUnitKenoUiItems {
        public string parent_id { get; set; }
        public string id { get; set; }
        public string text { get; set; }

    }
    public class BusinessUnitSearch : SearchParameter
    {
        public string Search_business_unit_type_name { get; set; }
        public string Search_business_unit_type_id { get; set; }
        public string Search_unit_address { get; set; }
        public string Search_unit_code { get; set; }
        public string Search_unit_name { get; set; }
        public string Search_parent_unit { get; set; }
        public Int32 Search_business_unit_level  { get; set; }

        public bool? Seach_is_finance { get; set; }
        public bool? Search_parent_unit_is_not_null { get; set; }


    }

    public class BusinessUnitTree : BusinessUnitDto
    {
        public List<BusinessUnitTree> children { get; set; }
    }
    public class BusinessUnitParent : BusinessUnitDto
    {
        public BusinessUnitParent parent { get; set; }
    }

    public class BusinessUnitChild : BusinessUnitDto
    {

    }
}