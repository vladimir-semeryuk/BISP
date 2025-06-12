using EchoesOfUzbekistan.Domain.Abstractions;

namespace EchoesOfUzbekistan.Domain.Likes;
public static class LikeErrors
{
    public static readonly Error AlreadyLiked = new(
        "Like.AlreadyLiked",
        "You have already liked this!");
    public static readonly Error NotFound = new(
        "Like.NotFound",
        "You have not liked this!");
}
