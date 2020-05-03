using System.Linq;

namespace Shared.Infrastructure.SortingHelper
{
   public interface ISorter<T>
    {
        IQueryable<T> ApplySort(IQueryable<T> entities, string orderByQueryString);
    }
}