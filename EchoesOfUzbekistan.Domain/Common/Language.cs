using EchoesOfUzbekistan.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Domain.Common;
public class Language : Entity
{
    public string Code { get; private set; } // ISO Code (e.g., "en", "ru", "es")
    public string Name { get; private set; } // Display Name (e.g., "English", "Русский")

    private Language() { }

    public Language(Guid id, string code, string name) : base(id)
    {
        if (string.IsNullOrWhiteSpace(code)) throw new ArgumentNullException(nameof(code));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

        Code = code.ToLowerInvariant();
        Name = name;
    }

    public override bool Equals(object? obj) => obj is Language other && Code == other.Code;
    public override int GetHashCode() => Code.GetHashCode();
}