using EchoesOfUzbekistan.Application.Abstractions.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EchoesOfUzbekistan.Application.Places.PostPlace;
public record PostPlaceCommand(
    string Title,
    string? Description,
    string Coordinates,
    string LanguageCode,
    Guid AuthorId,
    string? AudioLink,
    string? ImageLink) : ICommand<Guid>;
