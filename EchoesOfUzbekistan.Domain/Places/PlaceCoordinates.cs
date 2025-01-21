namespace EchoesOfUzbekistan.Domain.Places;

public record PlaceCoordinates(double latitude, double longitude)
{
    public static PlaceCoordinates Create(double latitude, double longitude)
    {
        if (latitude < -90 || latitude > 90)
            throw new ArgumentException("Latitude must be between -90 and 90 degrees.");
        if (longitude < -180 || longitude > 180)
            throw new ArgumentException("Longitude must be between -180 and 180 degrees.");

        return new PlaceCoordinates(latitude, longitude);
    }
}