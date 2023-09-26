using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class BankBranchDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(2147483647)]
        [Required]
        public string adress { get; set; }
        [StringLength(50)]
        [Required]
        public string bank_branch_id { get; set; }
        [StringLength(50)]
        [Required]
        public string bank_id { get; set; }
        [StringLength(50)]
        [Required]
        public string branch_code { get; set; }
        [StringLength(50)]
        [Required]
        public string phone { get; set; }
    }
    public class BankBranchNew
    {
        [StringLength(2147483647)]
        [Required]
        public string adress { get; set; }
        [StringLength(50)]
        [Required]
        public string bank_branch_id { get; set; }
        [StringLength(50)]
        [Required]
        public string bank_id { get; set; }
        [StringLength(50)]
        [Required]
        public string branch_code { get; set; }
        [StringLength(50)]
        [Required]
        public string phone { get; set; }
    }            
    public class BankBranchSearch : SearchParameter
    {
        public string Search_adress { get; set; }
        public string Search_bank_branch_id { get; set; }
        public string Search_bank_id { get; set; }
        public string Search_branch_code { get; set; }
        public string Search_phone { get; set; }
        
    }
}