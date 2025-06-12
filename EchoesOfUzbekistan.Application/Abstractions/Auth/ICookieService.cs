namespace EchoesOfUzbekistan.Application.Abstractions.Auth;
public interface ICookieService
{
    void SetAccessToken(string accessToken, int expiresIn);
    void SetRefreshToken(string refreshToken, int expiresIn);
    string? GetRefreshToken();
    void ClearTokenCookies();
}
