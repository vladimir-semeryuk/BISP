﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Application.Users.Services;
public interface IUserContextService
{
    Guid UserId { get; }
    string IdentityId { get; }
}
