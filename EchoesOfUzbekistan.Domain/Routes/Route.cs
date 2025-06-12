using EchoesOfUzbekistan.Domain.Abstractions;
using NetTopologySuite.Geometries;

namespace EchoesOfUzbekistan.Domain.Routes;
public class Route : Entity
{
    public Guid AudioGuideId { get; private set; }
    //public AudioGuide Guide { get; private set; }
    public LineString? RouteLine { get; private set; }
    private Route() { }

    public Route(Guid id, Guid audioGuideId, LineString? routeLine) : base(id)
    {
        AudioGuideId = audioGuideId;
        //Guide = guide;
        RouteLine = routeLine;
    }
}
