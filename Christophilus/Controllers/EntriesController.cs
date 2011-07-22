namespace Christophilus.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using Christophilus.Extensions;

    public class EntriesController : Controller
    {
        public ActionResult Edit(DateTime? day)
        {
            if (!day.HasValue)
            {
                day = DateTime.Now;
            }

            ViewBag.Message = "Hello {0}!".Formatted(HttpContext.User.Identity.Name);

            return View();
        }
    }
}
