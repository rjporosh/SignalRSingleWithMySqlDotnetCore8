using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SignalRSingleWithMySql.Services;

namespace SignalRSingleWithMySql.Pages.Register
{
    public class RegisterModel : PageModel
    {
        private readonly UserService _userService;

        public RegisterModel(UserService userService)
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

            // Attempt to register user
            var result = await _userService.RegisterUserAsync(Username, Password);
            if (result)
            {
                // Redirect to login page after successful registration
                return RedirectToPage("/Login");
            }
            else
            {
                // Show error message if registration fails
                ModelState.AddModelError(string.Empty, "Username already exists.");
                return Page();
            }
        }
    }
}
