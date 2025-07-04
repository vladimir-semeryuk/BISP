﻿using EchoesOfUzbekistan.Domain.Abstractions;

namespace EchoesOfUzbekistan.Domain.Common;
public class Language : Entity
{
    public string Code { get; private set; }
    public string Name { get; private set; }

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