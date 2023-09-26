using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.Pm.MasterData
{
    [Table("master_day_timesheet_user_locked", Schema = "pm")]
    public class MasterDayTimesheetUserLocked : PmEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
        [Column("day")]
        public int day { get; set; }
        [MaxLength(100)]
        [Column("description"), StringLength(50)]
        public string description { get; set; }
        [NotMapped]
        public string BusinessUnitName { get; set; }
    }
}
