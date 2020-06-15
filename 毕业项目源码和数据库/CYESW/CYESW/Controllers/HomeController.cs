using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CYESW.Models;
using Newtonsoft.Json;

namespace CYESW.Controllers
{
    public class HomeController : Controller
    {
        CYESWEntities db = new CYESWEntities();
        public ActionResult Index()
        {
            ViewBag.Title = "首页";
            return View();
        }

        public ActionResult getdata_ajax()
        {
            db.Configuration.LazyLoadingEnabled = false;//关闭延迟加载
            //使用贪婪加载
            var goodlist = db.Goods.Include("UserInfo").Include("Goodsimage").ToList();
            //var goodlistimg = db.Goods.Include("Goodsimage").Where(p => p.Goodsaddress.TypeName == "湖南").ToList();
            //借用newtonsoft。json包进行序列化,防止导航属性循环
            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            string ret = JsonConvert.SerializeObject(goodlist, jsSettings);
            //string retimg = JsonConvert.SerializeObject(goodlistimg, jsSettings);
            //转换为json格式输出
            var result = Json(ret, JsonRequestBehavior.AllowGet);
            return result;
        }

        public ActionResult Selete()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}