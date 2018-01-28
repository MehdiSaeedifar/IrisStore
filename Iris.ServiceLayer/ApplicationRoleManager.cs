using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Iris.DataLayer;
using Iris.DomainClasses;
using Iris.ServiceLayer.Contracts;
using Microsoft.AspNet.Identity;

namespace Iris.ServiceLayer
{
    public class ApplicationRoleManager : RoleManager<CustomRole, int>, IApplicationRoleManager
    {
        #region Fields
        private readonly IUnitOfWork _uow;
        private readonly IRoleStore<CustomRole, int> _roleStore;
        private readonly IDbSet<ApplicationUser> _users;
        #endregion

        #region Constractors
        public ApplicationRoleManager(IUnitOfWork uow, IRoleStore<CustomRole, int> roleStore)
            : base(roleStore)
        {
            _uow = uow;
            _roleStore = roleStore;
            _users = _uow.Set<ApplicationUser>();
        }
        #endregion

        #region FindRoleByName
        public CustomRole FindRoleByName(string roleName)
        {
            return this.FindByName(roleName); // RoleManagerExtensions
        }
        #endregion

        #region CreateRole
        public IdentityResult CreateRole(CustomRole role)
        {
            return this.Create(role); // RoleManagerExtensions
        }
        #endregion

        #region GetCustomUsersInRole
        public IList<CustomUserRole> GetCustomUsersInRole(string roleName)
        {
            return this.Roles.Where(role => role.Name == roleName)
                             .SelectMany(role => role.Users)
                             .ToList();
            // = this.FindByName(roleName).Users
        }
        #endregion

        #region GetApplicationUsersInRole
        public IList<ApplicationUser> GetApplicationUsersInRole(string roleName)
        {
            var roleUserIdsQuery = from role in this.Roles
                                  where role.Name == roleName
                                  from user in role.Users
                                  select user.UserId;
            return _users.Where(applicationUser => roleUserIdsQuery.Contains(applicationUser.Id))
                         .ToList();
        }
        #endregion

        #region FindUserRoles
        public IList<CustomRole> FindUserRoles(int userId)
        {
            var userRolesQuery = from role in this.Roles
                        from user in role.Users
                        where user.UserId == userId
                        select role;

            return userRolesQuery.OrderBy(x => x.Name).ToList();
        }
        #endregion

        #region GetRolesForUser
        public string[] GetRolesForUser(int userId)
        {
            var roles = FindUserRoles(userId);
            if (roles == null || !roles.Any())
            {
                return new string[] { };
            }

            return roles.Select(x => x.Name).ToArray();
        }
        #endregion

        #region IsUserInRole
        public bool IsUserInRole(int userId, string roleName)
        {
            var userRolesQuery = from role in this.Roles
                        where role.Name == roleName
                        from user in role.Users
                        where user.UserId == userId
                        select role;
            var userRole = userRolesQuery.FirstOrDefault();
            return userRole != null;
        }
        #endregion

        #region GetAllCustomRolesAsync
        public Task<List<CustomRole>> GetAllCustomRolesAsync()
        {
            return this.Roles.ToListAsync();
        }
        #endregion
    }
}