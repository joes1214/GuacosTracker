using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GuacosTracker3.Utilities
{
    public class Pagination
    {
        public class PaginatedList<T> : List<T>
        {
            public int PageIndex { get; private set; }
            public int TotalPages { get; private set; }

            public PaginatedList(IEnumerable<T> items, int count, int pageIndex, int pageSize)
            {
                PageIndex = pageIndex;
                TotalPages = (int)Math.Ceiling(count / (double)pageSize);

                this.AddRange(items);
            }

            public bool HasPreviousPage => PageIndex > 1;

            public bool HasNextPage => PageIndex < TotalPages;

            public static PaginatedList<T> CreatePagination(List<T> source, int pageIndex, int pageSize)
            {
                var count = source.Count;
                int totalPages = (int)Math.Ceiling(count / (double)pageSize);

                if (pageIndex < 1)
                {
                    pageIndex = 1;
                } else if (pageIndex > totalPages)
                {
                    pageIndex = totalPages;
                }

                var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize);

                return new PaginatedList<T>(items, count, pageIndex, pageSize);
            }
        }
    }
}
