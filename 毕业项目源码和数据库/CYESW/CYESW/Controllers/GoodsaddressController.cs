using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CYESW.Filter;
using CYESW.Models;

namespace CYESW.Controllers
{
    [Login]
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


        [Login]
        public ActionResult Jubao_1(string userid,int? address_type = 1)//userid为其他页面传过来的举报用户id
        {
            if (userid!=null)
            {
                ViewBag.id = userid;
            }
            Session["address_type"] = address_type;
            TempData["Title"] = "反馈举报";
            return View();
        }

        //举报
        public ActionResult Jubao_1_1(JuBao jubao)
        {
            try
            {
                if (db.UserInfo.Find(jubao.UserId2)==null)
                {
                    TempData["Title"] = "反馈举报";
                    TempData["exe"] = "举报失败，为查询到该用户id";
                    return RedirectToAction("Jubao_1");
                }
                jubao.States = 0;
                jubao.addTiem = DateTime.Now;
                db.JuBao.Add(jubao);
                db.SaveChanges();
                TempData["Title"] = "反馈举报";
                TempData["exe"] = "举报成功！请前往个人中心查看";
                return RedirectToAction("Jubao_1");
            }
            catch (Exception ex)
            {
                TempData["exe"] = "出现未知异常，请联系管理员解决。异常：" + ex.Message;
                return RedirectToAction("Jubao_1");
            }
        }

        //反馈
        public ActionResult Jubao_1_2(JuBao jubao)
        {
            try
            {
                jubao.States = 0;
                jubao.addTiem = DateTime.Now;
                db.JuBao.Add(jubao);
                db.SaveChanges();
                TempData["Title"] = "反馈举报";
                TempData["exe"] = "反馈成功！请前往个人中心查看";
                return RedirectToAction("Jubao_1");
            }
            catch (Exception ex)
            {
                TempData["exe"] = "出现未知异常，请联系管理员解决。异常：" + ex.Message;
                return RedirectToAction("Jubao_1");
            }
        }

        public ActionResult useraddres(int id)
        {
            List<Addres> list = db.Addres.Where(p => p.UserId == id).ToList();
            return View(list);
        }


        public ActionResult abcd()
        {
            return View();
        }
    }
}