using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTool;
using DataAccess;

namespace DataAccess.PhoenixERP.Finance
{
    public partial class vendor_quotation_vendor : IEntity, IBaseRecord
    {
        #region Default IBaseRecord

        public static readonly bool IsConstant;

        public static readonly string EntityId;

        public static readonly string EntityName;

        public static readonly string EntityDisplayName;

        public static readonly string DefaultView;

        public static readonly Dictionary<string, string> DefaultViewColumns;

        public static readonly Dictionary<string, string> DefaultViewColumnTitles;

        public static readonly Dictionary<string, string> DefaultViewOrders;

        public static readonly Dictionary<string, dynamic> FilterDynamicBuilder;

        #endregion

        static vendor_quotation_vendor()
        {
            IsConstant = false;
            EntityName = GetTableName();
            EntityDisplayName = GetTableName(true);
            EntityId = GUIDHash.ConvertToMd5HashGUID(EntityName).ToString();

            DefaultView = " SELECT "
                        + " r.id, "
                        + " r.created_by, "
                        + " r.created_on, "
                        + " r.modified_by, "
                        + " r.modified_on, "
                        + " r.approved_by, "
                        + " r.approved_on, "
                        + " r.deleted_by, "
                        + " r.deleted_on, "
                        + " r.is_active, "
                        + " r.is_locked, "
                        + " r.is_default, "
                        + " r.is_deleted, "
                        + " r.owner_id, "

                        + " r.vendor_quotation_id, "
                        + " r.currency_id, "
                        + " ccy.currency_name, "
                        + " r.vendor_id, "
                        + " v.company_name AS vendor_name, "
                        + " r.exchange_rate, "
                        + " r.delivery_date, "
                        + " r.expiry_date, "
                        + " r.delivery_address, "
                        + " r.term_of_payment, "
                        + " r.amount, "
                        + " r.discount, "
                        + " r.approval_id, "
                        + " apv.approval_number, "
                        + " u0.app_fullname AS record_created_by, "
                        + " u1.app_fullname AS record_modified_by, "
                        + " u2.app_fullname AS record_approved_by, "
                        + " u3.app_fullname AS record_owner, "
                        + " t.team_name AS record_owning_team, "
                        + " rfq.is_prefered_vendor, "
                        + " pb.id AS purchase_bidding_id, "
                        + " qu.purchase_order_id, "

                        + " u0.app_fullname AS record_created_by, "
                        + " u1.app_fullname AS record_modified_by, "
                        + " u2.app_fullname AS record_approved_by, "
                        + " u3.app_fullname AS record_owner, "
                        + " t.team_name AS record_owning_team "

                        + " FROM fn.vendor_quotation_vendor AS r "
                        + " LEFT OUTER JOIN fn.vendor_quotation AS qu ON r.vendor_quotation_id = qu.id "
                        + " LEFT OUTER JOIN dbo.currency AS ccy ON r.currency_id = ccy.id "
                        + " LEFT OUTER JOIN fn.request_for_quotation AS rfq ON qu.rfq_id = rfq.id "
                        + " LEFT OUTER JOIN fn.purchase_bidding AS pb ON rfq.id = pb.rfq_id AND pb.is_active = 1 AND COALESCE(pb.is_deleted,0)= 0 "
                        + " LEFT OUTER JOIN fn.company AS v ON r.vendor_id = v.id "
                        + " LEFT OUTER JOIN dbo.approval AS apv ON r.approval_id = apv.id "
                        + " LEFT OUTER JOIN dbo.application_user AS u0 ON r.created_by = u0.id "
                        + " LEFT OUTER JOIN dbo.application_user AS u1 ON r.modified_by = u1.id "
                        + " LEFT OUTER JOIN dbo.application_user AS u2 ON r.approved_by = u2.id "
                        + " LEFT OUTER JOIN dbo.application_user AS u3 ON r.owner_id = u3.id "
                        + " LEFT OUTER JOIN dbo.team AS t ON r.owner_id = t.id ";

            DefaultViewColumns = new Dictionary<string, string>();
            DefaultViewColumns.Add("id", "r.id");
            DefaultViewColumns.Add("created_by", "r.created_by");
            DefaultViewColumns.Add("created_on", "r.created_on");
            DefaultViewColumns.Add("modified_by", "r.modified_by");
            DefaultViewColumns.Add("modified_on", "r.modified_on");
            DefaultViewColumns.Add("approved_by", "r.approved_by");
            DefaultViewColumns.Add("approved_on", "r.approved_on");
            DefaultViewColumns.Add("deleted_by", "r.deleted_by");
            DefaultViewColumns.Add("deleted_on", "r.deleted_on");
            DefaultViewColumns.Add("is_active", "r.is_active");
            DefaultViewColumns.Add("is_locked", "r.is_locked");
            DefaultViewColumns.Add("is_default", "r.is_default");
            DefaultViewColumns.Add("is_deleted", "r.is_deleted");
            DefaultViewColumns.Add("owner_id", "r.owner_id");


            DefaultViewColumns.Add("vendor_quotation_id","r.vendor_quotation_id");
            DefaultViewColumns.Add("currency_id", "r.currency_id");
            DefaultViewColumns.Add("currency_name", "ccy.currency_name");
            DefaultViewColumns.Add("vendor_id", "r.vendor_id");
            DefaultViewColumns.Add("vendor_name", "v.company_name");
            DefaultViewColumns.Add("exchange_rate", "r.exchange_rate");
            DefaultViewColumns.Add("delivery_date", "r.delivery_date");
            DefaultViewColumns.Add("expiry_date", "r.expiry_date");
            DefaultViewColumns.Add("delivery_address", "r.delivery_address");
            DefaultViewColumns.Add("term_of_payment", "r.term_of_payment");
            DefaultViewColumns.Add("amount", "r.amount");
            DefaultViewColumns.Add("discount", "r.discount");
            DefaultViewColumns.Add("approval_id", "r.approval_id");
            DefaultViewColumns.Add("approval_number", "apv.approval_number");
            DefaultViewColumns.Add("is_prefered_vendor", "rfq.is_prefered_vendor");
            DefaultViewColumns.Add("purchase_bidding_id", "pb.id");
            DefaultViewColumns.Add("purchase_order_id", "qu.purchase_order_id");


            DefaultViewColumns.Add("record_created_by", "u0.app_fullname");
            DefaultViewColumns.Add("record_modified_by", "u1.app_fullname");
            DefaultViewColumns.Add("record_approved_by", "u2.app_fullname");
            DefaultViewColumns.Add("record_owner", "u3.app_fullname");
            DefaultViewColumns.Add("record_owning_team", "t.team_name");

            DefaultViewColumnTitles = new Dictionary<string, string>();
            TextInfo ti = new CultureInfo("en-US", false).TextInfo;

            foreach (var c in DefaultViewColumns)
            {
                DefaultViewColumnTitles.Add(c.Key, ti.ToTitleCase(c.Key.Replace("_", " ")));
            }

            DefaultViewOrders = new Dictionary<string, string>();
            DefaultViewOrders.Add("quotation_number", "ASC");
        }

        public bool CheckIfConstant()
        {
            return IsConstant;
        }

        public string GetEntityId()
        {
            return EntityId;
        }

        public string GetEntityName()
        {
            return EntityName;
        }

        public string GetEntityDisplayName()
        {
            return EntityDisplayName;
        }

        public string GetDefaultView()
        {
            return DefaultView;
        }

        public Dictionary<string, string> GetDefaultViewColumns()
        {
            return DefaultViewColumns;
        }

        public Dictionary<string, string> GetDefaultViewColumnTitles()
        {

            return DefaultViewColumnTitles;
        }

        public Dictionary<string, string> GetDefaultViewOrders()
        {
            return DefaultViewOrders;
        }

        public Dictionary<string, dynamic> GetFilterDynamicBuilder()
        {
            return FilterDynamicBuilder;
        }
    }
}
