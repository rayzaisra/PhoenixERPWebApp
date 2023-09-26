using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class LegalDocumentCounterpart : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Counterpart Name")]
        public string CounterpartId { get; set; }

        [DisplayName("Legal Document Id")]
        public string LegalDocumentId { get; set; }

        [DisplayName("Counterpart")]
        public string CounterpartName { get; set; }
    }
}