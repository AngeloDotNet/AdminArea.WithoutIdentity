using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCore_SimpleLogin.Models.Services.Application;
using AspNetCore_SimpleLogin.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore_SimpleLogin.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService userService;

        public HomeController(IUserService userService)
        {
            this.userService = userService;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string returnUrl)
        {
            string passwordCifrata = userService.PasswordSalt(password);

            bool result = await userService.IsLoginCorrectAsync(username, passwordCifrata);

            if (result)
            {
                Claim nameClaim = new Claim(ClaimTypes.Name, username);
                ClaimsIdentity identity = new ClaimsIdentity(new[] { nameClaim }, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(principal);
                return LocalRedirect(returnUrl);
            }
            else
            {
                ModelState.AddModelError("Login", "I dati di login non sono esatti");
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Admin()
        {
            if (User.Identity.IsAuthenticated)
            {
                string username = User.Identity.Name;
                UserDetailViewModel user = await userService.GetUserAsync(username);

                return View(user);
            }
            else
            {
                return View();
            }
            
        }
    }
}