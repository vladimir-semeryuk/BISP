namespace EchoesOfUzbekistan.Domain.Common;
public record Country(string Name, string IsoCode)
{
    public static Country Create(string name, string isoCode)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Country name cannot be empty.");

        if (!CountryDictionary.Countries.ContainsKey(isoCode.ToUpper()))
            throw new ArgumentException("Invalid country code.");

        if (!CountryDictionary.Countries[isoCode.ToUpper()].Equals(name, StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Country name does not match the provided country code.");

        return new Country(name, isoCode.ToUpper());
    }
}
