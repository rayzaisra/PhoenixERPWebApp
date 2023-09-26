using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;
using NLog;

namespace PhoenixWebApi
{
    public class EntityBinder : IModelBinder
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            string log = "";

            log = bindingContext.HttpContext.Request.ContentType;
            appLogger.Debug(log);

            log = bindingContext.ModelType.ToString();
            appLogger.Debug(log);

            return Task.Factory.StartNew(() =>
            {
                BindModel(
                    bindingContext);
            });

            //var isRequestFromForm = bindingContext.ActionContext.HttpContext.Request.HasFormContentType;
            //if (!bindingContext.ModelName.Equals("EntityBinder"))
            //{
            //    return Task.CompletedTask;
            //}
            //var modelResult = Activator.CreateInstance(bindingContext.ModelType);

            //if (isRequestFromForm)
            //{
            //    try
            //    {
            //        var record = DataContext.MapToEntityFromForm(Activator.CreateInstance(bindingContext.ModelType), bindingContext.HttpContext.Request.Form);
            //        bindingContext.Result = ModelBindingResult.Success(record);
            //    }
            //    catch (Exception ex)
            //    {
            //        appLogger.Error(ex);
            //    }
            //}
            //return Task.CompletedTask;
        }

        private static string GetBody(HttpRequest Request)
        {
            using (StreamReader reader = new StreamReader(Request.Body))
            {
                return reader.ReadToEnd();
            }
        }
       
        public virtual void BindModel(ModelBindingContext bindingContext)
        {
            var _context = bindingContext.HttpContext;
            var _contentType = _context.Request.ContentType;

            try
            {
                var modelResult = Activator.CreateInstance(bindingContext.ModelType);
                if (_contentType.Contains("json"))
                {
                    var bodyRequest = GetBody(bindingContext.HttpContext.Request);
                    var dynamicResult = JsonConvert.DeserializeObject(bodyRequest, bindingContext.ModelType);
                    if (dynamicResult != null)
                    {
                        modelResult = dynamicResult;
                    }

                    bindingContext.Result = ModelBindingResult.Success(modelResult);
                }
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);   
            }
        }
    }

    public class EntityBinderProviders : IModelBinderProvider
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                appLogger.Error("ModelBinderProviderContext is null");
                throw new ArgumentNullException(nameof(context));
            }

            //appLogger.Info("Binder Model Name Metadata : " + context.Metadata.);
            return new BinderTypeModelBinder(typeof(EntityBinder));
        }
    }
}