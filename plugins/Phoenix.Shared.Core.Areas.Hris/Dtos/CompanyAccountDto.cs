using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class CompanyAccountDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string bankbranch_id { get; set; }
        [StringLength(50)]
        [Required]
        public string bank_account_id { get; set; }
        [StringLength(50)]
        [Required]
        public string bank_id { get; set; }
        [StringLength(50)]
        [Required]
        public string branchcomapany_id { get; set; }
        [StringLength(2147483647)]
        [Required]
        public string branch_name { get; set; }
    }
    public class CompanyAccountNew
    {
        [StringLength(50)]
        [Required]
        public string bankbranch_id { get; set; }
        [StringLength(50)]
        [Required]
        public string bank_account_id { get; set; }
        [StringLength(50)]
        [Required]
        public string bank_id { get; set; }
        [StringLength(50)]
        [Required]
        public string branchcomapany_id { get; set; }
        [StringLength(2147483647)]
        [Required]
        public string branch_name { get; set; }
    }            
    public class CompanyAccountSearch : SearchParameter
    {
        public string Search_bankbranch_id { get; set; }
        public string Search_bank_account_id { get; set; }
        public string Search_bank_id { get; set; }
        public string Search_branchcomapany_id { get; set; }
        public string Search_branch_name { get; set; }
        
    }
}