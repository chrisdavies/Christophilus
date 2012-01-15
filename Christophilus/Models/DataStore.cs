namespace Christophilus.Models
{
    using System.Configuration;
    using MongoDB.Driver;

    public static class DataStore
    {
        public const string DateFormat = "yyyy-MM-dd";

        static DataStore()
        {
            var mongoUrl = ConfigurationManager.AppSettings["mongo"];
            DB = MongoServer.Create(mongoUrl).GetDatabase("christophilus");
        }

        public static MongoDatabase DB { get; set; }
    }
}