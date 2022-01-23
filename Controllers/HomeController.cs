using Authentication_and_authorization.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Authentication_and_authorization.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [Authorize(Roles ="Admin")]
        public IActionResult Secured()
        {
            return View();
        }
        [HttpGet("/Login")]
        public IActionResult Login(string returnUrl )
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost("/Login")]
        public async Task<IActionResult> Validate(string userName, string password ,string ReturnUrl)
        {
            if (userName == "bob" && password == "piza")
            {
                var claims = new List<Claim>();
                claims.Add(new Claim("userName", userName));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userName));
                claims.Add(new Claim(ClaimTypes.Name, "Edward John"));
                var identity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(claimsPrincipal);
                return Redirect(ReturnUrl);
            }
            TempData["error"] = "Error Invalid username or password";
            return Redirect("Login?ReturnUrl="+ReturnUrl);
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");

        }
        [HttpGet("Denied")]
        public IActionResult AccessDenied()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}