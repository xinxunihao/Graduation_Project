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
            TempData["Title"] = "切换城市";
            ViewData["diqu"] = db.Goodsaddress.Where(p => p.GoodsaddressBId == null).ToList();
            ViewData["sheng"] = db.Goodsaddress.Where(p => p.GoodsaddressBId >= 1 && p.GoodsaddressBId <= 7).ToList();
            ViewData["shi"] = db.Goodsaddress.Where(p => p.GoodsaddressBId>=8&& p.GoodsaddressBId <= 34).ToList();
            return View();
        }

        public ActionResult Update(string id,int? addres)
        {
            Session["adress"] = id;
            Session["adressid"] = addres;
            return RedirectToAction("Index","Home");
        }

        public ActionResult abcd()
        {
            return View();
        }
    }
}