using System;
using System.Web;

namespace Iris.Web.Helpers
{
    public static class CookieHelper
    {
        public static bool CanUserVoteBasedOnCookies(this HttpContextBase httpContext, int postId, string sectionType)
        {
            string key = sectionType + "-" + postId;
            var value = httpContext.GetCookieValue(key);
            if (string.IsNullOrWhiteSpace(value))
            {
                httpContext.AddCookie(key, key);
                return true;
            }
            return false;
        }

        public static void AddCookie(this HttpContextBase httpContextBase, string cookieName, string value)
        {
            httpContextBase.AddCookie(cookieName, value, DateTime.Now.AddDays(30));
        }

        public static void AddCookie(this HttpContextBase httpContextBase, string cookieName, string value, DateTime expires)
        {
            var cookie = new HttpCookie(cookieName)
            {
                Expires = expires,
                Value = httpContextBase.Server.UrlEncode(value) // For Cookies and Unicode characters
            };
            httpContextBase.Response.Cookies.Add(cookie);
        }

        public static string GetCookieValue(this HttpContextBase httpContext, string cookieName)
        {
            var cookie = httpContext.Request.Cookies[cookieName];
            if (cookie == null)
                return string.Empty; //cookie doesn't exist

            // For Cookies and Unicode characters
            return httpContext.Server.UrlDecode(cookie.Value);
        }
    }
}