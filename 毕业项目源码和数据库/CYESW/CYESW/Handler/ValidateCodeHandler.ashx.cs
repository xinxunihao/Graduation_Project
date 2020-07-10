using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace CYESW.Handler
{
    /// <summary>
    /// ValidateCodeHandler 的摘要说明
    /// </summary>
    public class ValidateCodeHandler : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 主体方法用来生成验证码
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            //定义多种绘制颜色
            Color[] color = {
                Color.DodgerBlue,
                Color.Yellow,
                Color.Beige,
                Color.Red,
                Color.BurlyWood,
                Color.Indigo,
                Color.White
            };
            //设置画布，长100px 宽36像素
            Image image = new Bitmap(100, 36);
            //将画布变成可绘制
            Graphics graphics = Graphics.FromImage(image);
            //设置画布的背景颜色
            graphics.Clear(Color.FromArgb(98, 145, 232));
            //准备随机数
            Random random = new Random(DateTime.Now.Millisecond);
            //生成四个字符 97-122小写字母 65-90大写字母 48-57是数字
            int charNum1 = random.Next(97, 122);
            int charNum2 = random.Next(65, 90);
            int charNum3 = random.Next(97, 122);
            int charNum4 = random.Next(48, 57);

            //将四个随机字符拼成一串验证码
            string vilidateCode = string.Format("{0}{1}{2}{3}", (char)charNum1, (char)charNum2, (char)charNum3, (char)charNum4);
            //将验证码存入到session中 用于登录判断
            context.Session["vilidateCode"] = vilidateCode.ToLower();

            //写入画布的字体
            Font font = new Font("幼圆", 24);
            //定义画笔,设置画笔颜色
            Brush brush1 = new SolidBrush(color[random.Next(0, color.Length - 1)]);
            graphics.DrawString(((char)charNum1).ToString(), font, brush1, 5, -4);

            Brush brush2 = new SolidBrush(color[random.Next(0, color.Length - 1)]);
            graphics.DrawString(((char)charNum2).ToString(), font, brush2, 30, 4);

            Brush brush3 = new SolidBrush(color[random.Next(0, color.Length - 1)]);
            graphics.DrawString(((char)charNum3).ToString(), font, brush3, 50, 0);

            Brush brush4 = new SolidBrush(color[random.Next(0, color.Length - 1)]);
            graphics.DrawString(((char)charNum4).ToString(), font, brush4, 70, 6);

            //解析为图片类型
            context.Response.ContentType = "image/jpeg";
            //保存图片，图片流的格式输出出去
            image.Save(context.Response.OutputStream, ImageFormat.Jpeg);
            //释放图片资源
            image.Dispose();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}