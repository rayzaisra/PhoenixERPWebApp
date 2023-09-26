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
    public partial class vendor_quotation_detail : IEntity, IBaseRecord
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

        static vendor_quotation_detail()
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

                        + " r.item_id, "
                        + " i1.item_code, "
                        + " r.item_name, "
                        + " r.task_detail, "
                        + " r.item_type_id, "
                        + " it.type_name, "
                        + " r.currency_id, "
                        + " r.exchange_rate, "
                        + " r.timeline_production, "
                        + " r.qty, "
                        + " (rfqd.unit_price * rfqd.exchange_rate) AS estimated_unit_price, "
                        + " rfqd.amount AS estimated_amount, "
                        + " r.unit_price, "
                        + " r.amount, "
                        + " r.discount_amount, "
                        + " r.is_discount_pct, "
                        + " r.tax_base, "
                        + " r.tax_vat, "
                        + " r.tax_whd, "
                        + " r.amount_final, "
                        + " r.uom_id, "
                        + " u.unit_name AS uom_name,"
                        + " v.vendor_quotation_number, "
                        + " r.rfq_detail_id, "
                        + " v.id AS vendor_quotation_id, "
                        + " v.vendor_id, "

                         + " u0.app_fullname AS record_created_by, "
                         + " u1.app_fullname AS record_modified_by, "
                         + " u2.app_fullname AS record_approved_by, "
                         + " u3.app_fullname AS record_owner, "
                         + " t.team_name AS record_owning_team "

                         + " FROM             fn.vendor_quotation_detail AS r "
                         + " LEFT OUTER JOIN fn.request_for_quotation_detail AS rfqd ON r.rfq_detail_id = rfqd.id "
                         + " LEFT OUTER JOIN fn.vendor_quotation AS v ON r.vendor_quotation_id = v.id "
                         + " LEFT OUTER JOIN dbo.item AS i1 ON r.item_id = i1.id "
                         + " LEFT OUTER JOIN dbo.uom AS u ON r.uom_id = u.id "
                         + " LEFT OUTER JOIN dbo.item_type AS it ON r.item_type_id = it.id "
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

            DefaultViewColumns.Add("item_id", "r.item_id");
            DefaultViewColumns.Add("item_code", "i1.item_code");
            DefaultViewColumns.Add("item_name", "r.item_name");
            DefaultViewColumns.Add("task_detail", "r.task_detail");
            DefaultViewColumns.Add("item_type_id", "r.item_type_id");
            DefaultViewColumns.Add("type_name", "it.type_name");
            DefaultViewColumns.Add("exchange_rate", "r.exchange_rate");
            DefaultViewColumns.Add("timeline_production", "r.timeline_production");
            DefaultViewColumns.Add("qty", "r.qty");
            DefaultViewColumns.Add("estimated_unit_price", "(rfqd.unit_price * rfqd.exchange_rate)");
            DefaultViewColumns.Add("estimated_amount", "rfqd.amount");
            DefaultViewColumns.Add("unit_price", "r.unit_price");
            DefaultViewColumns.Add("amount", "r.amount");
            DefaultViewColumns.Add("discount", "r.discount");
            DefaultViewColumns.Add("is_discount_pct", "r.is_discount_pct");
            DefaultViewColumns.Add("tax_base", "r.tax_base");
            DefaultViewColumns.Add("tax_vat", "r.tax_vat");
            DefaultViewColumns.Add("tax_whd", "r.tax_whd");
            DefaultViewColumns.Add("amount_final", "r.amount_final");
            DefaultViewColumns.Add("vendor_quotation_number", "v.vendor_quotation_number");
            DefaultViewColumns.Add("uom_id", "r.uom_id");
            DefaultViewColumns.Add("uom_name", "u.unit_name");
            DefaultViewColumns.Add("rfq_detail_id", "r.rfq_detail_id");


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
            DefaultViewOrders.Add("vendor_quotation_number", "ASC");
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
