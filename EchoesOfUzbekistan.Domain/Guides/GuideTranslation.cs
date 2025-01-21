using EchoesOfUzbekistan.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Domain.Guides;
public record GuideTranslation(Language language, GuideTitle title, GuideInfo description);
