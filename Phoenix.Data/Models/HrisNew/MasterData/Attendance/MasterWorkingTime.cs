using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("master_working_time", Schema = "hr")]
    public class MasterWorkingTime : HrisEntityBase
    {
        [Key]
        [MaxLength(100)]
        [Column(nameof(Id)), StringLength(100)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column(nameof(Name)), StringLength(50)]
        public string Name { get; set; }

        [Column("time_in")]
        public TimeSpan? TimeIn { get; set; }

        [Column("time_out")]
        public TimeSpan? TimeOut { get; set; }

		[Column(nameof(IsSunday))]
		public bool? IsSunday { get; set; }

		[Column(nameof(IsMonday))]
		public bool? IsMonday { get; set; }

		[Column(nameof(IsTuesday))]
		public bool? IsTuesday { get; set; }

		[Column(nameof(IsWednesday))]
		public bool? IsWednesday { get; set; }

		[Column(nameof(IsThursday))]
		public bool? IsThursday { get; set; }

		[Column(nameof(IsFriday))]
		public bool? IsFriday { get; set; }

		[Column(nameof(IsSaturday))]
		public bool? IsSaturday { get; set; }
	}
}