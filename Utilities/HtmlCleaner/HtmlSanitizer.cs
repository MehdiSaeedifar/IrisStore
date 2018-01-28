using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Utilities.HtmlCleaner
{
    public static class HtmlSanitizer
    {
        #region Fieldss (4)

        private static readonly Regex ChromeWhiteSpace = new Regex(@"style=""white-space\s*:\s*pre\s*;\s*""",
            RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex HtmlComments = new Regex("((<!-- )((?!<!-- ).)*( -->))(\r\n)*",
            RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex HtmlTagExpression =
            new Regex(
                @"(?'tag_start'</?)(?'tag'\w+)((\s+(?'attr'(?'attr_name'\w+)(\s*=\s*(?:"".*?""|'.*?'|[^'"">\s]+)))?)+\s*|\s*)(?'tag_end'/?>)",
                RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Dictionary<string, List<string>> ValidHtmlTags = new Dictionary<string, List<string>>
        {
            {"p", new List<string>()},
            {"table", new List<string> {"height", "width", "style"}},
            {"tr", new List<string> {"style"}},
            {"td", new List<string> {"style"}},
            {"tbody", new List<string> {"style"}},
            {"tfoot", new List<string> {"style", "class"}},
            {"th", new List<string> {"style"}},
            {"div", new List<string> {"dir", "align", "style"}},
            {"span", new List<string> {"dir", "color", "align", "style"}},
            {"pre", new List<string> {"language", "name"}},
            {"strong", new List<string>()},
            {"br", new List<string>()},
            {"label", new List<string> {"style", "class"}},
            {"font", new List<string> {"style", "class", "color", "face", "size"}},
            {"h1", new List<string>()},
            {"h2", new List<string>()},
            {"h3", new List<string>()},
            {"h4", new List<string>()},
            {"h5", new List<string>()},
            {"h6", new List<string>()},
            {"blockquote", new List<string> {"style", "class"}},
            {"b", new List<string>()},
            {"hr", new List<string>()},
            {"em", new List<string>()},
            {"i", new List<string>()},
            {"u", new List<string>()},
            {"strike", new List<string>()},
            {"ol", new List<string>()},
            {"ul", new List<string>()},
            {"li", new List<string>()},
            {"a", new List<string> {"href", "style"}},
            {"img", new List<string> {"src", "height", "width", "alt", "style"}},
            {"q", new List<string> {"cite"}},
            {"cite", new List<string>()},
            {"abbr", new List<string>()},
            {"acronym", new List<string>()},
            {"del", new List<string>()},
            {"ins", new List<string>()}
        };

        #endregion Fields

        #region Methods (3)

        private static string cleanHtml(this string html)
        {
            html = ChromeWhiteSpace.Replace(html, string.Empty);
            html = HtmlComments.Replace(html, string.Empty);
            return html;
        }

        /// <summary>
        ///     To the safe HTML.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string ToSafeHtml(this string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            text = text.cleanHtml();
            text = text.removeInvalidHtmlTags();
            return text.ApplyModeratePersianRules();
        }

        // Private Methods (1) 

        /// <summary>
        ///     Removes the invalid HTML tags.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        private static string removeInvalidHtmlTags(this string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            return HtmlTagExpression.Replace(text, m =>
            {
                if (!ValidHtmlTags.ContainsKey(m.Groups["tag"].Value.ToLowerInvariant()))
                    return String.Empty;

                var generatedTag = new StringBuilder(m.Length);

                Group tagStart = m.Groups["tag_start"];
                Group tagEnd = m.Groups["tag_end"];
                Group tag = m.Groups["tag"];
                Group tagAttributes = m.Groups["attr"];

                generatedTag.Append(tagStart.Success ? tagStart.Value : "<");
                generatedTag.Append(tag.Value);

                foreach (Capture attr in tagAttributes.Captures)
                {
                    int indexOfEquals = attr.Value.IndexOf('=');

                    // don't proceed any futurer if there is no equal sign or just an equal sign
                    if (indexOfEquals < 1)
                        continue;

                    string attrName = attr.Value.Substring(0, indexOfEquals).ToLowerInvariant();

                    // check to see if the attribute name is allowed and write attribute if it is
                    if (!ValidHtmlTags[tag.Value.ToLowerInvariant()].Contains(attrName)) continue;

                    generatedTag.Append(' ');
                    generatedTag.Append(attr.Value);
                }

                generatedTag.Append(tagEnd.Success ? tagEnd.Value : ">");

                return generatedTag.ToString();
            });
        }

        #endregion Methods
    }
}