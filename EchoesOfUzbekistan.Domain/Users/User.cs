using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Common;
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
    public FirstName FirstName { get; private set; }
    public Surname Surname { get; private set; }
    public Email Email { get; private set; }
    public DateTime RegistrationDateUtc { get; private set; }
    public Country Country { get; private set; }
    public City? City { get; private set; }
    public AboutMe? AboutMe { get; private set; }

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
        // Using the Static Factory pattern to enhance encapsulation and introduce
        // domain events
        var user = new User(Guid.NewGuid(), firstName, surname, email, DateTime.UtcNow, country);

        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));

        return user;
    }
    
}
