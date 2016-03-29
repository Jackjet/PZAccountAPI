using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Util;
using API.BLL;
using System.Threading.Tasks;

namespace WebApplication1.api
{
    /// <summary>
    /// upload 的摘要说明
    /// </summary>
    public class upload : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string base64 = context.Request.Form["base64"];
            string token = context.Request.Form["token"];


            if (token.ToStrings() != ApiConfig.RequestToken)
            {
                context.Response.Write(JsonHelper.JsonResult(JsonResult.JsonResultFailure, null, "token is not valid"));
                return;
            }

            string userid = context.Request.Form["operate_user"];
            string result = string.Empty;
            if (base64 != null && base64.Length > 0)
            {
                string filePath = context.Server.MapPath("/UserPhotos/");
                string imgName = ImageUtil.Base64StringToImage(filePath, base64);
                //最后要更新用户的头像
                string httpImgName = string.Format("http://imfyp.com/userphotos/{0}", imgName);
                Task.Factory.StartNew(() =>
                {
                    PZAccountAPI.Instance.UpdateUserPhoto(userid.ToInt(), httpImgName);
                });
                result = JsonHelper.JsonResult(imgName != null ? JsonResult.JsonResultSuccess : JsonResult.JsonResultFailure, null);
            }
            else
            {
                result = JsonHelper.JsonResult(JsonResult.JsonResultFailure, null, "image base64 is not valid");
            }
            context.Response.Write(result);
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