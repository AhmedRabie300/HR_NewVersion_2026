using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Common
{
    public static class LanguageResolver
    {
        public const int English = 1;
        public const int Arabic = 2;
        public const int Default = English;

        public static int Resolve(string? xLang)
        {
            if (string.IsNullOrWhiteSpace(xLang)) return Default;

            return xLang.Trim().ToLowerInvariant() switch
            {
                "ar" => Arabic,
                "en" => English,
                _ => Default
            };
        }
    }
}
