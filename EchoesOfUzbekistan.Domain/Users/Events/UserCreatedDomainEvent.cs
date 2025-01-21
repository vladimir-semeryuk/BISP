using EchoesOfUzbekistan.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Domain.Users.Events;
public record UserCreatedDomainEvent(Guid userId) : IDomainEvent;
