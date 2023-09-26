using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class PTKPSettingDto
    {
        [StringLength(50)]
        [Required]
        public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string name { get; set; }
        [Required]
        public decimal value { get; set; }
        [StringLength(50)]
        [Required]
        public string description { get; set; }
        
        
    }

    public class PTKPSettingUpdateDto
    {
        [StringLength(50)]
        [Required]
        public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string name { get; set; }
        [Required]
        public decimal value { get; set; }
        [StringLength(50)]
        [Required]
        public string description { get; set; }
        
      
    }
    public class PTKPSettingNew
    {
        [StringLength(50)]
        [Required]
        public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string name { get; set; }
        [StringLength(50)]
        [Required]
        public string description { get; set; }
        public DateTime created_on { get; set; }
    }
    public class PTKPSettingSearch : SearchParameter
    {
        public string Search_name { get; set; }
        public string description { get; set; }
        //public string Search_type { get; set; }

    }
}
