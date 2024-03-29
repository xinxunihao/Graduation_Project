﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CYESW.Controllers
{
    /// <summary> 
    /// 验证身份证号码类 
    /// </summary> 
    public class idCard
    {
        /// <summary> 
        /// 验证身份证合理性 --现在15位的身份证基本失效·了，就不写了，代码留着，留个纪念吧
        /// </summary> 
        /// <param name="id"></param> 
        /// <returns></returns> 
        public static bool checkidcard(string idnumber)
        {
            if (idnumber.Length == 18)
            {
                bool check = checkidcard18(idnumber);
                return check;
            }
            else if (idnumber.Length == 15)
            {
                bool check = checkidcard15(idnumber);
                return check;
            }
            else
            {
                return false;
            }
        }


        /// <summary> 
        /// 18位身份证号码验证 
        /// </summary> 
        private static bool checkidcard18(string idnumber)
        {
            long n = 0;
            if (long.TryParse(idnumber.Remove(17), out n) == false
                || n < Math.Pow(10, 16) || long.TryParse(idnumber.Replace('x', '0').Replace('x', '0'), out n) == false)
            {
                return false;//数字验证 
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(idnumber.Remove(2)) == -1)
            {
                return false;//省份验证 
            }
            string birth = idnumber.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证 
            }
            string[] arrvarifycode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] ai = idnumber.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(wi[i]) * int.Parse(ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrvarifycode[y] != idnumber.Substring(17, 1).ToLower())
            {
                return false;//校验码验证 
            }
            return true;//符合gb11643-1999标准 
        }


        /// <summary> 
        /// 15位身份证号码验证 
        /// </summary> 
        private static bool checkidcard15(string idnumber)
        {
            long n = 0;
            if (long.TryParse(idnumber, out n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证 
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(idnumber.Remove(2)) == -1)
            {
                return false;//省份验证 
            }
            string birth = idnumber.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证 
            }
            return true;
        }
    }
}