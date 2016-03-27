using System.Text.RegularExpressions;

namespace Utilities.HtmlCleaner
{
    public static class PersianProofWriter
    {
        #region Fields (4)

        public const char ArabicKeChar = (char)1603;
        public const char ArabicYeChar = (char)1610;
        public const char PersianKeChar = (char)1705;
        public const char PersianYeChar = (char)1740;

        #endregion Fields

        #region Methods (7)

        // Public Methods (7) 

        /// <summary>
        ///     Adds zwnj char between word and prefix/suffix
        /// </summary>
        /// <param name="text">Text to process</param>
        /// <returns>Processed Text</returns>
        public static string ApplyHalfSpaceRule(this string text)
        {
            //put zwnj between word and prefix (mi* nemi*)
            string phase1 = Regex.Replace(text, @"\s+(ن?می)\s+", @" $1‌");

            //put zwnj between word and suffix (*tar *tarin *ha *haye)
            string phase2 = Regex.Replace(phase1, @"\s+(تر(ی(ن)?)?|ها(ی)?)\s+", @"‌$1 ");
            return phase2;
        }

        public static string ApplyModeratePersianRules(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            if (!text.ContainsFarsi())
                return text;

            return text
                .ApplyPersianYeKe()
                .ApplyHalfSpaceRule()
                .YeHeHalfSpace()
                .CleanupZwnj()
                .CleanupExtraMarks();
        }

        /// <summary>
        ///     Fixes common writing mistakes caused by using a bad keyboard layout,
        ///     such as replacing Arabic Ye with Persian one and so on ...
        /// </summary>
        /// <param name="text">Text to process</param>
        /// <returns>Processed Text</returns>
        public static string ApplyPersianYeKe(this string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            return text.Replace(ArabicYeChar, PersianYeChar).Replace(ArabicKeChar, PersianKeChar).Trim();
        }

        /// <summary>
        ///     Replaces more than one ! or ? mark with just one
        /// </summary>
        /// <param name="text">Text to process</param>
        /// <returns>Processed Text</returns>
        public static string CleanupExtraMarks(this string text)
        {
            string phase1 = Regex.Replace(text, @"(!){2,}", "$1");
            string phase2 = Regex.Replace(phase1, "(؟){2,}", "$1");
            return phase2;
        }

        /// <summary>
        ///     Removes unnecessary zwnj char that are succeeded/preceded by a space
        /// </summary>
        /// <param name="text">Text to process</param>
        /// <returns>Processed Text</returns>
        public static string CleanupZwnj(this string text)
        {
            return Regex.Replace(text, @"\s+‌|‌\s+", " ");
        }

        /// <summary>
        ///     Does text contain Persian characters?
        /// </summary>
        /// <param name="text">Text to process</param>
        /// <returns>true/false</returns>
        public static bool ContainsFarsi(this string text)
        {
            return Regex.IsMatch(text, @"[\u0600-\u06FF]");
        }

        /// <summary>
        ///     Converts ه ی to ه‌ی
        /// </summary>
        /// <param name="text">Text to process</param>
        /// <returns>Processed Text</returns>
        public static string YeHeHalfSpace(this string text)
        {
            return Regex.Replace(text, @"(\S)(ه[\s‌]+[یي])(\s)", "$1ه‌ی‌$3"); // fix zwnj
        }

        #endregion Methods
    }
}