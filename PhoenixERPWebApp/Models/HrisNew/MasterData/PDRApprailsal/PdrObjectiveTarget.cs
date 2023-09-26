using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class PdrObjectiveTarget : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Employee Id")]
        public string EmployeeId { get; set; }

        [DisplayName("Year")]
        public int? Year { get; set; }

        //[NotMapped] 
        public InfoEmployee infoEmployee { get; set; }
        public List<PdrObjectiveTargetDetail> PdrObjectiveTargetDetails { get; set; }
    }
}