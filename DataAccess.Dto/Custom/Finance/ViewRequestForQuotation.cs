using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace DataAccess.PhoenixERP.Finance
{
    public partial class vw_request_for_quotationDTO
    {
        #region Custom Result
        public dynamic Vendor
        {
            get
            {
                try
                {
                    return JsonConvert.DeserializeObject(this.vendor_list);
                }
                catch (Exception)
                {
                    return new ExpandoObject();
                }

            }
        }
        #endregion
    }
}
