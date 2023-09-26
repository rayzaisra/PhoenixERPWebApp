using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.HrisNew.Transaction.Recruitment
{
    public class EmployeeHistory
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        public string EHistEmployeeBasicInfoId { get; set; }

        [DisplayName("Name Employee")]
        public string EHistEmployeeName{ get; set; }

        [DisplayName("Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }

        [DisplayName("Category")]
        public string Category { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        [DisplayName("Remarks")]
        public string Remarks { get; set; }
        [DisplayName("File")]
        public string FileId { get; set; }

    }
}
