using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace API.Util
{
    /// <summary>
    /// 序列化帮助类
    /// </summary>
    public class JsonHelper
    {
        /// <summary>
        /// 将消息序列化
        /// </summary>
        /// <param name="result"></param>
        /// <param name="data"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        public static string JsonResult(JsonResult result, object data, string errmsg = "")
        {
            var model = GetResult(result, data, errmsg);
            return JsonResult(model);
        }
        /// <summary>
        /// 将结果序列化
        /// </summary>
        /// <param name="resultModel"></param>
        /// <returns></returns>
        public static string JsonResult(JsonResultModel resultModel)
        {
            return JsonConvert.SerializeObject(resultModel);
        }
        /// <summary>
        /// 构建消息实体
        /// </summary>
        /// <param name="result"></param>
        /// <param name="data"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        private static JsonResultModel GetResult(JsonResult result, object data, string errmsg = "")
        {
            JsonResultModel resultModel = new JsonResultModel(result, data, errmsg);
            return resultModel;
        }

    }

    public class JsonResultModel
    {
        public JsonResultModel()
        {
            result = JsonResult.JsonResultSuccess;
        }
        public JsonResultModel(JsonResult result, object data, string errmsg)
        {
            this.result = result;
            this.data = data;
            msg = errmsg;
        }
        public JsonResult result { get; set; }
        public string code {
            get
            {
                return result == JsonResult.JsonResultSuccess ? "ok" : "err";
            }
        }
        public string msg { get; set; }
        public object data { get; set; }
    }

    /// <summary>
    /// 结果枚举 成功，失败
    /// </summary>
    public enum JsonResult
    {
        JsonResultSuccess = 0,
        JsonResultFailure = 1
    }
}