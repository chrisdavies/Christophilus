namespace Christophilus.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PaginatedCollection<T>
    {
        public PaginatedCollection()
        {
            this.PageSize = 10;
            this.PageRange = 3;
        }

        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the range of pages to be displayed.
        /// </summary>
        public int PageRange { get; set; }

        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets the total number of matches.
        /// </summary>
        public int TotalMatches { get; set; }

        /// <summary>
        /// Gets the last page number.
        /// </summary>
        public int LastPage
        {
            get { return (TotalMatches - 1) / PageSize; }
        }

        /// <summary>
        /// Gets the earliest page number to be displayed.
        /// </summary>
        public int FirstPageDisplay 
        { 
            get { return Math.Max(0, this.CurrentPage - PageRange); }
        }

        /// <summary>
        /// Gets the last page number to be displayed.
        /// </summary>
        public int LastPageDisplay 
        { 
            get { return Math.Min(this.LastPage, this.CurrentPage + PageRange); } 
        }

        /// <summary>
        /// Gets a value indicating whether or not there are more records.
        /// </summary>
        public bool HasMorePages
        {
            get { return this.CurrentPage != this.LastPageDisplay; }
        }

        /// <summary>
        /// Gets a value indicating whether or not there are previous records.
        /// </summary>
        public bool HasPreviousPages
        {
            get { return this.CurrentPage != this.FirstPageDisplay; }
        }

        /// <summary>
        /// Gets a value indicating whether or not pagination is
        /// required.
        /// </summary>
        public bool ShowPagination
        {
            get { return this.HasMorePages || this.HasPreviousPages; }
        }

        /// <summary>
        /// Gets or sets the set values in this page.
        /// </summary>
        public IEnumerable<T> Values { get; set; }

        /// <summary>
        /// Gets a value indicating whether or not the set is empty.
        /// </summary>
        public bool IsEmpty 
        {
            get { return Values.FirstOrDefault() == null; }
        }
    }
}