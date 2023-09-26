using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace Phoenix.Data.Models.Media.Transaction.MediaOrder
{
    [Table("order_detail", Schema = "media")]
    public class MediaOrderDetail : MediaPlanEntityBase
    {

        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Column("media_order_number"), StringLength(100)]
        public string MediaOrderNumber { get; set; }
        
        
        [Column("start_periode")]
        public DateTime StartPeriode { get; set; }

        [Column("end_periode")]
        public DateTime EndPeriode { get; set; }
        
        [Column("file_master_id"), StringLength(50)]
        public string FileMasterId { get; set; }

        [Column("file_name"), StringLength(100)]
        public string FileName { get; set; }

        [Column("revision")]
        public int Revision { get; set; }
        
        [Column("type_id"), StringLength(100)]
        public string TypeID { get; set; }

        [Column("is_checked")]
        public bool? IsChecked { get; set; }

        //[Column("campaign")]
        [NotMapped]
        public bool Campaign { get; set; }


        [NotMapped]
        public string TypeName { get; set; }
    }
}
