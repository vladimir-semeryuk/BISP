namespace EchoesOfUzbekistan.Application.Places.GetPlaces;
public record PlaceFilter(
    Guid? CreatedByUserId = null,
    int? GetTopN = null,
    int? PageNumber = 1,
    int? PageSize = 10,
    bool GetNewest = false
);
