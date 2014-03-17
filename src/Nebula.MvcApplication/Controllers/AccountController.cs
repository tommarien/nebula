using System;
using System.Web.Mvc;
using Nebula.Contracts.Registration.Commands;
using Nebula.Contracts.Registration.Exceptions;
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
            try
            {
                if (ModelState.IsValid)
                {
                    var command = new LogOnUserCommand
                        {
                            UserName = model.UserName, 
                            Password = model.Password
                        };

                    mediator.Execute(command);

                    formsAuthenticationService.SignIn(model.UserName, model.RememberMe);

                    if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);

                    return RedirectToAction("Index", "Home");
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

                    mediator.Execute(command);

                    changePasswordSucceeded = true;
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