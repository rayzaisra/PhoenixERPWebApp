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
    public partial class account_payable_temporary : IEntity, IBaseRecord
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

        static account_payable_temporary()
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

                        + " r.tap_number, "
                        + " r.legal_entity_id, "
                        + " leg.legal_entity_name, "
                        + " r.business_unit_id, "
                        + " bu.unit_name AS business_unit_name, "
                        + " r.affilition_id, "
                        + " aff.affiliation_name, "
                        + " r.account_id, "
                        + " acc.account_number, "
                        + " acc.account_name, "
                        + " r.vendor_id, "
                        + " v.company_name AS vendor_name, "
                        + " r.job_id, "
                        + " j.pa_number AS job_number, "
                        + " r.transaction_date, "
                        + " r.due_date, "
                        + " r.currency_id, "
                        + " ccy.currency_code, "
                        + " ccy.currency_name, "
                        + " r.amount, "
                        + " r.exchange_rate, "
                        + " r.bank_account_destination, "
                        + " r.charges, "
                        + " r.remarks, "
                        + " leg.organization_id, "
                        + " o1.organization_name, "

                        + " u0.app_fullname AS record_created_by, "
                        + " u1.app_fullname AS record_modified_by, "
                        + " u2.app_fullname AS record_approved_by, "
                        + " u3.app_fullname AS record_owner, "
                        + " t.team_name AS record_owning_team "

                        + " FROM             fn.account_payable_temporary AS r "
                        + " LEFT OUTER JOIN fn.account AS acc ON r.account_id = acc.id "
                        + " LEFT OUTER JOIN dbo.affiliation AS aff ON r.affilition_id = aff.id "
                        + " LEFT OUTER JOIN dbo.business_unit AS bu ON r.business_unit_id = bu.id "
                        + " LEFT OUTER JOIN dbo.legal_entity AS leg ON r.legal_entity_id = leg.id "
                        + " LEFT OUTER JOIN dbo.organization AS o1 ON leg.organization_id = o1.id "
                        + " LEFT OUTER JOIN fn.company AS v ON r.vendor_id = v.id "
                        + " LEFT OUTER JOIN pm.job_pa AS j ON r.job_id = j.id "
                        + " LEFT OUTER JOIN dbo.currency AS ccy ON r.currency_id = ccy.id "

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

            DefaultViewColumns.Add("tap_number", "r.tap_number");
            DefaultViewColumns.Add("legal_entity_id", "r.legal_entity_id");
            DefaultViewColumns.Add("legal_entity_name", "leg.legal_entity_name");
            DefaultViewColumns.Add("business_unit_id", "r.business_unit_id");
            DefaultViewColumns.Add("business_unit_name", "bu.unit_name");
            DefaultViewColumns.Add("affilition_id", "r.affilition_id");
            DefaultViewColumns.Add("affiliation_name", "aff.affiliation_name");
            DefaultViewColumns.Add("account_id", "r.account_id");
            DefaultViewColumns.Add("account_number", "acc.account_number");
            DefaultViewColumns.Add("account_name", "acc.account_name");
            DefaultViewColumns.Add("vendor_id", "r.vendor_id");
            DefaultViewColumns.Add("vendor_name", "v.company_name");
            DefaultViewColumns.Add("job_id", "r.job_id");
            DefaultViewColumns.Add("job_number", "j.pa_number");
            DefaultViewColumns.Add("transaction_date", "r.transaction_date");
            DefaultViewColumns.Add("due_date", "r.due_date");
            DefaultViewColumns.Add("currency_id", "r.currency_id");
            DefaultViewColumns.Add("currency_code", "ccy.currency_code");
            DefaultViewColumns.Add("currency_name", "ccy.currency_name");
            DefaultViewColumns.Add("amount", "r.amount");
            DefaultViewColumns.Add("exchange_rate", "r.exchange_rate");
            DefaultViewColumns.Add("bank_account_destination", "r.bank_account_destination");
            DefaultViewColumns.Add("charges", "r.charges");
            DefaultViewColumns.Add("remarks", "r.remarks");
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
            DefaultViewOrders.Add("tap_number", "DESC");
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
