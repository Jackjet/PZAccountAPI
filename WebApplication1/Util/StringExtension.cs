using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Util
{
    public static class StringExtension
    {
        /// <summary>
        /// 转换为int
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt(this object obj)
        {
            int val = 0;
            string str = obj.ToStrings();
            bool result = int.TryParse(str, out val);
            if (result) { return val; }
            return 0;
        }
        /// <summary>
        /// 转换为float
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static float ToFloat(this object obj)
        {
            float val = 0.0f;
            string str = obj.ToStrings();
            bool result = float.TryParse(str, out val);
            if (result) { return val; }
            return 0.0f;
        }

        /// <summary>
        /// 去掉空格
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToStrings(this object obj, bool trim = true)
        {
            //if (obj == null) { throw new ArgumentNullException("parameter obj is null"); }
            if (obj == null) { return ""; }
            if (trim) { return obj.ToString().Trim(); }
            return obj.ToString();
        }
    }
}