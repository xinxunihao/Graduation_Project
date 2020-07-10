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
                Session["adressid"] = 186;
                Session["adress"] = "长沙";
            }
            List<LunBo> list = db.LunBo.ToList();
            return View(list);
        }

        public ActionResult getdata_ajax(string sousuo="")
        {
            db.Configuration.LazyLoadingEnabled = false;//关闭延迟加载
            //使用贪婪加载
            var goodlist = db.Goods.Include("UserInfo").Include("Goodsimage").Where(p=>p.IsState==1&&p.Name.Contains(sousuo)).ToList();
            goodlist = goodlist.OrderByDescending(p=>p.UpdateTime).ToList();

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
                    if (user.IsDelete==1)
                    {
                        TempData["Title"] = "登录/注册";//标题
                        TempData["exe"] = "该账号已被🈲封，请联系管理员解封";
                        return View();
                    }
                    Session["user"] = user;
                    TempData["exe"] = "登录成功";
                    if (user.IsManage==1)
                    {
                        Response.Redirect("/BackgRound/Index");

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
        public ActionResult Register(string UserEmile1, string UserPwd1,string validateCode)
        {
            try
            {
                if (validateCode.ToLower() != Session["vilidateCode"].ToString())
                {
                    TempData["exe"] = "验证码错误！！";
                    return RedirectToAction("Login");
                }
                var aemile = db.UserInfo.Where(p => p.UserEmile == UserEmile1).SingleOrDefault();
                if (aemile != null)
                {
                    TempData["exe"] = "该邮箱已被注册";
                    return RedirectToAction("Login");
                }
                UserInfo user = new UserInfo() { UserEmile = UserEmile1, UserName = "用户9527", UserPwd = UserPwd1,Images= "man.jpg",IsDelete=0,AddTime=DateTime.Now,IsManage=0,moneys=0,Info="这个人很懒，什么都没有写。",Age=0 , EndTime = DateTime.Now , Sex="男"};//添加默认数据，也可以写个触发器
                db.UserInfo.Add(user);
                if (db.SaveChanges()>0)
                {
                    Addres addre = new Addres()
                    {
                        IsDelete = 1,
                        Name = "请修改",
                        Phone = "xxxxxxxxxxx",
                        UserId = user.UserId,
                        Addresss1 = "湖南省-长沙市 ",
                        Addresss2 = "xx街道，xx路，xx号"
                    };
                    db.Addres.Add(addre);
                    db.SaveChanges();
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