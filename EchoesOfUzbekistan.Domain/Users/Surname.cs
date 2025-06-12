namespace EchoesOfUzbekistan.Domain.Users;
public sealed record Surname
{
    public string Value { get; }

    public Surname(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Surname cannot be empty.", nameof(value));

        Value = value.Trim();
    }

    public override string ToString() => Value;
}
