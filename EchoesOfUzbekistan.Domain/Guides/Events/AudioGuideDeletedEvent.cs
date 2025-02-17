using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Places;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Domain.Guides.Events;
public record AudioGuideDeletedEvent(
    Guid AudioGuideId, 
    string? AudioLink, 
    string? ImageLink, 
    List<string?> TranslationAudioLinks,
    List<Place> Places) : IDomainEvent;
