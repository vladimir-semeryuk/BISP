namespace EchoesOfUzbekistan.Domain.Places;
public record PlaceTitle
{
    public string Value { get; }

    public PlaceTitle(string placeTitle)
    {
        if (string.IsNullOrWhiteSpace(placeTitle))
            throw new ArgumentException("Place title cannot be null or empty.", nameof(placeTitle));

        if (placeTitle.Length > 100)
            throw new ArgumentException("Place title cannot exceed 100 characters.", nameof(placeTitle));

        Value = placeTitle.Trim();
    }
}
