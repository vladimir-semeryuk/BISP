namespace EchoesOfUzbekistan.Domain.Abstractions;
public record Error(string Code, string Name)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "Null Value was provided");
    public static readonly Error UnauthorizedAccess = new("Error.UnauthorizedAccess",
        "You don't have necessary permissions to perform this action.");
    public static readonly Error CannotPostForOthers = new("Error.CannotPostForOthers",
        "You cannot perform actions on behalf of other users.");
    public static readonly Error UnsupportedTTSLanguage = new("Error.UnsupportedTTSLanguage",
        "Audio generation for the specified language is not supported.");
    public static readonly Error TextToSpeechGenerationError = new("Error.TextToSpeechGenerationError",
        "Audio generation went wrong. Try again later or change the text input.");
}
