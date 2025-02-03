using Dapper;
using EchoesOfUzbekistan.Application.Abstractions.Data;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.AudioGuides.GetAudioGuide;
using EchoesOfUzbekistan.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Application.Users.GetUser;
public class GetUserQueryHandler : IQueryHandler<GetUserQuery, UserResponse>
{
    private readonly ISQLConnectionFactory _connectionFactory;

    public GetUserQueryHandler(ISQLConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Result<UserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.GetDbConnection();
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
                    WHERE id = @userId;";
        var user = await connection.QueryFirstOrDefaultAsync<UserResponse>(
            sql,
            new
            {
                request.userId
            });
        return user;
    }
}
