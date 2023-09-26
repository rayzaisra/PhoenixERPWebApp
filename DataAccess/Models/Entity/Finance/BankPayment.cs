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
    public partial class bank_payment : IEntity, IBaseRecord
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

        static bank_payment()
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

                        + " r.payment_number, "
                        + " r.requested_by, "
                        + " u4.app_fullname AS requester_name, "
                        + " r.requested_on, " 
                        + " r.due_date, "
                        + " r.bank_payment_method_id, "
                        + " bpm.method_name AS bank_payment_method, "
                        + " r.reference_number, "
                        + " r.bank_account_destination, "
                        + " r.cash_voucher_id, "
                        + " r.currency_id, "
                        + " ccy.currency_code, "
                        + " ccy.currency_name, "
                        + " r.bank_payment_type_id, "
                        + " bpt.payment_type AS bank_payment_type, "
                        + " r.payment_amount, "
                        + " r.payment_charges, "
                        + " r.paid_to_vendor_id, "
                        + " cmp.company_name, "
                        + " r.source_account_id, "
                        + " a1.account_number, "
                        + " a1.account_name, "
                        + " r.exchange_rate, "
                        + " r.legal_entity_id, "
                        + " leg.legal_entity_name, "
                        + " r.business_unit_id, "
                        + " bu.unit_name AS business_unit_name, "
                        + " r.affiliation_id, "
                        + " aff.affiliation_name, "
                        + " r.remarks, "
                        + " r.notes, "
                        + " r.approval_id, "
                        + " r.financial_transaction_id, "
                        + " r.bank_payment_status_id, "
                        + " bps.status_name AS bank_payment_status, "
                        + " a1.organization_id, "
                        + " o1.organization_name, "

                        + " u0.app_fullname AS record_created_by, "
                        + " u1.app_fullname AS record_modified_by, "
                        + " u2.app_fullname AS record_approved_by, "
                        + " u3.app_fullname AS record_owner, "
                        + " t.team_name AS record_owning_team "

                        + " FROM             fn.bank_payment AS r "
                        + " LEFT OUTER JOIN dbo.application_user AS u4 ON r.requested_by = u4.id "
                        + " LEFT OUTER JOIN  fn.account AS a1 ON r.source_account_id = a1.id "
                        + " LEFT OUTER JOIN dbo.organization AS o1 ON a1.organization_id = o1.id "
                        + " LEFT OUTER JOIN  fn.bank_payment_method AS bpm ON r.bank_payment_method_id = bpm.id "
                        + " LEFT OUTER JOIN  fn.bank_payment_type AS bpt ON r.bank_payment_type_id = bpt.id " 
                        + " LEFT OUTER JOIN  fn.company AS cmp ON r.paid_to_vendor_id = cmp.id "
                        + " LEFT OUTER JOIN dbo.currency AS ccy ON r.currency_id = ccy.id "
                        + " LEFT OUTER JOIN dbo.legal_entity AS leg ON r.legal_entity_id = leg.id "
                        + " LEFT OUTER JOIN dbo.business_unit AS bu ON r.business_unit_id = bu.id "
                        + " LEFT OUTER JOIN dbo.affiliation AS aff ON r.affiliation_id = aff.id "
                        + " LEFT OUTER JOIN  fn.bank_payment_status AS bps ON r.bank_payment_status_id = bps.id "

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

            DefaultViewColumns.Add("payment_number", "r.payment_number");
            DefaultViewColumns.Add("requested_by", "r.requested_by");
            DefaultViewColumns.Add("requester_name", "u4.app_fullname");
            DefaultViewColumns.Add("requested_on", "r.requested_on");
            DefaultViewColumns.Add("due_date", "r.due_date");
            DefaultViewColumns.Add("bank_payment_method_id", "r.bank_payment_method_id");
            DefaultViewColumns.Add("bank_payment_method", "bpm.method_name");
            DefaultViewColumns.Add("reference_number", "r.reference_number");
            DefaultViewColumns.Add("bank_account_destination", "r.bank_account_destination");
            DefaultViewColumns.Add("cash_voucher_id", "r.cash_voucher_id");
            DefaultViewColumns.Add("currency_id", "r.currency_id");
            DefaultViewColumns.Add("currency_code", "ccy.currency_code");
            DefaultViewColumns.Add("currency_name", "ccy.currency_name");
            DefaultViewColumns.Add("bank_payment_type_id", "r.bank_payment_type_id");
            DefaultViewColumns.Add("bank_payment_type", "bpt.payment_type");
            DefaultViewColumns.Add("payment_amount", "r.payment_amount");
            DefaultViewColumns.Add("payment_charges", "r.payment_charges");
            DefaultViewColumns.Add("paid_to_vendor_id", "r.paid_to_vendor_id");
            DefaultViewColumns.Add("company_name", "cmp.company_name");
            DefaultViewColumns.Add("source_account_id", "r.source_account_id");
            DefaultViewColumns.Add("account_number", "a1.account_number");
            DefaultViewColumns.Add("account_name", "a1.account_name");
            DefaultViewColumns.Add("exchange_rate", "r.exchange_rate");
            DefaultViewColumns.Add("legal_entity_id", "r.legal_entity_id");
            DefaultViewColumns.Add("legal_entity_name", "leg.legal_entity_name");
            DefaultViewColumns.Add("business_unit_id", "r.business_unit_id");
            DefaultViewColumns.Add("business_unit_name", "bu.unit_name");
            DefaultViewColumns.Add("affiliation_id", "r.affiliation_id");
            DefaultViewColumns.Add("affiliation_name", "aff.affiliation_name");
            DefaultViewColumns.Add("remarks", "r.remarks");
            DefaultViewColumns.Add("notes", "r.notes");
            DefaultViewColumns.Add("approval_id", "r.approval_id");
            DefaultViewColumns.Add("financial_transaction_id", "r.financial_transaction_id");
            DefaultViewColumns.Add("bank_payment_status_id", "r.bank_payment_status_id");
            DefaultViewColumns.Add("bank_payment_status", "bps.status_name");
            DefaultViewColumns.Add("organization_id", "a1.organization_id");
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
            DefaultViewOrders.Add("payment_number", "DESC");
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
