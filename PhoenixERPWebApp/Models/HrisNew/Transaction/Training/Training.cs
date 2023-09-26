using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class Training : HrisEntityBase
    {
 
        public string Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Subject { get; set; }
		[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
		public decimal? TrainingCost { get; set; }
        public string BondingDetail { get; set; }
        public string Recruitment { get; set; }
        public string Description { get; set; }
        public string TargetAudiance { get; set; }
        public string TrainingType { get; set; }
        public string ApprovalId { get; set; }
    }
}