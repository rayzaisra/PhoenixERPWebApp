using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class PdrDetailM2below : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Pdr Employee Id")]
        public string PdrEmployeeId { get; set; }

        [DisplayName("Pdr Category Id")]
        public string PdrCategoryId { get; set; }

        [DisplayName("Pdr Parameter Id")]
        public string PdrParameterId { get; set; }

        [DisplayName("Score")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? Score { get; set; }

        [DisplayName("Percentage")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Percentage { get; set; }

        [DisplayName("Notes")]
        public string Notes { get; set; }

        //public PdrEmployee PdrEmployee { get; set; }
        //public PdrParameter PdrParameter { get; set; }
    }
}