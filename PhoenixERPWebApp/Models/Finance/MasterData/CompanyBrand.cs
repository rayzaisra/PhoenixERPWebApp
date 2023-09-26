using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class CompanyBrand : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Registration Code")]
        public string RegistrationCode { get; set; }

        [DisplayName("Company")]
        public string CompanyId { get; set; }

        [DisplayName("Brand Name")]
        public string BrandName { get; set; }

        [DisplayName("Website")]
        public string Website { get; set; }

        [DisplayName("ASF")]
        public int? ASFValue { get; set; }

        [DisplayName("Brand Address")]
        public string BrandAddress { get; set; }

        [DisplayName("Country")]
        public string CountryId { get; set; }

        [DisplayName("Province")]
        public string ProvinceId { get; set; }

        [DisplayName("City")]
        public string CityId { get; set; }

        [DisplayName("Zip Code")]
        public string ZipCode { get; set; }

        //[DisplayName("Bank")]
        //public string BankId { get; set; }

        //[DisplayName("Account Number")]
        //public string AccountNumber { get; set; }

        //[DisplayName("Currency")]
        //public string CurrencyId { get; set; }

        [DisplayName("Contact Name")]
        public string ContactName { get; set; }

        [DisplayName("Phone")]
        public string WorkPhone { get; set; }

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
        
        [DisplayName("Term Of Payment")]
        public int? TermOfPayment { get; set; }

        //[DisplayName("Division")]
        //public string DivisionId { get; set; }

        //[DisplayName("Sub Brand")]
        //public string SubBrand { get; set; }

        //[DisplayName("Team")]
        //public string TeamId { get; set; }

        //[DisplayName("Account Name")]
        //public string AccountName { get; set; }

        [DisplayName("Is Client")]
        public bool? IsClient { get; set; }

        [DisplayName("Is Vendor")]
        public bool? IsVendor { get; set; }

        //Not Mapped
        [DisplayName("Company Name")]
        public string CompanyName { get; set; }
    }
}