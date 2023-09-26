using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace DataAccess.PhoenixERP.Finance
{
    public partial class vw_purchase_request
    {
        #region Custom Result
        public dynamic RequestForQuotation
        {
            get
            {
                try
                {
                    return JsonConvert.DeserializeObject(this.rfq_list);
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
