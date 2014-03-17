using System;
using System.Web.Mvc;
using Nebula.Contracts.Registration.Commands;
using Nebula.Infrastructure.Commanding;
using Nebula.MvcApplication.Models;
using Nebula.MvcApplication.Services;

namespace Nebula.MvcApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly IFormsAuthenticationService formsAuthenticationService;
        private readonly IMediator mediator;

        public AccountController(IMediator mediator, IFormsAuthenticationService formsAuthenticationService)
        {
            this.formsAuthenticationService = formsAuthenticationService;
            this.mediator = mediator;
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
                var command = new LogOnUserCommand
                    {
                        UserName = model.UserName,
                        Password = model.Password
                    };

                AuthenticationResult result = mediator.Execute<LogOnUserCommand, AuthenticationResult>(command);

                if (result.Success)
                {
                    formsAuthenticationService.SignIn(result, model.RememberMe);

                    if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "The user name or password provided is incorrect.");
            }


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

                    changePasswordSucceeded = mediator.Execute<ChangePasswordCommand, bool>(command);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                    return RedirectToAction("ChangePasswordSuccess");

                ModelState.AddModelError("", "Either the current password or the new password is invalid.");
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