using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using E_shop.Models;
using Calabonga.Mvc.Extensions;

namespace E_shop.Controllers
{
    public class AccountController : BaseController
    {
        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Membership.ValidateUser(model.UserName, model.Password))
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", Resources.GlobalString.AccountErrorForNameOrPass);
                    }
                }

                return View(model);
            }
            catch (Exception) { return RedirectToAction("ErrorPage", "Home"); }
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            Session.Remove( Resources.GlobalString.SESSION_CART);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Captcher()]
        public ActionResult Register(RegisterModel model, string Phone)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MembershipCreateStatus createStatus;
                    Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

                    if (createStatus == MembershipCreateStatus.Success)
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, false);

                        if (!Roles.IsUserInRole(model.UserName, Constants.ROLE_USER))
                            Roles.AddUserToRole(model.UserName, Constants.ROLE_USER);

                        UserProfile profile = new UserProfile(model.UserName);
                        profile.Phone = Phone;
                        profile.Save();

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", ErrorCodeToString(createStatus));
                    }
                }

                return View(model);
            }
            catch (Exception) { return RedirectToAction("Index", "Home"); }
        }

        public ActionResult Captcha()
        {
            return new CaptchaResult();
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool changePasswordSucceeded;
                    try
                    {
                        MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                        changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("ChangePasswordSuccess");
                    }
                    else
                    {
                        ModelState.AddModelError("", Resources.GlobalString.AccountErrorNowPassword);
                    }
                }
                return View(model);
            }
            catch (Exception) { return RedirectToAction("Index", "Home"); }
        }

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // Полный список кодов состояния см. по адресу http://go.microsoft.com/fwlink/?LinkID=177550

            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return Resources.AccountRes.DuplicateUserName;

                case MembershipCreateStatus.DuplicateEmail:
                    return Resources.AccountRes.DuplicateEmail;

                case MembershipCreateStatus.InvalidPassword:
                    return Resources.AccountRes.InvalidPassword;

                case MembershipCreateStatus.InvalidEmail:
                    return Resources.AccountRes.InvalidEmail;

                case MembershipCreateStatus.InvalidAnswer:
                    return Resources.AccountRes.InvalidAnswer;

                case MembershipCreateStatus.InvalidQuestion:
                    return Resources.AccountRes.InvalidQuestion;

                case MembershipCreateStatus.InvalidUserName:
                    return Resources.AccountRes.InvalidUserName;

                case MembershipCreateStatus.ProviderError:
                    return Resources.AccountRes.ProviderError;

                case MembershipCreateStatus.UserRejected:
                    return Resources.AccountRes.UserRejected;

                default:
                    return Resources.AccountRes.DefaultError;
            }
        }
        #endregion
    }
}
