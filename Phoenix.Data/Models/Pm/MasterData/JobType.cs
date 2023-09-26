using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.Pm.MasterData
{
    [Table("job_type", Schema = "pm")]
    public class JobType : PmEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
        [MaxLength(50)]
        [Column("name"), StringLength(50)]
        public string name { get; set; }

        [Column("description")]
        public string description { get; set; }
        [NotMapped]
        public string BusinessUnitName { get; set; }
    }
}
