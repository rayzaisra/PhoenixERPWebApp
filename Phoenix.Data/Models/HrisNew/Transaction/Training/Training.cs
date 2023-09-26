using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("training", Schema = "hr")]
    public class Training : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column("id"), StringLength(50)]
        public string Id { get; set; }

        [Column("name"), StringLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        [Column("category"), StringLength(50)]
        public string Category { get; set; }

        [Column("subject")]
        public string Subject { get; set; }

        [Column("training_cost")]
        public decimal? TrainingCost { get; set; }

        [Column("bonding_detail")]
        public string BondingDetail { get; set; }

        [Column("recruitment")]
        public string Recruitment { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("target_audiance")]
        public string TargetAudiance { get; set; }

        [Column("training_type")]
        public string TrainingType { get; set; }

        [Column("approval_id")]
        public string ApprovalId { get; set; }
    }
}