namespace Christophilus.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Security.Cryptography;
    using Christophilus.Extensions;
    using MongoDB.Bson.Serialization.Attributes;
    using MongoDB.Driver;
    using MongoDB.Bson;
    
    /// <summary>
    /// Represents a single journal entry.
    /// </summary>
    public class JournalEntry
    {
        private string body = null;

        public JournalEntry()
        {
        }

        public JournalEntry(string user, DateTime day)
        {
            if (string.IsNullOrEmpty(user)) 
            {
                throw new ArgumentNullException("user");
            }

            this.User = user;
            this.Day = day.ToString("yyyy-MM-dd");
        }

        [BsonId]
        public string Id
        { 
            get
            {
                return (User + "@" + Day).Sha1Hash();
            }

            private set
            {
                // Required for serialization, but is really read-only.
            }
        }

        public string User { get; private set; }

        public string Day { get; private set; }

        public string Summary { get; private set; }

        public string Body 
        { 
            get
            {
                return this.body;
            }
            set
            {
                this.body = value;
                this.Summary = ComputedSummary();
            }
        }

        private string ComputedSummary()
        {
            if (string.IsNullOrEmpty(this.Body))
            {
                return string.Empty;
            }

            var sentenceEnd = this.Body.IndexOfAny(".!?".ToCharArray());
            if (sentenceEnd < 0)
            {
                sentenceEnd = this.Body.Length;
            }

            return this.Body.Slice(0, Math.Min(sentenceEnd + 1, 256));
        }
    }
}