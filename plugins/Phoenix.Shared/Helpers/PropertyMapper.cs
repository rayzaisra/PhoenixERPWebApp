using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Shared.Helpers
{
    public class PropertyMapper
    {
        private static string[] excepts = new string[] { "ID", "ID_ORIGINAL", "IS_ORIGINAL", "ENTRY_BY", "ENTRY_DATE", "UPDATED_BY", "UPDATED_DATE", "DELETED_BY", "DELETED_DATE", "APPROVED_BY", "APPROVED_DATE", "REF_DATA_STATUS_ID", "TIPE_APPROVAL" };

        public static string[] DefaultExcepts()
        {
            return excepts;
        }
        #region Set Properties
        public static void Except(string[] excepts, object fromRecord,
                                         object toRecord)
        {
            PropertyInfo[] fromFields = fromRecord.GetType().GetProperties();
            PropertyInfo[] toFields = toRecord.GetType().GetProperties();
            SetPropertiesExcept(excepts, fromFields, toFields, fromRecord, toRecord);
        }
        #endregion
        #region All
        public static void All(object fromRecord,
                                         object toRecord)
        {
            PropertyInfo[] fromFields = fromRecord.GetType().GetProperties();
            PropertyInfo[] toFields = toRecord.GetType().GetProperties();
            SetProperties(fromFields, toFields, fromRecord, toRecord);
        }
        #endregion
        #region Only
        public static void Only(string[] validKeys, object fromRecord,
                                         object toRecord)
        {
            PropertyInfo[] fromFields = fromRecord.GetType().GetProperties();
            PropertyInfo[] toFields = toRecord.GetType().GetProperties();
            SetPropertiesOnly(validKeys, fromFields, toFields, fromRecord, toRecord);
        }
        #endregion

        #region Only
        public static void OnlyWithDefaultExcepts(string[] validKeys, object fromRecord,
                                         object toRecord)
        {
            PropertyInfo[] fromFields = fromRecord.GetType().GetProperties();
            PropertyInfo[] toFields = toRecord.GetType().GetProperties();
            SetPropertiesOnlyWithDefaultExcepts(validKeys, fromFields, toFields, fromRecord, toRecord);
        }
        #endregion

        #region SetProperties
        private static void SetProperties(PropertyInfo[] fromFields,
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
        #region SetPropertiesOnly
        private static void SetPropertiesOnly(string[] validKeys, PropertyInfo[] fromFields,
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
                        toField.SetValue(toRecord,
                                     fromField.GetValue(fromRecord, null),
                                     null);
                    break;

                }

            }
        }
        #endregion


        #region SetPropertiesOnlyWithDefaultExcepts
        private static void SetPropertiesOnlyWithDefaultExcepts(string[] validKeys, PropertyInfo[] fromFields,
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
                    if (!excepts.Contains(toField.Name) && validKeys.Contains(toField.Name))
                        toField.SetValue(toRecord,
                                     fromField.GetValue(fromRecord, null),
                                     null);
                    break;

                }

            }
        }
        #endregion

        #region Set PropertiesExcept
        private static void SetPropertiesExcept(string[] exceptKeys, PropertyInfo[] fromFields,
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
    }
}
