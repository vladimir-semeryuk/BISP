using EchoesOfUzbekistan.Application.Abstractions.Auth;
using Microsoft.AspNetCore.Http;

namespace EchoesOfUzbekistan.Infrastructure.Auth;
public class CookieService : ICookieService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CookieService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void SetAccessToken(string accessToken, int expiresIn)
    {
        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddSeconds(expiresIn)
        };
        _httpContextAccessor.HttpContext.Response.Cookies.Append("access_token", accessToken, options);
    }

    public string? GetRefreshToken()
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        return request?.Cookies["refresh_token"];
    }

    public void SetRefreshToken(string refreshToken, int expiresIn)
    {
        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddSeconds(expiresIn)
        };
        _httpContextAccessor.HttpContext.Response.Cookies.Append("refresh_token", refreshToken, options);
    }

    public void ClearTokenCookies()
    {
        var response = _httpContextAccessor.HttpContext?.Response;

        if (response is null)
            return;

        response.Cookies.Delete("access_token");
        response.Cookies.Delete("refresh_token");
    }
}
