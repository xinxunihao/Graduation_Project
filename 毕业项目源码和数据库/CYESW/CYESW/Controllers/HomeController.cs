using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
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
                        ViewBag.UserEmile = UserEmile;
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
                    ViewBag.UserEmile = UserEmile;
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
        public ActionResult Register(string UserEmile1, string UserPwd1, string yanzhen)
        {
            try
            {
                if (yanzhen != Session["YanZhenStr"].ToString())
                {
                    TempData["Title"] = "登录/注册";//标题
                    TempData["exe"] = "邮箱验证码错误！（＞人＜；）";
                    //将用户输入的值显示到界面上
                    ViewBag.UserEmile1 = UserEmile1;
                    return RedirectToAction("Login");
                }
                
                var aemile = db.UserInfo.Where(p => p.UserEmile == UserEmile1).SingleOrDefault();
                if (aemile != null)
                {
                    TempData["exe"] = "该邮箱已被注册";
                    return RedirectToAction("Login");
                }
                //随机生成用户编号
                string username = "用户" + getAppCode();
                UserInfo user = new UserInfo() { UserEmile = UserEmile1, UserName = username, UserPwd = UserPwd1,Images= "man.jpg",IsDelete=0,AddTime=DateTime.Now,IsManage=0,moneys=0,Info="这个人很懒，什么都没有写。",Age=0 , EndTime = DateTime.Now , Sex="男"};//添加默认数据，也可以写个触发器
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



        public JsonResult validateCode(string validateCode)
        {
            if (validateCode.ToLower() != Session["vilidateCode"].ToString().ToLower())
            {
                var result = new
                {
                    res = "验证码错误！！",
                    datetime = string.Format("{0:yyyy-MM-dd hh:mm:ss}",DateTime.Now.ToString())
                };
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var result = new
                {
                    res = "ok",
                    datetime = string.Format("{0:yyyy-MM-dd hh:mm:ss}", DateTime.Now.ToString())
                };
                return Json(result, JsonRequestBehavior.AllowGet);
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
        public JsonResult GetYZ(string UserEmile)
        {
            
            if (db.UserInfo.Where(p=>p.UserEmile==UserEmile).Count()>0)
            {
                var result = new { res = "该邮箱已被注册！",yc="看来你已经注册过呢，不过我懒，不想搞找回密码功能┑(￣Д ￣)┍", datetime = string.Format("{0:yyyy-MM--dd hh:mm:ss}", DateTime.Now) };
                return Json(result,JsonRequestBehavior.AllowGet);
            }
            string datetime = string.Format("{0:yyyy年MM月dd日 hh:mm:ss}", DateTime.Now);
            YanZhenStr = getAppCode();
            Session["YanZhenStr"] = YanZhenStr;//用于注册界面使用

            //Response.Write(string.Format("邮箱：{0},验证码:{1}", UserEmile, YanZhenStr));--调试时使用
            string sendEmail = UserEmile;
            string sendHands = "朝阳网-验证码";
            string sendBodys = "【朝阳网】您的验证码为：" + YanZhenStr + "\n感谢您的注册，请不要将验证码透露给他人\n如果不是您本人操作，请忽略本信息。\n谢谢！\n有任何疑问请联系客服QQ：3303898033@qq.com(请在法定工作日联系)\n发送时间--北京时间：" + datetime;
            if (SendEmail(sendEmail, sendHands, sendBodys))
            {
                var result = new { res = "发送成功！d=====(￣▽￣*)b (请前往邮件查看验证码)\n【ps：由于发送邮件会被阿里云检测为恶意攻击其他服务器（发送垃圾邮件）受到处罚，所以暂时暂停邮件发送功能:验证码:" + YanZhenStr + "（请记好，点确定我会消失的哈）】", yc = string.Format("知道你懒得去邮箱看了，验证码为：{0}", YanZhenStr), datetime = string.Format("{0:yyyy-MM--dd hh:mm:ss}", DateTime.Now) };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = new { res = "发送失败！您可以尝试联系我们解决问题U•ェ•*U \nqq：3089218762", yc = "又失败了，不能吧，还有什么是我没想到的ε(┬┬﹏┬┬)3", datetime = string.Format("{0:yyyy-MM--dd hh:mm:ss}", DateTime.Now) };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }



        /// <summary>
        /// 通过System.Web.Mail.MailMessage去发送，可以不被阿里云限制25端口的使用
        /// 暂时一般都用465端口
        /// </summary>
        /// <param name="smtpserver">SMTP服务,譬如：smtp.126.com</param>
        /// <param name="userName">发件箱</param>
        /// <param name="pwd">密码</param>
        /// <param name="nickName">昵称</param>
        /// <param name="strfrom">发件箱</param>
        /// <param name="strto">收件箱</param>
        /// <param name="MessageSubject">主题</param>
        /// <param name="MessageBody">内容</param>
        /// <param name="SUpFile">附件</param>
        /// <param name="port">端口</param>
        /// <param name="enablessl">SSL加密</param>
        /// <returns></returns>
        [Obsolete]
        public static bool SendEmail(string strto,string MessageSubject, string MessageBody, string smtpserver="smtp.163.com", string userName= "lyk520dtf@163.com", string pwd= "3089218762lyk", string nickName="朝阳二手网", string strfrom= "lyk520dtf@163.com", int port=465, int enablessl = 0)
        {
            System.Web.Mail.MailMessage mmsg = new System.Web.Mail.MailMessage();
            //邮件主题
            mmsg.Subject = MessageSubject;
            mmsg.BodyFormat = System.Web.Mail.MailFormat.Html;
            //邮件正文
            mmsg.Body = MessageBody;
            //正文编码
            mmsg.BodyEncoding = Encoding.UTF8;
            //优先级
            mmsg.Priority = System.Web.Mail.MailPriority.High;

            System.Web.Mail.MailAttachment data = null;
            
            //发件者邮箱地址
            mmsg.From = string.Format("\"{0}\"<{1}>", nickName, strfrom);

            //收件人收箱地址
            mmsg.To = strto;
            mmsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
            //用户名
            mmsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", userName);
            //密码 不是邮箱登陆密码 而是邮箱设置POP3/SMTP 时生成的第三方客户端授权码
            mmsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", pwd);
            //端口
            mmsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", port);
            //使用SSL
            mmsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", "true");//SSL加密  
            //Smtp服务器
            System.Web.Mail.SmtpMail.SmtpServer = smtpserver;
            try
            {
                //System.Web.Mail.SmtpMail.Send(mmsg);
                //由于发送邮件会被阿里云处罚，所以暂时暂停邮件发送功能
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }




        //发送邮件代码，在服务器段被禁用
        //#region

        ///// <summary>
        ///// 发送邮件
        ///// </summary>
        ///// <param name="mailTo">要发送的邮箱</param>
        ///// <param name="mailSubject">邮箱主题</param>
        ///// <param name="mailContent">邮箱内容</param>
        ///// <returns>返回发送邮箱的结果</returns>
        //public static bool SendEmail(string mailTo, string mailSubject, string mailContent)
        //{
        //    // 设置发送方的邮件信息,例如使用腾讯的smtp
        //    string smtpServer = "smtp.163.com"; //SMTP服务器

        //    //下面两处要改,其他的别动
        //    string mailFrom = "lyk520dtf@163.com"; //发邮箱的账号
        //    string userPassword = "3089218762lyk";//登陆密码,如果使用的是腾讯的 用的是授权码

        //    //

        //    // 邮件服务设置
        //    SmtpClient smtpClient = new SmtpClient();
        //    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式
        //    smtpClient.Host = smtpServer; //指定SMTP服务器
        //    smtpClient.Port = 465;//用来发送邮件的端口，默认的25端口会被云服务器屏蔽，需要申请解封（587是qq可用）


        //    smtpClient.EnableSsl = true;
        //    smtpClient.UseDefaultCredentials = false;
        //    smtpClient.Credentials = new System.Net.NetworkCredential(mailFrom, userPassword);//用户名和密码

        //    // 发送邮件设置      
        //    MailMessage mailMessage = new MailMessage(mailFrom, mailTo); // 发送人和收件人
        //    mailMessage.Subject = mailSubject;//主题
        //    mailMessage.Body = mailContent;//内容
        //    mailMessage.BodyEncoding = Encoding.UTF8;//正文编码

        //    mailMessage.IsBodyHtml = true;//设置为HTML格式
        //    mailMessage.Priority = MailPriority.Low;//优先级
        //    try
        //    {
        //        smtpClient.Send(mailMessage); // 发送邮件
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //#endregion
    }
}