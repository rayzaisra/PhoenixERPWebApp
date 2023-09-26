using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.Pm.MasterData
{
    [Table("job_status", Schema = "pm")]
    public class JobStatus : PmEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("name"), StringLength(50)]
        public string name { get; set; }


        [NotMapped]
        public string StatusName { get; set; }
        [NotMapped]
        public string StatusOrder { get; set; }
        [Column("is_showing_on_timesheet")]
        public bool? is_showing_on_timesheet { get; set; }

    }
}
