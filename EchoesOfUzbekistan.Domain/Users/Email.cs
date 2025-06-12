using System.Text.RegularExpressions;

namespace EchoesOfUzbekistan.Domain.Users;
public sealed record Email
{
    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be empty.");

        if (!IsValidEmail(value))
            throw new ArgumentException("Invalid email format.");

        Value = value;
    }

    private static bool IsValidEmail(string email) =>
        Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
}
