using EchoesOfUzbekistan.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Domain.Guides;
public record GuideTranslation(Guid languageId, GuideTitle title, GuideInfo? description, Guid AudioGuideId);
