using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CYESW.Models;
using PagedList;
using System.Web.Mvc.Ajax;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Text;
using System.IO;
using CYESW.Filter;

namespace CYESW.Controllers
{
    [Login]
    public class UserController : Controller
    {
        CYESWEntities db = new CYESWEntities();
        // GET: User,type表示1上架，2下架，3卖出，4删除
        public ActionResult UserIndex(int? page, int? type, int sextype = 1)
        {
            //本来只要写这些。。
            UserInfo user = Session["user"] as UserInfo;
            TempData["Title"] = "个人中心";
            if (Session["user"] == null)
            {
                //TempData["exe"] = "请先登录，谢谢！";
                //return RedirectToAction("Login", "Home");
                Session["user"] = db.UserInfo.Find(1);//便于调试，项目完成后可注释
            }



            int pageNumber = page ?? 1;//页码
            //sextype，访问6个类别
            ViewData["sextype"] = sextype;//判断加载那个布局页
            if (sextype == 1)//访问我的发布界面
            {
                var goods = GetGoodslist(type);//获取用户当前类别的集合
                                               //分页

                int pageSize = 3;//每页个数
                Session["page_ug"] = page;
                return View(goods.ToPagedList(pageNumber, pageSize));
            }
            else if (sextype == 2)//访问我的收藏界面
            {
                TempData["Title"] = "我的收藏";
                List<Love> lovelist = db.Love.Where(p => p.UserId == user.UserId).ToList();
                return View(lovelist.ToPagedList(pageNumber, 10));
            }
            else if (sextype == 3)//访问我的订单界面
            {
                TempData["Title"] = "订单界面";
                Session["user_gh"] = type ?? 1;
                List<Orders> lovelist = db.Orders.Where(p => p.UserId2 == user.UserId).ToList();
                if (type == 2)//已完成
                {
                    lovelist = lovelist.Where(p => p.IsState == 4).ToList();
                }
                else//交易中
                {
                    lovelist = lovelist.Where(p => p.IsState != 4).ToList();
                }
                return View(lovelist.ToPagedList(pageNumber, 3));
            }
            else if (sextype == 4)//访问我的我的资料
            {
                TempData["Title"] = "我的资料";
                Session["user_uu"] = type ?? 1;
                return View(user);
            }
            else if (sextype == 5)//访问我的我的认证
            {
                TempData["Title"] = "我的认证";
                RealName rn = db.RealName.Where(p => p.UserId == user.UserId).FirstOrDefault();
                return View(rn);
            }
            else if (sextype == 6)//访问我的我的反馈
            {
                TempData["Title"] = "我的反馈";
                List<JuBao> jubao = db.JuBao.Where(p => p.UserId == user.UserId).ToList();
                return View(jubao.ToPagedList(pageNumber, 10));
            }
            else if (sextype == 7)//访问我的我的地址
            {
                TempData["Title"] = "收货地址";
                List<Addres> addres = db.Addres.Where(p => p.UserId == user.UserId).ToList();
                return View(addres.ToPagedList(pageNumber, 10));
            }
            else
            {
                TempData["Title"] = "充值记录";
                List<ChongZhi> chongzhi = db.ChongZhi.Where(p => p.UserId == user.UserId).ToList();
                chongzhi = chongzhi.OrderByDescending(p => p.addtime).ToList();
                return View(chongzhi.ToPagedList(pageNumber, 6));
            }

        }

        /// <summary>
        /// 传入类别（上架，下架，订单，卖出），返回集合
        /// </summary>
        /// <param name="type">宝贝类别</param>
        /// <returns></returns>
        private List<Goods> GetGoodslist(int? type)
        {

            UserInfo user = Session["user"] as UserInfo;
            //Ⅰ第一次网页打开需要数据，发布商品，否则报错（下面都是）
            var goods = db.Goods.Where(p => p.UserId == user.UserId).ToList();//查询该用户所有商品，下面细分
            type = type ?? 1;
            Session["user_gt"] = type;//显示当前页（共四页）
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
            return goods;
        }

        [HttpPost]
        public ActionResult UserIndex_update(UserInfo user_1 , HttpPostedFileBase images_user)
        {
            try
            {
                UserInfo user = db.UserInfo.Find(user_1.UserId);
                user.Info = user_1.Info;
                user.UserName = user_1.UserName;
                user.Sex = user_1.Sex;
                if (images_user!=null)
                {
                    string fileName = Path.GetFileName(images_user.FileName);
                    images_user.SaveAs(Server.MapPath("~/images/img/" + fileName));
                    user.Images = fileName;
                }
                


                db.Entry(user).State = System.Data.Entity.EntityState.Modified;//修改
                db.SaveChanges();
                TempData["exe"] = "修改成功！";
                Session["user"] = user;
                return RedirectToAction("UserIndex", new { sextype = 4 });
            }
            catch (Exception ex)
            {
                TempData["exe"] = "出现未知异常，请联系管理员。异常：" + ex.Message;
                return RedirectToAction("UserIndex", new { sextype = 4 });
            }

        }

        [HttpPost]
        public ActionResult UserIndex_update_pwd(int UserId1, string Newpwd, string yanzhen)
        {
            try
            {
                if (yanzhen == YanZhenStr)//通过验证码验证
                {
                    UserInfo user = db.UserInfo.Find(UserId1);
                    user.UserPwd = Newpwd;
                    db.Entry(user).State = System.Data.Entity.EntityState.Modified;//修改
                    db.SaveChanges();
                    TempData["exe"] = "修改成功！";
                    return RedirectToAction("UserIndex", new { sextype = 4 });
                }
                else
                {
                    TempData["exe"] = "验证码错误！！";
                    return RedirectToAction("UserIndex", new { sextype = 4 });
                }
            }
            catch (Exception ex)
            {
                TempData["exe"] = "出现未知异常，请联系管理员。异常：" + ex.Message;
                return RedirectToAction("UserIndex", new { sextype = 4 });
            }

        }

        [HttpPost]
        public ActionResult Certification_yz(string shenfenzhen, int UserId)//网站实名认证
        {
            try
            {
                if (db.RealName.Where(p=>p.IdCard== shenfenzhen).Count()>0)
                {
                    TempData["exe"] = "该身份证已经实名，每个身份证仅能实名一个账号！";
                    return RedirectToAction("UserIndex", new { sextype = 5 });
                }
                if (idCard.checkidcard(shenfenzhen))
                {
                    RealName rn = new RealName()
                    {
                        AddTime = DateTime.Now,
                        IsDelete = 0,
                        UserId = UserId,
                        IdCard = shenfenzhen
                    };
                    db.RealName.Add(rn);
                    db.SaveChanges();

                    TempData["exe"] = "恭喜！实名认证通过了！";
                    return RedirectToAction("UserIndex", new { sextype = 5 });
                }
                else
                {
                    TempData["exe"] = "抱歉！实名认证未通过！请仔细检查是否输入正确。";
                    return RedirectToAction("UserIndex", new { sextype = 5 });
                }
            }
            catch (Exception ex)
            {
                TempData["exe"] = "出现未知异常，请联系管理员。异常：" + ex.Message;
                return RedirectToAction("UserIndex", new { sextype = 5 });
            }

        }


        public ActionResult UserExit()
        {
            Session["user"] = null;
            return RedirectToAction("Login", "Home");
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
        public ActionResult Publish()
        {
            return View();
        }

        public ActionResult Love()
        {
            //db.Configuration.LazyLoadingEnabled = false;//关闭延迟加载
            ////使用贪婪加载
            //var goodlist = db.Love.Include("Goods").ToList();
            ////var goodlistimg = db.Goods.Include("Goodsimage").Where(p => p.Goodsaddress.TypeName == "湖南").ToList();
            ////借用newtonsoft。json包进行序列化,防止导航属性循环
            //JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            //jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //string ret = JsonConvert.SerializeObject(goodlist, jsSettings);
            ////string retimg = JsonConvert.SerializeObject(goodlistimg, jsSettings);
            ////转换为json格式输出
            //var result = Json(ret, JsonRequestBehavior.AllowGet);
            //return result;
            return View();
        }

        public ActionResult Theorder()
        {
            return View();
        }

        public ActionResult Setupthe()
        {
            return View();
        }

        public ActionResult Certification()
        {
            return View();
        }
        public ActionResult Feedback()
        {
            return View();
        }

        public ActionResult Address()
        {
            return View();
        }

        public ActionResult Precord()
        {
            TempData["Title"] ="充值记录";
            return View();
        }

        public ActionResult Delete(int id, int? type_e)//用ef删除报错，（更新条目出错），用sql删除吧
        {
            ViewData["sextype"] = 1;//判断加载那个布局页
            try
            {
                //sql语句
                string sql1 = string.Format("delete Goodsimage where  GoodsId={0}", id);//删除商品图片表
                string sql2 = string.Format("update Love set GoodsId=1 where GoodsId={0}", id);//删除商品图片表
                string sql3 = string.Format("delete HuDong_texts where  GoodsId={0}", id);//删除商品互动表
                string sql4 = string.Format("delete Goods where GoodsId={0}", id);//删除商品
                //执行sql语句，返回一个数字（影响的行数）
                int result1 = db.Database.ExecuteSqlCommand(sql1);
                int result2 = db.Database.ExecuteSqlCommand(sql2);
                int result3 = db.Database.ExecuteSqlCommand(sql3);
                int result4 = db.Database.ExecuteSqlCommand(sql4);
                if (result4 > 0)
                {
                    TempData["exe"] = "删除成功！";
                }
                else
                {
                    TempData["exe"] = "删除失败！";
                }
                
                var goods = GetGoodslist(type_e);
                return View("UserIndex", goods.ToPagedList(1, 3));
            }
            catch (Exception ex)
            {
                TempData["exe"] = "出现未知异常，请联系管理员解决。\t异常：" + ex.Message;
                var goods = GetGoodslist(type_e);
                return View("UserIndex", goods.ToPagedList(1, 3));
            }
        }

        //擦亮商品
        public ActionResult Updata_c(int id, int? type_e)
        {
            try
            {
                ViewData["sextype"] = 1;//判断加载那个布局页
                TempData["exe"] = "擦亮成功！";
                db.Goods.Find(id).UpdateTime = DateTime.Now;
                db.SaveChanges();
                var goods = GetGoodslist(type_e);
                return View("UserIndex", goods.ToPagedList(1, 3));
            }
            catch (Exception ex)
            {
                TempData["exe"] = "出现未知异常，请联系管理员解决。异常：" + ex.Message;
                var goods = GetGoodslist(type_e);
                return View("UserIndex", goods.ToPagedList(1, 3));
            }
        }
        
        /// <summary>
        /// 下架商品
        /// </summary>
        /// <param name="state">商品需要修改的状态</param>
        /// <param name="id">修改id</param>
        /// <param name="type_e">当前显示的小页面（下架，上架哎，交易中，，，）</param>
        /// <returns></returns>
        public ActionResult Updata_x(int state,int id, int? type_e)
        {
            try
            {
                ViewData["sextype"] = 1;//判断加载那个布局页
                db.Goods.Find(id).IsState = state;
                db.SaveChanges();
                if (state==1) TempData["exe"] = "已上架！一起期待宝贝找到新主人吧！";
                else TempData["exe"] = "已下架！修改宝贝信息在重新上架吧！";
                var goods = GetGoodslist(type_e);
                return View("UserIndex", goods.ToPagedList(1, 3));
            }
            catch (Exception ex)
            {
                TempData["exe"] = "出现未知异常，请联系管理员解决。异常：" + ex.Message;
                var goods = GetGoodslist(type_e);
                return View("UserIndex", goods.ToPagedList(1, 3)); 
            }

        }

        //访问他人用户中心
        public ActionResult Index_two(int UserId) {
            
            if (Session["user"] == null)
            {
                //TempData["exe"] = "请先登录，谢谢！";
                //return RedirectToAction("Login", "Home");
                Session["user"] = db.UserInfo.Find(1);//便于调试，项目完成后可注释
            }
            UserInfo user = Session["user"] as UserInfo;
            TempData["Title"] = "访问个人中心";
            if (user.UserId!=UserId)
            {
                UserInfo us = db.UserInfo.Find(UserId);
                return View(us);
            }
            else
            {
                return RedirectToAction("UserIndex", new { sextype = 1 });
            }
        }


        //新增收货地址
        public ActionResult AddAddres(Addres addres,int Addresss1_se, string Addresss1_shi)
        {
            string addressname = db.Goodsaddress.Find(Addresss1_se).TypeName;
            UserInfo user = Session["user"] as UserInfo;
            Goodsaddress goodsaddress1 = db.Goodsaddress.Find(Addresss1_se);
            Goodsaddress goodsaddress2 = db.Goodsaddress.Where(p => p.TypeName == Addresss1_shi).FirstOrDefault();
            addres.GoodsaddressId1 = goodsaddress1.GoodsaddressId;
            addres.GoodsaddressId2 = goodsaddress2.GoodsaddressId;
            addres.Addresss1 = addressname + "省-" + Addresss1_shi+"市";
            addres.IsDelete = 0;
            db.Addres.Add(addres);
            Session["user"] = db.UserInfo.Find(user.UserId);
            db.SaveChanges();
            TempData["exe"] = "新增收货地址成功！";
            return RedirectToAction("UserIndex", new { sextype = 7 });
        }



        //将收货地址设置为默认
        public ActionResult Defult_Addres(int AddresId)
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
                return RedirectToAction("UserIndex", new { sextype = 7 });
            }
            catch (Exception ex)
            {
                TempData["exe"] = "出现未知异常，请联系管理员解决。异常：" + ex.Message;
                return RedirectToAction("UserIndex", new { sextype = 7 });
            }

        }
        //将收货地址删除
        public ActionResult Delete_Addres(int AddresId)
        {
            try
            {
                UserInfo user = Session["user"] as UserInfo;
                Addres addres = db.Addres.Find(AddresId);
                List<Addres> list = db.Addres.Where(p => p.UserId == addres.UserId).ToList();
                if (list.Count()==1)//判断该地址是否为用户最后一个地址，如果是，就无法删除。
                {
                    TempData["exe"] = "删除失败！每个用户至少需要一个收货地址！";
                    return RedirectToAction("UserIndex", new { sextype = 7 });
                }
                if (addres.IsDelete==1)//判断该地址是否为默认地址，如果是默认地址，就将默认地址重新赋值给剩下的地址
                {
                    db.Addres.Remove(addres);
                    db.SaveChanges();
                    db.Addres.Where(p => p.UserId == user.UserId).FirstOrDefault().IsDelete = 1;
                    db.SaveChanges();
                }
                else
                {
                    db.Addres.Remove(addres);
                    db.SaveChanges();
                }
                Session["user"] = db.UserInfo.Find(user.UserId);//更新用户数据
                TempData["exe"] = "删除成功！";
                return RedirectToAction("UserIndex", new { sextype = 7 });
            }
            catch (Exception ex)
            {
                TempData["exe"] = "出现未知异常，请联系管理员解决。异常：" + ex.Message;
                return RedirectToAction("UserIndex", new { sextype = 7 });
            }

        }
        //修改地址，ajax获取修改值
        [HttpPost]
        public ActionResult Update_Addres_ss(int AddresId)
        {
            try
            {
                db.Configuration.LazyLoadingEnabled = false;//关闭延迟加载
                                                            ////使用贪婪加载
                Addres addres = db.Addres.Find(AddresId);
                //var goodlistimg = db.Goods.Include("Goodsimage").Where(p => p.Goodsaddress.TypeName == "湖南").ToList();
                //借用newtonsoft。json包进行序列化,防止导航属性循环
                JsonSerializerSettings jsSettings = new JsonSerializerSettings();
                jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                string ret = JsonConvert.SerializeObject(addres, jsSettings);
                //string retimg = JsonConvert.SerializeObject(goodlistimg, jsSettings);
                //转换为json格式输出
                var result = Json(ret, JsonRequestBehavior.AllowGet);
                return result;
            }
            catch (Exception ex)
            {
                TempData["exe"] = "出现未知异常，请联系管理员解决。异常：" + ex.Message;
                return RedirectToAction("UserIndex", new { sextype = 7 });
            }

        }

        //修改，ajax保存
        [HttpPost]
        public ActionResult Update_Addres(Addres adderes, int Addresss1_se, string Addresss1_shi) {
            Addres adds = db.Addres.Find(adderes.AddresId);
            Goodsaddress goodsaddress1 = db.Goodsaddress.Find(Addresss1_se);
            Goodsaddress goodsaddress2 = db.Goodsaddress.Where(p => p.TypeName == Addresss1_shi).FirstOrDefault();
            adds.GoodsaddressId1 = goodsaddress1.GoodsaddressId;
            adds.GoodsaddressId2 = goodsaddress2.GoodsaddressId;
            adds.Addresss1 = goodsaddress1.TypeName + "省-" + Addresss1_shi + "市";
            adds.Addresss2 = adderes.Addresss2;
            adds.Name = adderes.Name;
            adds.Phone = adderes.Phone;
            db.SaveChanges();
            TempData["exe"] = "修改收货地址成功！";
            return RedirectToAction("UserIndex", new { sextype = 7 });
        }

        //省市查询
        [HttpPost]
        public ActionResult SS_Addres(int ids)//ids:省编号
        {
            if (ids!=1)//返回市
            {
                db.Configuration.LazyLoadingEnabled = false;//关闭延迟加载
                                                            ////使用贪婪加载
                List<Goodsaddress> goodlist = db.Goodsaddress.Where(p => p.GoodsaddressBId == ids ).ToList();
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
            else//返回省
            {
                db.Configuration.LazyLoadingEnabled = false;//关闭延迟加载
                                                            ////使用贪婪加载
                List<Goodsaddress> goodlist = db.Goodsaddress.Where(p => p.Goodsaddress2.GoodsaddressBId == null&& p.GoodsaddressBId != null).ToList();
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
        }


        //将收货地址删除
        public ActionResult User_JuBao(int JuBaoId)
        {
            try
            {
                JuBao jubao = db.JuBao.Find(JuBaoId);
                TempData["title"] = "反馈详情"; 
                return View(jubao);
            }
            catch (Exception ex)
            {
                TempData["exe"] = "出现未知异常，请联系管理员解决。异常：" + ex.Message;
                TempData["title"] = "反馈详情！";
                return View();
            }

        }


        [HttpPost]
        //购买商品推广
        public ActionResult Tuiguan(int GoodsId,int Tpromote) {
            try
            {
                UserInfo user = Session["user"] as UserInfo;
                if (user.moneys < Tpromote)
                {
                    TempData["exe"] = "你确定你的余额足够这此推广吗？？小伙子";
                    return RedirectToAction("UserIndex", new { sextype = 1 });
                }
                WebIn webin = new WebIn();
                if (db.WebIn.Where(p=>p.GoodsId== GoodsId).Count()>0)//判断是否已经推广过了
                {
                    webin = db.WebIn.Where(p => p.GoodsId == GoodsId).FirstOrDefault();
                    webin.Tpromote += Tpromote;
                    db.SaveChanges();
                    TempData["exe"] = "追加推广成功！--当前推广金额："+ webin.Tpromote;
                    return RedirectToAction("UserIndex", new { sextype = 1 });
                }
                user.moneys = user.moneys - Tpromote;//开始生成推广表
                webin = new WebIn() { GoodsId = GoodsId, Tpromote = Tpromote, addtime = DateTime.Now, Theat = 0, type_1 = "默认" };
                db.WebIn.Add(webin);
                db.SaveChanges();
                TempData["exe"] = "推广成功！";
                return RedirectToAction("UserIndex", new { sextype = 1 });
            }
            catch (Exception ex)
            {
                TempData["exe"] = "出现未知异常，请联系管理员解决。异常：" + ex.Message;
                TempData["title"] = "反馈详情！";
                return View();
            }
        }


        public ActionResult ChongZhi()
        {
            TempData["title"] = "金币充值！";
            return View();
        }
        [HttpPost]
        public ActionResult ChongZhi(decimal money, decimal rmb) {

            try
            {
                UserInfo user = Session["user"] as UserInfo;
                user = db.UserInfo.Find(user.UserId);
                ChongZhi chongzhi = new ChongZhi() { addtime = DateTime.Now, UserId = user.UserId, moneys = money, rmb = rmb, type_1 = "自助充值" };
                db.ChongZhi.Add(chongzhi);
                user.moneys += money;
                db.SaveChanges();
                TempData["exe"] = money + "金币充值成功！";
                TempData["title"] = "金币充值！";
                Session["user"] = db.UserInfo.Find(user.UserId);
                return View();
            }
            catch (Exception ex)
            {
                TempData["exe"] = "出现未知异常，请联系管理员解决。异常：" + ex.Message;
                TempData["title"] = "反馈详情！";
                return View();
            }

            
        }





        private static string YanZhenStr = null;
        /// <summary>
        /// 获取随机验证码
        /// </summary>
        /// <returns></returns>
        public static String getAppCode()
        {
            Random ran = new Random((int)DateTime.Now.ToFileTimeUtc());
            double number = ran.NextDouble();
            string jqstr = number.ToString().Substring(2, 6);
            return jqstr;
        }

        /// <summary>
        /// 发送验证邮件
        /// </summary>
        /// <param name="UserEmile"></param>
        [HttpPost]
        public void GetYZ(string UserEmile)
        {
            YanZhenStr = getAppCode();
            Response.Write(string.Format("邮箱：{0},验证码:{1}", UserEmile, YanZhenStr));
            string sendEmail = UserEmile;
            string sendHands = "朝阳网-验证码";
            string sendBodys = "【朝阳网】您的验证码为：" + YanZhenStr + "\n感谢您的使用，请不要将验证码透露给他人\n如果不是您本人操作，请忽略本信息。\n谢谢！\n有任何疑问请联系客服QQ：3303898033@qq.com(请在法定工作日联系)";
            if (SendEmail(sendEmail, sendHands, sendBodys))
            {
                Response.Write("发送成功！请不要刷新本页面，以免使验证码失效");
            }
            else
            {
                Response.Write("发送失败！您可以尝试联系我们解决问题");
            }
        }


        #region

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailTo">要发送的邮箱</param>
        /// <param name="mailSubject">邮箱主题</param>
        /// <param name="mailContent">邮箱内容</param>
        /// <returns>返回发送邮箱的结果</returns>
        public static bool SendEmail(string mailTo, string mailSubject, string mailContent)
        {
            // 设置发送方的邮件信息,例如使用腾讯的smtp
            string smtpServer = "smtp.163.com"; //SMTP服务器

            //下面两处要改,其他的别动
            string mailFrom = "lyk520dtf@163.com"; //发邮箱的账号
            string userPassword = "3089218762lyk";//登陆密码,如果使用的是腾讯的 用的是授权码

            //

            // 邮件服务设置
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式
            smtpClient.Host = smtpServer; //指定SMTP服务器



            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential(mailFrom, userPassword);//用户名和密码

            // 发送邮件设置      
            MailMessage mailMessage = new MailMessage(mailFrom, mailTo); // 发送人和收件人
            mailMessage.Subject = mailSubject;//主题
            mailMessage.Body = mailContent;//内容
            mailMessage.BodyEncoding = Encoding.UTF8;//正文编码
            mailMessage.IsBodyHtml = true;//设置为HTML格式
            mailMessage.Priority = MailPriority.Low;//优先级
            try
            {
                smtpClient.Send(mailMessage); // 发送邮件
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

    }
}