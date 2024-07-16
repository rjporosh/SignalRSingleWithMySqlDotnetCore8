using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SignalRSingleWithMySql.Services;
using System.Security.Claims;
using Newtonsoft.Json;

namespace SignalRSingleWithMySql.Pages
{
    public class LoginModel : PageModel
    {
        private readonly UserService _userService;

        public LoginModel(UserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Attempt to authenticate user
            var user = await _userService.AuthenticateAsync(Username, Password);
            if (user != null)
            {
                // Create claims for authenticated user
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };

                // Sign in user
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                //// Store username in session
                //HttpContext.Session.SetString("Username", user.Username);
                //var userJson = JsonConvert.SerializeObject(user);
                //HttpContext.Session.SetString("User", userJson);
                var userJson = JsonConvert.SerializeObject(user);

                // Store the JSON string in TempData
                TempData["UserJson"] = userJson;
                TempData["Username"] = user.Username;
                TempData["Userid"] = user.UserId;

                // Redirect to chat page after successful login
                return RedirectToPage("/Chat");
            }
            else
            {
                // Show error message if login fails
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return Page();
            }
        }
    }
}
