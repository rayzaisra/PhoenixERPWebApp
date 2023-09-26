using System.Collections.Generic;

namespace DataAccess
{
    public interface IEntityNoBaseEntity
    {
        string GetEntityId();
        string GetEntityName();
        string GetEntityDisplayName();

        string GetCustomView();

        Dictionary<string, string> GetCustomViewColumns();

        Dictionary<string, string> GetCustomViewColumnTitles();

        Dictionary<string, string> GetCustomViewOrders();

        Dictionary<string, dynamic> GetFilterDynamicBuilder();
        
    }
}
