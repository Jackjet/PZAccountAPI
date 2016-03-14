using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Model;
using System.Data.SqlClient;
using API.Util;

namespace API.BLL
{
    public sealed class PZAccountAPI
    {
        #region 单例
        private PZAccountAPI() { }
        private static PZAccountAPI _instance;
        private static object _padLock = new object();
        public static PZAccountAPI Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock(_padLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new PZAccountAPI();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region 添加一条消费记录
        public string AddAccount(Account account)
        {
            try
            {
                var parameters = new List<SqlParameter> {
                DBUtil.MakeParameterInt("fromuser",account.from_user.user_id),
                DBUtil.MakeParameterInt("touser",account.to_user.user_id),
                DBUtil.MakeParameterInt("operateuser",account.operate_user.user_id),
                DBUtil.MakeParameterDecimal("money",account.money),
                DBUtil.MakeParameterInt("category",account.category.category_id),
                DBUtil.MakeParameterInt("type",account.account_type.GetHashCode()),
                DBUtil.MakeParameterVarChar("other",account.other)
            };
                int result = DBUtil.ExecuteNonQueryStoreProcedure("[sq_fanyuepan].[Proc_InsertAccount]", parameters.ToArray());
                return JsonHelper.JsonResult(JsonResult.JsonResultSuccess, null);
            }
            catch (Exception ex)
            {
                return JsonHelper.JsonResult(JsonResult.JsonResultFailure, null, ex.Message);
            }
        }
        #endregion

    }



}