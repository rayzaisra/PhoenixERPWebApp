using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.Media.MasterData.ProgramPosition
{
    [Table("TmProgramPosition", Schema = "media")]
    public class TmProgramPosition : MediaEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(150)]
        [Column("program_prosition_name"), StringLength(150)]
        public string ProgramPositionName { get; set; }
    }
}
