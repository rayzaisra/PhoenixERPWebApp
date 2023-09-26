using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phoenix.Data.Models
{
    public static class InputType
    {
        public const int Checkbox = 1;
        public const int Radiobutton = 2;
        public const int TextArea = 3;

        public static string InputTypeName(int value)
        {
            string result = "";
            switch (value)
            {
                case Checkbox:
                    result = "Checkbox";
                    break;
                case Radiobutton:
                    result = "Radiobutton";
                    break;
                case TextArea:
                    result = "TextArea";
                    break;
                default:
                    result = "Checkbox";
                    break;
            }
            return result;
        }
    }
}
