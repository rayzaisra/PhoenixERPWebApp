using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;


namespace Phoenix.WebExtension.Extentions
{
    public static class ObjectConverter
    {
        public static byte[] ToByteArray(this object obj)
        {
            if (obj == null)
                return null;

            var bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        // Convert a byte array to an Object
        public static object ToObject(this byte[] arrBytes)
        {
            if (arrBytes == null || arrBytes.Length == 0)
                return null;

            using (MemoryStream memStream = new MemoryStream())
            {
                BinaryFormatter binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                object obj = (object)binForm.Deserialize(memStream);

                return obj;
            }
        }

        public static T ToObject<T>(this byte[] arrBytes) where T : class
        {
            if (arrBytes == null || arrBytes.Length == 0)
                return null;

            using (MemoryStream memStream = new MemoryStream())
            {
                BinaryFormatter binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                object obj = binForm.Deserialize(memStream);

                return (T)Convert.ChangeType(obj, typeof(T));
            }
        }
    }
}
