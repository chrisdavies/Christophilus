﻿namespace Christophilus.Models
{
    using System;
    using System.Text.RegularExpressions;
    using Christophilus.Extensions;
    using MongoDB.Bson.Serialization.Attributes;
    
    /// <summary>
    /// Represents a single journal entry.
    /// </summary>
    public class JournalEntry
    {
        private string body = null;

        public JournalEntry()
        {
            this.Summary = string.Empty;
        }

        public JournalEntry(string user, DateTime day)
            : this()
        {
            if (string.IsNullOrEmpty(user)) 
            {
                throw new ArgumentNullException("user");
            }

            this.User = user;
            this.Day = day.ToString(DataStore.DateFormat);
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

        public string User { get; set; }

        public string Day { get; set; }

        public ulong Version { get; set; }

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

            return StripHtml(FirstSentence());
        }

        private static string StripHtml(string str)
        {
            return Regex.Replace(str, "<.*?>", string.Empty);
        }

        private string FirstSentence()
        {
            return this.Body.Slice(0, Math.Min(EndOfFirstSentence() + 1, 256));
        }

        private int EndOfFirstSentence()
        {
            var sentenceEnd = this.Body.IndexOfAny(".!?".ToCharArray());
            if (sentenceEnd < 0)
            {
                sentenceEnd = this.Body.Length;
            }

            return sentenceEnd;
        }
    }
}