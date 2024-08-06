namespace PlayingWithMediatR.Pagination;

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
        set => _pageSize = PaginationDefaults.CalculatePageSize(value);
    }

    public PageQuery(int pageNumber = 1, int pageSize = PaginationDefaults.DefaultPageSize)
    {
        PageNumber = pageNumber;
        PageSize   = pageSize;
    }
}

file static class PaginationDefaults
{
    public const int DefaultPageSize = 15;
    public const int MaxPageSize = 50;

    public static int CalculatePageSize(int pageSize)
    {
        // 0 < ... pageSize ... <= MaxPageSize
        // (Fallback: DefaultPageSize) ... pageSize ... (Fallback: MaxPageSize)

        return pageSize switch
        {
            <= 0          => DefaultPageSize,
            > MaxPageSize => MaxPageSize,
            _             => pageSize
        };
    }
}