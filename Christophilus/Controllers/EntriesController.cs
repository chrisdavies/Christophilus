namespace Christophilus.Controllers
{
    using System;
    using System.Web.Mvc;

    public class EntriesController : Controller
    {
        public ActionResult Edit(DateTime? day)
        {
            if (!day.HasValue)
            {
                day = DateTime.Now;
            }

            return View();
        }
    }
}
