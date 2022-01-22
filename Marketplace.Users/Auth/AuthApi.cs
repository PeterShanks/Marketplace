using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Users.Auth;

[ApiController]
[Route("api/auth")]
public class AuthApi : ControllerBase
{
    private readonly AuthService _authService;

    public AuthApi(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Post(Contracts.Login login)
    {
        if (!await _authService.CheckCredentials(
                login.Username, login.Password
            ))
            return Unauthorized();

        var claims = new List<Claim>
        {
            new("user", login.Password),
            new("name", login.Username),
            new("role", "Member")
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(
                new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                )
            )
        );

        return Ok();
    }
}