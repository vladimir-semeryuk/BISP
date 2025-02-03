using EchoesOfUzbekistan.Domain.Common;
using EchoesOfUzbekistan.Domain.Places;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Application.Places.GetPlace;
public class PlaceResponse
{
    public Guid PlaceId { get; init; }
    public string Title { get; init; }
    public string? Description { get; init; }
    public string Coordinates { get; init; }
    public PlaceStatus Status { get; init; }
    public DateTime DatePublished { get; init; }
    public DateTime? DateEdited { get; init; }
    public Guid AuthorId { get; init; }
    public Guid OriginalLanguageId { get; init; }
    public string? AudioLink { get; private set; } = null;
    public string? ImageLink { get; private set; } = null;
    // public ICollection<PlaceTranslation> Translations { get; private set; }
}
