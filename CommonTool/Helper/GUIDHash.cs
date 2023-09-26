using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CommonTool
{
    public class GUIDHash
    {
        public static Guid ConvertToMd5HashGUID(string value)
        {
            // convert null to empty string - null can not be hashed
            if (value == null) value = string.Empty;

            // get the byte representation
            var bytes = Encoding.Default.GetBytes(value);

            // create the md5 hash
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(bytes);

            // convert the hash to a Guid
            return new Guid(data);
        }
    }
}
