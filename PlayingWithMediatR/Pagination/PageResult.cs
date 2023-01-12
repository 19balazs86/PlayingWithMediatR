using System.Diagnostics;

namespace PlayingWithMediatR.Pagination;

[DebuggerDisplay("PageNumber = {PageNumber}, TotalPages = {TotalPages}, TotalCount = {TotalCount}, IsEmpty = {IsEmpty}")]
public sealed class PageResult<TEntity>
{
    public IEnumerable<TEntity> Items { get; private set; }

    public int PageNumber { get; private set; }
    public int PageSize { get; private set; }
    public int TotalPages { get; private set; }
    public long TotalCount { get; private set; }

    public bool IsEmpty => Items is null || !Items.Any();

    public bool IsFirstPage => PageNumber == 1;

    public bool IsLastPage => PageNumber == TotalPages;

    public static PageResult<TEntity> Empty => new PageResult<TEntity>();

    public PageResult()
    {
        Items = Enumerable.Empty<TEntity>();
    }

    public PageResult(IEnumerable<TEntity> items, int pageNumber, int pageSize, int totalPages, long totalCount)
    {
        Items = items;

        PageNumber = pageNumber;
        PageSize   = pageSize;
        TotalPages = totalPages;
        TotalCount = totalCount;
    }
}
