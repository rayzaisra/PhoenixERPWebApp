using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("v_clientbrief_list", Schema = "pm")]
    public  class vClientBriefList: BaseEntity<string>
    {

        public string job_number { get; set; }
        public string job_name { get; set; }
        public string account_manager { get; set; }

    }
    public class spjobnumber: BaseEntity<string>
    {

    }
}