using EchoesOfUzbekistan.Application.Abstractions.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Application.AudioGuides.PostAudioGuide;
public record PostAudioGuideCommand (
    string Title,
    string? Description,
    string City,
    decimal MoneyAmount,
    string CurrencyCode,
    Guid LanguageId,
    Guid AuthorId,
    string? AudioLink,
    string? ImageLink
    ) : ICommand<Guid>;