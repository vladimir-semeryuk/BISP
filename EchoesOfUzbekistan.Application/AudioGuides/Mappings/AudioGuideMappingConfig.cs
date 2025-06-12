using EchoesOfUzbekistan.Application.AudioGuides.GetAudioGuide;
using EchoesOfUzbekistan.Application.Places.GetPlace;
using EchoesOfUzbekistan.Domain.Guides;
using Mapster;

namespace EchoesOfUzbekistan.Application.AudioGuides.Mappings;

public class AudioGuideMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AudioGuide, AudioGuideResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Title, src => src.Title.guideTitle)
            .Map(dest => dest.Description, src => src.Description != null ? src.Description.value : null)
            .Map(dest => dest.City, src => src.City.Value)
            .Map(dest => dest.PriceAmount, src => src.Price.Amount)
            .Map(dest => dest.PriceCurrency, src => src.Price.Currency)
            .Map(dest => dest.Status, src => src.Status.ToString())
            .Map(dest => dest.DatePublished, src => src.DatePublished)
            .Map(dest => dest.DateEdited, src => src.DateEdited)
            .Map(dest => dest.AuthorId, src => src.AuthorId)
            .Map(dest => dest.OriginalLanguageId, src => src.OriginalLanguageId)
            .Map(dest => dest.AudioLink, src => src.AudioLink == null ? null : src.AudioLink!.Value)
            .Map(dest => dest.ImageLink, src => src.ImageLink == null ? null : src.ImageLink!.Value)
            .Map(dest => dest.Places, src => src.Places.Adapt<ICollection<PlaceResponse>>())
            .Map(dest => dest.Translations,
                src => src.Translations.Adapt<ICollection<AudioGuideTranslationResponse>>());

        config.NewConfig<GuideTranslation, AudioGuideTranslationResponse>()
            .Map(dest => dest.TranslationLanguageId, src => src.LanguageId)
            .Map(dest => dest.Title, src => src.Title.guideTitle)
            .Map(dest => dest.Description, src => src.Description == null ? null : src.Description!.value);
    }
}
