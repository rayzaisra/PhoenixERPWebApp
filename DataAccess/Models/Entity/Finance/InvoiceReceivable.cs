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
    public partial class invoice_receivable : IEntity, IBaseRecord
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

        public bool? is_locked { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool? is_default { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        #endregion

        static invoice_receivable()
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

                        + " r.invoice_type_id, "
                        + " ivt.type_name AS invoice_type_name, "
                        + " r.invoice_number, "
                        + " r.due_date, "
                        + " r.sales_order_id, "
                        + " so.sales_order_number, "
                        + " r.description, "
                        + " r.currency_id, "
                        + " ccy.currency_code, "
                        + " ccy.currency_name, "
                        + " r.total_amount, "
                        + " r.exchange_rate, "
                        + " r.business_unit_id, "
                        + " bu.unit_name AS business_unit_name, "
                        + " r.legal_entity_id, "
                        + " leg.legal_entity_name, "
                        + " r.affiliation_id, "
                        + " aff.affiliation_name, "
                        + " r.job_id, "
                        + " jd.job_number, "
                        + " jd.job_name, "
                        + " r.company_id, "
                        + " cmp.company_name, "
                        + " r.company_reference, "
                        + " r.reference_date, "
                        + " r.tax_date, "
                        + " r.tax_number, "
                        + " r.approval_id, "
                        + " r.organization_id, "
                        + " o1.organization_name, "

                        + " u0.app_fullname AS record_created_by, "
                        + " u1.app_fullname AS record_modified_by, "
                        + " u2.app_fullname AS record_approved_by, "
                        + " u3.app_fullname AS record_owner, "
                        + " t.team_name AS record_owning_team "

                        + " FROM             fn.invoice_receivable AS r "
                        + " LEFT OUTER JOIN  fn.invoice_type AS ivt ON r.invoice_type_id = ivt.id "
                        + " LEFT OUTER JOIN  fn.sales_order AS so ON r.sales_order_id = so.id "
                        + " LEFT OUTER JOIN dbo.currency AS ccy ON r.currency_id = ccy.id "
                        + " LEFT OUTER JOIN  fn.company AS cmp ON r.company_id = cmp.id "
                        + " LEFT OUTER JOIN  pm.job_detail AS jd ON r.job_id = jd.id "
                        + " LEFT OUTER JOIN dbo.business_unit AS bu ON r.business_unit_id = bu.id "
                        + " LEFT OUTER JOIN dbo.legal_entity AS leg ON r.legal_entity_id = leg.id "
                        + " LEFT OUTER JOIN dbo.affiliation AS aff ON r.affiliation_id = aff.id "
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
            DefaultViewColumns.Add("is_active", "r.is_active");
            DefaultViewColumns.Add("is_locked", "r.is_locked");
            DefaultViewColumns.Add("is_default", "r.is_default");
            DefaultViewColumns.Add("owner_id", "r.owner_id");

            DefaultViewColumns.Add("invoice_type_id", "r.invoice_type_id");            
            DefaultViewColumns.Add("invoice_type_name", "ivt.type_name");
            DefaultViewColumns.Add("invoice_number", "r.invoice_number");
            DefaultViewColumns.Add("due_date", "r.due_date");
            DefaultViewColumns.Add("sales_order_id", "r.sales_order_id");
            DefaultViewColumns.Add("sales_order_number", "so.sales_order_number");
            DefaultViewColumns.Add("description", "r.description");
            DefaultViewColumns.Add("currency_id", "r.currency_id");
            DefaultViewColumns.Add("currency_code", "ccy.currency_code");
            DefaultViewColumns.Add("currency_name", "ccy.currency_name");
            DefaultViewColumns.Add("total_amount", "r.total_amount");
            DefaultViewColumns.Add("exchange_rate", "r.exchange_rate");
            DefaultViewColumns.Add("business_unit_id", "r.business_unit_id");
            DefaultViewColumns.Add("business_unit_name", "bu.unit_name");
            DefaultViewColumns.Add("legal_entity_id", "r.legal_entity_id");
            DefaultViewColumns.Add("legal_entity_name", "leg.legal_entity_name");
            DefaultViewColumns.Add("affiliation_id", "r.affiliation_id");
            DefaultViewColumns.Add("affiliation_name", "aff.affiliation_name");
            DefaultViewColumns.Add("job_id", "r.job_id");
            DefaultViewColumns.Add("job_number", "r.job_number");
            DefaultViewColumns.Add("job_name", "jd.job_name");
            DefaultViewColumns.Add("company_id", "r.company_id");
            DefaultViewColumns.Add("company_name", "cmp.company_name");
            DefaultViewColumns.Add("company_reference", "r.company_reference");
            DefaultViewColumns.Add("reference_date", "r.reference_date");
            DefaultViewColumns.Add("tax_date", "r.tax_date");
            DefaultViewColumns.Add("tax_number", "r.tax_number");
            DefaultViewColumns.Add("approval_id", "r.approval_id");
            DefaultViewColumns.Add("organization_id", "r.organization_id");
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
            DefaultViewOrders.Add("created_on", "DESC");
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
