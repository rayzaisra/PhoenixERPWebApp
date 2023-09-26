using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("training_category", Schema = "hr")]
    public class TrainingCategory : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column("id"), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("category"), StringLength(50)]
        public string Category { get; set; }

    }
}