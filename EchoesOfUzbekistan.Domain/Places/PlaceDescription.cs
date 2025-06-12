namespace EchoesOfUzbekistan.Domain.Places;
public record PlaceDescription(string? value)
{
    public static PlaceDescription Create(string description)
    {
        if (description.Length > 5000)
            throw new ArgumentException("The place's description cannot exceed 5000 characters.");

        return new PlaceDescription(description);
    }
};
