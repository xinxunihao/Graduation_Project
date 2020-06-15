using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CYESW.Models;

namespace CYESW.Controllers
{
    public class GoodsaddressController : Controller
    {
        CYESWEntities db = new CYESWEntities();
        // GET: Goodsaddress
        public ActionResult GoodsaddressIndex()
        {
            ViewData["diqu"] = db.Goodsaddress.Where(p => p.GoodsaddressBId == null).ToList();
            ViewData["sheng"] = db.Goodsaddress.Where(p => p.GoodsaddressBId >= 1 && p.GoodsaddressBId <= 7).ToList();
            ViewData["shi"] = db.Goodsaddress.Where(p => p.GoodsaddressBId>=8&& p.GoodsaddressBId <= 34).ToList();
            return View();
        }
    }
}