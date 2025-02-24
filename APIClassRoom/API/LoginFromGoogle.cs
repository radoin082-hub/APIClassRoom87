using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[Microsoft.AspNetCore.Mvc.Route("Login")]
[ApiController]
public class AccountController : Controller
{
    [HttpGet("LoginGoogle")]
    public async Task Login()
    {
        var props = new AuthenticationProperties
        {
            RedirectUri = Url.Action("GoogleResponse", "Account")
        };

        await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, props);
    }
    
    [HttpGet("GoogleResponse")]
    public async Task<IActionResult> GoogleResponse()

    {
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        if (!result.Succeeded)
        {
            return RedirectToAction("Index", "dashboard");
        }

        var claims = result.Principal.Identities.FirstOrDefault()!.Claims.Select(claim => new
        {
            claim.Issuer,
            claim.OriginalIssuer,
            claim.Type,
            claim.Value
        });
        return Redirect("/");
    }

    [HttpPost]
    [Microsoft.AspNetCore.Mvc.Route("signoutGoogle")]
    public async Task<IActionResult> SignOut()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/");
    }
}