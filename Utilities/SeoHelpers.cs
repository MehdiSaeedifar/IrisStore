using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utilities
{
    public class SeoHelpers
    {
        private const int MaxLenghtSlug = 45;
        public static string GenerateSlug(string title)
        {
            var slug = RemoveAccent(title).ToLower();
            slug = Regex.Replace(slug, @"[^a-z0-9-\u0600-\u06FF]", "-");
            slug = Regex.Replace(slug, @"\s+", "-").Trim();
            slug = Regex.Replace(slug, @"-+", "-");
            slug = slug.Substring(0, slug.Length <= MaxLenghtSlug ? slug.Length : MaxLenghtSlug).Trim();

            return slug;
        }

        private static string RemoveAccent(string text)
        {
            var bytes = Encoding.GetEncoding("UTF-8").GetBytes(text);
            return Encoding.UTF8.GetString(bytes);
        }

        private const string SeparatorTitle = " - ";
        public static string GeneratePageTitle(params string[] crumbs)
        {
            var lstCrumbs = crumbs.Where(crumb => !string.IsNullOrWhiteSpace(crumb)).ToList();

            var title = "";

            for (int i = 0; i < lstCrumbs.Count; i++)
            {
                title += $"{lstCrumbs[i]}{((i < lstCrumbs.Count - 1) ? SeparatorTitle : string.Empty)}";
            }

            title = title.Substring(0, title.Length <= MaxLengthTitle ? title.Length : MaxLengthTitle).Trim();

            return title;
        }


        public static string GenerateMetaDescription(string description)
        {
            description = description.RemoveHtmlTags();

            return description.RemoveHtmlTags().Substring(0, description.Length <= MaxLengthDescription ? description.Length : MaxLengthDescription).Trim();
        }

        public enum CacheControlType
        {
            [Description("public")]
            Public,
            [Description("private")]
            Private,
            [Description("no-cache")]
            Nocache,
            [Description("no-store")]
            Nostore
        }



        private const int MaxLengthTitle = 60;
        private const int MaxLengthDescription = 170;
        private const string FaviconPath = "~/cdn/ui/favicon.ico";

        public static string GenerateMetaTag(string title, string description, bool allowIndexPage, bool allowFollowLinks, string author = "", string lastmodified = "", string expires = "never", string language = "fa", CacheControlType cacheControlType = CacheControlType.Private)
        {
            title = title.Substring(0, title.Length <= MaxLengthTitle ? title.Length : MaxLengthTitle).Trim();
            description = description.Substring(0, description.Length <= MaxLengthDescription ? description.Length : MaxLengthDescription).Trim();

            var meta = "";
            meta += $"<title>{title}</title>\n";
            meta += $"<link rel=\"shortcut icon\" href=\"{FaviconPath}\"/>\n";
            meta += $"<meta http-equiv=\"content-language\" content=\"{language}\"/>\n";
            meta += string.Format("<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\"/>\n");
            meta += string.Format("<meta charset=\"utf-8\"/>\n");
            meta += $"<meta name=\"description\" content=\"{description}\"/>\n";
            meta +=
                $"<meta http-equiv=\"Cache-control\" content=\"{((Enum)cacheControlType).GetEnumDescription()}\"/>\n";
            meta +=
                $"<meta name=\"robots\" content=\"{(allowIndexPage ? "index" : "noindex")}, {(allowFollowLinks ? "follow" : "nofollow")}\" />\n";
            meta += $"<meta name=\"expires\" content=\"{expires}\"/>\n";

            if (!string.IsNullOrEmpty(lastmodified))
                meta += $"<meta name=\"last-modified\" content=\"{lastmodified}\"/>\n";

            if (!string.IsNullOrEmpty(author))
                meta += $"<meta name=\"author\" content=\"{author}\"/>\n";

            //------------------------------------Google & Bing Doesn't Use Meta Keywords ...
            //meta += string.Format("<meta name=\"keywords\" content=\"{0}\"/>\n", keywords);

            return meta;
        }

    }
}
