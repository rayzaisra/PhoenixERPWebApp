using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("company_brand", Schema = "fn")]
    public class CompanyBrand : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("registration_code"), StringLength(50)]
        public string RegistrationCode { get; set; }

        [MaxLength(50)]
        [Column("company_id"), StringLength(50)]
        public string CompanyId { get; set; }

        [MaxLength(400)]
        [Column("brand_name"), StringLength(400)]
        public string BrandName { get; set; }

        [MaxLength(50)]
        [Column(nameof(Website)), StringLength(50)]
        public string Website { get; set; }

        [Column("asf_value")]
        public int? ASFValue { get; set; }

        [MaxLength(350)]
        [Column("brand_address"), StringLength(350)]
        public string BrandAddress { get; set; }

        [MaxLength(50)]
        [Column("country_id"), StringLength(50)]
        public string CountryId { get; set; }

        [MaxLength(50)]
        [Column("province_id"), StringLength(50)]
        public string ProvinceId { get; set; }

        [MaxLength(50)]
        [Column("city_id"), StringLength(50)]
        public string CityId { get; set; }

        [MaxLength(50)]
        [Column("zip_code"), StringLength(50)]
        public string ZipCode { get; set; }

        //[MaxLength(50)]
        //[Column("bank_id"), StringLength(50)]
        //public string BankId { get; set; }

        //[MaxLength(50)]
        //[Column("account_number"), StringLength(50)]
        //public string AccountNumber { get; set; }

        //[MaxLength(50)]
        //[Column("currency_id"), StringLength(50)]
        //public string CurrencyId { get; set; }

        [MaxLength(50)]
        [Column("contact_name"), StringLength(50)]
        public string ContactName { get; set; }

        [Column("work_phone")]
        public string WorkPhone { get; set; }

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

        //[MaxLength(50)]
        //[Column("division_id"), StringLength(50)]
        //public string DivisionId { get; set; }

        //[MaxLength(300)]
        //[Column("sub_brand"), StringLength(300)]
        //public string SubBrand { get; set; }

        //[MaxLength(50)]
        //[Column("team_id"), StringLength(50)]
        //public string TeamId { get; set; }

        //[MaxLength(50)]
        //[Column("account_name"), StringLength(50)]
        //public string AccountName { get; set; }

        [Column("is_client")]
        public bool? IsClient { get; set; }

        [Column("is_vendor")]
        public bool? IsVendor { get; set; }

        [NotMapped]
        public string CompanyName { get; set; }

        [Column("term_of_payment")]
        public int? TermOfPayment { get; set; }
    }
}