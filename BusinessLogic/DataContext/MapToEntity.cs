using System;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using DataAccess;
using DataAccess.PhoenixERP;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic
{
    public partial class DataContext
    {
        public static object MapToEntity<T>(NameValueCollection Collection, ref bool IsNew)
        {
            string log = "";
            var result = Activator.CreateInstance<T>();

            try
            {
                var id = "";
                if (Collection.AllKeys.Contains("id"))
                {
                    id = Collection["id"];
                }

                using (var db = new PhoenixERPRepo())
                {
                    try
                    {
                        result = db.FirstOrDefault<T>(" WHERE id = @0 ", id);

                        IsNew = (result == null);
                        if (IsNew)
                        {
                            result = Activator.CreateInstance<T>();
                            log = string.Format("New {0} record", typeof(T).Name);
                            appLogger.Debug(log);
                        }
                    }
                    catch (Exception ex)
                    {
                        appLogger.Error(ex.ToString());
                    }
                    finally
                    {
                        appLogger.Debug(db.LastCommand);
                    }
                }

                Type recordType = typeof(T);
                foreach (var mi in recordType.GetMembers())
                {
                    if (!Collection.AllKeys.Contains(mi.Name)) continue;
                    log = String.Format("Collection[{0}] = {1}", mi.Name, Collection[mi.Name]);
                    appLogger.Debug(log);

                    FieldInfo fi = recordType.GetField(mi.Name);
                    if (fi != null)
                    {
                        #region Set field value

                        Type t = Nullable.GetUnderlyingType(fi.FieldType) ?? fi.FieldType;
                        if (t == typeof(string))
                        {
                            fi.SetValue(result, Collection[mi.Name]);
                        }
                        else
                        {
                            try
                            {
                                object safeValue = null;
                                var converter = TypeDescriptor.GetConverter(t);
                                if (converter.CanConvertFrom(typeof(string))
                                    && !String.IsNullOrEmpty(Collection[mi.Name])
                                    && !String.IsNullOrWhiteSpace(Collection[mi.Name]))
                                {
                                    safeValue = converter.ConvertFrom(Collection[mi.Name]);
                                }

                                fi.SetValue(result, safeValue);
                            }
                            catch (Exception ex)
                            {
                                appLogger.Error(ex.ToString());
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        PropertyInfo pi = recordType.GetProperty(mi.Name);
                        if (pi != null)
                        {
                            #region Set property value

                            Type t = Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType;
                            if (t == typeof(string))
                            {
                                pi.SetValue(result, Collection[mi.Name], null);
                            }
                            else
                            {
                                try
                                {
                                    object safeValue = null;
                                    var converter = TypeDescriptor.GetConverter(t);
                                    if (converter.CanConvertFrom(typeof(string))
                                        && !String.IsNullOrEmpty(Collection[mi.Name])
                                        && !String.IsNullOrWhiteSpace(Collection[mi.Name]))
                                    {
                                        safeValue = converter.ConvertFrom(Collection[mi.Name]);
                                    }

                                    pi.SetValue(result, safeValue, null);
                                }
                                catch (Exception ex)
                                {
                                    appLogger.Error(ex.ToString());
                                }
                            }

                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                appLogger.Error(ex.ToString());
            }

            return result;
        }

        public static object MapToEntityFromForm<T>(T objT,IFormCollection Collection)
        {
            string log = "";
            var result = Activator.CreateInstance(objT.GetType());
            try
            {
                Type recordType = objT.GetType();
                foreach (var mi in recordType.GetMembers())
                {
                    if (!Collection.ContainsKey(mi.Name)) continue;
                    log = String.Format("Collection[{0}] = {1}", mi.Name, Collection[mi.Name].ToString());
                    appLogger.Debug(log);

                    FieldInfo fi = recordType.GetField(mi.Name);
                    if (fi != null)
                    {
                        #region Set field value

                        Type t = Nullable.GetUnderlyingType(fi.FieldType) ?? fi.FieldType;
                        if (t == typeof(string))
                        {
                            fi.SetValue(result, Collection[mi.Name].ToString());
                        }
                        else
                        {
                            try
                            {
                                object safeValue = null;
                                var converter = TypeDescriptor.GetConverter(t);
                                if (converter.CanConvertFrom(typeof(string))
                                    && !String.IsNullOrEmpty(Collection[mi.Name])
                                    && !String.IsNullOrWhiteSpace(Collection[mi.Name]))
                                {
                                    safeValue = converter.ConvertFrom(Collection[mi.Name]);
                                }

                                fi.SetValue(result, safeValue);
                            }
                            catch (Exception ex)
                            {
                                appLogger.Error(ex.ToString());
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        PropertyInfo pi = recordType.GetProperty(mi.Name);
                        if (pi != null)
                        {
                            #region Set property value

                            Type t = Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType;
                            if (t == typeof(string))
                            {
                                pi.SetValue(result, Collection[mi.Name].ToString(), null);
                            }
                            else
                            {
                                try
                                {
                                    object safeValue = null;
                                    var converter = TypeDescriptor.GetConverter(t);
                                    if (converter.CanConvertFrom(typeof(string))
                                        && !String.IsNullOrEmpty(Collection[mi.Name])
                                        && !String.IsNullOrWhiteSpace(Collection[mi.Name]))
                                    {
                                        safeValue = converter.ConvertFrom(Collection[mi.Name].ToString());
                                    }

                                    pi.SetValue(result, safeValue, null);
                                }
                                catch (Exception ex)
                                {
                                    appLogger.Error(ex.ToString());
                                }
                            }

                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                appLogger.Error(ex.ToString());
            }

            return result;
        }
    }
}
