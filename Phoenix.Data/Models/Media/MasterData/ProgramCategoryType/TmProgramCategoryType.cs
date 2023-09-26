using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.Media
{
    [Table("TmProgramCategoryType", Schema = "media")]
    public class TmProgramCategoryType : MediaEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("program_category_name"), StringLength(100)]
        public string ProgramCategoryName{ get; set; }
    }
}
