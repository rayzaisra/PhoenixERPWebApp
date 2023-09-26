using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class LegalEntity : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Registration Code")]
        public string RegistrationCode { get; set; }

        [DisplayName("Legal Entity Name")]
        public string LegalEntityName { get; set; }

        [DisplayName("Website")]
        public string Website { get; set; }

        [DisplayName("Director Name")]
        public string DirectorName { get; set; }

        [DisplayName("NPWP number")]
        public string NPWPNumber { get; set; }

        [DisplayName("Country")]
        public string OfficeCountryId { get; set; }

        [DisplayName("Province")]
        public string OfficeProvinceId { get; set; }

        [DisplayName("City")]
        public string OfficeCityId { get; set; }

        [DisplayName("NPWP Address")]
        public string OfficeAddress { get; set; }

        [DisplayName("Zip Code")]
        public string OfficeZipCode { get; set; }

        [DisplayName("Set From Office Address")]
        public bool? IsFromOfficeAddress { get; set; }

        [DisplayName("Country")]
        public string BillingCountryId { get; set; }

        [DisplayName("Province")]
        public string BillingProvinceId { get; set; }

        [DisplayName("City")]
        public string BillingCityId { get; set; }

        [DisplayName("Billing Address")]
        public string BillingAddress { get; set; }

        [DisplayName("Zip Code")]
        public string BillingZipCode { get; set; }

        [DisplayName("Tax Scan")]
        public string ImageTaxScan { get; set; }

        [DisplayName("Legal Scan")]
        public string ImageLegalScan { get; set; }

        [DisplayName("Contact Name")]
        public string ContactName { get; set; }

        [DisplayName("Phone")]
        public string Phone { get; set; }

        [DisplayName("Mobile Phone")]
        public string MobilePhone { get; set; }

        [DisplayName("Fax")]
        public string Fax { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Twitter")]
        public string Twitter { get; set; }

        [DisplayName("Facebook")]
        public string Facebook { get; set; }

        [DisplayName("Instagram")]
        public string Instagram { get; set; }

        //Not Mapped
        [DisplayName("Country")]
        public string OfficeCountryName { get; set; }

        [DisplayName("Province")]
        public string OfficeProvinceName { get; set; }

        [DisplayName("City")]
        public string OfficeCityName { get; set; }

        [DisplayName("Tax Scan")]
        public string ImageTaxScanName { get; set; }

        [DisplayName("Legal Scan")]
        public string ImageLegalScanName { get; set; }
    }
}