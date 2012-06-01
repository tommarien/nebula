﻿using System.Web.Mvc;
using Nebula.Contracts.Commands.Registration;
using Nebula.Infrastructure.Commanding;
using Nebula.Infrastructure.Commanding.CommandResults;
using Nebula.MvcApplication.Models;
using Nebula.MvcApplication.Services;

namespace Nebula.MvcApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly ICommandDispatcher commandDispatcher;
        private readonly IFormsAuthenticationService formsAuthenticationService;

        public AccountController(ICommandDispatcher commandDispatcher, IFormsAuthenticationService formsAuthenticationService)
        {
            this.commandDispatcher = commandDispatcher;
            this.formsAuthenticationService = formsAuthenticationService;
        }

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var command = new LogOnUserCommand {UserName = model.UserName, Password = model.Password};
                bool result = commandDispatcher.Dispatch<LogOnUserCommand, OperationResult>(command);

                if (result)
                {
                    formsAuthenticationService.SignIn(model.UserName, model.RememberMe);
                    return Url.IsLocalUrl(returnUrl) ? (ActionResult) Redirect(returnUrl) : RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "The user name or password provided is incorrect.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            formsAuthenticationService.SignOut();

            return RedirectToAction("Index", "Home");
        }

        ////
        //// GET: /Account/Register

        //public ActionResult Register()
        //{
        //    return View();
        //}

        ////
        //// POST: /Account/Register

        //[HttpPost]
        //public ActionResult Register(RegisterModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Attempt to register the user
        //        MembershipCreateStatus createStatus;
        //        Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

        //        if (createStatus == MembershipCreateStatus.Success)
        //        {
        //            FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
        //            return RedirectToAction("Index", "Home");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", ErrorCodeToString(createStatus));
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        ////
        //// GET: /Account/ChangePassword

        //[Authorize]
        //public ActionResult ChangePassword()
        //{
        //    return View();
        //}

        ////
        //// POST: /Account/ChangePassword

        //[Authorize]
        //[HttpPost]
        //public ActionResult ChangePassword(ChangePasswordModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // ChangePassword will throw an exception rather
        //        // than return false in certain failure scenarios.
        //        bool changePasswordSucceeded;
        //        try
        //        {
        //            MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
        //            changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
        //        }
        //        catch (Exception)
        //        {
        //            changePasswordSucceeded = false;
        //        }

        //        if (changePasswordSucceeded)
        //        {
        //            return RedirectToAction("ChangePasswordSuccess");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        ////
        //// GET: /Account/ChangePasswordSuccess

        //public ActionResult ChangePasswordSuccess()
        //{
        //    return View();
        //}
    }
}