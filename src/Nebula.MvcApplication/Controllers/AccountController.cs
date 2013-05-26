using System;
using System.Web.Mvc;
using Nebula.Contracts.Registration.Commands;
using Nebula.Contracts.Registration.Exceptions;
using Nebula.Infrastructure.Commanding;
using Nebula.Infrastructure.Commanding.CommandResults;
using Nebula.Infrastructure.Querying;
using Nebula.MvcApplication.Models;
using Nebula.MvcApplication.Services;

namespace Nebula.MvcApplication.Controllers
{
    public class AccountController : CQSController
    {
        private readonly IFormsAuthenticationService formsAuthenticationService;

        public AccountController(ICommandBus commandBus, IQueryHandlerFactory queryHandlerFactory,
                                 IFormsAuthenticationService formsAuthenticationService)
            : base(commandBus, queryHandlerFactory)
        {
            this.formsAuthenticationService = formsAuthenticationService;
        }

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
                    var command = new LogOnUserCommand {UserName = model.UserName, Password = model.Password};
                    Send(command);


                    formsAuthenticationService.SignIn(model.UserName, model.RememberMe);

                    return Url.IsLocalUrl(returnUrl)
                               ? (ActionResult) Redirect(returnUrl)
                               : RedirectToAction("Index", "Home");
                }
            }
            catch (AuthenticationFailedException)
            {
                ModelState.AddModelError(string.Empty, "The user name or password provided is incorrect.");
            }
            catch (InactiveAccountException)
            {
                ModelState.AddModelError(string.Empty, "The account has been deactivated, contact the administrator.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult LogOff()
        {
            formsAuthenticationService.SignOut();

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult MyProfile()
        {
            return View();
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
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

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                bool changePasswordSucceeded;
                try
                {
                    var command = new ChangePasswordCommand
                        {
                            UserName = User.Identity.Name,
                            OldPassword = model.OldPassword,
                            NewPassword = model.NewPassword
                        };

                    changePasswordSucceeded = SendAndReply<ChangePasswordCommand, OperationResult>(command);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                    return RedirectToAction("ChangePasswordSuccess");

                ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }
    }
}