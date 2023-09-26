using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("status_log", Schema = "dbo")]
    public class StatusLog
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(TransactionId)), StringLength(50)]
        public string TransactionId { get; set; }

        [Key]
        [Column(nameof(Seq))]
        public int? Seq { get; set; }

        [MaxLength(50)]
        [Column(nameof(EmployeeId)), StringLength(50)]
        public string EmployeeId { get; set; }

        [MaxLength(50)]
        [Column(nameof(EmployeeName)), StringLength(50)]
        public string EmployeeName { get; set; }

        [Column(nameof(StatusDate))]
        public DateTime? StatusDate { get; set; }

        [MaxLength(50)]
        [Column(nameof(Status)), StringLength(50)]
        public string Status { get; set; }

        [MaxLength(255)]
        [Column(nameof(Description)), StringLength(255)]
        public string Description { get; set; }
    }
}