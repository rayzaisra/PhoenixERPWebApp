using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Linq;
using BusinessLogic.Services.General;
using DataAccess.PhoenixERP;
using NLog;
using Newtonsoft.Json;
using DataAccess.PhoenixERP.General;

namespace BusinessLogic.Services.General
{
    public partial class BusinessUnitService
    {
        public List<dynamic> GetDetailBusinessUnitByLegalEntity()
        {
            List<dynamic> results = new List<dynamic>();
            using (var db = new PhoenixERPRepo())
            {
                try
                {
                    var qry = NPoco.Sql.Builder.Append(business_unit.DefaultView);
                    qry.Append(" WHERE r.is_active=1 and r.parent_unit is null ");
                    var data = db.Fetch<vw_business_unit>(qry);
                    List<string> childNodes = null;
                    int row = 0;
                    foreach (var item in data)
                    {
                        dynamic menu = new ExpandoObject();
                        if (row == 0) menu.expanded = true;

                        var children = GetDetailBusinessUnitByParentUnit(item.id, ref childNodes);
                        menu.id = item.id;
                        menu.text = item.unit_name;
                        menu.affiliation_id = item.affiliation_id;
                        menu.affiliation_name = item.affiliation_name;
                        menu.items = children;
                        results.Add(menu);
                        row++;
                    }
                    appLogger.Debug(JsonConvert.SerializeObject(results));
                }
                catch (Exception ex)
                {
                    appLogger.Error(ex);
                    throw;
                }
            }
            return results;
        }
        protected List<dynamic> GetDetailBusinessUnitByParentUnit(String parentId, ref List<string> childNodes)
        {
            List<dynamic> results = new List<dynamic>();
            if (childNodes == null)
            {
                childNodes = new List<string>();
                childNodes.Add(parentId);
            }
            using (var db = new PhoenixERPRepo())
            {
                try
                {
                    if (string.IsNullOrEmpty(parentId)) return results;
                    var qry = NPoco.Sql.Builder.Append(business_unit.DefaultView);
                    qry.Append(" WHERE r.parent_unit=@0", parentId);
                    var data = db.Fetch<vw_business_unit>(qry);
                    foreach (var item in data)
                    {
                        if (childNodes.Contains(item.id)) continue;
                        dynamic menu = new ExpandoObject();
                        menu.id = item.id;
                        menu.text = item.unit_name;
                        menu.affiliation_id = item.affiliation_id;
                        menu.affiliation_name = item.affiliation_name;
                        menu.items = new object(); // array of strings or objects
                        if (item.parent_unit != null)
                        {
                            menu.items = GetDetailBusinessUnitByParentUnit(item.id, ref childNodes);
                        }
                        results.Add(menu);
                    }
                }
                catch (Exception ex)
                {
                    appLogger.Error(ex);
                    throw;
                }
            }

            return results;
        }

        public List<dynamic> GetDetailBusinessUnitByLegalEntityJStree()
        {
            List<dynamic> results = new List<dynamic>();
            try
            {
                var data = business_unit.Fetch(" WHERE is_active=@0 and parent_unit is null ", true);
                List<string> childNodes = null;
                int row = 0;
                foreach (var item in data)
                {
                    dynamic menu = new ExpandoObject();
                    if (row == 0) menu.expanded = true;

                    var children = GetDetailBusinessUnitByParentUnitJStree(item.id, ref childNodes);
                    menu.id = item.id;
                    //menu.url = item.unit_name;
                    menu.text = item.unit_name;
                    //menu.hasChildren = false;
                    //menu.icon = "fa fa-briefcase m--font-success ";
                    //menu.state = new ExpandoObject();
                    //menu.state.opened = false;
                    //menu.state.disabled = false;
                    //menu.state.selected = false;
                    menu.children = children;
                    //menu.hasChildren = children.Count == 0 ? false : true;
                    //menu.li_attr = new ExpandoObject(); //attributes for the generated LI node
                    //menu.a_attr = new ExpandoObject(); // attributes for the generated A node
                    results.Add(menu);
                    row++;
                }
                appLogger.Debug(JsonConvert.SerializeObject(results));
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }
            return results;
        }
        protected List<dynamic> GetDetailBusinessUnitByParentUnitJStree(String parentId, ref List<string> childNodes)
        {
            List<dynamic> results = new List<dynamic>();
            if (childNodes == null)
            {
                childNodes = new List<string>();
                childNodes.Add(parentId);
            }

            try
            {
                if (string.IsNullOrEmpty(parentId)) return results;
                var data = business_unit.Fetch(" WHERE parent_unit=@0", parentId);
                foreach (var item in data)
                {
                    if (childNodes.Contains(item.id)) continue;
                    dynamic menu = new ExpandoObject();
                    menu.id = item.id;
                    //menu.url = item.unit_name;
                    //menu.parent = item.parent_unit;
                    menu.text = item.unit_name;
                    //menu.icon = "fa fa-briefcase m--font-success ";
                    //menu.state = new ExpandoObject();
                    //menu.state.opened = false;
                    //menu.state.disabled = false;
                    //menu.state.selected = false;
                    menu.items = new object(); // array of strings or objects
                    if (item.parent_unit != null)
                    {
                        menu.children = GetDetailBusinessUnitByParentUnitJStree(item.id, ref childNodes);
                    }
                    //menu.li_attr = new ExpandoObject(); //attributes for the generated LI node
                    //menu.a_attr = new ExpandoObject(); // attributes for the generated A node
                    results.Add(menu);
                }
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }
            return results;
        }
    }
}
