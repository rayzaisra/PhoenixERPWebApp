using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
    [Table(nameof(TransactionLog), Schema = "Um")]
    public class TransactionLog
    {
        [Key, Required]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string UniqueId { get; set; }
        public string IpAddress { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Area { get; set; }
        public string HttpMethod { get; set; }
        public string Route { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
