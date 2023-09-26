using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table(nameof(TmAuthorization), Schema = "Um")]
    public class TmAuthorization
    {
        [Key]
        [MaxLength(150)]
        [Column(nameof(MenuId)), StringLength(150)]
        public string MenuId { get; set; }

        [MaxLength(50)]
        [Column(nameof(AreaName)), StringLength(50)]
        public string AreaName { get; set; }

        [MaxLength(50)]
        [Column(nameof(ControllerName)), StringLength(50)]
        public string ControllerName { get; set; }

        [Column(nameof(IsDeleted))]
        public bool? IsDeleted { get; set; }

        [MaxLength(150)]
        [Column(nameof(CreatedBy)), StringLength(150)]
        public string CreatedBy { get; set; }

        [Column(nameof(CreatedOn))]
        public DateTime? CreatedOn { get; set; }

        [MaxLength(150)]
        [Column(nameof(ModifiedBy)), StringLength(150)]
        public string ModifiedBy { get; set; }

        [Column(nameof(ModifiedOn))]
        public DateTime? ModifiedOn { get; set; }

        [Column(nameof(IsActive))]
        public bool? IsActive { get; set; }
    }
}