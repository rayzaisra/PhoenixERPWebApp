using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Shared.Helpers
{
    public class PropertyMapper2
    {
        public static string[] except = new string[] { "ID", "ID_ORIGINAL", "IS_ORIGINAL", "ENTRY_BY", "ENTRY_DATE", "UPDATED_BY", "UPDATED_DATE", "DELETED_BY", "DELETED_DATE", "APPROVED_BY", "APPROVED_DATE", "REF_DATA_STATUS_ID", "TIPE_APPROVAL" };

        #region Set Properties
        public static void SetPropertiesUpdate(string[] validKeys, object fromRecord,
                                         object toRecord)
        {
            PropertyInfo[] fromFields = fromRecord.GetType().GetProperties();
            PropertyInfo[] toFields = toRecord.GetType().GetProperties();
            SetPropertiesUpdate(validKeys, fromFields, toFields, fromRecord, toRecord);
        }
        #endregion
        #region Set Properties
        public static void SetPropertiesUpdate(string[] validKeys, PropertyInfo[] fromFields,
                                         PropertyInfo[] toFields,
                                         object fromRecord,
                                         object toRecord)
        {
            PropertyInfo fromField = null;
            PropertyInfo toField = null;

            if (fromFields == null) return;
            if (toFields == null) return;

            for (int f = 0; f < fromFields.Length; f++)
            {

                fromField = (PropertyInfo)fromFields[f];

                for (int t = 0; t < toFields.Length; t++)
                {

                    toField = (PropertyInfo)toFields[t];

                    if (fromField.Name != toField.Name) continue;
                    if (!except.Contains(toField.Name) && validKeys.Contains(toField.Name))
                        toField.SetValue(toRecord,
                                     fromField.GetValue(fromRecord, null),
                                     null);
                    break;

                }

            }
        }
        #endregion
        #region Set Properties
        public static void SetProperties(object fromRecord,
                                         object toRecord)
        {
            PropertyInfo[] fromFields = fromRecord.GetType().GetProperties();
            PropertyInfo[] toFields = toRecord.GetType().GetProperties();
            SetProperties(fromFields, toFields, fromRecord, toRecord);
        }
        #endregion
        #region Set Properties
        public static void SetPropertiesEntity(object fromRecord,
                                         object toRecord)
        {
            string[] except = new string[] { "ID", "ID_ORIGINAL", "IS_ORIGINAL", "ENTRY_BY", "ENTRY_DATE", "UPDATED_BY", "UPDATED_DATE", "DELETED_BY", "DELETED_DATE", "APPROVED_BY", "APPROVED_DATE", "REF_DATA_STATUS_ID", "TIPE_APPROVAL" };
            PropertyInfo[] fromFields = fromRecord.GetType().GetProperties();
            PropertyInfo[] toFields = toRecord.GetType().GetProperties();
            SetPropertiesEntity(except, fromFields, toFields, fromRecord, toRecord);
        }
        #endregion
        #region Set Properties
        public static void SetProperties(string[] validKeys, object fromRecord,
                                         object toRecord)
        {
            PropertyInfo[] fromFields = fromRecord.GetType().GetProperties();
            PropertyInfo[] toFields = toRecord.GetType().GetProperties();
            SetProperties(validKeys, fromFields, toFields, fromRecord, toRecord);
        }
        #endregion

        #region Set Properties
        public static void SetProperties(string[] validKeys, PropertyInfo[] fromFields,
                                         PropertyInfo[] toFields,
                                         object fromRecord,
                                         object toRecord)
        {
            PropertyInfo fromField = null;
            PropertyInfo toField = null;

            if (fromFields == null) return;
            if (toFields == null) return;

            for (int f = 0; f < fromFields.Length; f++)
            {

                fromField = (PropertyInfo)fromFields[f];

                for (int t = 0; t < toFields.Length; t++)
                {

                    toField = (PropertyInfo)toFields[t];

                    if (fromField.Name != toField.Name) continue;
                    if (validKeys.Contains(toField.Name))
                    {
                        if (fromField.GetGetMethod().IsVirtual) continue;
                        if (toField.GetGetMethod().IsVirtual) continue;
                        toField.SetValue(toRecord,
                                     fromField.GetValue(fromRecord, null),
                                     null);
                        break;
                    }

                }

            }
        }
        #endregion
        #region Set Properties
        public static void SetPropertiesEntity(string[] exceptKeys, PropertyInfo[] fromFields,
                                         PropertyInfo[] toFields,
                                         object fromRecord,
                                         object toRecord)
        {
            PropertyInfo fromField = null;
            PropertyInfo toField = null;

            if (fromFields == null) return;
            if (toFields == null) return;

            for (int f = 0; f < fromFields.Length; f++)
            {

                fromField = (PropertyInfo)fromFields[f];

                for (int t = 0; t < toFields.Length; t++)
                {

                    toField = (PropertyInfo)toFields[t];

                    if (fromField.Name != toField.Name) continue;
                    if (!exceptKeys.Contains(toField.Name))
                    {
                        if (fromField.GetGetMethod().IsVirtual) continue;
                        if (toField.GetGetMethod().IsVirtual) continue;
                        toField.SetValue(toRecord,
                                     fromField.GetValue(fromRecord, null),
                                     null);
                        break;
                    }

                }

            }
        }
        #endregion
        #region Set Properties
        public static void SetProperties(PropertyInfo[] fromFields,
                                         PropertyInfo[] toFields,
                                         object fromRecord,
                                         object toRecord)
        {
            PropertyInfo fromField = null;
            PropertyInfo toField = null;

            if (fromFields == null) return;
            if (toFields == null) return;

            for (int f = 0; f < fromFields.Length; f++)
            {

                fromField = (PropertyInfo)fromFields[f];

                for (int t = 0; t < toFields.Length; t++)
                {

                    toField = (PropertyInfo)toFields[t];

                    if (fromField.Name != toField.Name || toField.GetGetMethod().IsVirtual) continue;
                    if (fromField.GetGetMethod().IsVirtual) continue;
                    if (toField.GetGetMethod().IsVirtual) continue;
                    toField.SetValue(toRecord,
                                     fromField.GetValue(fromRecord, null),
                                     null);
                    break;

                }

            }
        }
        #endregion
        #region Set Properties
        public static void SetProperties(PropertyInfo[] fromFields,
                                         object fromRecord,
                                         object toRecord)
        {
            PropertyInfo fromField = null;

            if (fromFields == null) return;

            for (int f = 0; f < fromFields.Length; f++)
            {

                fromField = (PropertyInfo)fromFields[f];
                if (!fromField.GetGetMethod().IsVirtual)
                    fromField.SetValue(toRecord,
                                       fromField.GetValue(fromRecord, null),
                                       null);
            }

        }
        #endregion
    }
}
