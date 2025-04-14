using BlogDemoMvc.Models;
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

        public IActionResult Login()
        {
            LoginRequestViewModel obj = new LoginRequestViewModel();
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestViewModel obj)
        {
            LoginResponseViewModel objResponse = new LoginResponseViewModel();
            objResponse=await _apiService.AuthenticateUser(obj);

            if (objResponse != null && !string.IsNullOrEmpty(objResponse.Token))
            {
                HttpContext.Session.SetString("APIToken", objResponse.Token);
                HttpContext.Session.SetString("UserFirstName", objResponse.UserDetails.FirstName ?? "");
                return RedirectToAction("Index","Post");
            }
            else
            {
                HttpContext.Session.SetString("APIToken", "");
            }
            return View(objResponse);
        }
        public async Task <IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString("APIToken", "");
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

    }
}
