using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Common;
using EchoesOfUzbekistan.Domain.Places;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Domain.Guides;
public class AudioGuide : Entity
{
    public GuideTitle Title { get; private set; }
    public GuideInfo? Description { get; private set; }
    public City City { get; private set; }
    public Money Price { get; private set; } = Money.Zero();
    public GuideStatus Status { get; private set; }
    public DateTime DatePublished { get; private set; }
    public DateTime? DateEdited { get; private set; }
    public Guid AuthorId { get; private set; }
    public Language OriginalLanguage { get; private set; }
    public AudioLink? AudioLink { get; private set; }
    public ICollection<GuideTranslation> Translations { get; private set; }
    public ICollection<Guid> PlaceIds { get; private set; }

    public AudioGuide(
        Guid id, 
        GuideTitle title, 
        City city, 
        Language language,
        Guid authorId) : base(id)
    {
        Title = title;
        City = city;
        OriginalLanguage = language;
        AuthorId = authorId;
        Translations = new List<GuideTranslation>();
        PlaceIds = new List<Guid>();
    }
    public void AddTranslation(GuideTranslation translation)
    {
        Translations.Add(translation);
    }

    public GuideTranslation? GetTranslation(string languageCode)
    {
        return Translations.FirstOrDefault(t => t.language.Code == languageCode.ToLowerInvariant());
    }
    public GuideTranslation? GetTranslation(Language language)
    {
        return Translations.FirstOrDefault(t => t.language == language);
    }
    // each audio guide may contain many places, although upon creation it can have non
    public void AddPlace(Guid placeId)
    {
        if (PlaceIds.Contains(placeId))
            throw new InvalidOperationException("Place is already part of this guide.");

        PlaceIds.Add(placeId);
    }

    public void RemovePlace(Guid placeId)
    {
        if (!PlaceIds.Contains(placeId))
            throw new InvalidOperationException("Place is not part of this guide.");

        PlaceIds.Remove(placeId);
    }
    public void MarkAsInactive()
    {
        if (Status == GuideStatus.Inactive)
            throw new InvalidOperationException("The guide is already inactive.");
        Status = GuideStatus.Inactive;
    }

    public void MarkAsActive()
    {
        if (Status == GuideStatus.Active)
            throw new InvalidOperationException("The guide is already active.");
        Status = GuideStatus.Active;
    }
}
