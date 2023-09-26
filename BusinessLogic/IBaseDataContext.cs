using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;

namespace BusinessLogic
{
    public interface IBaseService<T> 
    {
        bool SaveEntity(T record, ref bool isNew);

        object Details(string Id);

        bool Delete(string[] Ids, Action<string> outResult = null);

        DataTablesResponseNetCore GetListDataTables(IDataTablesRequest dataTablesRequest);
    }
}
