using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CYESW.Models;
using Newtonsoft.Json;
using PagedList;

namespace CYESW.Controllers
{
    [Filter.Login]
    public class BackgRoundController : Controller
    {
        CYESWEntities db = new CYESWEntities();
        // GET: BackgRound Rejist
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                Session["user"] = db.UserInfo.Find(3);
            }
            return View();
        }

        [HttpPost]//使用ajax删除
        public ActionResult Delete_Lunbo(int id)
        {
            db.LunBo.Remove(db.LunBo.Find(id));
            db.SaveChanges();
            string exe = "删除成功！";
            return Content(exe);
        }

        [HttpPost]//使用ajax删除
        public ActionResult Delete_Webin(int id)
        {
            db.WebIn.Remove(db.WebIn.Find(id));
            db.SaveChanges();
            string exe = "删除成功！";
            return Content(exe);
        }

        [HttpPost]//使用ajax删除
        public ActionResult Delete_Webout(int id)
        {
            db.WebOut.Remove(db.WebOut.Find(id));
            db.SaveChanges();
            string exe = "删除成功！";
            return Content(exe);
        }

        [HttpPost]//使用ajax删除
        public ActionResult Delete_Goods(int id)
        {
            try
            {
                string exe = "删除失败！";
                //sql语句
                string sql1 = string.Format("delete Goodsimage where  GoodsId={0}", id);//删除商品图片表
                string sql2 = string.Format("update Love set GoodsId=NULL where GoodsId={0}", id);//修改商品收藏表
                string sql3 = string.Format("delete HuDong_texts where  GoodsId={0}", id);//删除商品互动表
                string sql4 = string.Format("delete WebIn where GoodsId={0}", id);//删除站内推荐表
                if (db.Orders.Where(p => p.GoodsId == id).Count() > 0)//判断是否已经生成订单
                {
                    string sql5 = string.Format("delete Pingjia_texts where OrdersId={0}", db.Orders.Where(p => p.GoodsId == id).FirstOrDefault().OrdersId);//删除评价表
                    string sql6 = string.Format("delete Orders where GoodsId={0}", id);//删除订单表
                    int result5 = db.Database.ExecuteSqlCommand(sql5);
                    int result6 = db.Database.ExecuteSqlCommand(sql6);
                }
                string sql7 = string.Format("delete Goods where GoodsId={0}", id);//删除商品
                //执行sql语句，返回一个数字（影响的行数）
                int result1 = db.Database.ExecuteSqlCommand(sql1);
                int result2 = db.Database.ExecuteSqlCommand(sql2);
                int result3 = db.Database.ExecuteSqlCommand(sql3);
                int result4 = db.Database.ExecuteSqlCommand(sql4);
                int result7 = db.Database.ExecuteSqlCommand(sql7);
                if (result7 > 0)
                {
                    exe = "删除成功！";
                }
                return Content(exe);
            }
            catch (Exception ex)
            {
                string exe = "出现未知异常，请联系管理员解决。\t异常：" + ex.Message;
                return Content(exe);
            }

        }


        [HttpPost]//使用ajax删除
        public ActionResult Delete_GoodsType(int id)
        {
            GoodsType goodtype = db.GoodsType.Find(id);
            if (goodtype.Goods.Count() > 0)
            {
                string exe = "删除失败，该类别有商品引用，无法删除！";
                return Content(exe);
            }
            else if (goodtype.GoodsType1.Count() > 0)
            {
                string exe = "删除失败，该类为某些类别的父id，无法删除！";
                return Content(exe);
            }
            else
            {
                db.GoodsType.Remove(goodtype);
                db.SaveChanges();
                string exe = "删除成功！";
                return Content(exe);
            }
        }

        public ActionResult Select_Orders(int id)
        {
            db.Configuration.LazyLoadingEnabled = false;
            Orders orders = db.Orders.Find(id);
            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            string ret = JsonConvert.SerializeObject(orders, jsSettings);
            var result = Json(ret, JsonRequestBehavior.AllowGet);
            return result;
        }

        [HttpPost]
        public ActionResult Select_User(int id)
        {
            db.Configuration.LazyLoadingEnabled = false;
            UserInfo orders = db.UserInfo.Find(id);
            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            string ret = JsonConvert.SerializeObject(orders, jsSettings);
            var result = Json(ret, JsonRequestBehavior.AllowGet);
            return result;
        }


        public ActionResult Delete_User(int id)
        {
            UserInfo user = db.UserInfo.Find(id);
            user.IsDelete = 1;
            db.SaveChanges();
            string exe = "封禁用户成功！";
            return Content(exe);
        }


        public ActionResult Delete_User_No(int id)
        {
            UserInfo user = db.UserInfo.Find(id);
            user.IsDelete = 0;
            db.SaveChanges();
            string exe = "解封用户成功！";
            return Content(exe);
        }













        [HttpPost]//使用ajax新增
        public ActionResult Add_Lunbo(LunBo lunbo, HttpPostedFileBase Photos)
        {
            try
            {
                UserInfo user = Session["user"] as UserInfo;
                string exe = "";
                if (Photos != null)
                {
                    if (lunbo != null)
                    {
                        //添加初始化数据

                        lunbo.addtime = DateTime.Now;
                        db.LunBo.Add(lunbo);
                        if (db.SaveChanges() > 0)//影响的行数大于0
                        {
                            string fileName = "cyesw" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".jpg";//将时间转为数字，精确到毫秒,修改用户上传的文件名，防止重名覆盖
                            Photos.SaveAs(Server.MapPath("~/images/users/" + fileName));//将图片保存到文件夹中
                            lunbo.images = fileName;
                            db.SaveChanges();
                            exe = "新增成功！";
                        }
                        else
                        {
                            exe = "新增失败！";

                        }
                        return RedirectToAction("LunBo");
                    }
                    else
                    {
                        exe = "数据上传失败！";
                        return RedirectToAction("LunBo");
                    }
                }
                else
                {
                    exe = "图片上传失败！";
                    return RedirectToAction("LunBo");
                }
            }
            catch (Exception ex)
            {
                string exe = "出现未知异常，请联系管理员。异常：" + ex.Message;
                return RedirectToAction("LunBo");
            }
        }

        [HttpPost]//使用ajax新增站内推广
        public ActionResult Add_Webin(WebIn webin)
        {
            if (db.WebIn.Where(p => p.GoodsId == webin.GoodsId).Count() > 0)
            {
                TempData["exebg"] = "已经添加过了！请不要重复添加";
                return RedirectToAction("WebIn");
            }
            webin.addtime = DateTime.Now;
            db.WebIn.Add(webin);
            db.SaveChanges();
            return RedirectToAction("WebIn");
        }


        [HttpPost]//使用ajax新增
        public ActionResult Add_Webout(WebOut webout, HttpPostedFileBase Photos)
        {
            try
            {
                string exe = "";
                if (Photos != null)
                {
                    if (webout != null)
                    {
                        //添加初始化数据

                        webout.addtime = DateTime.Now;
                        db.WebOut.Add(webout);
                        if (db.SaveChanges() > 0)//影响的行数大于0
                        {
                            string fileName = "cyesw" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".jpg";//将时间转为数字，精确到毫秒,修改用户上传的文件名，防止重名覆盖
                            Photos.SaveAs(Server.MapPath("~/images/webout/" + fileName));//将图片保存到文件夹中
                            webout.images = fileName;
                            db.SaveChanges();
                            exe = "新增成功！";
                        }
                        else
                        {
                            exe = "新增失败！";

                        }
                        return RedirectToAction("WebOut");
                    }
                    else
                    {
                        exe = "数据上传失败！";
                        return RedirectToAction("WebOut");
                    }
                }
                else
                {
                    exe = "图片上传失败！";
                    return RedirectToAction("WebOut");
                }
            }
            catch (Exception ex)
            {
                string exe = "出现未知异常，请联系管理员。异常：" + ex.Message;
                return RedirectToAction("WebOut");
            }
        }

        [HttpPost]//使用ajax新增站内推广Add_User
        public ActionResult Add_GoodsType(GoodsType goodstype)
        {
            try
            {
                if (goodstype != null)
                {
                    db.GoodsType.Add(goodstype);
                    db.SaveChanges();
                    return RedirectToAction("GoodsType");
                }
                else
                {
                    return RedirectToAction("GoodsType");
                }

            }
            catch (Exception ex)
            {
                string exe = "出现未知异常，请联系管理员。异常：" + ex.Message;
                return RedirectToAction("GoodsType");
            }

        }


        [HttpPost]//使用ajax新增站内推广
        public ActionResult Add_User(UserInfo user, string IsManage)
        {
            try
            {
                user.IsManage = 0;
                if (IsManage == "true")
                {
                    user.IsManage = 1;
                }
                user.IsDelete = 0;
                user.Images = "man.jpg";
                user.Info = "这个人很懒，什么都没有写。";
                user.AddTime = DateTime.Now;
                user.EndTime = DateTime.Now;
                user.Sex = "男";
                user.moneys = 0;
                user.Age = 18;
                db.UserInfo.Add(user);
                db.SaveChanges();
                Addres addre = new Addres()//给用户一个默认收货地址，否则会报错
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
                return RedirectToAction("UserList");
            }
            catch (Exception ex)
            {
                string exe = "出现未知异常，请联系管理员。异常：" + ex.Message;
                return RedirectToAction("UserList");
            }

        }




        [HttpPost]
        public ActionResult Update_Goods(Goods goods)
        {

            Goods good_1 = db.Goods.Find(goods.GoodsId);
            good_1.Name = goods.Name;
            good_1.GoodsTypeId = goods.GoodsTypeId;
            good_1.IsNew = goods.IsNew;
            good_1.Info = goods.Info;
            good_1.Price = goods.Price;
            good_1.Price_Yuan = goods.Price_Yuan;
            good_1.IsState = goods.IsState;
            good_1.Ipone = goods.Ipone;
            db.SaveChanges();
            return RedirectToAction("GoodsInfo");
        }

        //ajax修改商品表信息

        [HttpPost]
        public ActionResult Select_Goods(int id)
        {
            db.Configuration.LazyLoadingEnabled = false;
            Goods addres = db.Goods.Find(id);
            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            string ret = JsonConvert.SerializeObject(addres, jsSettings);
            var result = Json(ret, JsonRequestBehavior.AllowGet);
            return result;

        }













        // 注销登录
        public ActionResult Rejist()
        {
            Session["user"] = null;
            return RedirectToAction("Login", "Home");
        }

        public ActionResult Tab()
        {
            var cz= db.ChongZhi.ToList();

            decimal? leiji = 0;
            decimal? jintian = 0;
            DateTime startSpan = DateTime.Parse(DateTime.Now.Date.ToString()).Date;//获取当前日期，判断是否为今日收入
            DateTime endSpan = new DateTime();
            foreach (var item in cz)
            {
                leiji += item.rmb;
                endSpan = DateTime.Parse(item.addtime.ToString()).Date;
                if (endSpan==startSpan)
                {
                    jintian += item.rmb;
                }
            }

            ViewBag.leiji = leiji;
            ViewBag.jintian = jintian;
            return View();
        }

        public ActionResult Tabinfo()
        {
            return View();
        }

        public ActionResult LunBo()
        {
            return View(db.LunBo.ToList());
        }

        public ActionResult WebIn()
        {
            return View(db.WebIn.ToList());
        }

        public ActionResult WebOut()
        {
            return View(db.WebOut.ToList());
        }

        /// <summary>
        /// 商品查询
        /// </summary>
        /// <param name="page">分页</param>
        /// <param name="paixu">排序</param>
        /// <param name="Isstate">状态</param>
        /// <param name="Name">模糊查询</param>
        /// <returns></returns>
        public ActionResult GoodsInfo(int? page, int? paixu, int? Isstate, string Name = "")
        {
            int pageNumber = page ?? 1;//页码
            paixu = paixu ?? 1;
            List<Goods> list = db.Goods.ToList();
            if (Isstate != null)
            {
                list = list.Where(p => p.IsState == Isstate).ToList();
            }

            if (Name != "")
            {
                list = list.Where(p => p.Name.Contains(Name)).ToList();
            }
            if (paixu == 2)
            {
                list = list.OrderByDescending(p => p.CreateTime).ToList();
            }
            else if (paixu == 3)
            {
                list = list.OrderByDescending(p => p.UpdateTime).ToList();
            }
            else if (paixu == 4)
            {
                list = list.OrderByDescending(p => p.munber).ToList();
            }
            ViewBag.paixu = paixu;
            ViewBag.Isstate = Isstate;
            ViewBag.Name = Name;
            return View(list.ToPagedList(pageNumber, 8));
        }

        /// <summary>
        /// 分类查询
        /// </summary>
        /// <param name="page">分页</param>
        /// <param name="leixin">按级别查询,1=所有，2=二级菜单，3=三级</param>
        /// <param name="Name">模糊查询</param>
        /// <returns></returns>
        public ActionResult GoodsType(int? page, int? leixin, int? fuid, string Name = "")
        {
            int pageNumber = page ?? 1;//页码
            List<GoodsType> list = db.GoodsType.Where(p => p.GoodsTypeBId != null).ToList();//八大类不允许更改
            if (Name != "")
            {
                list = list.Where(p => p.TypeName.Contains(Name)).ToList();
            }
            if (fuid != null)
            {
                list = list.Where(p => p.GoodsTypeBId == fuid).ToList();//父id查询
            }
            if (leixin == 2)
            {
                list = list.Where(p => p.GoodsType2.GoodsTypeBId == null).ToList();//查询二级
            }
            else if (leixin == 3)
            {
                list = list.Where(p => p.GoodsType2.GoodsTypeBId != null).ToList();//查询三级
            }

            ViewBag.leixin = leixin;
            ViewBag.fuid = fuid;
            ViewBag.name = Name;
            list = list.OrderBy(p => p.GoodsTypeBId).ToList();
            return View(list.ToPagedList(pageNumber, 8));
        }

        public ActionResult Orders(int? page, int? paixu, int? Isstate, string Name = "")
        {
            int pageNumber = page ?? 1;//页码
            paixu = paixu ?? 1;
            List<Orders> list = db.Orders.ToList();
            if (Isstate != null)
            {
                list = list.Where(p => p.IsState == Isstate).ToList();
            }

            if (Name != "")
            {
                list = list.Where(p => p.Goods.Name.Contains(Name)).ToList();
            }
            if (paixu == 2)
            {
                list = list.OrderByDescending(p => p.BuyTime1).ToList();
            }
            ViewBag.paixu = paixu;
            ViewBag.Isstate = Isstate;
            ViewBag.Name = Name;
            return View(list.ToPagedList(pageNumber, 8));
        }

        public ActionResult UserList(int? page, int? paixu, int? Isstate, string Name = "")
        {
            int pageNumber = page ?? 1;//页码
            paixu = paixu ?? 1;
            List<UserInfo> list = db.UserInfo.ToList();
            if (Isstate != null)
            {
                list = list.Where(p => p.IsDelete == Isstate).ToList();
            }

            if (Name != "")
            {
                list = list.Where(p => p.UserName.Contains(Name)).ToList();
            }
            if (paixu == 2)//注册时间
            {
                list = list.OrderByDescending(p => p.AddTime).ToList();
            }
            else if (paixu == 3)//最后登录
            {
                list = list.OrderByDescending(p => p.EndTime).ToList();
            }
            else if (paixu == 4)//发布宝贝个数
            {
                list = list.OrderByDescending(p => p.Goods.Count()).ToList();
            }
            ViewBag.paixu = paixu;
            ViewBag.Isstate = Isstate;
            ViewBag.Name = Name;
            return View(list.ToPagedList(pageNumber, 5));
        }

        public ActionResult UserFank(int? page, int? paixu, int? Isstate, string Name = "")
        {
            int pageNumber = page ?? 1;//页码
            paixu = paixu ?? 1;
            List<JuBao> list = db.JuBao.Where(p => p.JubaoType == 1).ToList();//1举报，2建议
            if (Isstate != null)
            {
                list = list.Where(p => p.States == Isstate).ToList();
            }

            if (Name != "")
            {
                list = list.Where(p => p.JuBaoName.Contains(Name)).ToList();
            }
            if (paixu == 2)//提交时间
            {
                list = list.OrderByDescending(p => p.addTiem).ToList();
            }
            else if (paixu == 3)//处理时间
            {
                list = list.OrderByDescending(p => p.endTiem).ToList();
            }
            list = list.OrderBy(p => p.States).ToList();//根据状态排序，为处理排前面
            ViewBag.paixu = paixu;
            ViewBag.Isstate = Isstate;
            ViewBag.Name = Name;
            return View(list.ToPagedList(pageNumber, 5));
        }

        public ActionResult UserFank_1(int? page, int? paixu, int? Isstate, string Name = "")//反馈小号
        {
            int pageNumber = page ?? 1;//页码
            paixu = paixu ?? 1;
            List<JuBao> list = db.JuBao.Where(p => p.JubaoType == 2).ToList();//1举报，2建议
            if (Isstate != null)
            {
                list = list.Where(p => p.States == Isstate).ToList();
            }

            if (Name != "")
            {
                list = list.Where(p => p.JuBaoName.Contains(Name)).ToList();
            }
            if (paixu == 2)//提交时间
            {
                list = list.OrderByDescending(p => p.addTiem).ToList();
            }
            else if (paixu == 3)//处理时间
            {
                list = list.OrderByDescending(p => p.endTiem).ToList();
            }
            list = list.OrderBy(p => p.States).ToList();//根据状态排序，为处理排前面
            ViewBag.paixu = paixu;
            ViewBag.Isstate = Isstate;
            ViewBag.Name = Name;
            return View(list.ToPagedList(pageNumber, 5));
        }

        public ActionResult UserCZ(int? page,DateTime? kaishi, DateTime? jieshu, string Name = "") 
        {
            ViewBag.kaishi = "";
            ViewBag.jieshu = "";
            int pageNumber = page ?? 1;//页码
            List<ChongZhi> list = db.ChongZhi.ToList();
            if (Name != "")
            {
                list = list.Where(p => p.type_1.Contains(Name)||p.UserInfo.UserName.Contains(Name)).ToList();
            }
            if (kaishi != null)
            {
                ViewBag.kaishi = kaishi;
                list = list.Where(p => p.addtime> kaishi).ToList();
            }
            if (jieshu != null)
            {
                ViewBag.jieshu = jieshu;
                list = list.Where(p => p.addtime < jieshu).ToList();
            }
            list = list.OrderByDescending(p => p.addtime).ToList();//根据状态排序，为处理排前面
            ViewBag.Name = Name;
            return View(list.ToPagedList(pageNumber, 6));

        }













        //ajax修改举报表信息
        [HttpPost]
        public ActionResult Delete_User_chuli(int id)
        {
            db.Configuration.LazyLoadingEnabled = false;
            JuBao addres = db.JuBao.Find(id);
            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            string ret = JsonConvert.SerializeObject(addres, jsSettings);
            var result = Json(ret, JsonRequestBehavior.AllowGet);
            return result;

        }

        //修改举报表信息
        [HttpPost]
        public ActionResult ChuLi_User(string yess, string HuiFu, int JuBaoId)
        {
            UserInfo user = Session["user"] as UserInfo;
            JuBao jubao = db.JuBao.Find(JuBaoId);
            jubao.HuiFu = HuiFu;
            jubao.CLUserId = user.UserId;
            jubao.States = 1;
            jubao.endTiem = DateTime.Now;
            if (yess == "true")
            {
                jubao.UserInfo1.IsDelete = 1;
            }
            db.SaveChanges();
            return RedirectToAction("UserFank");
        }


        //修改反馈表信息
        [HttpPost]
        public ActionResult ChuLi_User_1(int jianli, string HuiFu, int JuBaoId)
        {
            UserInfo user = Session["user"] as UserInfo;
            JuBao jubao = db.JuBao.Find(JuBaoId);
            jubao.HuiFu = HuiFu;
            jubao.CLUserId = user.UserId;
            jubao.States = 1;
            jubao.endTiem = DateTime.Now;
            jubao.UserInfo1.moneys += jianli;
            db.SaveChanges();
            return RedirectToAction("UserFank_1");
        }


        [HttpPost]
        public ActionResult Update_User(UserInfo user_1, HttpPostedFileBase images,string IsManage)
        {
            UserInfo user = db.UserInfo.Find(user_1.UserId);
            user.Info = user_1.Info;
            user.UserEmile = user_1.UserEmile;
            user.UserName = user_1.UserName;
            user.UserPwd = user_1.UserPwd;
            if (IsManage=="true")
            {
                user.IsManage = 1;
            }
            else
            {
                user.IsManage = 0;
            }
            
            if (images != null)
            {
                string fileName = "cyesw" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".jpg";//将时间转为数字，精确到毫秒,修改用户上传的文件名，防止重名覆盖
                images.SaveAs(Server.MapPath("~/images/img/" + fileName));
                user.Images = fileName;
            }
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;//修改
            db.SaveChanges();
            return RedirectToAction("UserList");
        }


        [HttpPost]
        public ActionResult Update_Lunbo(LunBo user_1, HttpPostedFileBase images)
        {
            LunBo user = db.LunBo.Find(user_1.LunBoId);
            user.type_1 = user_1.type_1;
            user.a_1 = user_1.a_1;
            if (images != null)
            {
                string fileName = "cyesw" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".jpg";//将时间转为数字，精确到毫秒,修改用户上传的文件名，防止重名覆盖
                images.SaveAs(Server.MapPath("~/images/users/" + fileName));
                user.images = fileName;
            }
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;//修改
            db.SaveChanges();
            return RedirectToAction("LunBo");
        }
        [HttpPost]
        public ActionResult Selset_Lunbo(int id)
        {
            db.Configuration.LazyLoadingEnabled = false;
            LunBo addres = db.LunBo.Find(id);
            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            string ret = JsonConvert.SerializeObject(addres, jsSettings);
            var result = Json(ret, JsonRequestBehavior.AllowGet);
            return result;
        }

        [HttpPost]
        public ActionResult Update_WebIn(WebIn user_1)
        {
            WebIn user = db.WebIn.Find(user_1.WebInId);
            user.GoodsId = user_1.GoodsId;
            user.type_1 = user_1.type_1;
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;//修改
            db.SaveChanges();
            return RedirectToAction("WebIn");
        }
        [HttpPost]
        public ActionResult Selset_WebIn(int id)
        {
            db.Configuration.LazyLoadingEnabled = false;
            WebIn addres = db.WebIn.Find(id);
            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            string ret = JsonConvert.SerializeObject(addres, jsSettings);
            var result = Json(ret, JsonRequestBehavior.AllowGet);
            return result;
        }

        [HttpPost]
        public ActionResult Update_WebOut(WebOut user_1, HttpPostedFileBase images)
        {
            WebOut user = db.WebOut.Find(user_1.WebOutId);
            user.Name = user_1.Name;
            user.Info = user_1.Info;
            user.a_1 = user_1.a_1;
            user.type_1 = user_1.type_1;
            if (images != null)
            {
                string fileName = "cyesw" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".jpg";//将时间转为数字，精确到毫秒,修改用户上传的文件名，防止重名覆盖
                images.SaveAs(Server.MapPath("~/images/webout/" + fileName));
                user.images = fileName;
            }
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;//修改
            db.SaveChanges();
            return RedirectToAction("WebOut");
        }
        [HttpPost]
        public ActionResult Selset_WebOut(int id)
        {
            db.Configuration.LazyLoadingEnabled = false;
            WebOut addres = db.WebOut.Find(id);
            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            string ret = JsonConvert.SerializeObject(addres, jsSettings);
            var result = Json(ret, JsonRequestBehavior.AllowGet);
            return result;
        }


        [HttpPost]
        public ActionResult Update_GoodsType(GoodsType user_1)
        {
            GoodsType user = db.GoodsType.Find(user_1.GoodsTypeId);
            user.TypeName = user_1.TypeName;
            user.GoodsTypeBId = user_1.GoodsTypeBId;
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;//修改
            db.SaveChanges();
            return RedirectToAction("GoodsType");
        }
        [HttpPost]
        public ActionResult Select_GoodsType(int id)
        {
            db.Configuration.LazyLoadingEnabled = false;
            GoodsType addres = db.GoodsType.Find(id);
            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            string ret = JsonConvert.SerializeObject(addres, jsSettings);
            var result = Json(ret, JsonRequestBehavior.AllowGet);
            return result;
        }
    }
}