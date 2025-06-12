namespace EchoesOfUzbekistan.Application.Abstractions.Data;
public class PagedList<T>
{
    public IList<T> Items { get; set; } = [];
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasNext => PageNumber < TotalPages;
    public bool HasPrevious => PageNumber > 1;

    private PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
    {
        Items = items.ToList();
        TotalCount = count;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    public static PagedList<T> Create(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        => new(items, count, pageNumber, pageSize);
}
