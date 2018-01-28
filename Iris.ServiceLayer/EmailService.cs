using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Iris.ServiceLayer
{
    public class EmailService : IIdentityMessageService
    {
        #region SendAsync
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
        #endregion
    }
}