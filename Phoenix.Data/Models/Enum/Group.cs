using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Data.Models
{
    public static class GroupAcess
    {
        //HRIS
		////id Dev
  //      public static string HCAdmin = "1be4026b-523e-46f0-9f59-b045ec6ee7a2";
  //      public static string SuperAdmin = "7f94d896-a2e7-4c9a-bc0c-d2c5c00a36db";

		//id prod
		public static string HCAdmin = "1be4026b-523e-46f0-9f59-b045ec6ee7a2";
		public static string SuperAdmin = "1908b01c-688a-4ed4-813e-5f622de0b794";


        //Finance
        //Proses Invoice Received, TAP dan AP
        public const string FrontDesk = "22c28111-4751-4e88-8d89-375dcbb61187"; 
        public const string AdminFinance = "a906b0b3-c466-4744-9010-3fcbc5dca939";
        public const string AccountingSupervisor = "c0f9fc8b-b2e7-432d-8399-aebd13c6e57b";
        public const string TaxSupervisor = "bf49a049-67a1-4173-a42f-d95a14f89d29";
        public const string FinanceManager = "ba1c39a1-603b-4985-a2a9-b4271a226ee4";
    }
}
