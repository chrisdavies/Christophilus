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
            return new PaginatedJournalEntries(
                Entries, user, currentPage, entriesPerPage);
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

        internal static void Save(JournalEntry entry)
        {
            entry.Version = (ulong)DateTime.UtcNow.ToBinary();
            Entries.Save(entry);
        }
    }
}