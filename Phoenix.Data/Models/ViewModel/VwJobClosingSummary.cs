using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    public class VwJobClosingSummary
    {
        public int recno { get; set; }
        public string Description { get; set; }
        public decimal? closed_ytd { get; set; }
        public decimal? unclosed_ytd { get; set; }
    }
}