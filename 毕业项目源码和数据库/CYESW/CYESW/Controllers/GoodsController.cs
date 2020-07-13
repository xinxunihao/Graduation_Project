using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CYESW.Models;
using Newtonsoft.Json;
using PagedList;
using CYESW.Filter;

namespace CYESW.Controllers
{
    [Login]
    public class GoodsController : Controller
    {
        CYESWEntities db = new CYESWEntities();
        // GET: Goods
        [Login]
        public ActionResult Index(int? id = 9)
        {
            //if (Session["user"]!=null)//登录的用户
            //{
            //    //TempData["exe"] = "请先登录，谢谢！";
            //    //return RedirectToAction("Login", "Home");
            //    Session["user"] = db.UserInfo.Find(1);//便于调试，项目完成后可注释
            //}
            ViewBag.WebOut = db.WebOut.ToList();
            List<WebIn> list1 = db.WebIn.ToList();
            foreach (var item in list1)
            {
                item.Theat = item.Tpromote + item.Goods.munber + item.Goods.UserInfo.Goods.Count();
            }
            db.SaveChanges();
            list1 = list1.OrderByDescending(p => p.Theat).ToList();
            ViewBag.WebIn = list1;//站内推广！按热度排名



            TempData["Title"] = "商品详情";
            Goods goods = db.Goods.Find(id);
            if (goods.IsState!=1)
            {
                TempData["Title"] = "首页";
                TempData["exe"] = "该商品已下架或已卖出！";
                return RedirectToAction("Index", "Home");
            }
            goods.munber++;
            db.SaveChanges();


            List<GoodsType> list = db.GoodsType.Where(p => p.GoodsTypeBId == goods.GoodsType.GoodsTypeBId).ToList();//获取相关类目



            ViewBag.GoodsType = list;
            ViewData["goodsinfo"] = goods;
            ViewData["user_1"] = db.UserInfo.Find(db.Goods.Find(id).UserId);//卖出的用户
            return View();
        }

        //分类视图，typeid表示类别
        /// <summary>
        /// 类别板块页，
        /// </summary>
        /// <param name="typeid">类别id（八种）</param>
        /// <param name="page">分页id</param>
        /// <returns></returns>

        public ActionResult IndexTwo(int? typeid, int? page, int? paixu, int? Price_min, int? Price_max, string Name = "")
        {
            int? goodsaddres = 0, isnew = 0;//地区限制，是否全新
            ViewBag.goodsaddres = 0;
            ViewBag.IsNew = 0;
            if (Request.QueryString["Goodsaddress"] != null)
            {
                goodsaddres = int.Parse(Request.QueryString["Goodsaddress"].ToString());
                ViewBag.goodsaddres = goodsaddres;
            }
            if (Request.QueryString["IsNew"] != null)
            {
                isnew = 1;
                ViewBag.IsNew = 1;
            }
            paixu = paixu ?? 1;
            if (Session["typeid"] == null)
            {

                Session["typeid"] = typeid = typeid ?? 1;
            }
            else
            {
                Session["typeid"] = typeid;
            }
            //设置标题名,地址表
            var address = db.Goodsaddress.Where(p => p.GoodsaddressId > 34).ToList();
            ViewBag.address = address;
            string name = db.GoodsType.Find(typeid).TypeName;
            TempData["Title"] = name;
            var list = db.Goods.Where(p => p.GoodsTypeId > 13 && p.IsState == 1).ToList();//八大分类--越过八大主分类，五个电脑小分类
            //分类，通用
            if (typeid == 1)
            {
                list = list.Where(p => p.GoodsType.GoodsType2.GoodsTypeId == 9 || p.GoodsType.GoodsType2.GoodsTypeId == 10 || p.GoodsType.GoodsType2.GoodsTypeId == 11 || p.GoodsType.GoodsType2.GoodsTypeId == 12 || p.GoodsType.GoodsType2.GoodsTypeId == 13).ToList();//电脑又两层，需要查两次--查询父id的父id为1的商品
            }
            else if (typeid == 8)
            {
                list = db.Goods.Where(p => p.GoodsTypeId == 8).ToList();//‘其他’类别，
            }
            else
            {
                list = list.Where(p => p.GoodsType.GoodsType2.GoodsTypeId == typeid).ToList();//剩下六类有两层，需要查两次--查询父id的父id为1的商品
            }
            var goodslist = list.Where(p => p.Name.Contains(Name.Trim())).ToList();//模糊查询
            ViewBag.Name = Name;
            //排序
            if (paixu == 1)//默认排序
            {
                ViewBag.paixu = 1;
            }
            else if (paixu == 2)//发布时间
            {
                goodslist = goodslist.OrderBy(p => p.UpdateTime).ToList();
                ViewBag.paixu = 2;
            }
            else//价格排序
            {
                goodslist = goodslist.OrderBy(p => p.Price).ToList();
                ViewBag.paixu = 3;
            }
            //价格区间
            if (Price_min != null) { goodslist = goodslist.Where(p => p.Price >= Price_min).ToList(); ViewBag.Price_min = Price_min; }
            if (Price_max != null) { goodslist = goodslist.Where(p => p.Price <= Price_max).ToList(); ViewBag.Price_max = Price_max; }
            //地区查询
            if (goodsaddres != 0) goodslist = goodslist.Where(p => p.GoodsaddressId == goodsaddres).ToList();
            //只查全新
            if (isnew == 1) goodslist = goodslist.Where(p => p.IsNew == isnew).ToList();
            //分页
            int pageNumber = page ?? 1;//页码
            int pageSize = 9;//每页个数
            ViewData["goodslist"] = goodslist.ToPagedList(pageNumber, pageSize);
            return View();
        }


        public ActionResult SearchList(List<Goods> list)
        {
            return View(list);
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


        public ActionResult Release()
        {
            db.Configuration.LazyLoadingEnabled = true;//关闭延迟加载
            ViewData["goodtype"] = db.GoodsType.ToList();
            return View();
        }

        [Login]
        public ActionResult Release_1(int typeid = 1)
        {
            try
            {
                if (Session["user"] == null)
                {
                    //TempData["exe"] = "请先登录，谢谢！";
                    //return RedirectToAction("Login", "Home");
                    Session["user"] = db.UserInfo.Find(1);//便于调试，项目完成后可注释
                }
                Session["fabulingshi"] = typeid;
                ViewBag.TypeName = db.GoodsType.Find(typeid).TypeName;
                return View();
            }
            catch (Exception ex)
            {
                TempData["exe"] = "出现未知异常，请联系管理员。异常：" + ex.Message;
                return View();
            }

        }

        //用来给修改界面过来的跳转
        public ActionResult Release_2() {
            return View();
        }

        [HttpPost]
        [Login]
        public ActionResult Release_2(Goods goods, List<HttpPostedFileBase> Photos)
        {
            try
            {
                UserInfo user = Session["user"] as UserInfo;
                if (db.Goods.Where(p => p.Name == goods.Name && p.Info == goods.Info && p.Price == goods.Price && p.UserId == user.UserId).Count() > 0)//判断是否刷新提交----所有的字段对比数据库是否已经有了。。。
                {
                    TempData["exe"] = "请不要刷新，重复提交！";
                    return View();
                }
                if (Photos != null)
                {
                    if (goods != null)
                    {
                        //添加初始化数据

                        goods.CreateTime = DateTime.Now;
                        goods.UpdateTime = DateTime.Now;
                        goods.munber = 0;
                        goods.IsState = 1;
                        goods.UserId = user.UserId;
                        db.Goods.Add(goods);
                        if (db.SaveChanges() > 0)//影响的行数大于0
                        {
                            foreach (var item in Photos)//将图片集合遍历出来
                            {
                                string fileName = Path.GetFileName(item.FileName);
                                item.SaveAs(Server.MapPath("~/images/img/" + fileName));
                                Goodsimage image = new Goodsimage()
                                {
                                    GoodsId = goods.GoodsId,
                                    images = fileName
                                };
                                db.Goodsimage.Add(image);
                            }

                            db.SaveChanges();
                            TempData["exe"] = "发布成功！";
                            goods = db.Goods.Find(goods.GoodsId);
                            Session["goods_ls"] = goods;
                        }
                        else
                        {
                            TempData["exe"] = "发布失败????";
                            return View("Release_1");
                        }
                        return View();
                    }
                    else
                    {
                        TempData["exe"] = "数据上传失败！";
                        return View("Release_1");
                    }
                }
                else
                {
                    TempData["exe"] = "图片上传失败！";
                    return View("Release_1");
                }
            }
            catch (Exception ex)
            {
                TempData["exe"] = "出现未知异常，请联系管理员。异常：" + ex.Message;
                return View("Release_1");
            }

        }


        public ActionResult Updata_g(int id)
        {
            Goods goods = db.Goods.Find(id);
            return View(goods);
        }


        [HttpPost]
        public ActionResult Updata_g(Goods goods, List<HttpPostedFileBase> Photos)
        {
            try
            {
                Goods goods1 = db.Goods.Find(goods.GoodsId);
                //手动修改吧~~
                goods1.IsState = 1;//将状态修改为上架；
                goods1.UpdateTime = DateTime.Now;

                goods1.Info = goods.Info;
                goods1.Name = goods.Name;
                goods1.GoodsaddressInfo = goods.GoodsaddressInfo;
                goods1.Ipone = goods.Ipone;
                goods1.IsNew = goods.IsNew;
                goods1.Peisong = goods.Peisong;
                goods1.Price = goods.Price;
                goods1.Price_Yuan = goods.Price_Yuan;

                db.Entry(goods1).State = System.Data.Entity.EntityState.Modified;//修改
                db.SaveChanges();

                foreach (var item in Photos)//将图片集合遍历出来
                {
                    if (item==null)
                    {
                        continue;
                    }
                    string fileName = Path.GetFileName(item.FileName);
                    item.SaveAs(Server.MapPath("~/images/img/" + fileName));
                    Goodsimage image = new Goodsimage()
                    {
                        GoodsId = goods.GoodsId,
                        images = fileName
                    };
                    db.Goodsimage.Add(image);
                }
                db.SaveChanges();

                Session["goods_ls"] = goods1;//到Release_2页面需要的数据（商品）
                return View("Release_2");
            }
            catch (Exception ex)
            {
                TempData["exe"] = "出现未知异常，请联系管理员。异常：" + ex.Message;
                Goods goods1 = db.Goods.Find(goods.GoodsId);
                return View(goods1);
            }

        }

        
        //使用删除图片
        public ActionResult Updata_g_deleteimg(int id,int goodsid)
        {
            db.Goodsimage.Remove(db.Goodsimage.Find(id));
            db.SaveChanges();
            TempData["exe"] = "删除成功！";
            return RedirectToAction("Updata_g",new { id = goodsid });
        }

        //商家的订单详情
        public ActionResult Order_1(int Orderid)
        {
            TempData["Title"] = "订单详情";
            Orders order1 = db.Orders.Find(Orderid);
            return View(order1);
        }

        //商家发货
        public ActionResult Order_1_wl(int id, string Wuliu)
        {
            try
            {
                Orders orders = db.Orders.Find(id);
                orders.IsState = 2;
                orders.BuyTime2 = DateTime.Now;
                orders.Wuliu = Wuliu;
                db.SaveChanges();
                return RedirectToAction("Order_1", new { Orderid = id });
            }
            catch (Exception ex)
            {
                TempData["exe"] = "出现未知异常，请联系管理员。异常：" + ex.Message;
                return RedirectToAction("Order_1", new { Orderid = id });
            }
        }

        //买家的订单详情
        public ActionResult Order_2(int Orderid)
        {
            TempData["Title"] = "订单详情";
            Orders order1 = db.Orders.Find(Orderid);
            return View(order1);
        }
        //买家确认收货
        public ActionResult Order_2_1(int? id)
        {
            try
            {
                Orders orders = db.Orders.Find(id);
                orders.IsState = 3;
                orders.BuyTime3 = DateTime.Now;
                orders.Goods.IsState = 4;//将商品表状态改为已完成
                db.SaveChanges();
                TempData["exe"] = "确认收获成功！给宝贝一个评价吧";
                return RedirectToAction("Order_2", new { Orderid = id });
            }
            catch (Exception ex)
            {
                TempData["exe"] = "出现未知异常，请联系管理员。异常：" + ex.Message;
                return RedirectToAction("Order_2", new { Orderid = id });
            }

        }

        //买家评价
        [HttpPost]
        public ActionResult Order_2_2(int id, string pingjia)
        {
            try
            {
                Orders orders = db.Orders.Find(id);
                UserInfo us = Session["user"] as UserInfo;
                Texts txt = new Texts() { UserId = us.UserId, addTiem = DateTime.Now, Textbody = pingjia };
                db.Texts.Add(txt);
                db.SaveChanges();

                Pingjia_texts htxt = new Pingjia_texts() { TextsId = txt.TextsId, OrdersId = id, States = 0 };
                db.Pingjia_texts.Add(htxt);
                db.SaveChanges();

                orders.IsState = 4;
                orders.BuyTime4 = DateTime.Now;

                db.SaveChanges();
                TempData["exe"] = "评价成功，交易结束！";
                return RedirectToAction("Order_2", new { Orderid = id });
            }
            catch (Exception ex)
            {
                TempData["exe"] = "出现未知异常，请联系管理员。异常：" + ex.Message;
                return RedirectToAction("Order_2", new { Orderid = id });
            }

        }


        
        //商品收藏--用ajax实现order_state
        public string AddLove(int id)
        {
            UserInfo user = Session["user"] as UserInfo;
            if (db.Love.Where(p => p.UserId == user.UserId && p.GoodsId == id).Count() > 0)
            {
                return "你已经添加过了！";
            }
            else
            {
                Love love_1 = new Love()
                {
                    GoodsId = id,
                    UserId = user.UserId,
                    addTiem = DateTime.Now
                };
                db.Love.Add(love_1);
                db.SaveChanges();
                return "添加成功！";
            }

        }

        public ActionResult Play_buy(int GoodsId)
        {
            
            return View(db.Goods.Find(GoodsId));
        }

        [HttpPost]
        public ActionResult Play_buy_addres_ajax(int userid) {//ajax获取用户收货地址集合

            db.Configuration.LazyLoadingEnabled = false;
            List<Addres> goodlist = db.Addres.Where(p => p.UserId==userid).ToList();
            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            string ret = JsonConvert.SerializeObject(goodlist, jsSettings);
            var result = Json(ret, JsonRequestBehavior.AllowGet);
            return result;
        }

        public ActionResult Play_buy_addres(int GoodsId ,int AddresId)
        {
            try
            {
                Addres addres = db.Addres.Find(AddresId);
                List<Addres> addreslist = db.Addres.Where(p => p.UserId == addres.UserId).ToList();//将该用户其他地址全部设为非默认
                foreach (var item in addreslist)
                {
                    item.IsDelete = 0;
                }
                addres.IsDelete = 1;
                db.SaveChanges();
                TempData["exe"] = "设置成功！";
                Session["user"] = db.UserInfo.Find(addres.UserId);//刷新界面用户信息（用户的收货地址）
                return RedirectToAction("Play_buy",new { GoodsId = GoodsId });
            }
            catch (Exception ex)
            {
                TempData["exe"] = "出现未知异常，请联系管理员解决。异常：" + ex.Message;
                return RedirectToAction("Play_buy", new { GoodsId = GoodsId });
            }
            
        }
        


        //商品购买
        [HttpPost]
        public ActionResult Play_buy_1(int GoodsId)
        {
            try
            {
                UserInfo us = Session["user"] as UserInfo;
                int? userid = db.Goods.Find(GoodsId).UserId;
                if (userid == us.UserId)
                {
                    TempData["Title"] = "商品详情";
                    //ViewData["goodsinfo"] = db.Goods.Find(GoodsId);
                    //ViewData["user_1"] = db.UserInfo.Find(db.Goods.Find(GoodsId).UserId);//卖出的用户
                    TempData["exe"] = "我的天啊！这是你自己发布的啊小伙子！";
                    return RedirectToAction("Index", new { id = GoodsId });
                }
                if (db.Orders.Where(p => p.GoodsId == GoodsId).Count() > 0)
                {
                    TempData["exe"] = "抱歉，该商品已被人购买！";
                    return RedirectToAction("IndexTwo");
                }
                else
                {
                    db.Goods.Find(GoodsId).IsState = 3;
                    Orders order1 = new Orders()
                    {
                        UserId2 = us.UserId,
                        UserId1 = userid,
                        GoodsId = GoodsId,
                        IsState = 1,
                        BuyTime1 = DateTime.Now
                    };
                    db.Orders.Add(order1);
                    db.SaveChanges();
                    return RedirectToAction("Order_2", new { Orderid = order1.OrdersId });
                }

            }
            catch (Exception ex)
            {
                TempData["exe"] = "出现未知异常，请联系管理员。异常：" + ex.Message;
                return RedirectToAction("IndexTwo");
            }

        }


        public ActionResult Goods_qxLove(int loveid)
        {
            try
            {
                Love love = db.Love.Find(loveid);
                db.Love.Remove(love);
                db.SaveChanges();
                TempData["exe"] = "已经取消收藏！";
                return RedirectToAction("UserIndex", "User", new { sextype = 2 });
            }
            catch (Exception ex)
            {
                TempData["exe"] = "出现未知异常，请联系管理员。异常：" + ex.Message;
                return RedirectToAction("UserIndex", "User", new { sextype = 2 });
            }
        }








        //站内推广排行；
        public ActionResult Goods_TOP()
        {
            try
            {
                List<WebIn> list = db.WebIn.ToList();
                foreach (var item in list)
                {
                    item.Theat = item.Tpromote + item.Goods.munber + item.Goods.UserInfo.Goods.Count();
                }
                list = list.OrderByDescending(p => p.Theat).ToList();
                return View(list);
            }
            catch (Exception ex)
            {
                TempData["exe"] = "出现未知异常，请联系管理员。异常：" + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }
    }
}