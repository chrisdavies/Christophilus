namespace Christophilus.Models
{
    using System;
    using MongoDB.Bson;
    using MongoDB.Driver;

    public class JournalEntryService
    {
        private static MongoCollection<JournalEntry> Entries
        {
            get
            {
                return DataStore.DB.GetCollection<JournalEntry>("JournalEntries");
            }
        }

        public static PaginatedCollection<JournalEntry> GetEntries(
            string user, int currentPage = 0, int entriesPerPage = 10)
        {
            var entries = Entries.Find(new QueryDocument("User", user))
                .SetSkip(currentPage * entriesPerPage)
                .SetLimit(entriesPerPage);

            return new PaginatedCollection<JournalEntry>()
            {
                Values = entries,
                CurrentPage = currentPage,
                TotalPages = entries.Count() / entriesPerPage
            };
        }

        public static JournalEntry GetEntry(string user, DateTime day)
        {
            return Entries.FindOneById(new JournalEntry(user, day).Id);
        }

        internal static void InitializeDB()
        {
            var keys = new IndexKeysDocument();
            keys.Add(new BsonElement("User", 1));
            keys.Add(new BsonElement("Day", -1));

            Entries.EnsureIndex(keys);
        }
    }
}