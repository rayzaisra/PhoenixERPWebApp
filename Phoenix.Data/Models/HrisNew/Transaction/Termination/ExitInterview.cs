using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("exit_interview", Schema = "hr")]
    public class ExitInterview : HrisEntityBase
    {
		[Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("termination_id"), StringLength(50)]
        public string TerminationId { get; set; }

        [MaxLength(50)]
        [Column("category_exit_interview_id"), StringLength(50)]
        public string CategoryExitInterviewId { get; set; }

        [MaxLength(50)]
        [Column("exit_interview_item_id"), StringLength(50)]
        public string ExitInterviewItemId { get; set; }

        [Column(nameof(Result)), StringLength(2147483647)]
        public string Result { get; set; }

		[NotMapped]
        public Termination Termination { get; set; }

        [NotMapped]
        [MaxLength(255)]
        [Column("text_item"), StringLength(255)]
        public string TextItem { get; set; }

        [NotMapped]
        [Column(nameof(Type))]
        public int Type { get; set; }

        [NotMapped]
        [Column(nameof(No))]
        public int? No { get; set; }
    }
}