using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.Finance.Transaction.Warehouse
{
    [Table("warehouse")]
    public class Warehouse : FinanceEntityBase
    {
        [Column("organization_id")]  // ,<, nvarchar(50),>
        public string OrganizationId { get; set; }
        [Column("warehouse_name")]  //    ,<, nvarchar(450),>
        public string WarehouseName { get; set; }
        [Column("country_id")]  //   ,<, nvarchar(50),>
        public string CountryId { get; set; }
        [Column("city_id")]  //   ,<, nvarchar(50),>
        public string CityId { get; set; }
        [Column("address")]  //   ,<, nvarchar(450),>
        public string Address { get; set; }
        [Column("zip_code")]  //    ,<, nvarchar(5),>
        public string ZipCode { get; set; }
        [Column("phone")]  //   ,<, nvarchar(50),>
        public string Phone { get; set; }
        [Column("fax")]  //,<, nvarchar(50),>
        public string Fax { get; set; }
        [Column("manager_id")]//,<, nvarchar(50),
        public string ManagerId { get; set; }
    }
}
