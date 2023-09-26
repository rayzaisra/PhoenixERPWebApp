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
    public partial class purchase_bidding_detail : IEntity, IBaseRecord
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

        static purchase_bidding_detail()
        {
            IsConstant = false;
            EntityName = GetTableName();
            EntityDisplayName = GetTableName(true);
            EntityId = GUIDHash.ConvertToMd5HashGUID(EntityName).ToString();

            var sb = new System.Text.StringBuilder(1827);
            sb.AppendLine(@"SELECT");
            sb.AppendLine(@"  r.id,");
            sb.AppendLine(@"  r.created_by,");
            sb.AppendLine(@"  r.created_on,");
            sb.AppendLine(@"  r.modified_by,");
            sb.AppendLine(@"  r.modified_on,");
            sb.AppendLine(@"  r.approved_by,");
            sb.AppendLine(@"  r.approved_on,");
            sb.AppendLine(@"  r.is_active,");
            sb.AppendLine(@"  r.is_locked,");
            sb.AppendLine(@"  r.is_default,");
            sb.AppendLine(@"  r.owner_id,");

            sb.AppendLine(@"  r.purchase_bidding_id,");
            sb.AppendLine(@"  pb.bidding_number,");
            sb.AppendLine(@"  pb.bidding_date,");
            sb.AppendLine(@"  r.rfq_id,");
            sb.AppendLine(@"  r.rfq_detail_id,");
            sb.AppendLine(@"  rfq.request_for_quotation_number,");
            sb.AppendLine(@"  rfq.request_for_quotation_date,");
            sb.AppendLine(@"  rfqd.item_id,");
            sb.AppendLine(@"  vq.vendor_id,");
            sb.AppendLine(@"  r.vendor_quotation_detail_id,");
            sb.AppendLine(@"  vqd.exchange_rate,");
            sb.AppendLine(@"  vqd.unit_price,");
            sb.AppendLine(@"  vqd.item_type_id,");
            sb.AppendLine(@"  vqd.item_name,");
            sb.AppendLine(@"  vqd.task_detail,");
            sb.AppendLine(@"  vqd.timeline_production,");
            sb.AppendLine(@"  vqd.qty,");
            sb.AppendLine(@"  vqd.uom_id,");
            sb.AppendLine(@"  vqd.amount,");
            sb.AppendLine(@"  vqd.discount_amount,");
            sb.AppendLine(@"  vqd.is_discount_pct,");
            sb.AppendLine(@"  vqd.tax_base,");
            sb.AppendLine(@"  vqd.tax_vat,");
            sb.AppendLine(@"  vqd.tax_whd,");
            sb.AppendLine(@"  vq.vendor_quotation_number,");
            sb.AppendLine(@"  vq.vendor_reference_number,");
            sb.AppendLine(@"  vq.delivery_address,");
            sb.AppendLine(@"  vq.delivery_date,");
            sb.AppendLine(@"  r.is_approved,");

            sb.AppendLine(@"  u0.app_fullname AS record_created_by,");
            sb.AppendLine(@"  u1.app_fullname AS record_modified_by,");
            sb.AppendLine(@"  u2.app_fullname AS record_approved_by,");
            sb.AppendLine(@"  u3.app_fullname AS record_owner,");
            sb.AppendLine(@"  t.team_name AS record_owning_team");

            sb.AppendLine(@"  FROM fn.purchase_bidding_detail AS r");
            sb.AppendLine(@"  LEFT OUTER JOIN fn.purchase_bidding AS pb");
            sb.AppendLine(@"  ON r.purchase_bidding_id = pb.id");
            sb.AppendLine(@"  LEFT OUTER JOIN fn.request_for_quotation_detail AS rfqd");
            sb.AppendLine(@"  ON r.rfq_detail_id = rfqd.id");
            sb.AppendLine(@"  LEFT OUTER JOIN fn.request_for_quotation AS rfq");
            sb.AppendLine(@"  ON rfqd.request_for_quotation_id = rfq.id");
            sb.AppendLine(@"  LEFT OUTER JOIN fn.vendor_quotation_detail AS vqd");
            sb.AppendLine(@"  ON vqd.id = r.vendor_quotation_detail_id");
            sb.AppendLine(@"  LEFT OUTER JOIN fn.vendor_quotation AS vq");
            sb.AppendLine(@"  ON vqd.vendor_quotation_id = vq.id");

            sb.AppendLine(@"  LEFT OUTER JOIN dbo.application_user AS u0");
            sb.AppendLine(@"  ON r.created_by = u0.id");
            sb.AppendLine(@"  LEFT OUTER JOIN dbo.application_user AS u1");
            sb.AppendLine(@"  ON r.modified_by = u1.id");
            sb.AppendLine(@"  LEFT OUTER JOIN dbo.application_user AS u2");
            sb.AppendLine(@"  ON r.approved_by = u2.id");
            sb.AppendLine(@"  LEFT OUTER JOIN dbo.application_user AS u3");
            sb.AppendLine(@"  ON r.owner_id = u3.id");
            sb.AppendLine(@"  LEFT OUTER JOIN dbo.team AS t");
            sb.AppendLine(@"  ON r.owner_id = t.id");
            sb.AppendLine(@"");


            DefaultView = sb.ToString();

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

            DefaultViewColumns.Add("purchase_bidding_id", "r.purchase_bidding_id");
            DefaultViewColumns.Add("bidding_number", "pb.bidding_number");
            DefaultViewColumns.Add("bidding_date", "pb.bidding_date");
            DefaultViewColumns.Add("rfq_id", "r.rfq_id");
            DefaultViewColumns.Add("rfq_detail_id", "r.rfq_detail_id");
            DefaultViewColumns.Add("request_for_quotation_number", "rfq.request_for_quotation_number");
            DefaultViewColumns.Add("request_for_quotation_date", "rfq.request_for_quotation_date");
            DefaultViewColumns.Add("item_id", "rfq.item_id");
            DefaultViewColumns.Add("vendor_id", "vq.vendor_id");
            DefaultViewColumns.Add("vendor_quotation_detail_id", "r.vendor_quotation_detail_id");
            DefaultViewColumns.Add("exchange_rate", "vqd.exchange_rate");
            DefaultViewColumns.Add("unit_price", "vqd.unit_price");
            DefaultViewColumns.Add("item_type_id", "vqd.item_type_id");
            DefaultViewColumns.Add("item_name", "vqd.item_name");
            DefaultViewColumns.Add("task_detail", "vqd.task_detail");
            DefaultViewColumns.Add("timeline_production", "vqd.timeline_production");
            DefaultViewColumns.Add("qty", "vqd.qty");
            DefaultViewColumns.Add("uom_id", "vqd.uom_id");
            DefaultViewColumns.Add("amount", "vqd.amount");
            DefaultViewColumns.Add("discount_amount", "vqd.discount_amount");
            DefaultViewColumns.Add("is_discount_pct", "vqd.is_discount_pct");
            DefaultViewColumns.Add("tax_base", "vqd.tax_base");
            DefaultViewColumns.Add("tax_vat", "vqd.tax_vat");
            DefaultViewColumns.Add("tax_whd", "vqd.tax_whd");
            DefaultViewColumns.Add("vendor_quotation_number", "vq.vendor_quotation_number");
            DefaultViewColumns.Add("vendor_reference_number", "vq.vendor_reference_number");
            DefaultViewColumns.Add("delivery_address", "vq.delivery_address");
            DefaultViewColumns.Add("delivery_date", "vq.delivery_date");
            DefaultViewColumns.Add("is_approved", "r.is_approved");

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
            DefaultViewOrders.Add("bidding_number", "DESC");
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
