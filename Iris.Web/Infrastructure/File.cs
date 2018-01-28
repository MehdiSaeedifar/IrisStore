namespace Infrastructure
{
    public static class File
    {
        #region ReadFromFiles_Read(string pathName)

        /// <summary>
        /// متد خواندن فابل ها در پروژه
        /// این متد یک مسیر فیزیکی میگیرد
        /// </summary>
        /// <param name="pathName"></param>
        /// <returns></returns>
        public static string Read(string pathName)
        {
            //اگر مسیر نال بود امپتی برمیگردونه
            if (pathName == null)
            {
                return (string.Empty);
            }

            //تریم میکنیم
            pathName = pathName.Trim();

            //اگر باز خالی بود خالی برمیگردونه
            if (pathName == string.Empty)
            {
                return (string.Empty);
            }

            //اگر فایل وجود نداشت خالی برمیگردونه
            if (System.IO.File.Exists(pathName) == false)
            {
                return (string.Empty);
            }

            string strResult = string.Empty;

            //از استریم ریدر استفاده کردیم
            System.IO.StreamReader oStream = null;

            try
            {
                oStream =
                    new System.IO.StreamReader(pathName, System.Text.Encoding.UTF8);

                strResult = oStream.ReadToEnd();
            }
            //بستن شی استریم ریدر
            finally
            {
                if (oStream != null)
                {
                    oStream.Close();
                    oStream.Dispose();
                    oStream = null;
                }
            }
            // ریترن میشه
            return (strResult);
        }

        #endregion ReadFromFiles_Read(string pathName)

        #region WriteToFiles_Write(string pathName, string text, bool append)

        /// <summary>
        /// متد رایت که یک مسیر و یک متن و یک بولین اپند میگیره
        /// </summary>
        /// <param name="pathName"></param>
        /// <param name="text"></param>
        /// <param name="append"></param>
        public static void Write(string pathName, string text, bool append)
        {
            //اگر مسیر نال باشه از متد خارج میشود
            if (pathName == null)
            {
                return;
            }

            //رشته حاوی مسیر تریم میشود
            pathName = pathName.Trim();

            //اگر امپتی شد از متد خارج میشود
            if (pathName == string.Empty)
            {
                return;
            }

            //پارامتر دوم متد که یک متن هست اگر نال بود برابر با امپتی میشود
            if (text == null)
            {
                text = string.Empty;
            }

            //از استریم رایتر استفاده کردیم
            System.IO.StreamWriter oStream = null;
            try
            {
                //نیو شدن استریم رایتر با پارامتر اول و سوم متد جاری
                oStream =
                    new System.IO.StreamWriter(pathName, append, System.Text.Encoding.UTF8);
                //نوشتن در متن که پرامتر دوم متد جاری است
                oStream.Write(text);
            }
            //بستن شی استریم رایتر
            finally
            {
                if (oStream != null)
                {
                    oStream.Close();
                    oStream.Dispose();
                    oStream = null;
                }
            }
        }

        #endregion Write(string pathName, string text, bool append)
    }
}
