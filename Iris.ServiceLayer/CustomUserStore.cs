using Iris.DataLayer;
using Iris.DomainClasses;
using Iris.ServiceLayer.Contracts;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Iris.ServiceLayer
{
    public class CustomUserStore :
        UserStore<ApplicationUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>,
        ICustomUserStore
    {
        #region Fields
        //private readonly IDbSet<ApplicationUser> _myUserStore;
        #endregion

        #region Constractors
        public CustomUserStore(ApplicationDbContext context)
            : base(context)
        {
            //_myUserStore = context.Set<ApplicationUser>();
        }
        #endregion

        #region FindByIdAsync
        //public override Task<ApplicationUser> FindByIdAsync(int userId)
        //{
        //   return Task.FromResult(_myUserStore.Find(userId));
        //}
        #endregion
    }
}