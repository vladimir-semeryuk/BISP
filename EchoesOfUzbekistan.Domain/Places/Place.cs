using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Common;
using EchoesOfUzbekistan.Domain.Guides;
using EchoesOfUzbekistan.Domain.Users;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Domain.Places;
public class Place : Entity
{
    public PlaceTitle Title { get; private set; }
    public PlaceDescription? Description { get; private set; }
    public Point Coordinates { get; private set; }
    public PlaceStatus Status { get; private set; }
    public DateTime DatePublished { get; private set; }
    public DateTime? DateEdited { get; private set; }
    public Guid AuthorId { get; private set; }
    //public User Author { get; private set; }
    public Guid OriginalLanguageId { get; private set; }
    // public Language OriginalLanguage { get; private set; }
    public ResourceLink? AudioLink { get; private set; } = null;
    public ResourceLink? ImageLink { get; private set; } = null;
    public ICollection<PlaceTranslation> Translations { get; private set; }
    [NotMapped]
    public ICollection<AudioGuide> Guides { get; private set; }
    private Place() { }
    public Place (
        Guid id, 
        PlaceTitle title,
        Point coordinates,
        PlaceStatus status,
        Guid originalLanguageId,
        Guid authorId
        ) : base(id)
    {
        Title = title;
        Coordinates = coordinates;
        Status = status;
        OriginalLanguageId = originalLanguageId;
        AuthorId = authorId;
        Translations = new List<PlaceTranslation>();
        DatePublished = DateTime.UtcNow;
        Guides = new List<AudioGuide>();
    }
    public void AddTranslation(PlaceTranslation translation)
    {
        Translations.Add(translation);
    }
    public PlaceTranslation? GetTranslation(Language language)
    {
        return Translations.FirstOrDefault(t => t.languageId == language.Id);
    }
    public void MarkAsHidden()
    {
        if (Status == PlaceStatus.Hidden)
            throw new InvalidOperationException("The place is already hidden.");
        Status = PlaceStatus.Hidden;
    }

    public void MarkAsActive()
    {
        if (Status == PlaceStatus.Visible)
            throw new InvalidOperationException("The place is already visible.");
        if (Guides.Count <= 0)
            throw new InvalidOperationException("The place that is not attached to any guide cannot be visible");
        Status = PlaceStatus.Visible;
    }
}
