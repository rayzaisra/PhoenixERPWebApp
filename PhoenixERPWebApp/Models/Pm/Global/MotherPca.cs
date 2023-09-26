using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class MotherPca : PmEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Code")]
        public string Code { get; set; }

        [DisplayName("Business Unit Id")]
        public string BusinessUnitId { get; set; }

        [DisplayName("Detail")]
        public string Detail { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Total")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public double? Total { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Revision")]
        public string Revision { get; set; }
    }
}