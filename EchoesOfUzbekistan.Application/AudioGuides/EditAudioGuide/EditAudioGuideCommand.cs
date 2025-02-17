using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Domain.Guides;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Application.AudioGuides.EditAudioGuide;
public record EditAudioGuideCommand(
    Guid GuideId,
    string Title,
    string? Description,
    string City,
    decimal MoneyAmount,
    string CurrencyCode,
    Guid LanguageId,
    string GuideStatus,
    string? AudioLink,
    string? ImageLink
    ) : ICommand;
