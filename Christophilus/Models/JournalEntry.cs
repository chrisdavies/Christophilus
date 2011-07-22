namespace Christophilus.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Security.Cryptography;
    using Christophilus.Extensions;

    /*
      TODO: Mongo integration:
            Id => SHA5[user + day]
            EnsureIndex(User, Day)
    */

    /// <summary>
    /// Represents a single journal entry.
    /// </summary>
    public class JournalEntry
    {
        public JournalEntry(string user, DateTime day)
        {
            if (string.IsNullOrEmpty(user)) 
            {
                throw new ArgumentNullException("user");
            }

            this.User = user;
            this.Day = day.ToString("yyyy-MM-dd");
        }

        public string Id 
        { 
            get
            {
                return (User + "@" + Day).Sha1Hash();
            } 
        }

        public string User { get; set; }

        public string Day { get; set; }

        public string Summary 
        {
            get
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

        public string Body { get; set; }
    }
}