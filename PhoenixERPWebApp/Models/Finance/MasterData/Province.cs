using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class Province : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Province Name")]
        public string ProvinceName { get; set; }

        [DisplayName("Country Name")]
        public string CountryId { get; set; }

        [DisplayName("Country Name")]
        public string CountryName { get; set; }
    }
}