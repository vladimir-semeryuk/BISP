using EchoesOfUzbekistan.Application.Abstractions.Data;
using EchoesOfUzbekistan.Application.Abstractions.Messages;

namespace EchoesOfUzbekistan.Application.AudioGuides.GetAudioGuides;
public record GetAudioGuidesQuery(AudioGuideFilter Filter) : IQuery<PaginatedResponse<AudioGuideShortResponse>>;