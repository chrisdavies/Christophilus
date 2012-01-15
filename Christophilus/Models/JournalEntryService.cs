namespace Christophilus.Models
{
    using System;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using System.Collections.Generic;
    using MongoDB.Driver.Builders;
    using System.Linq;

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
            return new PaginatedJournalEntries(
                Entries, user, currentPage, entriesPerPage);
        }

        public static JournalEntry GetEntry(string user, DateTime day)
        {
            return Entries.FindOneById(new JournalEntry(user, day).Id);
        }
        
        internal static IEnumerable<string> GetEntries(string userId, DateTime start, DateTime end)
        {
            return Entries.Find(Query.And(
                    Query.EQ("User", userId),
                    Query.GTE("Day", start.ToString(DataStore.DateFormat)),
                    Query.LTE("Day", end.ToString(DataStore.DateFormat))))
                .SetFields("Body")
                .Select(j => j.Body.ToLowerInvariant());
        }

        internal static void InitializeDB()
        {
            var keys = new IndexKeysDocument();
            keys.Add(new BsonElement("User", 1));
            keys.Add(new BsonElement("Day", -1));

            Entries.EnsureIndex(keys);
        }

        internal static void Save(JournalEntry entry)
        {
            entry.Version = (ulong)DateTime.UtcNow.ToBinary();
            Entries.Save(entry);
        }
    }
}