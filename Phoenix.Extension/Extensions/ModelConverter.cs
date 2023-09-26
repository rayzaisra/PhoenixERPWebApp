using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Phoenix.ApiExtension.Extensions
{
    public static class ModelConverter
    {
        public static byte[] ToByteArray(this Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
