using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.Pm.MasterData
{
    [Table("master_non_job", Schema = "pm")]
    public class MasterNonJob : PmEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
        [MaxLength(50)]
        [Column("name"), StringLength(50)]
        public string name { get; set; }
        [MaxLength(100)]
        [Column("description"), StringLength(50)]
        public string description { get; set; }
        [NotMapped]
        public string BusinessUnitName { get; set; }
    }
}
