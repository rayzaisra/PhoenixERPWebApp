using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class PdrObjectiveTargetDetail : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Pdr Objective Target Id")]
        public string PdrObjectiveTargetId { get; set; }

        [DisplayName("Objectives")]
        public string PdrObjectiveSettingId { get; set; }

        [DisplayName("Target")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? Target { get; set; }

        [DisplayName("Objectives")]
        public string PdrObjectiveSettingItem { get; set; }

    }
}