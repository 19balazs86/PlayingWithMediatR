using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlayingWithMediatR.Entities;

namespace PlayingWithMediatR.Extensions
{
  public static class PaginationExtension
  {
    public static async Task<PageResult<T>> PaginateAsync<T>(
      this IQueryable<T> query,
      int page,
      int pageSize,
      CancellationToken ct = default)
    {
      long totalCount = await query.CountAsync(ct);

      if (totalCount == 0)
        return PageResult<T>.Empty;

      int pageCount = (int)Math.Ceiling((decimal)totalCount / pageSize);

      if (page > pageCount) page = pageCount;

      int skip = (page - 1) * pageSize;

      IEnumerable<T> items = await query
        .Skip(skip)
        .Take(pageSize)
        .ToListAsync(ct);

      return new PageResult<T>(items, page, pageSize, pageCount, totalCount);
    }
  }
}
