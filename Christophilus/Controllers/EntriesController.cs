namespace Christophilus.Controllers
{
    using System;
    using System.Web.Mvc;
    using Christophilus.Models;
    using System.Web;
    using System.Net;
    using Christophilus.Extensions;

    public class EntriesController : Controller
    {
        public ActionResult Show(DateTime day)
        {
            var entry = JournalEntryService.GetEntry(CurrentUser, day);

            if (entry == null)
            {
                throw new HttpException(
                    (int)HttpStatusCode.NotFound, 
                    "No entry could be found for {0}.".Formatted(entry.Day));
            }

            return View(entry);
        }

        public ActionResult Edit(DateTime? day)
        {
            if (!day.HasValue)
            {
                day = DateTime.Now;
            }

            var entry = JournalEntryService.GetEntry(CurrentUser, day.Value) ??
                new JournalEntry(CurrentUser, day.Value);

            return View(entry);
        }

        public ActionResult Index(int page = 0)
        {
            var entries = JournalEntryService.GetEntries(CurrentUser, page);
            return View(entries);
        }

        [ValidateInput(false)]
        public ActionResult Update(JournalEntry entry)
        {
            entry.User = CurrentUser;
            JournalEntryService.Save(entry);
            return Json(new { version = entry.Version });
        }

        public string CurrentUser { get { return System.Web.HttpContext.Current.User.Identity.Name; } }
    }
}
