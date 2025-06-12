using EchoesOfUzbekistan.Application.Abstractions.Auth;
using EchoesOfUzbekistan.Domain.Abstractions;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using EchoesOfUzbekistan.Application.Users.LoginUser;

namespace EchoesOfUzbekistan.Infrastructure.Auth;
internal class TokenService : ITokenService
{
    private static readonly Error AuthenticationFailed = new(
        "Keycloak.AuthenticationFailed",
        "There was an error retrieving access token due to authentication failure");

    private readonly HttpClient _httpClient;
    private readonly KeycloakOptions _keycloakOptions;

    public TokenService(HttpClient httpClient, IOptions<KeycloakOptions> keycloakOptions)
    {
        _httpClient = httpClient;
        _keycloakOptions = keycloakOptions.Value;
    }

    public async Task<Result<TokenResponse>> GetAccessTokenAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var authRequestParameters = new KeyValuePair<string, string>[]
            {
                new("client_id", _keycloakOptions.AuthClientId),
                new("client_secret", _keycloakOptions.AuthClientSecret),
                new("scope", "openid email"),
                new("grant_type", "password"),
                new("username", email),
                new("password", password)
            };

            using var authorizationRequestContent = new FormUrlEncodedContent(authRequestParameters);

            HttpResponseMessage response = await _httpClient.PostAsync(
            "",
                authorizationRequestContent,
                cancellationToken);
            response.EnsureSuccessStatusCode();

            TokenResponse? authToken = await response
                .Content
                .ReadFromJsonAsync<TokenResponse>(cancellationToken);

            if (authToken is null)
            {
                return Result.Failure<TokenResponse>(AuthenticationFailed);
            }

            return authToken;
        }
        catch (HttpRequestException)
        {
            return Result.Failure<TokenResponse>(AuthenticationFailed);
        }
    }

    public async Task<Result<TokenResponse>> RefreshAccessTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var authRequestParameters = new KeyValuePair<string, string>[]
            {
                new("client_id", _keycloakOptions.AuthClientId),
                new("client_secret", _keycloakOptions.AuthClientSecret),
                new("grant_type", "refresh_token"),
                new("refresh_token", refreshToken)
            };

            using var authorizationRequestContent = new FormUrlEncodedContent(authRequestParameters);

            HttpResponseMessage response = await _httpClient.PostAsync(
                "",
                authorizationRequestContent,
                cancellationToken);
            response.EnsureSuccessStatusCode();

            TokenResponse? authToken = await response
                .Content
                .ReadFromJsonAsync<TokenResponse>(cancellationToken);

            if (authToken is null)
            {
                return Result.Failure<TokenResponse>(AuthenticationFailed);
            }

            return authToken;
        }
        catch (HttpRequestException)
        {
            return Result.Failure<TokenResponse>(AuthenticationFailed);
        }
    }
}
