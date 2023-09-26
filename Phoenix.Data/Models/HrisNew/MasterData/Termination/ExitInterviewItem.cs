using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("exit_interview_item", Schema = "hr")]
    public class ExitInterviewItem : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Column(nameof(No))]
        public int? No { get; set; }

        [MaxLength(255)]
        [Column("text_item"), StringLength(255)]
        public string TextItem { get; set; }

        [MaxLength(50)]
        [Column("category_exit_interview_id"), StringLength(50)]
        public string CategoryExitInterviewId { get; set; }

        [Column(nameof(Type))]
        public int Type { get; set; }

		[NotMapped]
        public CategoryExitInterview CategoryExitInterview { get; set; }
    }
}