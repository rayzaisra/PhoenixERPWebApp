using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.Media
{
    [Table("tmmediaplanrequesttype", Schema = "media")]
    public class TmMediaPlanRequestType : MediaEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("request_type_name"), StringLength(100)]
        public string RequestTypeName { get; set; }
    }
}
