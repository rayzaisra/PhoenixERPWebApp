using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class MassLeaveDetail : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Mass Leave Id")]
        public string MassLeaveId { get; set; }

        [DisplayName("Except Employee")]
        public string ExceptEmployee { get; set; }

        //public MassLeave MassLeave { get; set; }
        public InfoEmployee InfoEmployee { get; set; }
    }
}