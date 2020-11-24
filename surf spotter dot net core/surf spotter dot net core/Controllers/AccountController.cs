using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using surf_spotter_dot_net_core.Models;
using surf_spotter_dot_net_core.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace surf_spotter_dot_net_core.Controllers
{
    //This lets swagger know that that the actions in this controller, should NOT be displayed!
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;


        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //Loads the login view
        [HttpGet]
        [Route("Login")]
        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }

        //This post request handles user logins. 
        [HttpPost, Route("Login")]
        public async Task<ActionResult> Login(LoginViewModel login, string returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(login);

            var result = await _signInManager.PasswordSignInAsync(
                login.UserName, login.Password,
                login.RememberMe, false
                );
            //If something goes wrong with passwordsigninAsync
            if (!result.Succeeded)
            {
                //Error message
                ModelState.AddModelError("", "Login error!");
                return View();
            }
            //Otherwise redirect to home
            if (string.IsNullOrWhiteSpace(returnUrl))
                return RedirectToAction("Index", "Home");

            return Redirect(returnUrl);
        }

        //Post request that handles user logout
        [HttpPost]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();

            if(string.IsNullOrWhiteSpace(returnUrl))
                return RedirectToAction("Index");

            return Redirect(returnUrl);
        }

        //Get request returning signup page.
        [HttpGet]
        [Route("Signup")]
        public IActionResult SignUp()
        {
            return View(new RegisterViewModel());
        }

        //Post request handling user signups and adding them to the identity db
        [HttpPost, Route("Signup")]
        public async Task<IActionResult> SignUp(RegisterViewModel registration)
        {
            if (!ModelState.IsValid)
                return View(registration);

            var newUser = new IdentityUser
            {
                Email = registration.Email,
                UserName = registration.UserName
            };

            var result = await _userManager.CreateAsync(newUser, registration.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors.Select(x => x.Description))
                {
                    ModelState.AddModelError("", error);
                }
                return View();
            }

            return RedirectToAction("Login");             
        }
    }
}
