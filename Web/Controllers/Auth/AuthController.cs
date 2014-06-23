﻿using System;
using System.Web.Mvc;
using Template.Objects;
using Template.Services;

namespace Template.Controllers.Auth
{
    [AllowAnonymous]
    public class AuthController : ServicedController<IAuthService>
    {
        public AuthController(IAuthService service)
            : base(service)
        {
        }

        [HttpGet]
        public ActionResult Register()
        {
            if (Service.IsLoggedIn())
                return RedirectToDefault();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Exclude = "Id")] AuthView account)
        {
            if (Service.IsLoggedIn())
                return RedirectToDefault();

            if (!Service.CanRegister(account))
                return View(account);

            Service.Register(account);

            Service.AddSuccessfulRegistrationMessage();

            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Login(String returnUrl)
        {
            if (Service.IsLoggedIn())
                return RedirectToLocal(returnUrl);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AuthView account, String returnUrl)
        {
            if (!Service.CanLogin(account))
                return View();

            Service.Login(account);
            return RedirectToLocal(returnUrl);
        }

        [HttpGet]
        public RedirectToRouteResult Logout()
        {
            Service.Logout();

            return RedirectToAction("Login");
        }
    }
}
