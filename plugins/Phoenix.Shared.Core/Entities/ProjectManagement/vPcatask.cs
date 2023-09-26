using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("v_pcatask", Schema = "pm")]
    public  class vPcatask: BaseEntity<string>
    {
 
        public decimal total { get; set; }

    }
 
}