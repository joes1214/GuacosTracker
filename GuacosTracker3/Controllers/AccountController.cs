using Auth0.AspNetCore.Authentication;
using GuacosTracker3.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GuacosTracker3.Controllers
{
    public class AccountController : Controller
    {
        public async Task Login(string returnUrl = "/")
        {
            var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
              .WithRedirectUri(returnUrl)
              .Build();

            await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        }

        [Authorize]
        public async Task Logout()
        {
            var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
              .WithRedirectUri(Url?.Action("Index", "Home") ?? "")
              .Build();

            await HttpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [Authorize]
        public IActionResult Profile()
        {
            Account account = new();

            account.id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            account.Name = User.Identity?.Name;
            account.Email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value.ToString();
            account.Role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            account.Picture = User.Claims.FirstOrDefault(c => c.Type == "picture")?.Value;

            return View(account);
        }

        [Authorize]
        public IActionResult Claims()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
