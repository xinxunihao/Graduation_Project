using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CYESW.Models;
using Newtonsoft.Json;
using PagedList;

namespace CYESW.Controllers
{
    public class GoodsController : Controller
    {
        CYESWEntities db = new CYESWEntities();
        // GET: Goods
        public ActionResult Index(int? id)
        {
            TempData["Title"] = "商品详情";
            ViewData["goodsinfo"] = db.Goods.Find(id);
            ViewData["user_1"] = db.UserInfo.Find(db.Goods.Find(id).UserId);
            return View();
        }

        //分类视图，typeid表示类别
        /// <summary>
        /// 类别板块页，
        /// </summary>
        /// <param name="typeid">类别id（八种）</param>
        /// <param name="page">分页id</param>
        /// <returns></returns>
        public ActionResult IndexTwo(int? typeid,int? page,int? paixu,int? Price_min, int? Price_max, string Name="")
        {
            int? goodsaddres = 0, isnew = 0;//地区限制，是否全新
            ViewBag.goodsaddres = 0;
            ViewBag.IsNew = 0;
            if (Request.QueryString["Goodsaddress"] !=null)
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
            if (Session["typeid"]==null)
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
            var list = db.Goods.Where(p=>p.GoodsTypeId>13&&p.IsState==1).ToList();//八大分类--越过八大主分类，五个电脑小分类
            //分类，通用
            if (typeid==1)
            {
                list = list.Where(p => p.GoodsType.GoodsType2.GoodsTypeId == 9|| p.GoodsType.GoodsType2.GoodsTypeId == 10 || p.GoodsType.GoodsType2.GoodsTypeId == 11 || p.GoodsType.GoodsType2.GoodsTypeId == 12 || p.GoodsType.GoodsType2.GoodsTypeId == 13 ).ToList();//电脑又两层，需要查两次--查询父id的父id为1的商品
            }
            else if(typeid == 8)
            {
                list = db.Goods.Where(p => p.GoodsTypeId == null).ToList();//‘其他’类别，
            }
            else
            {
                list = list.Where(p => p.GoodsType.GoodsType2.GoodsTypeId == typeid).ToList();//剩下六类有两层，需要查两次--查询父id的父id为1的商品
            }
            var goodslist = list.Where(p => p.Name.Contains(Name.Trim())).ToList();//模糊查询
            ViewBag.Name = Name;
            //排序
            if (paixu==1)//默认排序
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
            if (isnew ==1) goodslist = goodslist.Where(p => p.IsNew == isnew).ToList();
            //分页
            int pageNumber = page ?? 1;//页码
            int pageSize = 9;//每页个数
            ViewData["goodslist"] = goodslist.ToPagedList(pageNumber,pageSize);
            return View(); 
        }


        public ActionResult SearchList(List<Goods> list)
        {
            return View(list);
        }
        //public ActionResult getdata_ajax()
        //{
        //    db.Configuration.LazyLoadingEnabled = false;//关闭延迟加载
        //    //使用贪婪加载
        //    var goodlist = db.Goods.Include("UserInfo").Include("Goodsimage").ToList();
        //    //var goodlistimg = db.Goods.Include("Goodsimage").Where(p => p.Goodsaddress.TypeName == "湖南").ToList();
        //    //借用newtonsoft。json包进行序列化,防止导航属性循环
        //    JsonSerializerSettings jsSettings = new JsonSerializerSettings();
        //    jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        //    string ret = JsonConvert.SerializeObject(goodlist, jsSettings);
        //    //string retimg = JsonConvert.SerializeObject(goodlistimg, jsSettings);
        //    //转换为json格式输出
        //    var result = Json(ret, JsonRequestBehavior.AllowGet);
        //    return result;
        //}

    }
}