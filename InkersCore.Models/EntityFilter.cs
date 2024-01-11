using System.Linq.Expressions;

namespace InkersCore.Models
{
    public class EntityFilter<T>
    {
        public long RowCount { get; set; }
        public Expression<Func<T, bool>> Predicate { get; set; }
        public Expression<Func<T, object>> SortBy { get; set; }
        public bool SortAscending { get; set; } = true;
    }
}
