using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class TrainingRequisitionDetail : HrisEntityBase
    {
        public string Id { get; set; }
        public string TrainingRequisitionId { get; set; }
        public string EmployeeBasicInfoId { get; set; }


        public string EmployeeBasicInfoName { get; set; }
        public string JobTitleId { get; set; }
        public string JobTitleName { get; set; }
        public string JobGradeId { get; set; }
        public string JobGradeName { get; set; }
        public string DivisionId { get; set; }
        public string DivisionName { get; set; }
    }

    public class TrainingRequisitionDetailFilter
    {

        public string FilterGroupId { get; set; }
        public string FilterSubgroupId { get; set; }
        public string FilterDivisionId { get; set; }
        public string FilterDepartementId { get; set; }
    }
}