using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Common;
using EchoesOfUzbekistan.Domain.Places;
using EchoesOfUzbekistan.Domain.Users.Events;
using EchoesOfUzbekistan.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EchoesOfUzbekistan.Domain.Guides.Events;

namespace EchoesOfUzbekistan.Domain.Guides;
public class AudioGuide : Entity
{
    public GuideTitle Title { get; private set; }
    public GuideInfo? Description { get; private set; } = null;
    public City City { get; private set; }
    public Money Price { get; private set; } = Money.Zero();
    public GuideStatus Status { get; private set; }
    public DateTime DatePublished { get; private set; }
    public DateTime? DateEdited { get; private set; }
    public Guid AuthorId { get; private set; }
    // public User Author { get; private set; }
    public Guid OriginalLanguageId { get; private set; }
    // public Language OriginalLanguage { get; private set; }
    public ResourceLink? AudioLink { get; private set; } = null;
    public ResourceLink? ImageLink { get; private set; } = null;
    public ICollection<GuideTranslation> Translations { get; private set; }
    public ICollection<Place> Places { get; private set; }

    private AudioGuide() { }

    private AudioGuide(
        Guid id, 
        GuideTitle title, 
        GuideInfo? description,
        City city, 
        Money price,
        Guid languageId,
        Guid authorId,
        DateTime datePublished,
        ResourceLink? audioLink,
        ResourceLink? imageLink) : base(id)
    {
        Title = title;
        Description = description;
        City = city;
        Price = price ?? Money.Zero();
        OriginalLanguageId = languageId;
        AuthorId = authorId;
        DatePublished = datePublished;
        AudioLink = audioLink;
        ImageLink = imageLink;
        Translations = new List<GuideTranslation>();
        Places = new List<Place>();
    }
    public static AudioGuide Create(
        GuideTitle title,
        GuideInfo description,
        City city,
        Money price,
        Guid languageId,
        Guid authorId,
        ResourceLink? audioLink,
        ResourceLink? imageLink)
    {
        // Using the Static Factory pattern to enhance encapsulation and introduce domain events
        var guide = new AudioGuide(Guid.NewGuid(), title, description, city, price ?? Money.Zero(),
                                  languageId, authorId, DateTime.UtcNow, audioLink, imageLink);

        guide.RaiseDomainEvent(new GuideCreatedDomainEvent(guide.Id));

        guide.DateEdited = DateTime.UtcNow;

        return guide;
    }

    public void Update(
        GuideTitle title,
        GuideInfo? description,
        City city,
        Money price,
        Guid languageId,
        GuideStatus guideStatus,
        ResourceLink? newAudioLink,
        ResourceLink? newImageLink)
    {
        Title = title;
        Description = description;
        City = city;
        Price = price;
        Status = guideStatus;
        OriginalLanguageId = languageId;

        var oldImageLink = ImageLink?.value;
        var oldAudioLink = AudioLink?.value;

        // Update the image link.
        ImageLink = newImageLink;
        AudioLink = newAudioLink;

        // If the image link has changed, add a domain event.
        if (oldImageLink != null && !string.Equals(oldImageLink, newImageLink?.value, StringComparison.OrdinalIgnoreCase))
        {
            var imageUpdatedEvent = new AudioGuideResourceUpdatedEvent(oldImageLink);
            RaiseDomainEvent(imageUpdatedEvent);
        }
        if (oldAudioLink != null && !string.Equals(oldAudioLink, newAudioLink?.value, StringComparison.OrdinalIgnoreCase))
        {
            var audioUpdatedEvent = new AudioGuideResourceUpdatedEvent(oldAudioLink);
            RaiseDomainEvent(audioUpdatedEvent);
        }

        DateEdited = DateTime.UtcNow;
    }

    public void MarkAsDeleted()
    {
        var deletionEvent = new AudioGuideDeletedEvent(
            Id,
            AudioLink?.value,
            ImageLink?.value,
            Translations.Select(t => t.audioLink?.value).ToList(),
            Places.ToList());
        RaiseDomainEvent(deletionEvent);
    }

    public void AddTranslation(GuideTranslation translation)
    {
        Translations.Add(translation);
    }
    public GuideTranslation? GetTranslation(Language language)
    {
        return Translations.FirstOrDefault(t => t.languageId == language.Id);
    }
    // each audio guide may contain many places, although upon creation it can have non
    public void AddPlace(Place place)
    {
        if (Places.Contains(place))
            throw new InvalidOperationException("Place is already part of this guide.");

        Places.Add(place);
    }

    public void RemovePlace(Place place)
    {
        if (!Places.Contains(place))
            throw new InvalidOperationException("Place is not part of this guide.");

        Places.Remove(place);
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
