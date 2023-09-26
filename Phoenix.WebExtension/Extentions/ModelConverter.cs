using System.IO;

namespace Phoenix.WebExtension.Extentions
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
