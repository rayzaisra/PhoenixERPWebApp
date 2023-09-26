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
    public partial class request_for_quotation_temp : IEntity, IBaseRecord
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

        static request_for_quotation_temp()
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
                        + " r.rfq_temp_number, "
                        + " r.legal_entity_id, "
                        + " leg.legal_entity_name, "
                        + " r.task_id, "
                        + " tt.job_number, "
                        + " tt.task_name, "
                        + " st.sub_task, "
                        + " pad.brand, "
                        + " pad.sub_brand, "
                        + " r.account_id, "
                        + " acc.account_number, "
                        + " acc.account_name AS category, "
                        + " rfqc.category_name AS vendor_category, "
                        + " pad.quantity, "
                        + " pad.uom, "
                        + " pad.price_per_quantity, "
                        + @" (COALESCE(pad.quantity, 0) * COALESCE(pad.price_per_quantity, 0)) AS amount, "
                        + " pad.pph, "
                        + " pad.ppn, "
                        + " pad.asf, "
                        + @" (COALESCE(pad.quantity, 0) * COALESCE(pad.price_per_quantity, 0)
                            + COALESCE(pad.pph, 0) + COALESCE(pad.ppn, 0) + COALESCE(pad.asf, 0)) AS total_amount, "
                        +" leg.organization_id, "
                        + " o1.organization_name, "
                        + " u0.app_fullname AS record_created_by, "
                        + " u1.app_fullname AS record_modified_by, "
                        + " u2.app_fullname AS record_approved_by, "
                        + " u3.app_fullname AS record_owner, "
                        + " t.team_name AS record_owning_team "

                        + " FROM             fn.request_for_quotation_temp AS r " 
                        + " LEFT OUTER JOIN dbo.organization AS o1 ON r.organization_id = o1.id "
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

            DefaultViewColumns.Add("rfq_temp_number", "r.rfq_temp_number");
            DefaultViewColumns.Add("legal_entity_id", "r.legal_entity_id");
            DefaultViewColumns.Add("legal_entity_name", "leg.legal_entity_name");
            DefaultViewColumns.Add("task_id", "r.task_id");
            DefaultViewColumns.Add("job_number", "tt.job_number");
            DefaultViewColumns.Add("task_name", "tt.task_name");
            DefaultViewColumns.Add("sub_task", "st.sub_task");
            DefaultViewColumns.Add("brand", "pad.brand");
            DefaultViewColumns.Add("sub_brand", "pad.sub_brand");
            DefaultViewColumns.Add("account_id", "r.account_id");
            DefaultViewColumns.Add("account_number", "acc.account_number");
            DefaultViewColumns.Add("category", "acc.account_name");
            DefaultViewColumns.Add("vendor_category", "rfqc.category_name");
            DefaultViewColumns.Add("quantity", "pad.quantity");
            DefaultViewColumns.Add("uom", "pad.uom");
            DefaultViewColumns.Add("price_per_quantity", "pad.price_per_quantity");
            DefaultViewColumns.Add("amount", @"(COALESCE(pad.quantity, 0) * COALESCE(pad.price_per_quantity, 0))");
            DefaultViewColumns.Add("pph", "pad.pph");
            DefaultViewColumns.Add("ppn", "pad.ppn");
            DefaultViewColumns.Add("asf", "pad.asf");
            DefaultViewColumns.Add("total_amount", @"(COALESCE(pad.quantity, 0) * COALESCE(pad.price_per_quantity, 0)
                            + COALESCE(pad.pph, 0) + COALESCE(pad.ppn, 0) + COALESCE(pad.asf, 0))");
            DefaultViewColumns.Add("organization_id", "leg.organization_id");
            DefaultViewColumns.Add("organization_name", "o1.organization_name");

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
            DefaultViewOrders.Add("rfq_temp_number", "DESC");
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
