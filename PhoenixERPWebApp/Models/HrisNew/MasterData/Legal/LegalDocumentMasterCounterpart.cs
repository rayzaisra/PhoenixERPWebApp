using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class LegalDocumentMasterCounterpart : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Employee")]
        public string EmployeeBasicInfoId { get; set; }

        [DisplayName("Legal Category")]
        public string LegalCategoryId { get; set; }


        [DisplayName("Employee")]
        public string FullName { get; set; }
        [DisplayName("Legal Category")]
        public string LegalCategory { get; set; }
    }
}