using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utilities
{
    public static class StringExtensions
    {
        public static string RemoveHtmlTags(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return string.Empty;
            }

            var noHtml = Regex.Replace(str, @"<[^>]+>|&nbsp;", "").Trim();
            var noHtmlNormalised = Regex.Replace(noHtml, @"\s{2,}", " ");

            return noHtmlNormalised;
        }
    }
}
