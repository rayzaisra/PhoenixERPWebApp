using CommonTool.DataTables;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixWebApi.ModelBinder
{
    public class DataTablesModelBinderProviders: IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (context.Metadata.ModelType == typeof(DataTableRequestNetCore))
            {
                return new BinderTypeModelBinder(typeof(DataTableModelBinder));
            }
            return null;
        }
    }
}
