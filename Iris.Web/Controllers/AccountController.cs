using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;
using Iris.DataLayer;
using Iris.DomainClasses;
using Iris.ServiceLayer.Contracts;
using Iris.Web.ViewModels.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Iris.Web.Controllers
{
    [RoutePrefix("Account")]
    public partial class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationUserManager _userManager;
        private readonly IApplicationSignInManager _signInManager;
        private readonly IAuthenticationManager _authenticationManager;

        public AccountController(IUnitOfWork unitOfWork, IApplicationUserManager userManager, IApplicationSignInManager applicationSignInManager,
            IAuthenticationManager authenticationManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = applicationSignInManager;
            _authenticationManager = authenticationManager;
        }

        [Route("Login")]
        [AllowAnonymous]
        public virtual ActionResult Login(string returnUrl, bool isUser = false)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (!isUser)
                return View();

            return View("UserLogin");
        }

        [Route("Login")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Login(LoginViewModel model, string returnUrl, bool isUser = false)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = SignInStatus.Failure;

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, shouldLockout: true);
            }

            switch (result)
            {
                case SignInStatus.Success:
                    return redirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
                default:

                    ModelState.AddModelError("", "نام کاربری یا کلمه عبور اشتباه است");
                    if (!isUser)
                        return View(model);
                    else
                        return View("UserLogin", model);
            }
        }

        [Route("LogOff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> LogOff()
        {
            var userId = User.Identity.GetUserId<int>();
            _authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            await _userManager.UpdateSecurityStampAsync(userId);

            return RedirectToAction((string)MVC.Account.ActionNames.Login, (string)MVC.Account.Name);
        }

        [Route("ResetPassword")]
        [AllowAnonymous]
        public virtual ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        [Route("ResetPassword")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await _userManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            addErrors(result);
            return View();
        }

        [Route("Register")]
        [AllowAnonymous]
        public virtual ActionResult Register(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [Route("Register")]
        [HttpPost]
        public virtual async Task<ActionResult> Register(RegisterViewModel userViewModel, string returnUrl)
        {

            if (!ModelState.IsValid)
            {
                return View(userViewModel);
            }


            var user = new ApplicationUser
            {
                UserName = userViewModel.UserName,
                Email = userViewModel.Email,
                EmailConfirmed = true
            };

            var adminresult = await _userManager.CreateAsync(user, userViewModel.Password);

            if (adminresult.Succeeded)
            {
                var result = await _userManager.AddToRolesAsync(user.Id, "User");
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", adminresult.Errors.First());

                return View();
            }

            await _signInManager.PasswordSignInAsync(userViewModel.UserName, userViewModel.Password, false, shouldLockout: false);


            return redirectToLocal(returnUrl);
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public virtual ActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        [Route("ForgotPassword")]
        [AllowAnonymous]
        public virtual ActionResult ForgotPassword()
        {
            return View();
        }


        [Route("ForgotPassword")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account",
                    new { userId = user.Id, code }, protocol: Request.Url.Scheme);
                await _userManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
                ViewBag.Link = callbackUrl;
                return View("ForgotPasswordConfirmation");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public virtual ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [Route("ChangePassword")]
        public virtual ActionResult ChangePassword()
        {
            return View();
        }

        [Route("ChangeUserPassword")]
        public virtual ActionResult ChangeUserPassword()
        {
            return View("ChangeUserPassword");
        }

        [Route("ChangeUserPassword")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> ChangeUserPassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ChangeUserPassword", model);
            }
            var result = await _userManager.ChangePasswordAsync(_userManager.GetCurrentUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await _userManager.GetCurrentUserAsync();
                if (user != null)
                {
                    await signInAsync(user, isPersistent: false);
                }
                TempData["message"] = "کلمه عبور با موفقیت ویرایش شد!";
                return RedirectToAction("Index", "Home", routeValues: new { Area = "User" });
            }
            addErrors(result);
            return View("ChangeUserPassword", model);
        }

        [Route("ChangePassword")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _userManager.ChangePasswordAsync(_userManager.GetCurrentUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await _userManager.GetCurrentUserAsync();
                if (user != null)
                {
                    await signInAsync(user, isPersistent: false);
                }
                TempData["message"] = "کلمه عبور با موفقیت ویرایش شد";
            }
            addErrors(result);
            return View(model);
        }


        private void addErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult redirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(MVC.AdminPanel.Dashboard.ActionNames.Index, MVC.AdminPanel.Dashboard.Name, new { area = MVC.AdminPanel.Name });
        }

        private async Task signInAsync(ApplicationUser user, bool isPersistent)
        {
            _authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            _authenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent },
                await _userManager.GenerateUserIdentityAsync(user));
        }


    }
}