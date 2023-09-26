using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTool.Helper
{
    public static class RandomBase21
    {
        public static string Generate(int length)
        {
            Random random = new Random();
            string characters = "ABCDEFGHJKLMNPRSTUWXZ";
            StringBuilder result = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }

            return result.ToString();
        }
    }
}
