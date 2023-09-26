using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
    [Table(nameof(Institution), Schema = "hr")]
    public class Institution : HrisEntityBase
    {

        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("institution_name"), StringLength(50)]
        public string InstitutionName { get; set; }

        [Required]
        [Column("institution_address"), StringLength(2147483647)]
        public string InstitutionAddress { get; set; }

        [MaxLength(50)]
        [Column("institution_type"), StringLength(50)]
        public string InstitutionType { get; set; }

        [MaxLength(50)]
        [Column(nameof(City)), StringLength(50)]
        public string City { get; set; }
    }
}
