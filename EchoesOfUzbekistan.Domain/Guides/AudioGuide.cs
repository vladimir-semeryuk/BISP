using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Common;
using EchoesOfUzbekistan.Domain.Places;
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

        UpdateImageLink(newImageLink);
        UpdateAudioLink(newAudioLink);

        DateEdited = DateTime.UtcNow;
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
    public void Delete()
    {
        var deletionEvent = new AudioGuideDeletedEvent(
            Id,
            AudioLink?.Value,
            ImageLink?.Value,
            Translations.Select(t => t.AudioLink?.Value).ToList(),
            Places.ToList());
        RaiseDomainEvent(deletionEvent);
    }

    public void AddTranslation(GuideTranslation translation)
    {
        Translations.Add(translation);
    }
    public GuideTranslation? GetTranslation(Language language)
    {
        return Translations.FirstOrDefault(t => t.LanguageId == language.Id);
    }
    // each audio guide may contain many places, although upon creation it can have none
    public void AddPlace(Place place)
    {
        if (Places.Contains(place))
            return;

        Places.Add(place);
    }

    public void RemovePlace(Place place)
    {
        if (!Places.Contains(place))
            return;

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
