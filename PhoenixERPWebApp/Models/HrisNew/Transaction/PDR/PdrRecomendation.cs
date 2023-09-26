using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class PdrRecomendation : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Pdr Employee Id")]
        public string PdrEmployeeId { get; set; }

        [DisplayName("A. Contract Extended")]
        public int? ContractExtended { get; set; }

        [DisplayName("B. Contract Terminated")]
        public bool Terminated { get; set; }

        [DisplayName("C. Promoted to be permanent employee with effective date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EffectivePermanent { get; set; }

        [DisplayName("Type")]
        public string Type { get; set; }

        //public PdrEmployee PdrEmployee { get; set; }
    }
}