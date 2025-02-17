using EchoesOfUzbekistan.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Domain.Common;
public static class LanguageErrors
{
    public static readonly Error NotFound = new(
        "Language.Found",
        "The language with the specified id was not found");

    public static readonly Error NotFoundCode = new(
        "User.Found",
        "The language with the specified code was not found");
}
