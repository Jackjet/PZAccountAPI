using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Model;
using System.Data.SqlClient;
using API.Util;
using System.Data;

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

        #region 查询用户汇总
        public string QueryUserSummary(int userid)
        {
            const string spName = "Proc_GetUserSummaryDetail";
            var dt = DBUtil.ExecuteDataTableStoreProcedure(spName, DBUtil.MakeParameterInt("userid", userid));
            object data;
            if (dt.Rows.Count > 0)
            {
                data = new
                {
                    //,"cost_money":26116.11,"cost_last":1.00,"cost_salary":8500.00,"trans_out":0.00,"trans_in":16900.00}]}
                    user = new User
                    {
                        user_id = userid,
                        user_name = dt.Rows[0]["username"].ToStrings(),
                        user_photo = dt.Rows[0]["userphoto"].ToStrings(),
                    },
                    detail = new
                    {
                        cost_money = dt.Rows[0]["cost_money"].ToFloat(),
                        cost_last = dt.Rows[0]["cost_last"].ToFloat(),
                        cost_salary = dt.Rows[0]["cost_salary"].ToFloat(),
                        trans_out = dt.Rows[0]["trans_out"].ToFloat(),
                        trans_in = dt.Rows[0]["trans_in"].ToFloat()
                    }
                };
            }
            else
            {
                data = null;
            }
            return JsonHelper.JsonResult(new JsonResultModel
            {
                data = data,
                result = JsonResult.JsonResultSuccess
            });


        }
        #endregion

        #region 查询消费记录
        public string QueryUserCostList(int userid, int pageIndex, int pageSize = 20)
        {
            return QueryAccountList(userid, AccountType.AccountTypeCost.GetHashCode(), pageIndex, pageSize);
        }

        #endregion

        #region 查询工资记录
        public string QueryUserSalaryList(int userid, int pageIndex, int pageSize = 20)
        {
            if (userid == 0)
            {
                return JsonHelper.JsonResult(new JsonResultModel
                {
                    result = JsonResult.JsonResultFailure,
                    msg = "operate_user can't be 0"
                });
            }
            return QueryAccountList(userid, AccountType.AccountTypeSalary.GetHashCode(), pageIndex, pageSize);
        }
        #endregion

        #region 查询消费类别
        public string QueryCategory()
        {
            string sql = "SELECT [id],[parentid],[name] FROM [category]";
            var dt = DBUtil.ExecuteDateTableSQL(sql, null);
            return JsonHelper.JsonResult(dt);
        }
        #endregion

        #region 查询用户的账户交易记录
        public string QueryUserTransfrom(int fromid, int toid, int lastid, int pageSize = 20)
        {
            if (fromid == 0 || toid == 0)
            {
                return JsonHelper.JsonResult(new JsonResultModel
                {
                    result = JsonResult.JsonResultFailure,
                    msg = "from_user or to_user can't be 0"
                });
            }

            return QueryTransform(fromid,toid,lastid,pageSize);
        }
        #endregion

        #region 查询汇总
        public string QueryAllSummary()
        {
            var dt = DBUtil.ExecuteDataTableStoreProcedure("[Proc_QueryAllSummary]", null);
            if (dt.Rows.Count == 1)
            {
                return JsonHelper.JsonResult(new { last = dt.Rows[0]["last_all"].ToFloat(), cost = dt.Rows[0]["cost_all"].ToFloat(), salary = dt.Rows[0]["salary_all"].ToFloat() });
            }
            return JsonHelper.JsonResult(new { last = 0, cost = 0, salary = 0 });
        }
        #endregion

        #region 私有总查询
        private string QueryAccountList(int userid, int type, int pageIndex = 1, int pageSize = 20, int category = 0)
        {
            if (pageSize == 0) { pageSize = 20; }
            const string spName = "[Proc_GetUserAccountsList]";
            var parameters = new List<SqlParameter> {
                DBUtil.MakeParameterInt("type",type),
                 DBUtil.MakeParameterInt("userid",userid),
                  DBUtil.MakeParameterInt("category",category),
                   DBUtil.MakeParameterInt("pageindex",pageIndex),
                   DBUtil.MakeParameterInt("pagesize",pageSize),
            };
            var dt = DBUtil.ExecuteDataTableStoreProcedure(spName, parameters.ToArray());

            #region 暂不采用此方法，转换比较复杂
            //var data = new List<AccountResult>();
            //foreach (DataRow dr in dt.Rows)
            //{
            //    int uid = dr["operate_user"].ToInt();
            //    if (!data.Any(x => x.user.user_id == uid))
            //    {
            //        data.Add(new AccountResult
            //        {
            //            user = new User
            //            {
            //                user_id = dr["operate_user"].ToInt(),
            //                user_name = dr["username"].ToString(),
            //                user_photo = dr["userphoto"].ToString()
            //            }
            //        });
            //    }
            //    var first = data.First(x => x.user.user_id == uid);
            //    first.details.Add(new AccountDetail
            //    {
            //        id = dr["id"].ToInt(),
            //        addtime = dr["addtime"].ToString(),
            //        other = dr["other"].ToString(),
            //        account_type = (AccountType)dr["t"].ToInt(),
            //        category = new Category { category_id = dr["category"].ToInt(), category_name = dr["name"].ToString() },
            //        money = dr["money"].ToFloat()
            //    });
            //}
            #endregion

            return JsonHelper.JsonResult(dt);
        }

        private string QueryTransform(int fuserid, int touserid, int lastid, int pageSize = 20)
        {
            const string spName = "[Proc_QueryTransform]";
            var parameters = new List<SqlParameter> {
                DBUtil.MakeParameterInt("fuserid",fuserid),
                DBUtil.MakeParameterInt("touserid",touserid),
                DBUtil.MakeParameterInt("lastid",lastid),
                DBUtil.MakeParameterInt("pagesize",pageSize)
            };
            var dt = DBUtil.ExecuteDataTableStoreProcedure(spName, parameters.ToArray());
            return JsonHelper.JsonResult(dt);
        }
        #endregion

    }



}