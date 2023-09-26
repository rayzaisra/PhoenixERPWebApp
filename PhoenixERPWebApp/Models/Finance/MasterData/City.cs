using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class City : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Country Name")]
        public string CountryId { get; set; }

        [DisplayName("Province Name")]
        public string ProvinceId { get; set; }

        [DisplayName("City Name")]
        public string CityName { get; set; }

        [DisplayName("Country Name")]
        public string CountryName { get; set; }

        [DisplayName("Province Name")]
        public string ProvinceName { get; set; }
    }
}