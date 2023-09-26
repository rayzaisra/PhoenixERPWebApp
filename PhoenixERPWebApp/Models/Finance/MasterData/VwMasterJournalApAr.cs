using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class VwMasterJournalApAr
    {
        [DisplayName("Main Service Category")]
        public string MainserviceCategoryId { get; set; }

        [DisplayName("Main Service Category")]
        public string MainserviceCategoryName { get; set; }
        
        [DisplayName("Name Setting")]
        public string NameSetting { get; set; }

        [DisplayName("Pce Type Cross Billing")]
        public bool? PceTypeCrossBilling { get; set; }

        [DisplayName("Invoice Type")]
        public string InvoiceType { get; set; }

        //NOT MAPPED
        [DisplayName("Pce Type Cross Billing")]
        public string PceTypeCrossBillingName { get { return (PceTypeCrossBilling ?? false) ? "Yes" : "No"; } }
        public bool? isEdit { get; set; }
        public List<MapCoaTransaksi> Detail { get; set; }
    }
}