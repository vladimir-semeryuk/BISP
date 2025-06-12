using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Common;
using EchoesOfUzbekistan.Domain.Friendships;
using EchoesOfUzbekistan.Domain.Guides.Events;
using EchoesOfUzbekistan.Domain.Users.Events;

namespace EchoesOfUzbekistan.Domain.Users;
public class User : Entity
{
    // Using private setters to make sure the values cannot be changed
    // outside of the entity's scope 
    private readonly List<Role> _roles = new();
    public FirstName FirstName { get; private set; }
    public Surname Surname { get; private set; }
    public Email Email { get; private set; }
    public DateTime RegistrationDateUtc { get; private set; }
    public Country Country { get; private set; }
    public City? City { get; private set; }
    public AboutMe? AboutMe { get; private set; }
    public string IdentityId { get; private set; } = string.Empty;
    public ResourceLink? ImageLink { get; private set; } = null;
    public IReadOnlyCollection<Role> Roles => _roles.ToList();
    public ICollection<Friendship> Following { get; private set; } = new List<Friendship>();
    public ICollection<Friendship> Followers { get; private set; } = new List<Friendship>();

    private User() { }

    private User(
        Guid id,
        FirstName firstName, 
        Surname surname, 
        Email email, 
        DateTime registrationDateUtc,
        Country country,
        City? city) : base(id)
    {
        FirstName = firstName;
        Surname = surname;
        Email = email;
        RegistrationDateUtc = registrationDateUtc;
        Country = country;
        City = city;
    }

    public static User Create(FirstName firstName, Surname surname, Email email, Country country, City? city)
    {
        return Create(firstName, surname, email, country, city, Role.OrdinaryUser);
    }

    public static User Create(FirstName firstName, Surname surname, Email email, Country country, City? city, Role role)
    {
        var user = new User(Guid.NewGuid(), firstName, surname, email, DateTime.UtcNow, country, city);

        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));
        user._roles.Add(role);

        return user;
    }

    public static User CreateAdmin(FirstName firstName, Surname surname, Email email, City? city, Country country)
    {
        return Create(firstName, surname, email, country, city, Role.Administrator);
    }

    public static User CreateModerator(FirstName firstName, Surname surname, Email email, City? city, Country country)
    {
        return Create(firstName, surname, email, country, city, Role.Moderator);
    }

    public void UpdateProfile(
        FirstName? firstName,
        Surname? surname,
        AboutMe? aboutMe,
        Country? country,
        City? city)
    {
        if (firstName is not null)
            FirstName = firstName;

        if (surname is not null)
            Surname = surname;

        AboutMe = aboutMe;

        if (country is not null)
            Country = country;

        if (city is not null)
            City = city;
    }

    public Result UpdateProfilePicture(ResourceLink newImageLink)
    {
        if (newImageLink == null || string.IsNullOrWhiteSpace(newImageLink.Value))
            return Result.Failure(UserErrors.UpdateImageWithNullImageLink);

        var oldImage = ImageLink;
        ImageLink = newImageLink;

        if (oldImage != null) 
            RaiseDomainEvent(new UserProfilePictureUpdatedDomainEvent(Id, oldImage.Value, newImageLink.Value));

        return Result.Success();
    }

    public Result ClearProfilePicture()
    {
        if (this.ImageLink != null)
            RaiseDomainEvent(new EntityFileResourceDeletedEvent(this.ImageLink.Value));
        ImageLink = null;

        return Result.Success();
    }

    public Result Delete()
    {
        if (this.ImageLink!=null)
            RaiseDomainEvent(new EntityFileResourceDeletedEvent(ImageLink.Value));

        return Result.Success();
    }

    public void SetIdentityId(string identityProviderId)
    {
        IdentityId = identityProviderId;
    }

    public Result Follow(User target)
    {
        if (target.Id == this.Id)
            return Result.Failure(UserErrors.CannotFollowYourself);

        if (IsFollowing(target))
            return Result.Failure(UserErrors.AlreadyFollow);

        var friendship = new Friendship(this, target);

        Following.Add(friendship);

        return Result.Success();
    }

    public Result Unfollow(User target)
    {
        if (target.Id == this.Id)
            return Result.Failure(UserErrors.CannotUnfollowYourself);

        var friendship = Following.FirstOrDefault(f => f.FolloweeId == target.Id);
        if (friendship == null)
            return Result.Failure(UserErrors.FollowNotFound);

        Following.Remove(friendship);

        return Result.Success();
    }

    public bool IsFollowing(User target)
    {
        return Following.Any(f => f.FolloweeId == target.Id);
    }

    public bool IsFollowedBy(User target)
    {
        return Followers.Any(f => f.FollowerId == target.Id);
    }
}
