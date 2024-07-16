namespace SignalRSingleWithMySql.Services
{
    using System;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using SignalRSingleWithMySql.Database.Models;
    using SignalRSingleWithMySql.Database;
    using Newtonsoft.Json;

    public class UserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ChatContext _context;

        public UserService(ChatContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> RegisterUserAsync(string username, string password)
        {
            // Check if username already exists
            if (await _context.Users.AnyAsync(u => u.Username == username))
            {
                return false;
            }

            // Hash password and save user
            var hashedPassword = HashPassword(password);
            var user = new User { Username = username, Password = hashedPassword };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var hashedPassword = HashPassword(password);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == hashedPassword);
            // Access HttpContext using _httpContextAccessor.HttpContext
            //var httpContext = _httpContextAccessor.HttpContext;
            ////if (httpContext != null && httpContext.Session != null)
            ////{
            ////    // Access session state
            ////    //var username = httpContext.Session.GetString("Username");
            ////    // Use the retrieved username as needed
            ////}
            //// Store username in session
            //httpContext.Session.SetString("Username", user.Username);
            //var userJson = JsonConvert.SerializeObject(user);
            //httpContext.Session.SetString("User", userJson);
            return user;
        }
        public void YourMethod()
        {
            // Access HttpContext using _httpContextAccessor.HttpContext
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null && httpContext.Session != null)
            {
                // Access session state
                var username = httpContext.Session.GetString("Username");
                // Use the retrieved username as needed
            }
        }
        private string HashPassword(string password)
        {
            // Implement your password hashing logic here (e.g., using bcrypt)
            return password;
        }
    }

}
