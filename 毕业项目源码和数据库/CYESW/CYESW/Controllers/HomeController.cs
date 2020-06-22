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
            TempData["Title"] = "首页";
            //默认用户所在地为长沙
            if (Session["adress"]==null)
            {
                Session["adress"] = "长沙";
            }
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

        public ActionResult Login()
        {
            TempData["Title"] = "登录/注册";//标题
            if (Session["adress"] == null)
            {
                Session["adress"] = "长沙";
            }
            return View();
        }
        
        [HttpPost]
        public ActionResult Login(string UserEmile, string UserPwd)
        {
            try
            {
                var user = db.UserInfo.Where(p => p.UserEmile == UserEmile && p.UserPwd== UserPwd).SingleOrDefault();
                if (user!=null)
                {
                    Session["user"] = user;
                    TempData["exe"] = "登录成功";
                    if (user.IsManage==1)
                    {
                        TempData["exe"] = "管理员登录成功";
                        return RedirectToAction("Index","BackgRound");
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Title"] = "登录/注册";//标题
                    TempData["exe"] = "用户名或密码错误";
                    return View();
                }
                
            }
            catch (Exception ex)
            {
                TempData["exe"] = "抱歉出现异常，具体为："+ex.Message+"/r请与管理员联系。";
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public ActionResult Register(string UserEmile1,string UserName, string UserPwd1)
        {
            try
            {
                var aemile = db.UserInfo.Where(p => p.UserEmile == UserEmile1).SingleOrDefault();
                if (aemile != null)
                {
                    TempData["exe"] = "该邮箱已被注册";
                    return RedirectToAction("Login");
                }
                UserInfo user = new UserInfo() { UserEmile = UserEmile1, UserName = UserName, UserPwd = UserPwd1,Images= "man.jpg",IsDelete=0,AddTime=DateTime.Now,IsManage=0,moneys=0,Info="这个人很懒，什么都没有写。",Age=0 };//添加默认数据，也可以写个触发器
                db.UserInfo.Add(user);
                if (db.SaveChanges()>0)
                {
                    Session["user"] = user;
                    TempData["exe"] = "注册成功";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["exe"] = "注册失败？";
                    return RedirectToAction("Login");
                }
                
            }
            catch (Exception ex)
            {
                TempData["exe"] = "抱歉出现异常，具体为：" + ex.Message + "/r请与管理员联系。";
                return RedirectToAction("Index");
            }
        }

        
    }
}