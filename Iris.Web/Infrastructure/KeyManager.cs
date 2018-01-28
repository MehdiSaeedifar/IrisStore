namespace Infrastructure
{
    public static class KeyManager
    {
        #region GetValue(key)
        public static string GetValue(string key)
        {
            return (GetValue(key, string.Empty));
        }
        #endregion

        #region GetValue(key,defaultValue)
        public static string GetValue(string key, string defaultValue)
        {
            if (defaultValue == null)
            {
                defaultValue = string.Empty;
            }
            else
            {
                defaultValue = defaultValue.Trim();
            }

            if (key == null)
            {
                return (defaultValue);
            }

            key = key.Trim();

            if (key == string.Empty)
            {
                return (defaultValue);
            }

            try
            {
                string strData =
                    System.Configuration.ConfigurationManager.AppSettings[key];

                if (string.IsNullOrEmpty(strData))
                {
                    return (defaultValue);
                }

                return (strData.Trim());
            }
            catch
            {
                return (defaultValue);
            }
        }
        #endregion
    }
}
