namespace Infrastructure
{
    public static class Email
    {
        #region SendEmailAfterRegistration
        //متد ارسال ایمیل بعد از ثبت نام
        public static void SendEmailAfterRegistration
            (string userId, string userName, string password, string code)
        {
            //استفاده از قالب موجود در ای پی پی دیتا
            //از قالب پیش  در ای پی پی دیتا کپی گرفته و سفارشی نمائید!
            string strRootRelativePathName =
                "~/App_Data/CustomEmailTemplate/CustomEmailTemplate.htm";

            //ایجاد یک رشته و مراحل تبدیل مراحل نسبی به فیزیکی
            string strPathName =
                System.Web.HttpContext.Current.Server.MapPath(strRootRelativePathName);

            //استفاده از متد رید کلاس فایل برای خواندن مسیر فیزیکی
            string strEmailBody = File.Read(strPathName);

            //جایگزینی مقادیر موجود در فایل خوانده شده با مقادیر داده شده
            strEmailBody = strEmailBody
                            .Replace("[USER_ID]", userId)
                            .Replace("[USER_NAME]", userName)
                            .Replace("[PASSWORD]", password)
                            .Replace("[CODE]", code);
            //ایجاد یک شی از میل آدرس با 3 پارامتر
            System.Net.Mail.MailAddress oMailAddress =
                new System.Net.Mail.MailAddress(userName, userName, System.Text.Encoding.UTF8);
            //استفاده از متد سند کلاس میل مسیج
            MailMessage.Send
                (oMailAddress, "تائید رایانامه!", strEmailBody, System.Net.Mail.MailPriority.High);
        }
        #endregion

        #region Sendcode
        // متد ارسال کلید تائید 
        public static void Sendcode(string userId, string userName, string code)
        {
            SendEmailAfterRegistration(userId, userName, "", code);
        }

        #endregion

        #region SendEmailForgotPassword
        //متد ارسال ایمیل بعد از ثبت نام
        public static void SendEmailForgotPassword
            (string userId, string userName, string code)
        {
            //استفاده از قالب موجود در ای پی پی دیتا
            //از قالب پیش  در ای پی پی دیتا کپی گرفته و سفارشی نمائید!
            string strRootRelativePathName =
                "~/App_Data/CustomEmailTemplate/ForgotPasswordUserEmail.htm";

            //ایجاد یک رشته و مراحل تبدیل مراحل نسبی به فیزیکی
            string strPathName =
                System.Web.HttpContext.Current.Server.MapPath(strRootRelativePathName);

            //استفاده از متد رید کلاس فایل برای خواندن مسیر فیزیکی
            string strEmailBody = File.Read(strPathName);

            //جایگزینی مقادیر موجود در فایل خوانده شده با مقادیر داده شده
            strEmailBody = strEmailBody
                            .Replace("[USER_ID]", userId)
                            .Replace("[USER_NAME]", userName)
                            .Replace("[CODE]", code);
            //ایجاد یک شی از میل آدرس با 3 پارامتر
            System.Net.Mail.MailAddress oMailAddress =
                new System.Net.Mail.MailAddress(userName, userId, System.Text.Encoding.UTF8);
            //استفاده از متد سند کلاس میل مسیج
            MailMessage.Send
                (oMailAddress, "بازیابی گذرواژه!", strEmailBody, System.Net.Mail.MailPriority.High);
        }

        #endregion

        #region SendNewsLetter
        //متد ارسال خبرنامه
        public static void SendNewsLetter
            (string userId, string email, string category, string name, string content)
        {
            //استفاده از قالب موجود در ای پی پی دیتا
            //از قالب پیش  در ای پی پی دیتا کپی گرفته و سفارشی نمائید!
            string strRootRelativePathName =
                "~/App_Data/CustomEmailTemplate/News.htm";

            //ایجاد یک رشته و مراحل تبدیل مراحل نسبی به فیزیکی
            string strPathName =
                System.Web.HttpContext.Current.Server.MapPath(strRootRelativePathName);

            //استفاده از متد رید کلاس فایل برای خواندن مسیر فیزیکی
            string strEmailBody = File.Read(strPathName);

            //جایگزینی مقادیر موجود در فایل خوانده شده با مقادیر داده شده
            strEmailBody = strEmailBody
                            .Replace("[USER_NAME]", email)
                            .Replace("[CAT]", category)
                            .Replace("[NAME]", name)
                            .Replace("[CONTENT]", content);
            //ایجاد یک شی از میل آدرس با 3 پارامتر
            System.Net.Mail.MailAddress oMailAddress =
                new System.Net.Mail.MailAddress(email, userId, System.Text.Encoding.UTF8);
            //استفاده از متد سند کلاس میل مسیج
            MailMessage.Send
                (oMailAddress, "خبرنامه!", strEmailBody, System.Net.Mail.MailPriority.High);
        }

        #endregion

        #region SendContact
        //متد ارسال تماس با ما
        public static void SendContact
            (string name, string email, string subject, string message)
        {
            //استفاده از قالب موجود در ای پی پی دیتا
            //از قالب پیش  در ای پی پی دیتا کپی گرفته و سفارشی نمائید!
            string strRootRelativePathName =
                "~/App_Data/CustomEmailTemplate/Contact.htm";

            //ایجاد یک رشته و مراحل تبدیل مراحل نسبی به فیزیکی
            string strPathName =
                System.Web.HttpContext.Current.Server.MapPath(strRootRelativePathName);

            //استفاده از متد رید کلاس فایل برای خواندن مسیر فیزیکی
            string strEmailBody = File.Read(strPathName);

            //جایگزینی مقادیر موجود در فایل خوانده شده با مقادیر داده شده
            strEmailBody = strEmailBody
                            .Replace("[NAME]", name)
                            .Replace("[MAIL]", email)
                            .Replace("[SUBJECT]", subject)
                            .Replace("[MESSAGE]", message);
            //ایجاد یک شی از میل آدرس با 3 پارامتر
            System.Net.Mail.MailAddress oMailAddress =
                new System.Net.Mail.MailAddress(email, email, System.Text.Encoding.UTF8);
            //استفاده از متد سند کلاس میل مسیج
            MailMessage.Send
                (oMailAddress, "تماس با ما!", strEmailBody, System.Net.Mail.MailPriority.High);
        }

        #endregion

        #region SendEmailComments
        //متد ارسال دیدگاه
        public static void SendEmailComments
            (string id, string email, string message)
        {
            //استفاده از قالب موجود در ای پی پی دیتا
            //از قالب پیش  در ای پی پی دیتا کپی گرفته و سفارشی نمائید!
            string strRootRelativePathName =
                "~/App_Data/CustomEmailTemplate/Comment.htm";

            //ایجاد یک رشته و مراحل تبدیل مراحل نسبی به فیزیکی
            string strPathName =
                System.Web.HttpContext.Current.Server.MapPath(strRootRelativePathName);

            //استفاده از متد رید کلاس فایل برای خواندن مسیر فیزیکی
            string strEmailBody = File.Read(strPathName);

            //جایگزینی مقادیر موجود در فایل خوانده شده با مقادیر داده شده
            strEmailBody = strEmailBody
                            .Replace("[ID]", id)
                            .Replace("[MAIL]", email)
                            .Replace("[MESSAGE]", message);
            //ایجاد یک شی از میل آدرس با 3 پارامتر
            System.Net.Mail.MailAddress oMailAddress =
                new System.Net.Mail.MailAddress(email, email, System.Text.Encoding.UTF8);
            //استفاده از متد سند کلاس میل مسیج
            MailMessage.Send
                (oMailAddress, "ارسال دیدگاه!", strEmailBody, System.Net.Mail.MailPriority.High);
        }

        #endregion

        #region SendEmailReComments
        //متد ارسال پاسخ دیدگاه
        public static void SendEmailReComments
            (string id, string email, string reMessage)
        {
            //استفاده از قالب موجود در ای پی پی دیتا
            //از قالب پیش  در ای پی پی دیتا کپی گرفته و سفارشی نمائید!
            string strRootRelativePathName =
                "~/App_Data/CustomEmailTemplate/ReComment.htm";

            //ایجاد یک رشته و مراحل تبدیل مراحل نسبی به فیزیکی
            string strPathName =
                System.Web.HttpContext.Current.Server.MapPath(strRootRelativePathName);

            //استفاده از متد رید کلاس فایل برای خواندن مسیر فیزیکی
            string strEmailBody = File.Read(strPathName);

            //جایگزینی مقادیر موجود در فایل خوانده شده با مقادیر داده شده
            strEmailBody = strEmailBody
                            .Replace("[ID]", id)
                            .Replace("[MAIL]", email)
                            .Replace("[MESSAGE]", reMessage);
            //ایجاد یک شی از میل آدرس با 3 پارامتر
            System.Net.Mail.MailAddress oMailAddress =
                new System.Net.Mail.MailAddress(email, email, System.Text.Encoding.UTF8);
            //استفاده از متد سند کلاس میل مسیج
            MailMessage.Send
                (oMailAddress, "ارسال پاسخ دیدگاه!", strEmailBody, System.Net.Mail.MailPriority.High);
        }
        #endregion

        #region SendEmailAfterOrderRegistration
        //متد ارسال ایمیل سفارش
        public static void SendEmailAfterOrderRegistration
            (string id, string code, string fullname, string email, string details, string comment, string date, string address)
        {
            //استفاده از قالب موجود در ای پی پی دیتا
            //از قالب پیش  در ای پی پی دیتا کپی گرفته و سفارشی نمائید!
            string strRootRelativePathName =
                "~/App_Data/CustomEmailTemplate/UserEmailOrder.htm";

            //ایجاد یک رشته و مراحل تبدیل مراحل نسبی به فیزیکی
            string strPathName =
                System.Web.HttpContext.Current.Server.MapPath(strRootRelativePathName);

            //استفاده از متد رید کلاس فایل برای خواندن مسیر فیزیکی
            string strEmailBody = File.Read(strPathName);

            //جایگزینی مقادیر موجود در فایل خوانده شده با مقادیر داده شده
            strEmailBody = strEmailBody
                            .Replace("[CODE]", code)
                            .Replace("[ID]", id)
                            .Replace("[FULL_NAME]", fullname)
                            .Replace("[COMMENT]", comment)
                            .Replace("[DATE]", date)
                            .Replace("[ADDRESS]", address)
                            .Replace("[CONTENT]", details);
            //ایجاد یک شی از میل آدرس با 3 پارامتر
            System.Net.Mail.MailAddress oMailAddress =
                new System.Net.Mail.MailAddress(email, fullname, System.Text.Encoding.UTF8);
            //استفاده از متد سند کلاس میل مسیج
            MailMessage.Send
                (oMailAddress, "سفارش شما با موفقیت به ثبت رسید!", strEmailBody, System.Net.Mail.MailPriority.High);
        }

        #endregion
    }

}