﻿namespace EchoesOfUzbekistan.Api.Controllers.AudioGuides;

public record CreateAudioGuideRequest(
    string Title,
    string? Description,
    string City,
    decimal MoneyAmount,
    string CurrencyCode,
    string LanguageCode,
    Guid AuthorId,
    DateTime DatePublished,
    string? AudioLink,
    string? ImageLink,
    List<Guid>? PlaceIds);
