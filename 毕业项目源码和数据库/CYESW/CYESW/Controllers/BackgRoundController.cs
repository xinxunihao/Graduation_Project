using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CYESW.Controllers
{
    public class BackgRoundController : Controller
    {
        // GET: BackgRound
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Tab()
        {
            ViewBag.exe = "nihaoa1";
            return View();
        }
    }
}