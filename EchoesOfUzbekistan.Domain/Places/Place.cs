using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Common;
using EchoesOfUzbekistan.Domain.Guides;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Domain.Places;
public class Place : Entity
{
    public PlaceTitle Title { get; private set; }
    public PlaceDescription? Description { get; private set; }
    public PlaceCoordinates Coordinates { get; private set; }
    public PlaceStatus Status { get; private set; }
    public DateTime DatePublished { get; private set; }
    public DateTime? DateEdited { get; private set; }
    public Guid AuthorId { get; private set; }
    public Language OriginalLanguage { get; private set; }
    public AudioLink? Audio { get; private set; }
    public ICollection<PlaceTranslation> Translations { get; private set; }
    public Place (
        Guid id, 
        PlaceTitle title,
        PlaceCoordinates coordinates,
        PlaceStatus status,
        Language originalLanguage,
        Guid authorId
        ) : base(id)
    {
        Title = title;
        Coordinates = coordinates;
        Status = status;
        OriginalLanguage = originalLanguage;
        AuthorId = authorId;
        Translations = new List<PlaceTranslation>();
        DatePublished = DateTime.UtcNow;
    }
    public void AddTranslation(PlaceTranslation translation)
    {
        Translations.Add(translation);
    }

    public PlaceTranslation? GetTranslation(string languageCode)
    {
        return Translations.FirstOrDefault(t => t.language.Code == languageCode.ToLowerInvariant());
    }
    public PlaceTranslation? GetTranslation(Language language)
    {
        return Translations.FirstOrDefault(t => t.language == language);
    }
}
