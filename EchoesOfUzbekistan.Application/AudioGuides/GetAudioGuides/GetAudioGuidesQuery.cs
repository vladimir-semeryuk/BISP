using EchoesOfUzbekistan.Application.Abstractions.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Application.AudioGuides.GetAudioGuides;
public record GetAudioGuidesQuery(AudioGuideFilter Filter) : IQuery<IReadOnlyList<AudioGuideShortResponse>>;