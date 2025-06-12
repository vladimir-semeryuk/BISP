using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Common;
using EchoesOfUzbekistan.Domain.Guides;
using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations.Schema;
using EchoesOfUzbekistan.Domain.Guides.Events;
using EchoesOfUzbekistan.Domain.Places.Events;

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
        PlaceDescription? description,
        Point coordinates,
        PlaceStatus status,
        Guid originalLanguageId,
        Guid authorId,
        ResourceLink? audioLink,
        ResourceLink? imageLink
        ) : base(id)
    {
        Title = title;
        Description = description;
        Coordinates = coordinates;
        Status = status;
        OriginalLanguageId = originalLanguageId;
        AuthorId = authorId;
        Translations = new List<PlaceTranslation>();
        DatePublished = DateTime.UtcNow;
        Guides = new List<AudioGuide>();
        AudioLink = audioLink;
        ImageLink = imageLink;
    }

    public void Edit(
        PlaceTitle title,
        PlaceDescription? description,
        Point coordinates,
        Guid languageId,
        ResourceLink? newAudioLink,
        ResourceLink? newImageLink)
    {
        Title = title;
        Description = description;
        Coordinates = coordinates;
        OriginalLanguageId = languageId;

        UpdateImageLink(newImageLink);
        UpdateAudioLink(newAudioLink);

        DateEdited = DateTime.UtcNow;
    }

    public void Delete()
    {
        RaiseDomainEvent(new PlaceDeletedDomainEvent(ImageLink?.Value, AudioLink?.Value, Translations.ToList()));
    }

    private void UpdateImageLink(ResourceLink? newImageLink)
    {
        if (ImageLink is not null && newImageLink is null)
        {
            // Image is being removed
            var oldLink = ImageLink.Value;
            ImageLink = null;

            RaiseDomainEvent(new EntityFileResourceDeletedEvent(oldLink));
        }
        else if (newImageLink is not null)
        {
            var oldLink = ImageLink?.Value;
            ImageLink = newImageLink;

            // Raise updated event if the value actually changed
            if (oldLink is not null && !string.Equals(oldLink, newImageLink.Value, StringComparison.OrdinalIgnoreCase))
            {
                RaiseDomainEvent(new EntityFileResourceUpdatedEvent(oldLink));
            }
        }
    }

    private void UpdateAudioLink(ResourceLink? newAudioLink)
    {
        if (AudioLink is not null && newAudioLink is null)
        {
            var oldLink = AudioLink.Value;
            AudioLink = null;

            RaiseDomainEvent(new EntityFileResourceDeletedEvent(oldLink));
        }
        else if (newAudioLink is not null)
        {
            var oldLink = AudioLink?.Value;
            AudioLink = newAudioLink;

            // Raise updated event if the value actually changed
            if (oldLink is not null && !string.Equals(oldLink, newAudioLink.Value, StringComparison.OrdinalIgnoreCase))
            {
                RaiseDomainEvent(new EntityFileResourceUpdatedEvent(oldLink));
            }
        }
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
