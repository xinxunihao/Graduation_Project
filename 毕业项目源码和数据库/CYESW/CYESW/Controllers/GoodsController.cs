using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CYESW.Models;

namespace CYESW.Controllers
{
    public class GoodsController : Controller
    {
        CYESWEntities db = new CYESWEntities();
        // GET: Goods
        public ActionResult Index()
        {
            TempData["Title"] = "商品详情";
            return View();
        }
    }
}