using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class Company : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Registration Code")]
        public string RegistrationCode { get; set; }

        [DisplayName("Company Name")]
        public string CompanyName { get; set; }

        [DisplayName("Company Alias Name")]
        public string CompanyAliasName { get; set; }

        [DisplayName("Website")]
        public string Website { get; set; }

        [DisplayName("Tax Registration Number")]
        public string TaxRegistrationNumber { get; set; }

        [DisplayName("Country")]
        public string OfficeCountryId { get; set; }

        [DisplayName("Province")]
        public string OfficeProvinceId { get; set; }

        [DisplayName("City")]
        public string OfficeCityId { get; set; }

        [DisplayName("Office Address")]
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

        [DisplayName("Bank")]
        public string BankId { get; set; }

        [DisplayName("Bank")]
        public string BankName { get; set; }

        [DisplayName("Account Number")]
        public string AccountNumber { get; set; }
        [DisplayName("Account Name(Beneficiary)")]
        public string AccountName { get; set; }

        [DisplayName("Currency")]
        public string CurrencyId { get; set; }

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

        //[DisplayName("Payment Term Day")]
        //public int? PaymentTermDay { get; set; }

        //[DisplayName("Shipping Term Day")]
        //public int? ShippingTermDay { get; set; }

        [DisplayName("Is Client")]
        public bool? IsClient { get; set; }

        [DisplayName("Is Vendor")]
        public bool? IsVendor { get; set; }

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

        [NotMapped]
        public string ParamColumn { get; set; }

        [NotMapped]
        public string ParamValue { get; set; }
    }
}