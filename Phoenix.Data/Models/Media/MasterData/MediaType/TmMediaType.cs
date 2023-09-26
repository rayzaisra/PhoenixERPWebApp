using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
    [Table("tmmediatype", Schema = "media")]
    public class TmMediaType : MediaEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("media_type_name"), StringLength(100)]
        public string MediaTypeName { get; set; }
    }
}
