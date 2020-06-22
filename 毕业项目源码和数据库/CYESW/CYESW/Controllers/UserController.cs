using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CYESW.Models;
using PagedList;
using System.Web.Mvc.Ajax;
using Newtonsoft.Json;

namespace CYESW.Controllers
{
    
    public class UserController : Controller
    {
        CYESWEntities db = new CYESWEntities();
        // GET: User,type表示1上架，2下架，3卖出，4删除
        public ActionResult UserIndex(int? page,int? type)
        {
            //本来只要写这些。。
            TempData["Title"] = "个人中心";
            if (Session["user"]==null)
            {
                //TempData["exe"] = "请先登录！";
                //return RedirectToAction("Login", "Home");
                Session["user"] = db.UserInfo.Find(1);//便于调试，项目完成后可注释
            }
            UserInfo user = Session["user"] as UserInfo;
            //Ⅰ第一次网页打开需要数据，发布商品，否则报错（下面都是）

            var goods = db.Goods.Where(p=>p.UserId== user.UserId).ToList();//查询该用户所有商品，下面细分
            type = type ?? 1;
            ViewBag.type = type;
            if (type == 1)//查询上架商品
            {
                goods = goods.Where(p => p.IsState == 1).ToList();
            }
            else if (type == 2)//查询下架商品
            {
                goods = goods.Where(p => p.IsState == 2).ToList();
            }
            else if (type == 3)//查询购买中商品--已生成订单表
            {

                goods = goods.Where(p => p.IsState == 3).ToList();
            }
            else
            {//查询已购买商品
                goods = goods.Where(p => p.IsState == 4).ToList();
            }
            
            //分页
            int pageNumber = page ?? 1;//页码
            int pageSize = 3;//每页个数
            return View(goods.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult UserExit()
        {
            Session["user"] = null;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult userpage()
        {
            return View();
        }

        //public ActionResult SearchIndex( int? page)
        //{
        //    int pageNumber = page ?? 1;//page为空就赋值1
        //    int pageSize = 3;
        //    var persons = db.UserInfo.ToList();
        //    return View(persons.ToPagedList(pageNumber, pageSize));
        //}

        //下面为六大分布页

        /// <summary>
        /// 发布分布页
        /// </summary>
        /// <returns></returns>
        public ActionResult Publish( )
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Love(int? type)
        {
            db.Configuration.LazyLoadingEnabled = false;//关闭延迟加载
            //使用贪婪加载
            var goodlist = db.Love.Include("Goods").ToList();
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

        public ActionResult Info()
        {
            return View();
        }
        public ActionResult Set()
        {
            return View();
        }
        public ActionResult Approve()
        {
            return View();
        }
        public ActionResult FeedBack()
        {
            return View();
        }

        //擦亮商品
        public ActionResult Updata_c(int id)
        {
            
            TempData["exe"] = "擦亮成功！";
            return RedirectToAction("UserIndex",new { id=1});
        }
        //下架商品
        public ActionResult Updata_x(int id)
        {
            db.Goods.Find(id).IsState = 2;
            db.SaveChanges();
            TempData["exe"] = "已下架！";
            return RedirectToAction("UserIndex", new { id = 1 });
        }

    }
}