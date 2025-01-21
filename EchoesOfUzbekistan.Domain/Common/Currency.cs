﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Domain.Common;
public record Currency
{
    internal static readonly Currency None = new("");
    public static readonly Currency Usd = new("USD");
    public static readonly Currency Eur = new("EUR");
    public static readonly Currency Uzs = new("UZS");
    public static readonly Currency Rub = new("RUB");

    private Currency(string code) => Code = code;

    public string Code { get; init; }

    public static Currency FromCode(string code)
    {
        return All.FirstOrDefault(c => c.Code == code) ??
               throw new ArgumentException("The currency code is invalid");
    }

    public static readonly IReadOnlyCollection<Currency> All = new[]
    {
        Usd,
        Eur,
        Uzs,
        Rub
    };
}