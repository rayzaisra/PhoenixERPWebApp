using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class PdrDevelopmentPlan : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Pdr Employee Id")]
        public string PdrEmployeeId { get; set; }

        [Required]
        [DisplayName("Activities")]
        public string Activities { get; set; }

        [Required]
        [DisplayName("QUARTER")]
        public string Quarter { get; set; }

        //public PdrEmployee PdrEmployee { get; set; }
    }
}