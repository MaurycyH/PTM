using System;
using System.Reflection;
using System.Linq;
using Tesseract.Common;

namespace PTM.Utilities
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Zwraca wartość atrybutu Description enuma, lub pusty string jeżeli enum go nie posiada
        /// </summary>
        public static string GetDescription(this Enum text)
        {
            Ensure.ParamNotNull(text, nameof(text));

            return text.GetType()
                    .GetMember(text.ToString())
                    .FirstOrDefault()
                    ?.GetCustomAttribute<EnumDescription>()
                    ?.Description ?? "";
        }
    }
}
