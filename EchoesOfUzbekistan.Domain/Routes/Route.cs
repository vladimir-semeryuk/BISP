using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Places;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Domain.Routes;
public class Route : Entity
{
    public Guid AudioGuideId { get; private set; }
    public ICollection<PlaceCoordinates> RouteLine { get; private set; }

    public Route(Guid id, Guid audioGuideId, ICollection<PlaceCoordinates>? routeLine) : base(id)
    {
        AudioGuideId = audioGuideId;
        RouteLine = routeLine ?? new List<PlaceCoordinates>();
    }
}
