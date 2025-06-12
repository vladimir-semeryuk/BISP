namespace EchoesOfUzbekistan.Domain.Users;
public sealed record FirstName
{
    public string Value { get; }

    public FirstName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("First name cannot be empty.", nameof(value));

        Value = value.Trim();
    }

    public override string ToString() => Value;
}
