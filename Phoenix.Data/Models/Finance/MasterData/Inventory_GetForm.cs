using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.Finance.MasterData
{ 
    [Table("inventory_get_from", Schema = "fn")]
    public class Inventory_GetForm: FinanceEntityBase
    {

        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
        [MaxLength(50)]
        [StringLength(450)]
        public string getform_name { get; set; }
        [MaxLength(50)]
        [StringLength(450)]
        public string getform_status { get; set; }
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

        /*
         * [id] [nvarchar](50) NOT NULL,
	[getform_name] [nvarchar](450) NULL,
	[getform_status] [nvarchar](450) NULL,
	[created_by] [nvarchar](50) NULL,
	[created_on] [datetime] NULL,
	[modified_by] [nvarchar](50) NULL,
	[modified_on] [datetime] NULL,
	[approved_by] [nvarchar](50) NULL,
	[approved_on] [datetime] NULL,
	[deleted_by] [nvarchar](50) NULL,
	[deleted_on] [datetime] NULL,
	[is_active] [bit] NULL,
	[is_locked] [bit] NULL,
	[is_deleted] [bit] NULL,
	[is_default] [bit] NULL,
	[owner_id] [nvarchar](50) NULL,
         */
    }
}
