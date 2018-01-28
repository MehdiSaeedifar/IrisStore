namespace Infrastructure
{
    public static class MailMessage
    {
        #region MailBody_ConvertTextForBodyEmail(string text)

        //ایجاد یک رشته که قالب رایانامه را شبیه سازی میکند
        public static string ConvertTextForBodyEmail(string text)
        {
            text = text.Replace(System.Convert.ToChar(10).ToString(), ""); // Return Key.
            text = text.Replace(System.Convert.ToChar(13).ToString(), "<br />"); // Return Key.
            text = text.Replace(System.Convert.ToChar(9).ToString(), "&nbsp;&nbsp;&nbsp;&nbsp;"); // TAB Key.

            return (text);
        }
        #endregion ConvertTextForBodyEmail(string text)

        #region SendMailMethods_MailMessage&SmtpConfiguration()

        public static void Send
            (
                string subject,
                string body
            )
        {
            Send
                (
                    null,
                    null,
                    subject,
                    body,
                    System.Net.Mail.MailPriority.High,
                    null,
                    System.Net.Mail.DeliveryNotificationOptions.Never
                );
        }

        public static void Send
            (
                System.Net.Mail.MailAddress recipient,
                string subject,
                string body,
                System.Net.Mail.MailPriority priority
            )
        {
            System.Net.Mail.MailAddressCollection oRecipients =
                new System.Net.Mail.MailAddressCollection();

            oRecipients.Add(recipient);

            Send(null, oRecipients, subject, body, priority, null, System.Net.Mail.DeliveryNotificationOptions.Never);
        }

        public static void Send
    (
        System.Net.Mail.MailAddress sender,
        System.Net.Mail.MailAddressCollection recipients,
        string subject,
        string body,
        System.Net.Mail.MailPriority priority,
        System.Net.Mail.AttachmentCollection attachments,
        System.Net.Mail.DeliveryNotificationOptions deliveryNotification
    )
        {
            System.Net.Mail.MailAddress oSender = null; ;
            System.Net.Mail.SmtpClient oSmtpClient = null;
            System.Net.Mail.MailMessage oMailMessage = null;

            try
            {
                // Mail Message Configuration 
                oMailMessage = new System.Net.Mail.MailMessage();

                if (sender != null)
                {
                    oSender = sender;
                }
                else
                {
                    string strAddress = KeyManager.GetValue("NoReplyAddress");
                    string strDisplayName = KeyManager.GetValue("NoReplyDisplayName");

                    if (string.IsNullOrEmpty(strDisplayName))
                    {
                        oSender =
                            new System.Net.Mail.MailAddress(strAddress, strAddress, System.Text.Encoding.UTF8);
                    }
                    else
                    {
                        oSender =
                            new System.Net.Mail.MailAddress(strAddress, strDisplayName, System.Text.Encoding.UTF8);
                    }
                }

                oMailMessage.From = oSender;
                oMailMessage.Sender = oSender;
                oMailMessage.ReplyTo = oSender;

                oMailMessage.To.Clear();
                oMailMessage.CC.Clear();
                oMailMessage.Bcc.Clear();
                oMailMessage.Attachments.Clear();

                if (recipients == null)
                {
                    System.Net.Mail.MailAddress oMailAddress = null;
                    string strAddress = KeyManager.GetValue("SupportAddress");
                    string strDisplayName = KeyManager.GetValue("SupportDisplayName");

                    if (string.IsNullOrEmpty(strDisplayName))
                    {
                        oMailAddress =
                            new System.Net.Mail.MailAddress(strAddress, strAddress, System.Text.Encoding.UTF8);
                    }
                    else
                    {
                        oMailAddress =
                            new System.Net.Mail.MailAddress(strAddress, strDisplayName, System.Text.Encoding.UTF8);
                    }

                    oMailMessage.To.Add(oMailAddress);
                }
                else
                {
                    foreach (System.Net.Mail.MailAddress oMailAddress in recipients)
                    {
                        oMailMessage.To.Add(oMailAddress);
                    }
                }

                string strBccAddresses = KeyManager.GetValue("BccAddresses");

                if (string.IsNullOrEmpty(strBccAddresses))
                {
                    oMailMessage.Bcc.Add("Admin@email.com");
                }
                else
                {
                    oMailMessage.Bcc.Add(strBccAddresses);
                }

                oMailMessage.Body = body;

                string strEmailSubjectPrefix = KeyManager.GetValue("EmailSubjectPrefix");
                if (string.IsNullOrEmpty(strEmailSubjectPrefix))
                {
                    oMailMessage.Subject = subject;
                }
                else
                {
                    oMailMessage.Subject = strEmailSubjectPrefix + " " + subject;
                }

                oMailMessage.IsBodyHtml = true;
                oMailMessage.Priority = priority;
                oMailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                oMailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                oMailMessage.DeliveryNotificationOptions = deliveryNotification;

                if (attachments != null)
                {
                    foreach (System.Net.Mail.Attachment oAttachment in attachments)
                    {
                        oMailMessage.Attachments.Add(oAttachment);
                    }
                }

                //هدر رایانامه 
                oMailMessage.Headers.Add("Company_Mailer_Version", "1.2.1");
                oMailMessage.Headers.Add("Company_Mailer_Date", "2018/12/12");
                oMailMessage.Headers.Add("Company_Mailer_Author", "Mr. your name");
                oMailMessage.Headers.Add("Company_Mailer_Company", "your site");
                // End Mail Message Configuration 

                //Smtp Client Configuration 
                oSmtpClient = new System.Net.Mail.SmtpClient();

                //کلا تو این گت ولیو ها دیفالت ولیو آخرین مقدار هست
                //بررسی امضا الکترونیکی رایانامه و مقدار دهی آن توسط متد گت ولیو
                if (KeyManager.GetValue("SmtpClientEnableSsl", "0") == "1")
                {
                    oSmtpClient.EnableSsl = true;
                }
                else
                {
                    oSmtpClient.EnableSsl = false;
                }

                //مدت زمان برقراری اتصال برای ارسال رایانامه پیش فرض 100 ثانیه است
                oSmtpClient.Timeout =
                    System.Convert.ToInt32(KeyManager.GetValue("SmtpClientTimeout", "100000"));

                //End Smtp Client Configuration 

                //Final!
                oSmtpClient.Send(oMailMessage);
            }
            catch (System.Exception ex)
            {
                System.Collections.Hashtable oHashtable =
                    new System.Collections.Hashtable();

                if (oSender != null)
                {
                    oHashtable.Add("Address", oSender.Address);
                    oHashtable.Add("DisplayName", oSender.DisplayName);
                }

                oHashtable.Add("Subject", subject);
                oHashtable.Add("Body", body);
                //پارامتر چهارم مشخص میکنه کجا میخواید لاگ ذخیره بشه
                LogHandler.Report(typeof(MailMessage), oHashtable, ex, LogHandler.LogTypes.LogToFile);
                //string strErrorMessage = System.Web.HttpContext.GetGlobalResourceObject("Library", "ErrorOnSendingEmail").ToString();
            }
            finally
            {
                if (oMailMessage != null)
                {
                    oMailMessage.Dispose();
                    oMailMessage = null;
                }

                if (oSmtpClient != null)
                {
                    oSmtpClient = null;
                }
            }
        }
        #endregion SendMailMethods_MailMessage&SmtpConfiguration()
    }
}
