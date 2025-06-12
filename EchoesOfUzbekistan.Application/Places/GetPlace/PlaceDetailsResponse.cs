namespace EchoesOfUzbekistan.Application.Places.GetPlace;
public class PlaceDetailsResponse : PlaceResponse
{
    // TODO: Add translations in future implementation to handle multiple languages for the places
    // public ICollection<PlaceTranslationResponse> PlaceTranslations { get; init; } = new List<PlaceTranslationResponse>();
    public ICollection<Guid> AudioGuidesIds { get; set; }
    public ICollection<string> AudioGuidesNames { get; set; }
}
