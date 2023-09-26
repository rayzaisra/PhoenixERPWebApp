using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class BscCorporateObjective : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Bsc Category")]
        public string BscCategory { get; set; }

        [DisplayName("Objective Name")]
        public string BscName { get; set; }

        [DisplayName("Refer Corporate Objective")]
        public string ParentId { get; set; }

        [DisplayName("Year")]
        public string Year { get; set; }

        [DisplayName("Refer Corporate Objective")]
        public string ParentName { get; set; }
    }
}