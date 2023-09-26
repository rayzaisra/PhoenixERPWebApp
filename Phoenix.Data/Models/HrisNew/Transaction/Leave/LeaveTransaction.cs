using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("leave_transaction", Schema = "hr")]
    public class LeaveTransaction : HrisEntityBase
    {
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("leave_id"), StringLength(50)]
        public string LeaveId { get; set; }

        [Required]
        [Column(nameof(Total))]
        public int Total { get; set; }

        [Required]
        [MaxLength(50)]
        [Column(nameof(Type)), StringLength(50)]
        public string Type { get; set; }

        [Required]
        [Column(nameof(Month))]
        public int Month { get; set; }

        [Required]
        [Column(nameof(Year))]
        public int Year { get; set; }

        [Required]
        [Column("taken_date")]
        public DateTime TakenDate { get; set; }

        public Leave Leave { get; set; }
    }
}