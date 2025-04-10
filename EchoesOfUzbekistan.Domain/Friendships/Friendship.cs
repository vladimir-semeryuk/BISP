using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Domain.Friendships;
public class Friendship : Entity
{
    public Guid FollowerId { get; set; }
    public User Follower { get; set; }
    public Guid FolloweeId { get; set; }
    public User Followee { get; set; }
    public DateTime FollowedAt { get; set; } = DateTime.UtcNow;
}
