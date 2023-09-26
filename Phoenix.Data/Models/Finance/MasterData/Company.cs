using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table(nameof(Company), Schema = "fn")]
    public class Company : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("registration_code"), StringLength(50)]
        public string RegistrationCode { get; set; }

        [MaxLength(400)]
        [Column("company_name"), StringLength(400)]
        public string CompanyName { get; set; }

        [MaxLength(400)]
        [Column("company_alias_name"), StringLength(400)]
        public string CompanyAliasName { get; set; }

        [MaxLength(50)]
        [Column(nameof(Website)), StringLength(50)]
        public string Website { get; set; }

        [MaxLength(50)]
        [Column("tax_registration_number"), StringLength(50)]
        public string TaxRegistrationNumber { get; set; }

        [MaxLength(50)]
        [Column("office_country_id"), StringLength(50)]
        public string OfficeCountryId { get; set; }

        [MaxLength(50)]
        [Column("office_province_id"), StringLength(50)]
        public string OfficeProvinceId { get; set; }

        [MaxLength(50)]
        [Column("office_city_id"), StringLength(50)]
        public string OfficeCityId { get; set; }

        [MaxLength(350)]
        [Column("office_address"), StringLength(350)]
        public string OfficeAddress { get; set; }

        [MaxLength(50)]
        [Column("office_zip_code"), StringLength(50)]
        public string OfficeZipCode { get; set; }

        [Column("is_from_office_address")]
        public bool? IsFromOfficeAddress { get; set; }

        [MaxLength(50)]
        [Column("billing_country_id"), StringLength(50)]
        public string BillingCountryId { get; set; }

        [MaxLength(50)]
        [Column("billing_province_id"), StringLength(50)]
        public string BillingProvinceId { get; set; }

        [MaxLength(50)]
        [Column("billing_city_id"), StringLength(50)]
        public string BillingCityId { get; set; }

        [MaxLength(350)]
        [Column("billing_address"), StringLength(350)]
        public string BillingAddress { get; set; }

        [MaxLength(50)]
        [Column("billing_zip_code"), StringLength(50)]
        public string BillingZipCode { get; set; }

        [MaxLength(50)]
        [Column("image_tax_scan"), StringLength(50)]
        public string ImageTaxScan { get; set; }

        [MaxLength(50)]
        [Column("image_legal_scan"), StringLength(50)]
        public string ImageLegalScan { get; set; }

        [MaxLength(50)]
        [Column("bank_id"), StringLength(50)]
        public string BankId { get; set; }

        [MaxLength(50)]
        [Column("bank_name"), StringLength(50)]
        public string BankName { get; set; }

        [MaxLength(50)]
        [Column("account_number"), StringLength(50)]
        public string AccountNumber { get; set; }

        [MaxLength(50)]
        [Column("account_name"), StringLength(50)]
        public string AccountName { get; set; }

        [MaxLength(50)]
        [Column("currency_id"), StringLength(50)]
        public string CurrencyId { get; set; }

        [MaxLength(50)]
        [Column("contact_name"), StringLength(50)]
        public string ContactName { get; set; }

        [MaxLength(50)]
        [Column(nameof(Phone)), StringLength(50)]
        public string Phone { get; set; }

        [MaxLength(50)]
        [Column("mobile_phone"), StringLength(50)]
        public string MobilePhone { get; set; }

        [MaxLength(50)]
        [Column(nameof(Fax)), StringLength(50)]
        public string Fax { get; set; }

        [MaxLength(50)]
        [Column(nameof(Email)), StringLength(50)]
        public string Email { get; set; }

        [MaxLength(50)]
        [Column(nameof(Twitter)), StringLength(50)]
        public string Twitter { get; set; }

        [MaxLength(50)]
        [Column(nameof(Facebook)), StringLength(50)]
        public string Facebook { get; set; }

        [MaxLength(50)]
        [Column(nameof(Instagram)), StringLength(50)]
        public string Instagram { get; set; }

        //[Column("payment_term_day")]
        //public int? PaymentTermDay { get; set; }

        //[Column("shipping_term_day")]
        //public int? ShippingTermDay { get; set; }

        [Column("is_client")]
        public bool? IsClient { get; set; }

        [Column("is_vendor")]
        public bool? IsVendor { get; set; }

        [NotMapped]
        public string OfficeCountryName { get; set; }

        [NotMapped]
        public string OfficeProvinceName { get; set; }

        [NotMapped]
        public string OfficeCityName { get; set; }

        [NotMapped]
        public IFormFile fileImageTaxScan { get; set; }
        [NotMapped]
        public IFormFile fileImageLegalScan { get; set; }
        [NotMapped]
        public string ImageTaxScanName { get; set; }
        [NotMapped]
        public string ImageLegalScanName { get; set; }
    }
}