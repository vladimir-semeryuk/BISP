using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Infrastructure.Auth;
internal class JwtBearerOptionsConfiguration : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly AuthJwtBearerOptions _jwtBearerOptions;

    public JwtBearerOptionsConfiguration(IOptions<AuthJwtBearerOptions> jwtBearerOptions)
    {
        _jwtBearerOptions = jwtBearerOptions.Value;
    }

    public void Configure(string? name, JwtBearerOptions options)
    {
        Configure(options);
    }

    public void Configure(JwtBearerOptions options)
    {
        options.Audience = _jwtBearerOptions.Audience;
        options.MetadataAddress = _jwtBearerOptions.MetadataUrl;
        options.RequireHttpsMetadata = _jwtBearerOptions.RequireHttpsMetadata;
        options.TokenValidationParameters.ValidIssuer = _jwtBearerOptions.Issuer;
    }
}
