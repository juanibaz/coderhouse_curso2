using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace baseAdminProject.Controllers
{
    public class TestAceThemeController : Controller
    {
        //
        // GET: /TestTheme/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Form()
        {
            return View();
        }

        public ActionResult Void()
        {
            return Content("");
        }

    }
}
