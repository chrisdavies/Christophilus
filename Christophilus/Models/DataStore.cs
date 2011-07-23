namespace Christophilus.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using MongoDB.Driver;
    using System.Configuration;

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