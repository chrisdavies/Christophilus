namespace Christophilus.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    public static class RoutingEx
    {
        public static void Get(
            this RouteCollection context,
            string url,
            string fullyQualifiedAction,
            string routeName = null)
        {
            context.Do("GET", url, fullyQualifiedAction, routeName);
        }

        public static void Post(
            this RouteCollection context,
            string url,
            string fullyQualifiedAction,
            string routeName = null)
        {
            context.Do("POST", url, fullyQualifiedAction, routeName);
        }

        public static void Do(
            this RouteCollection context,
            string verb,
            string url,
            string fullyQualifiedAction,
            string routeName = null)
        {
            var constraint = verb == "*" ? null : new { httpMethod = new HttpMethodConstraint(verb) };

            context.MapRoute(
                routeName ?? fullyQualifiedAction,
                url,
                GetDefaults(fullyQualifiedAction),
                constraint);
        }

        public static void Get(
            this AreaRegistrationContext context,
            string url,
            string fullyQualifiedAction,
            string routeName = null)
        {
            context.Do("GET", url, fullyQualifiedAction, routeName);
        }

        public static void Post(
            this AreaRegistrationContext context,
            string url,
            string fullyQualifiedAction,
            string routeName = null)
        {
            context.Do("POST", url, fullyQualifiedAction, routeName);
        }

        public static void Do(
            this AreaRegistrationContext context,
            string verb,
            string url,
            string fullyQualifiedAction,
            string routeName = null)
        {
            var constraint = verb == "*" ? null : new { httpMethod = new HttpMethodConstraint(verb) };

            context.MapRoute(
                routeName ?? fullyQualifiedAction,
                url,
                GetDefaults(fullyQualifiedAction),
                constraint);
        }

        private static object GetDefaults(string fullyQualifiedAction)
        {
            var split = fullyQualifiedAction.Split(".".ToCharArray());
            return new
            {
                action = split[split.Length - 1],
                controller = split[split.Length - 2],
                id = UrlParameter.Optional
            };
        }
    }

}