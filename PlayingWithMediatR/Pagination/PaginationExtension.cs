using Microsoft.EntityFrameworkCore;
using PlayingWithMediatR.Pagination;

namespace System.Linq;

public static class PaginationExtension
{
    public static async Task<PageResult<T>> ToPageResultAsync<T>(
        this IQueryable<T> source,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default)
    {
        long totalCount = await source.CountAsync(ct);

        if (totalCount == 0)
            return PageResult<T>.Empty;

        int totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

        if (pageNumber > totalPages) pageNumber = totalPages;

        int skip = (pageNumber - 1) * pageSize;

        IEnumerable<T> items = await source
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PageResult<T>(items, pageNumber, pageSize, totalPages, totalCount);
    }
}
