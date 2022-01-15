using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wiki.Data;
using Wiki.Models;
using Wiki.Models.Constants;
using Wiki.ViewModels;

namespace Wiki.Controllers
{
    [Authorize]
    public class AccountsController : Controller
    {
        private readonly WikiContext _wikiContext;
        private readonly UserManager<User> _userManager;
        // private readonly RoleManager<User> _roleManager;
        private readonly SignInManager<User> _signInManager;

        public AccountsController(WikiContext wikiContext,
            UserManager<User> userManager,
            //RoleManager<User> roleManager,
            SignInManager<User> signInManager)
        {
            this._wikiContext = wikiContext;
            this._userManager = userManager;
            // this._roleManager = roleManager;
            this._signInManager = signInManager;
        }

        [HttpGet, AllowAnonymous]
        public IActionResult RegisterUser()
        {
            return View();
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> RegisterUser(Register model)
        {
            if (ModelState.IsValid)
            {
                Author user1 = new Author()
                {
                    Email = model.Email,
                    UserName = model.Email,
                    FirstName = model.Name,
                    LastName = model.LastName,
                    Phone = model.Phone,
                    CreationDate = DateTime.Now
                };

                var resultCreateUser = await _userManager.CreateAsync(user1, model.Password);

                if (resultCreateUser.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user1, Const.AuthorRoleName);

                    await _signInManager.SignInAsync(user1, isPersistent: true);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in resultCreateUser.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> EmailAvailable(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"El email {email} ya esta registrado.");
            }
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            TempData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> LogIn(Login model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    var returl = TempData["returnUrl"] as string;
                    if (!string.IsNullOrEmpty(returl))
                    {
                        return Redirect(returl);
                    }
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Inicio de sesion invalido");

                return View(model);
            }

            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet, AllowAnonymous]
        [Route("Account/AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
