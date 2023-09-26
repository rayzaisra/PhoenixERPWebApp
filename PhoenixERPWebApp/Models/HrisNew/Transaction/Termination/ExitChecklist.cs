using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class ExitChecklist : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Termination Id")]
        public string TerminationId { get; set; }
        [DisplayName("Employee Checklist Id")]
        public string EmployeeChecklistId { get; set; }

        [DisplayName("Comment")]
        public string Notes { get; set; }

        [DisplayName("Done")]
        public bool? IsDone { get; set; }

        [Required]
        [DisplayName("Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        public Termination Termination { get; set; }

        [DisplayName("ITEM")]
        public string Checklistitem { get; set; }

        [DisplayName("In-Charge")]
        public string InCharge { get; set; }
    }
}