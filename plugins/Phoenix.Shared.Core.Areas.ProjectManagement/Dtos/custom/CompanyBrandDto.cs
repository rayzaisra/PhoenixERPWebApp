using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class CompanyBrandDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(400)]
        public string brand_name { get; set; }
        [StringLength(50)]
        public string company_id { get; set; }
        public string sub_brand { get; set; }
        public List<string> brand_account { get; set; }

    }
    public class CompanyList : SearchParameter
    {
        public string id { get; set; }
        public string company_name { get; set; }
 
    }
    public class LegalList
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class AccountList
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class BrandDetailDto
    {
        public string id { get; set; }
        public string company_id { get; set; }
        public string brand_name { get; set; }
        public string subbrand1 { get; set; }
        public string subbrand2 { get; set; }
        public string account_name { get; set; }
        public string legal_enitity_id { get; set; }
        public string name { get; set; }
        public string division_id { get; set; }
        public int? asf_fee { get; set; }
    }

    public class DivisionList
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class AffilateList
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class CompanyBrandWithTeamList: CompanyBrandList
    {
 
        public List<AccountManagementWithTeamList> team { get; set; }
    }
    public class CompanyBrandTeam
    {
        public string company_brand_id { get; set; }
        public List<CompanyBrandTeamlist> team { get; set; }
    }
    public class CompanyBrandTeamlist
    {
        public string id { get; set; }
        public string act_as { get; set; }
    }

    public class CompanyBrandList
    {
        public string Id { get; set; }
 
        public string brand_name { get; set; }
 
        public string company_id { get; set; }
        public string company_name { get; set; }
        public string sub_brand { get; set; }
    }
    public class CompanyBrandNew
    {
        [StringLength(400)]
        public string brand_name { get; set; }
        [StringLength(50)]
        public string company_id { get; set; }
        public string sub_brand { get; set; }
        public List<string> brand_account { get; set; }
    }            
    public class CompanyBrandSearch : SearchParameter
    {
        public string Search_brand_name { get; set; }
        public string Search_company_id { get; set; }
        
    }
}