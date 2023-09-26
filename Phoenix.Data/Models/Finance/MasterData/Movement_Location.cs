using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.Finance.MasterData
{
    [Table("inventory_movement", Schema = "fn")]
    public class Movement_Location : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
        [MaxLength(50)]
        [StringLength(450)]
        public string remarks { get; set; }
        [MaxLength(50)]
        [StringLength(450)]
        public string movement_name { get; set; }
        public string created_by { get; set; }
        public DateTime? created_on { get; set; }
        public string modified_by { get; set; }
        public DateTime? modified_on { get; set; }
        public string approved_by { get; set; }
        public DateTime? approved_on { get; set; }
        public string deleted_by { get; set; }
        public DateTime? deleted_on { get; set; }
        public bool? is_active { get; set; }
        public bool? is_locked { get; set; }
        public bool? is_default { get; set; }
        public bool? is_deleted { get; set; }
        public string owner_id { get; set; }
        public bool? location { get; set; }
 
    }
}
