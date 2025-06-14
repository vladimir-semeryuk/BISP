﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Infrastructure.Auth.Models;
internal class AuthorisationToken
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; init; } = string.Empty;
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; init; } = string.Empty;
}
