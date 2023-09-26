//using CommonTool;
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
    public partial class purchase_order : IEntity, IBaseRecord
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

        static purchase_order()
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

                        + " r.purchase_order_number, "
                        + " r.purchase_order_date, "
                        + " r.delivery_date, "
                        + " r.purchase_request_id, "
                        + " r.purchase_request_number, "
                        + " r.job_id, "
                        + " r.job_number, "
                        + " r.company_id, "
                        + " c1.company_name AS current_company_name, "
                        + " r.company_name, "
                        + " r.company_reference, "
                        + " r.description, "
                        + " r.business_unit_id, "
                        + " b1.unit_name, "
                        + " r.legal_entity_id, "
                        + " l1.legal_entity_name, "
                        + " r.affiliation_id, "
                        + " a1.affiliation_name, "
                        + " r.currency_id, "
                        + " ccy.currency_code, "
                        + " ccy.currency_name, "
                        + " r.exchange_rate, "
                        + " r.organization_id, "
                        + " o1.organization_name, "
                        + " r.discount_amount, "
                        + " r.discount_percent, "
                        + " r.tax_amount, "
                        + " r.tax_percent, "
                        + " r.amount, "
                        + " r.pic_name, "
                        + " CAST( (r.amount - r.discount_amount) AS DECIMAL(18,2)) AS tax_base, "
                        + " r.vat_percent, "
                        + " CAST( ((r.amount - r.discount_amount) * (0.01 * r.vat_percent) ) AS DECIMAL(18,2)) AS vat_amount, "
                        + " CAST((r.amount + (r.amount - r.discount_amount) * (0.01 * r.vat_percent) - r.discount_amount) AS DECIMAL(18,2)) AS grand_total, "
                        + " r.percent_payment_term1, "
                        + " r.percent_payment_term2, "
                        + " r.percent_payment_term3, "
                        + " r.payment_method_id, "
                        + " pm.method_name AS payment_method_name, "
                        + " r.invoicing_type_id, "
                        + " ivt.type_name AS invoicing_type_name, "
                        + " r.vendor_quotation_id, "
                        + " vq.vendor_quotation_number, "
                        + " r.approval_id, "

                        + " sr.special_name, "
                        + " r.special_request_id, "

                        + " pos.status_name AS purchase_order_status_name, "
                        + " r.purchase_order_status_id, "
                        + " c1.id AS 'vendor_id', "
                        + " c1.company_name AS 'vendor_name', "

                        + " u0.app_fullname AS record_created_by, "
                        + " u1.app_fullname AS record_modified_by, "
                        + " u2.app_fullname AS record_approved_by, "
                        + " u3.app_fullname AS record_owner, "
                        + " t.team_name AS record_owning_team "

                        + " FROM fn.purchase_order AS r "
                        + " LEFT OUTER JOIN  fn.company AS c1 ON r.company_id = c1.id "
                        + " LEFT OUTER JOIN dbo.affiliation AS a1 ON r.affiliation_id = a1.id "
                        + " LEFT OUTER JOIN dbo.currency AS ccy ON r.currency_id = ccy.id "
                        + " LEFT OUTER JOIN dbo.business_unit AS b1 ON r.business_unit_id = b1.id "
                        + " LEFT OUTER JOIN dbo.legal_entity AS l1 ON r.legal_entity_id = l1.id "
                        + " LEFT OUTER JOIN dbo.organization AS o1 ON r.organization_id = o1.id "
                        + " LEFT OUTER JOIN  fn.payment_method AS pm ON r.payment_method_id = pm.id "
                        + " LEFT OUTER JOIN  fn.invoicing_type AS ivt ON r.invoicing_type_id = ivt.id "
                        + " LEFT OUTER JOIN  fn.vendor_quotation AS vq ON r.vendor_quotation_id = vq.id "
                        + " LEFT OUTER JOIN  fn.purchase_order_status AS pos ON r.purchase_order_status_id = pos.id "
                        + " LEFT OUTER JOIN fn.special_request AS sr ON r.special_request_id = sr.id  "

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

            DefaultViewColumns.Add("purchase_order_number", "r.purchase_order_number");
            DefaultViewColumns.Add("purchase_order_date", "r.purchase_order_date");
            DefaultViewColumns.Add("delivery_date", "r.delivery_date");
            DefaultViewColumns.Add("purchase_request_id", "r.purchase_request_id");
            DefaultViewColumns.Add("purchase_request_number", "r.purchase_request_number");
            DefaultViewColumns.Add("job_id", "r.job_id");
            DefaultViewColumns.Add("job_number", "r.job_number");
            DefaultViewColumns.Add("company_id", "r.company_id");
            DefaultViewColumns.Add("current_company_name", "c1.company_name");
            DefaultViewColumns.Add("organization_id", "r.organization_id");
            DefaultViewColumns.Add("company_name", "r.company_name");
            DefaultViewColumns.Add("company_reference", "r.company_reference");
            DefaultViewColumns.Add("description", "r.description");

            DefaultViewColumns.Add("currency_id", "r.currency_id");
            DefaultViewColumns.Add("currency_code", "ccy.currency_code");
            DefaultViewColumns.Add("currency_name", "ccy.currency_name");
            DefaultViewColumns.Add("exchange_rate", "r.exchange_rate");
            DefaultViewColumns.Add("affiliation_id", "r.affiliation_id");
            DefaultViewColumns.Add("affiliation_name", "r.affiliation_name");
            DefaultViewColumns.Add("business_unit_id", "r.business_unit_id");
            DefaultViewColumns.Add("unit_name", "b1.unit_name");
            DefaultViewColumns.Add("legal_entity_id", "r.legal_entity_id");
            DefaultViewColumns.Add("legal_entity_name", "l1.legal_entity_name");
            DefaultViewColumns.Add("organization_name", "o1.organization_name");
            DefaultViewColumns.Add("discount_amount", "r.discount_amount");
            DefaultViewColumns.Add("discount_percent", "r.discount_percent");
            DefaultViewColumns.Add("tax_amount", "r.tax_amount");
            DefaultViewColumns.Add("tax_percent", "r.tax_percent");
            DefaultViewColumns.Add("pic_name", "r.pic_name");
            DefaultViewColumns.Add("amount", "r.amount");
            DefaultViewColumns.Add("amount_net", "r.amount_net");
            DefaultViewColumns.Add("percent_payment_term1", "r.percent_payment_term1");
            DefaultViewColumns.Add("percent_payment_term2", "r.percent_payment_term2");
            DefaultViewColumns.Add("percent_payment_term3", "r.percent_payment_term3");
            DefaultViewColumns.Add("payment_method_id", "r.payment_method_id");
            DefaultViewColumns.Add("payment_method_name", "pm.method_name");
            DefaultViewColumns.Add("invoicing_type_id", "r.invoicing_type_id");
            DefaultViewColumns.Add("invoicing_type_name", "ivt.type_name");
            DefaultViewColumns.Add("vendor_quotation_id", "r.vendor_quotation_id");
            DefaultViewColumns.Add("vendor_quotation_number", "vq.vendor_quotation_number");
            DefaultViewColumns.Add("purchase_order_status_id", "r.purchase_order_status_id");
            DefaultViewColumns.Add("purchase_order_status_name", "pos.status_name");
            DefaultViewColumns.Add("approval_id", "r.approval_id");

            DefaultViewColumns.Add("special_request_id", "r.special_request_id");
            DefaultViewColumns.Add("special_name", "sr.special_name");

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
            DefaultViewOrders.Add("purchase_order_number", "DESC");
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
