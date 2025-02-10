﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Infrastructure.Auth;
internal class KeycloakOptions
{
    public string AdminUrl { get; set; } = string.Empty;

    public string TokenUrl { get; set; } = string.Empty;

    public string AdminClientId { get; init; } = string.Empty;

    public string AdminClientSecret { get; init; } = string.Empty;

    public string AuthClientId { get; init; } = string.Empty;

    public string AuthClientSecret { get; init; } = string.Empty;
}
