namespace EchoesOfUzbekistan.Application.AudioGuides.GetAudioGuides;
public record AudioGuideFilter(
    Guid? CreatedByUserId = null,
    int? GetTopN = null, 
    int? PageNumber = 1, 
    int? PageSize = 10, 
    string? SearchQuery = null,
    bool GetNewest = false
);