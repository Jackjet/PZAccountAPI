using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Util;
using API.BLL;
using API.Model;

namespace API.api
{
    /// <summary>
    /// account 的摘要说明
    /// </summary>
    public class account : IHttpHandler
    {
        /// <summary>
        /// 添加账单
        /// </summary>
        const string OPERATE_ADD = "add";
        /// <summary>
        /// 查询个人汇总
        /// </summary>
        const string OPERATE_QUERY_PERSONAL_SUMMARY = "user_summary";
        /// <summary>
        /// 查询历史消费记录
        /// </summary>
        const string OPERATE_QUERY_COST_LIST = "user_cost_list";
        /// <summary>
        /// 查询工资列表
        /// </summary>
        const string OPERATE_QUERY_PERSONAL_SALARY_LIST = "user_salary_list";
        /// <summary>
        /// 查询转账记录
        /// </summary>
        const string OPERATE_QUERY_USER_TRANSFEROM = "user_transform";
        private object GetRequestValue(HttpContext context, string key,bool form = false)
        {
            if (form)
            {
                return context.Request.Form[key];
            }
            return context.Request.QueryString[key];
        }

        private void ValidateToken(HttpContext context,out bool rightToken)
        {
            object tokenNotForm = GetRequestValue(context, "token");
            object tokenForm = GetRequestValue(context, "token",true);
            if (tokenNotForm == null && tokenForm == null)
            {
                rightToken = false;
            }
            else
            {
                if (tokenForm.ToStrings() == "123456" || tokenNotForm.ToStrings() == "123456")
                {
                    rightToken = true;
                }
                else {
                    rightToken = false;
                }
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            bool validToken = false;
            ValidateToken(context, out validToken);
            //如果token没有验证通过
            if (!validToken) {
                context.Response.Write(JsonHelper.JsonResult(JsonResult.JsonResultFailure, null, "token is not valid"));
                return;
            }
            string op = GetRequestValue(context, "op").ToStrings();
            bool form = op == OPERATE_ADD;//只有添加的时候才用form提交
            int from_user = GetRequestValue(context, "from_user",form).ToInt();
            int to_user = GetRequestValue(context, "to_user",form).ToInt();
            int operate_user = GetRequestValue(context, "operate_user", form).ToInt();
            int category_id = GetRequestValue(context, "category", form).ToInt();
            int type = GetRequestValue(context, "type", form).ToInt();
            int pageindex = GetRequestValue(context, "page_index", form).ToInt();
            int pagesize = GetRequestValue(context, "page_size", form).ToInt();
            int lastid = GetRequestValue(context, "last_id", form).ToInt();
            float money = GetRequestValue(context, "money", form).ToFloat();
            string other = GetRequestValue(context, "other", form).ToStrings();
          

            string result = string.Empty;
            switch (op)
            {
                case OPERATE_ADD:
                    result = Add(type, from_user, to_user, category_id, money, operate_user, other);
                    break;
                case OPERATE_QUERY_PERSONAL_SUMMARY:
                    result = PZAccountAPI.Instance.QueryUserSummary(operate_user);
                    break;
                case OPERATE_QUERY_COST_LIST:
                    result = PZAccountAPI.Instance.QueryUserCostList(operate_user, pageindex, pagesize);
                    break;
                case OPERATE_QUERY_PERSONAL_SALARY_LIST:
                    result = PZAccountAPI.Instance.QueryUserSalaryList(operate_user, pageindex, pagesize);
                    break;
                case OPERATE_QUERY_USER_TRANSFEROM:
                    result = PZAccountAPI.Instance.QueryUserTransfrom(from_user, to_user, lastid, pagesize);
                    break;
                default:
                    result = JsonHelper.JsonResult(new JsonResultModel
                    {
                        result = JsonResult.JsonResultFailure,
                        msg = "op is not valid"
                    });
                    break;
            }
            context.Response.Write(result);
        }

        #region 私有方法
        private string Add(int type,int from_user,int to_user,int category_id,float money,int operate_user,string other)
        {
            if (type == 0) {
                return JsonHelper.JsonResult(JsonResult.JsonResultFailure, null, "type is not valid");
            }
            string result = PZAccountAPI.Instance.AddAccount(new Model.Account
            {
                account_type = (AccountType)type,
                from_user = new User { user_id = from_user },
                to_user = new User { user_id = to_user },
                category = new Category { category_id = category_id },
                money = money,
                operate_user = new User { user_id = operate_user },
                other = other
            });
            return result;
        }

        #endregion
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}