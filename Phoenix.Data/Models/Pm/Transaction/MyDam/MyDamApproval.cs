using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{

    [Table("mydam_approval", Schema = "pm")]
    public class MyDamApproval : PmEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
        [Column("mydam_id")]
        public string MyDamId { get; set; }
        [Column("status")]
        public int? Status { get; set; }
        [Column("request_by")]
        public string RequestBy { get; set; }
        [Column("request_date")]
        public DateTime? RequestDate { get; set; }

    }
}
