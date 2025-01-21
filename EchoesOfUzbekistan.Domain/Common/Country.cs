using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Domain.Common;
public record Country(string Name, string IsoCode)
{
    public static Country Create(string name, string isoCode)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Country name cannot be empty.");

        if (string.IsNullOrWhiteSpace(isoCode) || isoCode.Length != 2)
            throw new ArgumentException("ISO code must be a valid 2-letter code.");

        return new Country(name, isoCode.ToUpper());
    }
}
