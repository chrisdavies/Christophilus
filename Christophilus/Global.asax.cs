namespace Christophilus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Christophilus.Extensions;
    using MongoDB.Driver;
    using Christophilus.Models;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.Get(string.Empty, "Authentication.Show");
            routes.Get("login", "Authentication.Login");
            routes.Get("logout", "Authentication.Logout");
            routes.Get("entries/{day}/edit", "Entries.Edit");
            routes.Get("entries/{day}", "Entries.Show");
            routes.Get("entries", "Entries.Index");
            routes.Post("entries/{day}", "Entries.Update");
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            InitializeMongoDB();
        }

        private void InitializeMongoDB()
        {
            JournalEntryService.InitializeDB();
        }
    }
}