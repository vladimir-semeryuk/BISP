using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Guides;
using EchoesOfUzbekistan.Domain.Places;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
