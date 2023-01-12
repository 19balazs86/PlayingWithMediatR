namespace PlayingWithMediatR.Pagination;

public sealed class PaginationDefaults
{
    public const int PageSize    = 15;
    public const int MaxPageSize = 50;
}

public class PageQuery
{
    private int _pageNumber;
    private int _pageSize;

    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value <= 0 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value <= 0 ? PaginationDefaults.PageSize : value < PaginationDefaults.MaxPageSize ? value : PaginationDefaults.MaxPageSize;
    }

    public PageQuery(int pageNumber = 1, int pageSize = PaginationDefaults.PageSize)
    {
        PageNumber = pageNumber;
        PageSize   = pageSize;
    }
}
