using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("employee_service_request", Schema = "hr")]
    public class EmployeeServiceRequest : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(30)]
        [Column("request_code"), StringLength(30)]
        public string RequestCode { get; set; }

        [Column("request_date")]
        public DateTime? RequestDate { get; set; }

        [Column(nameof(Status))]
        public int? Status { get; set; }

        [MaxLength(50)]
        [Column("employee_basic_info_id"), StringLength(50)]
        public string EmployeeBasicInfoId { get; set; }

        [MaxLength(50)]
        [Column("master_mapping_request_id"), StringLength(50)]
        public string MasterMappingRequestId { get; set; }

        [Column("due_date")]
        public DateTime? DueDate { get; set; }

        [Column(nameof(Remarks)), StringLength(2147483647)]
        public string Remarks { get; set; }

        [Column(nameof(Fullfilment)), StringLength(2147483647)]
        public string Fullfilment { get; set; }

        [Column("follow_up_remarks"), StringLength(2147483647)]
        public string FollowUpRemarks { get; set; }

        [Column("date_close")]
        public DateTime? DateClose { get; set; }

        [Column(nameof(Comment)), StringLength(2147483647)]
        public string Comment { get; set; }

        [MaxLength(10)]
        [Column(nameof(Rating)), StringLength(10)]
        public string Rating { get; set; }
        [NotMapped]
        public string RequestBy { get; set; }
        [NotMapped]
        public string ServiceRequestCatalogue { get; set; }
        [NotMapped]
        public string Description { get; set; }
        [NotMapped]
        public string PIC { get; set; }
        [NotMapped]
        public InfoEmployee requestEmployee { get; set; }
    }
}