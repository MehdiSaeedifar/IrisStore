namespace Infrastructure
{
    public static class ServerTimeZone
    {
        #region GetServerTimeNow

        public static System.DateTime GetServerTimeNow()
        {
            return (System.DateTime.Now.AddMinutes
                (ServerTimeZone.ServerTimeZoneDifference));
        }

        //تعریف یک فیلد نالیبل برای اینکه زمان سرور رو بدست بیاوریم
        private static int? _serverTimeZoneDifference;
        public static int ServerTimeZoneDifference
        {
            get
            {
                //اگر فیلد مقدار نداشت
                if (_serverTimeZoneDifference.HasValue == false)
                {
                    //زمان ریست میشود
                    _serverTimeZoneDifference = 0;
                    try
                    {
                        //با استفاده از گت ولیو زمان را بدست میاوریم
                        _serverTimeZoneDifference =
                            System.Convert.ToInt32(KeyManager.GetValue("ServerTimeZoneDifference", "0"));
                    }
                    //خطا
                    catch { }
                }

                //ارسال زمان سرور به برنامه
                return (_serverTimeZoneDifference.Value);
            }
        }
        #endregion
    }
}