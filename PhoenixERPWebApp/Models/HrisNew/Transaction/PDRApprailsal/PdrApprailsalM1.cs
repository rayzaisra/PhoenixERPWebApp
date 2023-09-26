using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class PdrApprailsalM1 : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Pdr Apprailsal M1 Header Id")]
        public string PdrApprailsalM1HeaderId { get; set; }

        [Required]
        [DisplayName("Objectives")]
        public string PdrObjectiveSettingId { get; set; }

        [Required]
        [DisplayName("Target")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Target { get; set; }

        [Required]
        [DisplayName("Achievement")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Achievement { get; set; }

        [Required]
        [DisplayName("Weight")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Weight { get; set; }

        [Required]
        [DisplayName("Rating Score")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? RatingScore { get; set; }

        [DisplayName("Weighted Score")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? WeightedScore { get; set; }

        [DisplayName("Objectives")]
        public string PdrObjectiveSettingItem { get; set; }
    }
}