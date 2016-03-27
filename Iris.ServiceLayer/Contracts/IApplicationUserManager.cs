using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Claims;
using System.Threading.Tasks;
using Iris.DomainClasses;
using Iris.ViewModels;
using JqGridHelper.DynamicSearch;
using JqGridHelper.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;

namespace Iris.ServiceLayer.Contracts
{
    public interface IApplicationUserManager : IDisposable
    {
        /// <summary>
        /// Used to hash/verify passwords
        /// </summary>
        IPasswordHasher PasswordHasher { get; set; }

        /// <summary>
        /// Used to validate users before changes are saved
        /// </summary>
        IIdentityValidator<ApplicationUser> UserValidator { get; set; }

        /// <summary>
        /// Used to validate passwords before persisting changes
        /// </summary>
        IIdentityValidator<string> PasswordValidator { get; set; }

        /// <summary>
        /// Used to create claims identities from users
        /// </summary>
        IClaimsIdentityFactory<ApplicationUser, int> ClaimsIdentityFactory { get; set; }

        /// <summary>
        /// Used to send email
        /// </summary>
        IIdentityMessageService EmailService { get; set; }

        /// <summary>
        /// Used to send a sms message
        /// </summary>
        IIdentityMessageService SmsService { get; set; }

        /// <summary>
        /// Used for generating reset password and confirmation tokens
        /// </summary>
        IUserTokenProvider<ApplicationUser, int> UserTokenProvider { get; set; }

        /// <summary>
        /// If true, will enable user lockout when users are created
        /// </summary>
        bool UserLockoutEnabledByDefault { get; set; }

        /// <summary>
        /// Number of access attempts allowed before a user is locked out (if lockout is enabled)
        /// </summary>
        int MaxFailedAccessAttemptsBeforeLockout { get; set; }

        /// <summary>
        /// Default amount of time that a user is locked out for after MaxFailedAccessAttemptsBeforeLockout is reached
        /// </summary>
        TimeSpan DefaultAccountLockoutTimeSpan { get; set; }

        /// <summary>
        /// Returns true if the store is an IUserTwoFactorStore
        /// </summary>
        bool SupportsUserTwoFactor { get; }

        /// <summary>
        /// Returns true if the store is an IUserPasswordStore
        /// </summary>
        bool SupportsUserPassword { get; }

        /// <summary>
        /// Returns true if the store is an IUserSecurityStore
        /// </summary>
        bool SupportsUserSecurityStamp { get; }

        /// <summary>
        /// Returns true if the store is an IUserRoleStore
        /// </summary>
        bool SupportsUserRole { get; }

        /// <summary>
        /// Returns true if the store is an IUserLoginStore
        /// </summary>
        bool SupportsUserLogin { get; }

        /// <summary>
        /// Returns true if the store is an IUserEmailStore
        /// </summary>
        bool SupportsUserEmail { get; }

        /// <summary>
        /// Returns true if the store is an IUserPhoneNumberStore
        /// </summary>
        bool SupportsUserPhoneNumber { get; }

        /// <summary>
        /// Returns true if the store is an IUserClaimStore
        /// </summary>
        bool SupportsUserClaim { get; }

        /// <summary>
        /// Returns true if the store is an IUserLockoutStore
        /// </summary>
        bool SupportsUserLockout { get; }

        /// <summary>
        /// Returns true if the store is an IQueryableUserStore
        /// </summary>
        bool SupportsQueryableUsers { get; }

        /// <summary>
        /// Maps the registered two-factor authentication providers for users by their id
        /// </summary>
        IDictionary<string, IUserTokenProvider<ApplicationUser, int>> TwoFactorProviders { get; }

        /// <summary>
        /// Creates a ClaimsIdentity representing the user
        /// </summary>
        /// <param name="user"/><param name="authenticationType"/>
        /// <returns/>
        Task<ClaimsIdentity> CreateIdentityAsync(ApplicationUser user, string authenticationType);

        /// <summary>
        /// Create a user with no password
        /// </summary>
        /// <param name="user"/>
        /// <returns/>
        Task<IdentityResult> CreateAsync(ApplicationUser user);

        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="user"/>
        /// <returns/>
        Task<IdentityResult> UpdateAsync(ApplicationUser user);

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="user"/>
        /// <returns/>
        Task<IdentityResult> DeleteAsync(ApplicationUser user);

        /// <summary>
        /// Find a user by id
        /// </summary>
        /// <param name="userId"/>
        /// <returns/>
        Task<ApplicationUser> FindByIdAsync(int userId);

        /// <summary>
        /// Find a user by user name
        /// </summary>
        /// <param name="userName"/>
        /// <returns/>
        Task<ApplicationUser> FindByNameAsync(string userName);

        /// <summary>
        /// Create a user with the given password
        /// </summary>
        /// <param name="user"/><param name="password"/>
        /// <returns/>
        Task<IdentityResult> CreateAsync(ApplicationUser user, string password);

        /// <summary>
        /// Return a user with the specified username and password or null if there is no match.
        /// </summary>
        /// <param name="userName"/><param name="password"/>
        /// <returns/>
        Task<ApplicationUser> FindAsync(string userName, string password);

        /// <summary>
        /// Returns true if the password is valid for the user
        /// </summary>
        /// <param name="user"/><param name="password"/>
        /// <returns/>
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);

        /// <summary>
        /// Returns true if the user has a password
        /// </summary>
        /// <param name="userId"/>
        /// <returns/>
        Task<bool> HasPasswordAsync(int userId);

        /// <summary>
        /// Add a user password only if one does not already exist
        /// </summary>
        /// <param name="userId"/><param name="password"/>
        /// <returns/>
        Task<IdentityResult> AddPasswordAsync(int userId, string password);

        /// <summary>
        /// Change a user password
        /// </summary>
        /// <param name="userId"/><param name="currentPassword"/><param name="newPassword"/>
        /// <returns/>
        Task<IdentityResult> ChangePasswordAsync(int userId, string currentPassword, string newPassword);

        /// <summary>
        /// Remove a user's password
        /// </summary>
        /// <param name="userId"/>
        /// <returns/>
        Task<IdentityResult> RemovePasswordAsync(int userId);

        /// <summary>
        /// Returns the current security stamp for a user
        /// </summary>
        /// <param name="userId"/>
        /// <returns/>
        Task<string> GetSecurityStampAsync(int userId);

        /// <summary>
        /// Generate a new security stamp for a user, used for SignOutEverywhere functionality
        /// </summary>
        /// <param name="userId"/>
        /// <returns/>
        Task<IdentityResult> UpdateSecurityStampAsync(int userId);

        /// <summary>
        /// Generate a password reset token for the user using the UserTokenProvider
        /// </summary>
        /// <param name="userId"/>
        /// <returns/>
        Task<string> GeneratePasswordResetTokenAsync(int userId);

        /// <summary>
        /// Reset a user's password using a reset password token
        /// </summary>
        /// <param name="userId"/><param name="token"/><param name="newPassword"/>
        /// <returns/>
        Task<IdentityResult> ResetPasswordAsync(int userId, string token, string newPassword);

        /// <summary>
        /// Returns the user associated with this login
        /// </summary>
        /// <returns/>
        Task<ApplicationUser> FindAsync(UserLoginInfo login);

        /// <summary>
        /// Remove a user login
        /// </summary>
        /// <param name="userId"/><param name="login"/>
        /// <returns/>
        Task<IdentityResult> RemoveLoginAsync(int userId, UserLoginInfo login);

        /// <summary>
        /// Associate a login with a user
        /// </summary>
        /// <param name="userId"/><param name="login"/>
        /// <returns/>
        Task<IdentityResult> AddLoginAsync(int userId, UserLoginInfo login);

        /// <summary>
        /// Gets the logins for a user.
        /// </summary>
        /// <param name="userId"/>
        /// <returns/>
        Task<IList<UserLoginInfo>> GetLoginsAsync(int userId);

        /// <summary>
        /// Add a user claim
        /// </summary>
        /// <param name="userId"/><param name="claim"/>
        /// <returns/>
        Task<IdentityResult> AddClaimAsync(int userId, Claim claim);

        /// <summary>
        /// Remove a user claim
        /// </summary>
        /// <param name="userId"/><param name="claim"/>
        /// <returns/>
        Task<IdentityResult> RemoveClaimAsync(int userId, Claim claim);

        /// <summary>
        /// Get a users's claims
        /// </summary>
        /// <param name="userId"/>
        /// <returns/>
        Task<IList<Claim>> GetClaimsAsync(int userId);

        /// <summary>
        /// Add a user to a role
        /// </summary>
        /// <param name="userId"/><param name="role"/>
        /// <returns/>
        Task<IdentityResult> AddToRoleAsync(int userId, string role);

        /// <summary>
        /// Method to add user to multiple roles
        /// </summary>
        /// <param name="userId">user id</param><param name="roles">list of role names</param>
        /// <returns/>
        Task<IdentityResult> AddToRolesAsync(int userId, params string[] roles);

        /// <summary>
        /// Remove user from multiple roles
        /// </summary>
        /// <param name="userId">user id</param><param name="roles">list of role names</param>
        /// <returns/>
        Task<IdentityResult> RemoveFromRolesAsync(int userId, params string[] roles);

        /// <summary>
        /// Remove a user from a role.
        /// </summary>
        /// <param name="userId"/><param name="role"/>
        /// <returns/>
        Task<IdentityResult> RemoveFromRoleAsync(int userId, string role);

        /// <summary>
        /// Returns the roles for the user
        /// </summary>
        /// <param name="userId"/>
        /// <returns/>
        Task<IList<string>> GetRolesAsync(int userId);

        /// <summary>
        /// Returns true if the user is in the specified role
        /// </summary>
        /// <param name="userId"/><param name="role"/>
        /// <returns/>
        Task<bool> IsInRoleAsync(int userId, string role);

        /// <summary>
        /// Get a user's email
        /// </summary>
        /// <param name="userId"/>
        /// <returns/>
        Task<string> GetEmailAsync(int userId);

        /// <summary>
        /// Set a user's email
        /// </summary>
        /// <param name="userId"/><param name="email"/>
        /// <returns/>
        Task<IdentityResult> SetEmailAsync(int userId, string email);

        /// <summary>
        /// Find a user by his email
        /// </summary>
        /// <param name="email"/>
        /// <returns/>
        Task<ApplicationUser> FindByEmailAsync(string email);

        /// <summary>
        /// Get the email confirmation token for the user
        /// </summary>
        /// <param name="userId"/>
        /// <returns/>
        Task<string> GenerateEmailConfirmationTokenAsync(int userId);

        /// <summary>
        /// Confirm the user's email with confirmation token
        /// </summary>
        /// <param name="userId"/><param name="token"/>
        /// <returns/>
        Task<IdentityResult> ConfirmEmailAsync(int userId, string token);

        /// <summary>
        /// Returns true if the user's email has been confirmed
        /// </summary>
        /// <param name="userId"/>
        /// <returns/>
        Task<bool> IsEmailConfirmedAsync(int userId);

        /// <summary>
        /// Get a user's phoneNumber
        /// </summary>
        /// <param name="userId"/>
        /// <returns/>
        Task<string> GetPhoneNumberAsync(int userId);

        /// <summary>
        /// Set a user's phoneNumber
        /// </summary>
        /// <param name="userId"/><param name="phoneNumber"/>
        /// <returns/>
        Task<IdentityResult> SetPhoneNumberAsync(int userId, string phoneNumber);

        /// <summary>
        /// Set a user's phoneNumber with the verification token
        /// </summary>
        /// <param name="userId"/><param name="phoneNumber"/><param name="token"/>
        /// <returns/>
        Task<IdentityResult> ChangePhoneNumberAsync(int userId, string phoneNumber, string token);

        /// <summary>
        /// Returns true if the user's phone number has been confirmed
        /// </summary>
        /// <param name="userId"/>
        /// <returns/>
        Task<bool> IsPhoneNumberConfirmedAsync(int userId);

        /// <summary>
        /// Generate a code that the user can use to change their phone number to a specific number
        /// </summary>
        /// <param name="userId"/><param name="phoneNumber"/>
        /// <returns/>
        Task<string> GenerateChangePhoneNumberTokenAsync(int userId, string phoneNumber);

        /// <summary>
        /// Verify the code is valid for a specific user and for a specific phone number
        /// </summary>
        /// <param name="userId"/><param name="token"/><param name="phoneNumber"/>
        /// <returns/>
        Task<bool> VerifyChangePhoneNumberTokenAsync(int userId, string token, string phoneNumber);

        /// <summary>
        /// Verify a user token with the specified purpose
        /// </summary>
        /// <param name="userId"/><param name="purpose"/><param name="token"/>
        /// <returns/>
        Task<bool> VerifyUserTokenAsync(int userId, string purpose, string token);

        /// <summary>
        /// Get a user token for a specific purpose
        /// </summary>
        /// <param name="purpose"/><param name="userId"/>
        /// <returns/>
        Task<string> GenerateUserTokenAsync(string purpose, int userId);

        /// <summary>
        /// Register a two factor authentication provider with the TwoFactorProviders mapping
        /// </summary>
        /// <param name="twoFactorProvider"/><param name="provider"/>
        void RegisterTwoFactorProvider(string twoFactorProvider, IUserTokenProvider<ApplicationUser, int> provider);

        /// <summary>
        /// Returns a list of valid two factor providers for a user
        /// </summary>
        /// <param name="userId"/>
        /// <returns/>
        Task<IList<string>> GetValidTwoFactorProvidersAsync(int userId);

        /// <summary>
        /// Verify a two factor token with the specified provider
        /// </summary>
        /// <param name="userId"/><param name="twoFactorProvider"/><param name="token"/>
        /// <returns/>
        Task<bool> VerifyTwoFactorTokenAsync(int userId, string twoFactorProvider, string token);

        /// <summary>
        /// Get a token for a specific two factor provider
        /// </summary>
        /// <param name="userId"/><param name="twoFactorProvider"/>
        /// <returns/>
        Task<string> GenerateTwoFactorTokenAsync(int userId, string twoFactorProvider);

        /// <summary>
        /// Notify a user with a token using a specific two-factor authentication provider's Notify method
        /// </summary>
        /// <param name="userId"/><param name="twoFactorProvider"/><param name="token"/>
        /// <returns/>
        Task<IdentityResult> NotifyTwoFactorTokenAsync(int userId, string twoFactorProvider, string token);

        /// <summary>
        /// Get whether two factor authentication is enabled for a user
        /// </summary>
        /// <param name="userId"/>
        /// <returns/>
        Task<bool> GetTwoFactorEnabledAsync(int userId);

        /// <summary>
        /// Set whether a user has two factor authentication enabled
        /// </summary>
        /// <param name="userId"/><param name="enabled"/>
        /// <returns/>
        Task<IdentityResult> SetTwoFactorEnabledAsync(int userId, bool enabled);

        /// <summary>
        /// Send an email to the user
        /// </summary>
        /// <param name="userId"/><param name="subject"/><param name="body"/>
        /// <returns/>
        Task SendEmailAsync(int userId, string subject, string body);

        /// <summary>
        /// Send a user a sms message
        /// </summary>
        /// <param name="userId"/><param name="message"/>
        /// <returns/>
        Task SendSmsAsync(int userId, string message);

        /// <summary>
        /// Returns true if the user is locked out
        /// </summary>
        /// <param name="userId"/>
        /// <returns/>
        Task<bool> IsLockedOutAsync(int userId);

        /// <summary>
        /// Sets whether lockout is enabled for this user
        /// </summary>
        /// <param name="userId"/><param name="enabled"/>
        /// <returns/>
        Task<IdentityResult> SetLockoutEnabledAsync(int userId, bool enabled);

        /// <summary>
        /// Returns whether lockout is enabled for the user
        /// </summary>
        /// <param name="userId"/>
        /// <returns/>
        Task<bool> GetLockoutEnabledAsync(int userId);

        /// <summary>
        /// Returns when the user is no longer locked out, dates in the past are considered as not being locked out
        /// </summary>
        /// <param name="userId"/>
        /// <returns/>
        Task<DateTimeOffset> GetLockoutEndDateAsync(int userId);

        /// <summary>
        /// Sets the when a user lockout ends
        /// </summary>
        /// <param name="userId"/><param name="lockoutEnd"/>
        /// <returns/>
        Task<IdentityResult> SetLockoutEndDateAsync(int userId, DateTimeOffset lockoutEnd);

        /// <summary>
        /// Increments the access failed count for the user and if the failed access account is greater than or equal
        ///             to the MaxFailedAccessAttempsBeforeLockout, the user will be locked out for the next DefaultAccountLockoutTimeSpan
        ///             and the AccessFailedCount will be reset to 0. This is used for locking out the user account.
        /// </summary>
        /// <param name="userId"/>
        /// <returns/>
        Task<IdentityResult> AccessFailedAsync(int userId);

        /// <summary>
        /// Resets the access failed count for the user to 0
        /// </summary>
        /// <param name="userId"/>
        /// <returns/>
        Task<IdentityResult> ResetAccessFailedCountAsync(int userId);

        /// <summary>
        /// Returns the number of failed access attempts for the user
        /// </summary>
        /// <param name="userId"/>
        /// <returns/>
        Task<int> GetAccessFailedCountAsync(int userId);



        // Our new custom methods

        Func<CookieValidateIdentityContext, Task> OnValidateIdentity();
        Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUser applicationUser);
        Task<bool> HasPassword(int userId);
        Task<bool> HasPhoneNumber(int userId);
        void SeedDatabase();
        Task<List<ApplicationUser>> GetAllUsersAsync();
        ApplicationUser FindById(int userId);
        ApplicationUser GetCurrentUser();
        Task<ApplicationUser> GetCurrentUserAsync();
        int GetCurrentUserId();

        Task<DataGridViewModel<UserDataGridViewModel>> GetDataGridSource(string orderBy, JqGridRequest request, NameValueCollection form,
            DateTimeType dateTimeType, int page, int pageSize);
    }
}