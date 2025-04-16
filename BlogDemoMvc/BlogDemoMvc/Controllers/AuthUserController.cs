using BlogDemoMvc.Models.User;
using BlogDemoMvc.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.AccessControl;
using System.Security.Claims;

namespace BlogDemoMvc.Controllers
{
    public class AuthUserController : Controller
    {

        private readonly ApiUserService _apiService;
        private string baseURL = "https://localhost:7278/";

        public AuthUserController(ApiUserService apiService)
        {
            _apiService = apiService;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool result = await _apiService.RegisterUser(model);
            if (result)
                return RedirectToAction("Index","Post");
            else
                return View("ErrorPage","Post");
        }

        public IActionResult Login()
        {
            LoginRequestViewModel obj = new LoginRequestViewModel();
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestViewModel obj)
        {
            if (!ModelState.IsValid)
            {
                return View(obj); 
            }

            var objResponse = await _apiService.AuthenticateUser(obj);

            if (objResponse != null && !string.IsNullOrEmpty(objResponse.Token))
            {
                HttpContext.Session.SetString("APIToken", objResponse.Token);
                HttpContext.Session.SetString("UserFirstName", objResponse.UserDetails.FirstName ?? "");
                return RedirectToAction("Index", "Post");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password."); 
                return View(obj);
            }
        }
        public async Task <IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString("APIToken", "");
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Post");
        }

    }
}
