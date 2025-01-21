using EchoesOfUzbekistan.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Domain.Places;
public record PlaceTranslation(Language language, PlaceTitle title, PlaceDescription description);
