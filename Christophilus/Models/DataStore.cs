namespace Christophilus.Models
{
    using System.Configuration;
    using MongoDB.Driver;

    public static class DataStore
    {
        static DataStore()
        {
            var mongoUrl = ConfigurationManager.AppSettings["mongo"];
            DB = MongoServer.Create(mongoUrl).GetDatabase("christophilus");
        }

        public static MongoDatabase DB { get; set; }
    }
}