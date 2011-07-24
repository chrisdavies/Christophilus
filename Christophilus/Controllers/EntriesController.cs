﻿namespace Christophilus.Controllers
{
    using System;
    using System.Web.Mvc;
    using Christophilus.Models;
    using System.Web;
    using System.Net;
    using Christophilus.Extensions;

    [Authorize]
    public class EntriesController : Controller
    {
        public string UserEmail 
        { 
            get { return System.Web.HttpContext.Current.User.Identity.Name; } 
        }

        public string UserId
        {
            get { return UserEmail.Sha1Hash(); }
        }

        public ActionResult Edit(DateTime? day)
        {
            if (!day.HasValue)
            {
                day = DateTime.Now;
            }

            var entry = JournalEntryService.GetEntry(UserId, day.Value) ??
                new JournalEntry(UserId, day.Value);

            return View(entry);
        }

        public ActionResult Index(int page = 0)
        {
            var entries = JournalEntryService.GetEntries(UserId, page);
            return View(entries);
        }

        [ValidateInput(false)]
        public ActionResult Update(JournalEntry entry)
        {
            entry.User = UserId;
            JournalEntryService.Save(entry);
            return Json(new { version = entry.Version });
        }
    }
}
