using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public interface IEntity
    {
        bool CheckIfConstant();

        string GetEntityId();

        string GetEntityName();

        string GetEntityDisplayName();

        string GetDefaultView();

        Dictionary<string, string> GetDefaultViewColumns();

        Dictionary<string, string> GetDefaultViewColumnTitles();

        Dictionary<string, string> GetDefaultViewOrders();

        Dictionary<string, dynamic> GetFilterDynamicBuilder();
    }
}
