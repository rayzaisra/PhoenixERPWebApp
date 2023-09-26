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
    public partial class inventory_transaction_detail : IEntity, IBaseRecord
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

        static inventory_transaction_detail()
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
                        + " r.is_active, "
                        + " r.is_locked, "
                        + " r.is_default, "
                        + " r.owner_id, "

                        + " r.inventory_transaction_id, "
                        + " f1.transaction_number, "
                        + " r.warehouse_id, "
                        + " w1.warehouse_name, "
                        + " r.item_id, "
                        + " i1.item_name, "
                        + " i1.item_code, "
                        + " r.uom_id, "
                        + " m1.unit_name, "
                        + " r.credit, "
                        + " r.debit, "
                        + " COALESCE(r.company_name, f1.company_name) AS company_name, "
                        + " COALESCE(r.company_id, f1.company_id) AS company_id, "
                        + " COALESCE(c2.company_name, f1.current_company_name) AS current_company_name, "
                        + " COALESCE(r.business_unit_id, f1.business_unit_id) AS business_unit_id, "
                        + " COALESCE(b1.unit_name, f1.unit_name) AS business_unit_name, "
                        + " COALESCE(r.legal_entity_id, f1.legal_entity_id) AS legal_entity_id, "
                        + " COALESCE(l1.legal_entity_name, f1.legal_entity_name) AS legal_entity_name, "
                        + " COALESCE(r.affiliation_id, f1.affiliation_id) AS affiliation_id, "
                        + " COALESCE(a2.affiliation_name, f1.affiliation_name) AS affiliation_name, "
                        + " f1.organization_id, "
                        + " o1.organization_name, "

                        + " u0.app_fullname AS record_created_by, "
                        + " u1.app_fullname AS record_modified_by, "
                        + " u2.app_fullname AS record_approved_by, "
                        + " u3.app_fullname AS record_owner, "
                        + " t.team_name AS record_owning_team "

                        + " FROM             fn.inventory_transaction_detail AS r "
                        + " LEFT OUTER JOIN dbo.warehouse AS w1 ON r.warehouse_id = w1.id "
                        + " LEFT OUTER JOIN dbo.item AS i1 ON r.item_id = i1.id "
                        + " LEFT OUTER JOIN dbo.uom AS m1 ON r.uom_id = m1.id "
                        + " LEFT OUTER JOIN  fn.company AS c2 ON r.company_id = c2.id "
                        + " LEFT OUTER JOIN dbo.business_unit AS b1 ON r.business_unit_id = b1.id "
                        + " LEFT OUTER JOIN dbo.legal_entity AS l1 ON r.legal_entity_id = l1.id "
                        + " LEFT OUTER JOIN dbo.affiliation AS a2 ON r.affiliation_id = a2.id "
                        + " LEFT OUTER JOIN  fn.vw_inventory_transaction AS f1 ON r.inventory_transaction_id = f1.id "
                        + " LEFT OUTER JOIN dbo.organization AS o1 ON f1.organization_id = o1.id " 

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
            DefaultViewColumns.Add("is_active", "r.is_active");
            DefaultViewColumns.Add("is_locked", "r.is_locked");
            DefaultViewColumns.Add("is_default", "r.is_default");
            DefaultViewColumns.Add("owner_id", "r.owner_id");

            DefaultViewColumns.Add("inventory_transaction_id", "r.inventory_transaction_id");
            DefaultViewColumns.Add("transaction_number", "f1.transaction_number");
            DefaultViewColumns.Add("warehouse_id", "r.warehouse_id");
            DefaultViewColumns.Add("warehouse_name", "w1.warehouse_name");
            DefaultViewColumns.Add("item_id", "r.item_id");
            DefaultViewColumns.Add("item_name", "i1.item_name");
            DefaultViewColumns.Add("item_code", "i1.item_code");
            DefaultViewColumns.Add("uom_id", "r.uom_id");
            DefaultViewColumns.Add("unit_name", "m1.unit_name");
            DefaultViewColumns.Add("credit", "r.credit");
            DefaultViewColumns.Add("debit", "r.debit");
            DefaultViewColumns.Add("company_name", "COALESCE(r.company_name, f1.company_name)");
            DefaultViewColumns.Add("company_id", "COALESCE(r.company_id, f1.company_id)");
            DefaultViewColumns.Add("current_company_name", "COALESCE(c2.company_name, f1.current_company_name)");
            DefaultViewColumns.Add("business_unit_id", "COALESCE(r.business_unit_id, f1.business_unit_id)");
            DefaultViewColumns.Add("business_unit_name", "COALESCE(b1.unit_name, f1.unit_name)");
            DefaultViewColumns.Add("legal_entity_id", "COALESCE(r.legal_entity_id, f1.legal_entity_id)");
            DefaultViewColumns.Add("legal_entity_name", "COALESCE(l1.legal_entity_name, f1.legal_entity_name)");
            DefaultViewColumns.Add("affiliation_id", "COALESCE(r.affiliation_id, f1.affiliation_id)");
            DefaultViewColumns.Add("affiliation_name", "COALESCE(a2.affiliation_name, f1.affiliation_name)");
            DefaultViewColumns.Add("organization_id", "f1.organization_id");
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
            DefaultViewOrders.Add("transaction_number", "DESC");
            DefaultViewOrders.Add("item_code", "ASC");
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
