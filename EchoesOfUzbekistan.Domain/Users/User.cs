using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Common;
using EchoesOfUzbekistan.Domain.Friendships;
using EchoesOfUzbekistan.Domain.Users.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Country country) : base(id)
    {
        FirstName = firstName;
        Surname = surname;
        Email = email;
        RegistrationDateUtc = registrationDateUtc;
        Country = country;
    }

    public static User Create(FirstName firstName, Surname surname, Email email, Country country)
    {
        return Create(firstName, surname, email, country, Role.OrdinaryUser);
    }

    public static User Create(FirstName firstName, Surname surname, Email email, Country country, Role role)
    {
        var user = new User(Guid.NewGuid(), firstName, surname, email, DateTime.UtcNow, country);

        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));
        user._roles.Add(role);

        return user;
    }

    public static User CreateAdmin(FirstName firstName, Surname surname, Email email, Country country)
    {
        return Create(firstName, surname, email, country, Role.Administrator);
    }

    public static User CreateModerator(FirstName firstName, Surname surname, Email email, Country country)
    {
        return Create(firstName, surname, email, country, Role.Moderator);
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

        var friendship = new Friendship
        {
            Follower = this,
            Followee = target
        };

        Following.Add(friendship);

        return Result.Success();
    }

    public void Unfollow(User target)
    {
        var friendship = Following.FirstOrDefault(f => f.FolloweeId == target.Id);
        if (friendship != null)
            Following.Remove(friendship);
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
