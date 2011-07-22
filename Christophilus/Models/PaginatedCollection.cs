namespace Christophilus.Models
{
    using System.Collections.Generic;

    public class PaginatedCollection<T>
    {
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public IEnumerable<T> Values { get; set; }
        public bool HasNext { get { return CurrentPage < TotalPages; } }
        public bool HasPrevious { get { return CurrentPage > 0; } }
    }
}