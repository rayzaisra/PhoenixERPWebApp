using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class VwJobClosingSummary
    {
        [DisplayName("No")]
        public int recno { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Sum Closed Year To Date")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? closed_ytd { get; set; }

        [DisplayName("Sum UnClosed Year To Date")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? unclosed_ytd { get; set; }
    }
}