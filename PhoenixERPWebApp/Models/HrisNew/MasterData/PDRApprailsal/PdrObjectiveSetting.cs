using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class PdrObjectiveSetting : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Corporate Objective")]
        public string CorporateObjective { get; set; }

        [DisplayName("Objective Item")]
        public string ObjectiveItem { get; set; }

        [DisplayName("Refer Corporate Objective")]
        public string BscCorporateObjectiveId { get; set; }

        [DisplayName("Refer Corporate Objective")]
        public string BscCorporateObjectiveName { get; set; }
        [DisplayName("Job Grade")]
        public string JobGradeId { get; set; }
        [DisplayName("Job Grade")]
        public string JobGradeName { get; set; }
    }
}