using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
    [Table("trmediafilemaster", Schema = "media")]
    public class TrMediaFileMaster : MediaEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
        
        [MaxLength(50)]
        [Column("filemaster_id"), StringLength(50)]
        public string FileMasterId { get; set; }
        
        [MaxLength(50)]
        [Column("trmediaplantv_id"), StringLength(50)]
        public string TrMediaPlanTvId { get; set; }
        
        [MaxLength(50)]
        [Column("tmmediatype_id"), StringLength(50)]
        public string TmMediaTypeId { get; set; }

        [NotMapped]
        public string FileMasterName { get; set; }

        [NotMapped]
        public IFormFile TemplateFile { get; set; }
    }
}
