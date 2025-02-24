using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using APIClassRoom.Storage;
using APIClassRoom.Model;
using Microsoft.AspNetCore.Components;

using Microsoft.JSInterop;


namespace APIClassRoom.API
{
    [Microsoft.AspNetCore.Mvc.Route("Login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserStorage _userStorage;
        private readonly IConfiguration _configuration;

        public LoginController(UserStorage userStorage, IConfiguration configuration)
        {
            _userStorage = userStorage;
            _configuration = configuration;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] string email, [FromForm] string password)
        {
            try
            {
                User? user = await _userStorage.AuthenticateUserAsync(email, password);
                if (user is null)
                {
                    return Unauthorized(new { Message = "Invalid email or password." });
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                    new Claim(ClaimTypes.Sid, user.Id.ToString()),
                    new Claim("LevelId", user.LevelId.ToString())
                };

                // ✅ Create Cookie Authentication
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var authProperties = new AuthenticationProperties();
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                    authProperties);

            
                string redirectUrl = user.Role switch
                {
                    UserRole.Teacher => "/reservation",
                    UserRole.Student => "/ClassCompensation",
                    UserRole.Admin => "/dashboard",
                    _ => "/login"
                };

                if (HttpContext.Request.Headers["Accept"].ToString().Contains("text/html"))
                {
                    return Redirect(redirectUrl);
                }

                return Ok(new
                {
                 Role = user.ToString(), Message = "Login successful.", RedirectUrl = redirectUrl
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error", Error = ex.Message });
            }
        }

        [Inject] public IJSRuntime js { set; get; }

        [HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("logout")]
    
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return Redirect("/login");
        }
      
    }
}