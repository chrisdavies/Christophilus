namespace Christophilus.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using MongoDB.Driver;

    public static class DataStore
    {
        static DataStore()
        {
            DB = MongoServer.Create().GetDatabase("Christophilus");
        }

        public static MongoDatabase DB { get; set; }
    }
}