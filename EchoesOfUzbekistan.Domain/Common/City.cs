namespace EchoesOfUzbekistan.Domain.Common;
public record City
{
    public string Value { get; }

    public City(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("City cannot be empty.", nameof(value));

        Value = value.Trim();
    }

    public override string ToString() => Value;
}
