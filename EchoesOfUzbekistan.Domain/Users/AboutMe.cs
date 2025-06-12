namespace EchoesOfUzbekistan.Domain.Users;

public record AboutMe
{
    public const int MaxLength = 5000;

    public string? Value { get; }

    public AboutMe(string? value)
    {
        if (value is not null && value.Length > MaxLength)
            throw new ArgumentException($"AboutMe cannot exceed {MaxLength} characters.");

        Value = value;
    }

    public override string ToString() => Value ?? string.Empty;
};
