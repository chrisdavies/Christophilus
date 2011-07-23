namespace Christophilus.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MongoDB.Driver;

    public class PaginatedJournalEntries : PaginatedCollection<JournalEntry>
    {
        private string user;

        public PaginatedJournalEntries(
            MongoCollection<JournalEntry> entries,
            string user,
            int currentPage,
            int entriesPerPage)
        {
            var pertinentEntries = entries.Find(new QueryDocument("User", user))
                .SetSortOrder(new SortByDocument("Day", -1))
                .SetFields(new string[] { "User", "Day", "Summary" })
                .SetSkip(currentPage * entriesPerPage)
                .SetLimit(entriesPerPage);

            this.user = user;
            this.Values = pertinentEntries;
            this.CurrentPage = currentPage;
            this.PageSize = entriesPerPage;
            this.TotalMatches = pertinentEntries.Count();

            this.EnsureTodayIsAvailable();
        }

        private void EnsureTodayIsAvailable()
        {
            if (this.CurrentPage > 0)
            {
                return;
            }

            var firstEntry = this.Values.FirstOrDefault();

            if (!IsToday(firstEntry))
            {
                this.Values = ValuesWithToday(this.Values);
            }
        }

        private static bool IsToday(JournalEntry first)
        {
            return first != null && DateTime.Parse(first.Day).Date == DateTime.Now.Date;
        }

        private IEnumerable<JournalEntry> ValuesWithToday(IEnumerable<JournalEntry> entries)
        {
            yield return new JournalEntry(this.user, DateTime.Now)
            {
                Body = "Create today's entry."
            };

            foreach (var entry in entries)
            {
                yield return entry;
            }
        }
    }
}