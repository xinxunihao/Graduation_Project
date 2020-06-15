using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CYESW.Models;

namespace CYESW.Controllers
{
    
    public class UserController : Controller
    {
        CYESWEntities db = new CYESWEntities();
        // GET: User
        public ActionResult UserIndex()
        {
            return View();
        }
    }
}