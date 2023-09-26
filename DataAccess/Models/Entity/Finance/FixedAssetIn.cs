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
    public partial class fixed_asset_in : IEntity, IBaseRecord
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

        static fixed_asset_in()
        {
            IsConstant = false;
            EntityName = GetTableName();
            EntityDisplayName = GetTableName(true);
            EntityId = GUIDHash.ConvertToMd5HashGUID(EntityName).ToString();

            var sb = new System.Text.StringBuilder();
            sb.AppendLine(@"SELECT");
            sb.AppendLine(@"	r.id,");
            sb.AppendLine(@"	r.created_by,");
            sb.AppendLine(@"	r.created_on,");
            sb.AppendLine(@"	r.modified_by,");
            sb.AppendLine(@"	r.modified_on,");
            sb.AppendLine(@"	r.approved_by,");
            sb.AppendLine(@"	r.approved_on,");
            sb.AppendLine(@"	r.is_active,");
            sb.AppendLine(@"	r.is_locked,");
            sb.AppendLine(@"	r.is_default,");
            sb.AppendLine(@"	r.owner_id,");

            sb.AppendLine(@"	r.faa_number,");
            sb.AppendLine(@"	r.faa_datetime,");
            sb.AppendLine(@"	r.fixed_asset_in_category_id,");
            sb.AppendLine(@"	fcat.category_name AS fa_category_name,");
            sb.AppendLine(@"	r.fixed_asset_out_id,");
            sb.AppendLine(@"	fout.faa_number AS fout_number,");
            sb.AppendLine(@"	r.location_id,");
            sb.AppendLine(@"	r.execution_date,");
            sb.AppendLine(@"	r.legal_entity_id,");
            sb.AppendLine(@"    leg.legal_entity_name,");
            sb.AppendLine(@"	r.business_unit_id,");
            sb.AppendLine(@"	bu.unit_name AS business_unit_name,");
            sb.AppendLine(@"	r.affiliation_id,");
            sb.AppendLine(@"	aff.affiliation_name,");
            sb.AppendLine(@"	r.remark,");
            sb.AppendLine(@"	r.approval_id,");
            sb.AppendLine(@"	r.organization_id,");
            sb.AppendLine(@"	o1.organization_name,");

            sb.AppendLine(@"	u0.app_fullname AS record_created_by,");
            sb.AppendLine(@"	u1.app_fullname AS record_modified_by,");
            sb.AppendLine(@"	u2.app_fullname AS record_approved_by,");
            sb.AppendLine(@"	u3.app_fullname AS record_owner,");
            sb.AppendLine(@"	t.team_name AS record_owning_team ");

            sb.AppendLine(@"FROM");
            sb.AppendLine(@"	fn.fixed_asset_in AS r");
            sb.AppendLine(@"	LEFT OUTER JOIN dbo.legal_entity AS leg ON r.legal_entity_id = leg.id");
            sb.AppendLine(@"	LEFT OUTER JOIN dbo.business_unit AS bu ON r.business_unit_id = bu.id");
            sb.AppendLine(@"	LEFT OUTER JOIN dbo.affiliation AS aff ON r.affiliation_id = aff.id");
            sb.AppendLine(@"	LEFT OUTER JOIN  fn.fixed_asset_in_category AS fcat ON r.fixed_asset_in_category_id = fcat.id");
            sb.AppendLine(@"	LEFT OUTER JOIN  fn.fixed_asset_out AS fout ON r.fixed_asset_out_id = fout.id");
            sb.AppendLine(@"	LEFT OUTER JOIN dbo.organization AS o1 ON r.organization_id = o1.id");

            sb.AppendLine(@"	LEFT OUTER JOIN dbo.application_user AS u0 ON r.created_by = u0.id");
            sb.AppendLine(@"	LEFT OUTER JOIN dbo.application_user AS u1 ON r.modified_by = u1.id");
            sb.AppendLine(@"	LEFT OUTER JOIN dbo.application_user AS u2 ON r.approved_by = u2.id");
            sb.AppendLine(@"	LEFT OUTER JOIN dbo.application_user AS u3 ON r.owner_id = u3.id");
            sb.AppendLine(@"	LEFT OUTER JOIN dbo.team AS t ON r.owner_id = t.id");

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
            
            DefaultViewColumns.Add("faa_number", "r.faa_number");
            DefaultViewColumns.Add("faa_datetime", "r.faa_datetime");
            DefaultViewColumns.Add("fixed_asset_in_category_id", "r.fixed_asset_in_category_id");
            DefaultViewColumns.Add("fa_category_name", "fcat.category_name");
            DefaultViewColumns.Add("fixed_asset_out_id", "r.fixed_asset_out_id");
            DefaultViewColumns.Add("fout_number", "fout.faa_number");
            DefaultViewColumns.Add("location_id", "r.location_id");
            DefaultViewColumns.Add("execution_date", "r.execution_date");
            DefaultViewColumns.Add("legal_entity_id", "r.legal_entity_id");
            DefaultViewColumns.Add("legal_entity_name", "leg.legal_entity_name");
            DefaultViewColumns.Add("business_unit_id", "r.business_unit_id");
            DefaultViewColumns.Add("business_unit_name", "bu.unit_name");
            DefaultViewColumns.Add("affiliation_id", "r.affiliation_id");
            DefaultViewColumns.Add("affiliation_name", "aff.affiliation_name");
            DefaultViewColumns.Add("remark", "r.remark");
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
            DefaultViewOrders.Add("faa_number", "DESC");
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
