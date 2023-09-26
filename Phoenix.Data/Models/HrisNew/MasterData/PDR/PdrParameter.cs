using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("pdr_parameter", Schema = "hr")]
    public class PdrParameter : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("pdr_category_id"), StringLength(50)]
        public string PdrCategoryId { get; set; }

        [MaxLength(1)]
        [Column(nameof(Alphabet)), StringLength(1)]
        public string Alphabet { get; set; }

        [MaxLength(100)]
        [Column(nameof(Subject)), StringLength(100)]
        public string Subject { get; set; }

        [MaxLength(250)]
        [Column(nameof(Description)), StringLength(250)]
        public string Description { get; set; }

        [Column("key_behavior_1"), StringLength(2147483647)]
        public string KeyBehavior1 { get; set; }

        [Column("key_behavior_2"), StringLength(2147483647)]
        public string KeyBehavior2 { get; set; }

        [Column("key_behavior_3"), StringLength(2147483647)]
        public string KeyBehavior3 { get; set; }

        [Column("key_behavior_4"), StringLength(2147483647)]
        public string KeyBehavior4 { get; set; }

        [Column("key_behavior_5"), StringLength(2147483647)]
        public string KeyBehavior5 { get; set; }

        [MaxLength(50)]
        [Column("job_grade_id"), StringLength(50)]
        public string JobGradeId { get; set; }

        [NotMapped]
        public string Grade { get; set; }

        [NotMapped]
        public string PdrCategoryName { get; set; }
    }
}