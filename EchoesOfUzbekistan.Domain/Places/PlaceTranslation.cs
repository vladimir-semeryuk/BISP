using EchoesOfUzbekistan.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Domain.Places;
public record PlaceTranslation(Guid languageId, PlaceTitle title, PlaceDescription? description, Guid placeId);
