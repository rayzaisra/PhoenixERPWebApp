using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.Pm.MasterData
{
    [Table("bussiness_type", Schema = "pm")]
    public class BusinessType : PmEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
        [MaxLength(50)]
        [Column("propose_type"), StringLength(50)]
        public string proposeType { get; set; }

        [NotMapped]
        public string BusinessUnitName { get; set; }
    }
}
