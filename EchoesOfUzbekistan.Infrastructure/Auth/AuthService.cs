using EchoesOfUzbekistan.Application.Abstractions.Auth;
using EchoesOfUzbekistan.Domain.Users;
using EchoesOfUzbekistan.Infrastructure.Auth.Models;
using System.Net.Http.Json;

namespace EchoesOfUzbekistan.Infrastructure.Auth;
internal class AuthService : IAuthService
{
    private const string PasswordCredentialType = "password";
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> Signup(
        User user,
        string password,
        CancellationToken cancellationToken = default)
    {
        var userRepresentationModel = UserKeycloakRepresentationModel.FromUser(user);

        userRepresentationModel.Credentials = new CredentialRepresentationModel[]
        {
            new()
            {
                Value = password,
                Temporary = false,
                Type = PasswordCredentialType
            }
        };

        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(
            "users",
            userRepresentationModel,
            cancellationToken);

        return ExtractIdentityIdFromLocationHeader(response);
    }

    public async Task ChangePasswordAsync(
        string userId,
        string newPassword,
        CancellationToken cancellationToken)
    {
        var resetPasswordPayload = new CredentialRepresentationModel
        {
            Type = PasswordCredentialType,
            Temporary = false,
            Value = newPassword
        };

        HttpResponseMessage resetResponse = await _httpClient.PutAsJsonAsync(
            $"users/{userId}/reset-password",
            resetPasswordPayload,
            cancellationToken);

        resetResponse.EnsureSuccessStatusCode();
    }

    private static string ExtractIdentityIdFromLocationHeader(
        HttpResponseMessage httpResponseMessage)
    {
        const string usersSegmentName = "users/";

        string? locationHeader = httpResponseMessage.Headers.Location?.PathAndQuery;

        if (locationHeader is null)
        {
            throw new InvalidOperationException("Location header cannot be null");
        }

        int userSegmentValueIndex = locationHeader.IndexOf(
            usersSegmentName,
            StringComparison.InvariantCultureIgnoreCase);

        string userIdentityId = locationHeader.Substring(
            userSegmentValueIndex + usersSegmentName.Length);

        return userIdentityId;
    }
}