namespace Christophilus.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    
    public class LoginController : Controller
    {
        public ActionResult Show()
        {
            dotless.Core.EngineFactory eng = null;
            ViewBag.Eng = eng;
            return View();
        }
    }
}
