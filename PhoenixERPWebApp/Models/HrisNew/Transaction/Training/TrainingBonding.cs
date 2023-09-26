using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class TrainingBonding : HrisEntityBase
    {
        public string Id { get; set; }
        public string TrainingId { get; set; }
        public string EmployeeBasicInfoId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Venue { get; set; }
        public string Status { get; set; }
        public string Code { get; set; }
        public string BusinessUnitId { get; set; }

        //not mapped
        public string GroupId { get; set; }
        public string SubgroupId { get; set; }
        public string DivisionId { get; set; }
        public string DepartementId { get; set; }

        //notmapped
        public string EmployeeBasicInfoName { get; set; }
        public string BusinessUnitName { get; set; }
        public string TrainingName { get; set; }
        public string TrainingType { get; set; }
        public bool? isApprove { get; set; }
        public string RemarkRejected { get; set; }
        public string FileUpload { get; set; }
        public string FileUploadName { get; set; }
        public string GradeName { get; set; }
        public string TrainingRequestCode { get; set; }

    }
}