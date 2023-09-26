//using CommonTool;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTool;
using DataAccess;

namespace DataAccess.PhoenixERP.General
{
    public partial class country : IEntity, IBaseRecord
    {
        #region Default IBaseRecord

        public static readonly Boolean IsConstant;

        public static readonly String EntityId;

        public static readonly String EntityName;

        public static readonly String EntityDisplayName;

        public static readonly String DefaultView;

        public static readonly Dictionary<String, String> DefaultViewColumns;

        public static readonly Dictionary<String, String> DefaultViewColumnTitles;

        public static readonly Dictionary<String, String> DefaultViewOrders;

        public static readonly Dictionary<String, dynamic> FilterDynamicBuilder;

        #endregion

        static country()
        {
            IsConstant = false;
            EntityName = country.GetTableName();
            EntityDisplayName = country.GetTableName(true);
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
                        
                        + " r.country_name, "
                        + " r.country_code, "                        
                        + " r.organization_id, "
                        + " o1.organization_name, "

                        + " u0.app_fullname AS record_created_by, "
                        + " u1.app_fullname AS record_modified_by, "
                        + " u2.app_fullname AS record_approved_by, "
                        + " u3.app_fullname AS record_owner, "
                        + " t.team_name AS record_owning_team "

                        + " FROM            dbo.country AS r "
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

            DefaultViewColumns.Add("country_name", "r.country_name");
            DefaultViewColumns.Add("country_code", "r.country_code");
            DefaultViewColumns.Add("organization_id", "r.organization_id");
            DefaultViewColumns.Add("organization_name", "o1.organization_name");

            DefaultViewColumns.Add("record_created_by", "u0.app_fullname");
            DefaultViewColumns.Add("record_modified_by", "u1.app_fullname");
            DefaultViewColumns.Add("record_approved_by", "u2.app_fullname");
            DefaultViewColumns.Add("record_owner", "u3.app_fullname");
            DefaultViewColumns.Add("record_owning_team", "t.team_name");

            DefaultViewColumnTitles = new Dictionary<String, String>();
            TextInfo ti = new CultureInfo("en-US", false).TextInfo;

            foreach (var c in DefaultViewColumns)
            {
                DefaultViewColumnTitles.Add(c.Key, ti.ToTitleCase(c.Key.Replace("_", " ")));
            }

            DefaultViewOrders = new Dictionary<String, String>();
            DefaultViewOrders.Add("country_name", "ASC");
        }

        public Boolean CheckIfConstant()
        {
            return country.IsConstant;
        }

        public String GetEntityId()
        {
            return EntityId;
        }

        public String GetEntityName()
        {
            return EntityName;
        }

        public String GetEntityDisplayName()
        {
            return EntityDisplayName;
        }

        public String GetDefaultView()
        {
            return DefaultView;
        }

        public Dictionary<String, String> GetDefaultViewColumns()
        {
            return DefaultViewColumns;
        }

        public Dictionary<String, String> GetDefaultViewColumnTitles()
        {

            return DefaultViewColumnTitles;
        }

        public Dictionary<String, String> GetDefaultViewOrders()
        {
            return DefaultViewOrders;
        }

        public Dictionary<String, dynamic> GetFilterDynamicBuilder()
        {
            return FilterDynamicBuilder;
        }
    } 
}
