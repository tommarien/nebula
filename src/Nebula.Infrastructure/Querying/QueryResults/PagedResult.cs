using System.Collections.Generic;
using System.Linq;

namespace Nebula.Infrastructure.Querying.QueryResults
{
    public class PagedResult<T>
    {
        public PagedResult(IEnumerable<T> results, int totalResults)
        {
            TotalResults = totalResults;
            Results = results.ToArray();
        }

        public int TotalResults { get; set; }
        public T[] Results { get; set; }
    }
}