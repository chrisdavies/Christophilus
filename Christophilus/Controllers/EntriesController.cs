namespace Christophilus.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Christophilus.Extensions;
    using Christophilus.Models;
    using Sparc.TagCloud;
    using Sparc.Mvc;
    using System.Linq;

    [Authorize]
    public class EntriesController : SparcBaseController
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

        [OutputCache(Duration = 500)]
        public ActionResult TagCloud(DateTime start, DateTime end)
        {
            var phrases = JournalEntryService.GetEntries(UserId, start, end);
            var model = new TagCloudAnalyzer()
                .ComputeTagCloud(phrases)
                .Shuffle();
            return Json(new { tags = model });
        }

        [ValidateInput(false)]
        public ActionResult Update(JournalEntry entry)
        {
            entry.User = UserId;
            JournalEntryService.Save(entry);
            return Json(new { version = entry.Version });
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            ViewBag.UserEmail = UserEmail;
        }
    }
}
