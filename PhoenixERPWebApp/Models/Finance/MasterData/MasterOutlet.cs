using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class MasterOutlet : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Client")]
        public string ClientId { get; set; }

        [DisplayName("Outlet Name")]
        public string OutletName { get; set; }

        [DisplayName("Address")]
        public string Address { get; set; }

        [DisplayName("Country")]
        public string CountryId { get; set; }

        [DisplayName("Province")]
        public string ProvinceId { get; set; }

        [DisplayName("City")]
        public string CityId { get; set; }

        [DisplayName("Zip Code")]
        public string ZipCode { get; set; }

        [DisplayName("Phone")]
        public string Phone { get; set; }

        [DisplayName("Mobile Phone")]
        public string MobilePhone { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Pic Name")]
        public string PicName { get; set; }

        [DisplayName("Type Outlet")]
        public string TypeOutlet { get; set; }

        //Not Mapped
        [DisplayName("Country")]
        public string CountryName { get; set; }

        [DisplayName("Province")]
        public string ProvinceName { get; set; }

        [DisplayName("City")]
        public string CityName { get; set; }
        //Not Mapped
        [DisplayName("Client Name")]
        public string ClientName { get; set; }
    }
}