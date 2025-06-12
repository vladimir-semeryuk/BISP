using EchoesOfUzbekistan.Application.Places.GetPlace;
using EchoesOfUzbekistan.Domain.Places;
using Mapster;

namespace EchoesOfUzbekistan.Application.Places.Mappings;
public class PlaceMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Place, PlaceResponse>()
            .Map(dest => dest.PlaceId, src => src.Id)
            .Map(dest => dest.Title, src => src.Title.Value)
            .Map(dest => dest.Description, src => src.Description == null ? null : src.Description!.value)
            .Map(dest => dest.Status, src => src.Status.ToString())
            .Map(dest => dest.DatePublished, src => src.DatePublished)
            .Map(dest => dest.DateEdited, src => src.DateEdited)
            .Map(dest => dest.AuthorId, src => src.AuthorId)
            .Map(dest => dest.OriginalLanguageId, src => src.OriginalLanguageId)
            .Map(dest => dest.AudioLink, src => src.AudioLink == null ? null : src.AudioLink!.Value)
            .Map(dest => dest.ImageLink, src => src.ImageLink == null ? null : src.ImageLink!.Value);
    }
}
