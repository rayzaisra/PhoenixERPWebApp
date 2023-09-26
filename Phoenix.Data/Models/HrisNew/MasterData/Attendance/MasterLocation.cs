using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("master_location", Schema = "hr")]
    public class MasterLocation : HrisEntityBase
    {
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(255)]
        [Column("location_name"), StringLength(255)]
        public string LocationName { get; set; }

        [MaxLength(255)]
        [Column("map_lat"), StringLength(255)]
        public string MapLat { get; set; }

        [MaxLength(255)]
        [Column("map_long"), StringLength(255)]
        public string MapLong { get; set; }
    }
}