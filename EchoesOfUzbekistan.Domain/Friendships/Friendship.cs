using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Users;

namespace EchoesOfUzbekistan.Domain.Friendships;
public class Friendship : Entity
{
    public Guid FollowerId { get; set; }
    public User Follower { get; set; }
    public Guid FolloweeId { get; set; }
    public User Followee { get; set; }
    public DateTime FollowedAt { get; set; } = DateTime.UtcNow;

    public Friendship(User follower, User followee)
    {
        if (follower.Id == followee.Id)
        {
            return;
        }

        FolloweeId = followee.Id;
        FollowerId = follower.Id;
        Follower = follower;
        Followee = followee;
        FollowedAt = DateTime.UtcNow;
    }
    private Friendship()
    {
        
    }
}
