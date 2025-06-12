using EchoesOfUzbekistan.Domain.Abstractions;

namespace EchoesOfUzbekistan.Domain.Places;
public static class PlaceErrors
{
    public static readonly Error NotFound = new(
        "Place.NotFound",
        "The specified place was not found.");
}
