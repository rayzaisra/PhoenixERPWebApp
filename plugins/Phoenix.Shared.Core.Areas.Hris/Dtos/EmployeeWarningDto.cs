using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class EmployeeWarningDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(2147483647)]
        public string description { get; set; }
        [StringLength(2147483647)]
        public string notes { get; set; }
        [StringLength(100)]
        public string subject { get; set; }
        [StringLength(50)]
        public string warning_by { get; set; }
        public DateTime? warning_date { get; set; }
        [StringLength(50)]
        public string warning_to { get; set; }
    }
    public class EmployeeWarningNew
    {
        [StringLength(2147483647)]
        public string description { get; set; }
        [StringLength(2147483647)]
        public string notes { get; set; }
        [StringLength(100)]
        public string subject { get; set; }
        [StringLength(50)]
        public string warning_by { get; set; }
        public DateTime? warning_date { get; set; }
        [StringLength(50)]
        public string warning_to { get; set; }
    }            
    public class EmployeeWarningSearch : SearchParameter
    {
        public string Search_description { get; set; }
        public string Search_notes { get; set; }
        public string Search_subject { get; set; }
        public string Search_warning_by { get; set; }
        public DateTime? Search_warning_date { get; set; }
        public string Search_warning_to { get; set; }
        
    }
}