using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.Pm.MasterData
{

    [Table(nameof(Channel), Schema = "pm")]
    public class Channel : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Required]
        [MaxLength(250)]
        [Column("channel"), StringLength(250)]
        public string ChannelName { get; set; }

        [Column("central_resource_id")]
        public long? CentralResourceId { get; set; }

        [Column("value")]
        public float? Value { get; set; }

        [MaxLength(250)]
        [Column("note"), StringLength(250)]
        public string Note { get; set; }
    }
}
