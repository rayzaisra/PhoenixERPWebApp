using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class LoanCategoryDto
    {
        [Required]
        public string Id { get; set; }
        [StringLength(50)]
        public string name { get; set; }
        public string description { get; set; }
        public string code { get; set; }
        public int? interest_month { get; set; }
    }
    public class LoanCategoryNew
    {
        [StringLength(50)]
        public string name { get; set; }
        public int? interest_month { get; set; }
        public string description { get; set; }
        public string code { get; set; }
    }
    public class LoanCategorySearch : SearchParameter
    {
        public string Search_name { get; set; }
        public int? Search_interest_month { get; set; }
    }
}