using Iris.DomainClasses;
using Iris.ServiceLayer.Contracts;
using Microsoft.AspNet.Identity;

namespace Iris.ServiceLayer
{
    public class CustomRoleStore : ICustomRoleStore
    {
        #region Feilds
        private readonly IRoleStore<CustomRole, int> _roleStore;
        #endregion

        #region Constractors
        public CustomRoleStore(IRoleStore<CustomRole, int> roleStore)
        {
            _roleStore = roleStore;
        }
        #endregion
    }
}