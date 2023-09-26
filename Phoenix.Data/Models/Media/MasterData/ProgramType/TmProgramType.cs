using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.Media.MasterData.ProgramType
{
    [Table("TmProgramType", Schema = "media")]
    public class TmProgramType : MediaEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(200)]
        [Column("program_type_name"), StringLength(200)]
        public string ProgramTypeName { get; set; }
    }
}
