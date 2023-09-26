using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class VwMasterJobClosing
    {
        [DisplayName("Main Service Category Related")]
        public string MainserviceCategoryId { get; set; }

        [DisplayName("Main Service Category")]
        public string MainserviceCategoryName { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnitId { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnitName { get; set; }

        public List<MapCoaTransaksi> AP { get; set; }
        public List<MapCoaTransaksi> AR { get; set; }

        public bool? isEdit { get; set; }
    }
}