using System.Data;
using EchoesOfUzbekistan.Application.Abstractions.Auth;
using EchoesOfUzbekistan.Application.Abstractions.Data;
using EchoesOfUzbekistan.Domain.Abstractions;
using Dapper;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Users.GetUser;
using EchoesOfUzbekistan.Application.Users.Services;

namespace EchoesOfUzbekistan.Application.Users.GetLoggedInUser;

internal sealed class GetLoggedInUserQueryHandler
    : IQueryHandler<GetLoggedInUserQuery, UserResponse>
{
    private readonly ISQLConnectionFactory _sqlConnectionFactory;
    private readonly IUserContextService _userContext;

    public GetLoggedInUserQueryHandler(
        ISQLConnectionFactory sqlConnectionFactory,
        IUserContextService userContext)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _userContext = userContext;
    }

    public async Task<Result<UserResponse>> Handle(
        GetLoggedInUserQuery request,
        CancellationToken cancellationToken)
    {
        using IDbConnection connection = _sqlConnectionFactory.GetDbConnection();

        var sql = @"SELECT id AS Id,
                    first_name AS FirstName,
                    surname AS Surname,
                    email AS Email,
                    registration_date_utc AS RegistrationDateUtc, 
                    country_name AS CountryName,
                    country_iso_code AS CountryCode,
                    city AS City,
                    about_me AS AboutMe
                    FROM users
                    WHERE identity_id = @IdentityId;";

        UserResponse user = await connection.QuerySingleAsync<UserResponse>(
            sql,
            new
            {
                _userContext.IdentityId
            });

        return user;
    }
}
