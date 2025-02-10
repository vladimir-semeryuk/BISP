using EchoesOfUzbekistan.Application.Abstractions.Auth;
using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Infrastructure.Auth.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

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

    public async Task<Result<string>> GetAccessTokenAsync(
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

            AuthorisationToken? authToken = await response
                .Content
                .ReadFromJsonAsync<AuthorisationToken>(cancellationToken);

            if (authToken is null)
            {
                return Result.Failure<string>(AuthenticationFailed);
            }

            return authToken.AccessToken;
        }
        catch (HttpRequestException)
        {
            return Result.Failure<string>(AuthenticationFailed);
        }
    }
}
