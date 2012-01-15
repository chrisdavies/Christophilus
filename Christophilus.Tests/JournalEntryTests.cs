namespace Christophilus.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Should;
    using Christophilus.Models;
    using Statement = Should.Core.Assertions.Assert;
    using System.Security.Cryptography;

    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class JournalEntryTests
    {
        [TestMethod]
        public void User_cannot_be_null()
        {
            Statement.Throws<ArgumentNullException>(
                () => new JournalEntry(null, DateTime.Now));
        }

        [TestMethod]
        public void User_cannot_be_empty()
        {
            Statement.Throws<ArgumentNullException>(
                () => new JournalEntry(string.Empty, DateTime.Now));
        }

        [TestMethod]
        public void Id_should_be_a_hash_of_user_and_day()
        {
            var entry = new JournalEntry("foo@bar.com", DateTime.Now);

            using (var sha1 = new SHA1CryptoServiceProvider())
            {
                var unhashed = Encoding.ASCII.GetBytes(entry.User + "@" + entry.Day);
                var expectedId = Convert.ToBase64String(sha1.ComputeHash(unhashed));
                entry.Id.ShouldEqual(expectedId);
            }
        }

        [TestMethod]
        public void Day_should_be_formatted_yyyy_mm_dd()
        {
            var today = DateTime.Now;
            var entry = new JournalEntry("foo@bar.com", today);
            entry.Day.ShouldEqual(today.ToString(DataStore.DateFormat));
        }
        
        [TestMethod]
        public void Summary_should_default_to_empty()
        {
            var entry = new JournalEntry("foo@bar.com", DateTime.Now);
            entry.Summary.ShouldEqual(string.Empty);
        }

        [TestMethod]
        public void Summary_should_be_the_first_sentence_of_body()
        {
            var entry = new JournalEntry("foo@bar.com", DateTime.Now);
            entry.Body = "Hello world, and everyone! Nope";
            entry.Summary.ShouldEqual("Hello world, and everyone!");
            
            entry.Body = "Hello.  My name is John!";
            entry.Summary.ShouldEqual("Hello.");

            entry.Body = "What is this? And stuff.";
            entry.Summary.ShouldEqual("What is this?");

            entry.Body = "What?";
            entry.Summary.ShouldEqual("What?");

            entry.Body = "No punctuation";
            entry.Summary.ShouldEqual("No punctuation");
        }

        [TestMethod]
        public void Summary_should_no_longer_than_256_chars()
        {
            var entry = new JournalEntry("foo@bar.com", DateTime.Now);
            entry.Body = new string('h', 258);
            entry.Summary.Length.ShouldEqual(256);
        }
    }
}
