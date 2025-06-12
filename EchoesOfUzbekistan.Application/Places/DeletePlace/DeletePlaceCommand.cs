using EchoesOfUzbekistan.Application.Abstractions.Messages;

namespace EchoesOfUzbekistan.Application.Places.DeletePlace;

public record DeletePlaceCommand(Guid PlaceId) : ICommand;
