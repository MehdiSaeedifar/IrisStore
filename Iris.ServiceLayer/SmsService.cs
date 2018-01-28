﻿using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Iris.ServiceLayer
{
    public class SmsService : IIdentityMessageService
    {
        #region SendAsync
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your sms service here to send a text message.
            return Task.FromResult(0);
        }
        #endregion
    }
}
