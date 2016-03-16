using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using System.Data;

namespace API.BLL
{
    public sealed class DBUtil
    {
        private static readonly string SqlConnectionString = "server=59.188.255.7;user id=sq_fanyuepan;password=panzi123;database=sq_fanyuepan;Min Pool Size=16;";

        #region ExecuteNonQuery
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int ExecuteNonQuerySQL(string sql, params SqlParameter[] parameters)
        {
            return ExecuteNonQuery(CommandType.Text, sql, parameters);
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int ExecuteNonQueryStoreProcedure(string spName, params SqlParameter[] parameters)
        {
            return ExecuteNonQuery(CommandType.StoredProcedure, spName, parameters);
        }
        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static int ExecuteNonQuery(CommandType commandType, string sql, params SqlParameter[] parameters)
        {
            return SqlHelper.ExecuteNonQuery(SqlConnectionString, commandType, sql, parameters);
        }
        #endregion

        #region ExecuteDataTable

        /// <summary>
        /// 查询Sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataTable ExecuteDateTableSQL(string sql, params SqlParameter[] parameters)
        {
            return ExecuteDateTable(CommandType.Text, sql, parameters);
        }
        /// <summary>
        /// 查询存储过程
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTableStoreProcedure(string spName, params SqlParameter[] parameters)
        {
            return ExecuteDateTable(CommandType.StoredProcedure, spName, parameters);
        }


        /// <summary>
        /// 查询DataTable
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataTable ExecuteDateTable(CommandType commandType,string sql,params SqlParameter[] parameters)
        {
            var ds = SqlHelper.ExecuteDataset(SqlConnectionString, commandType, sql, parameters);
            if (ds.Tables.Count > 0) {
                return ds.Tables[0];
            }
            throw new ArgumentNullException("query result has no tables");
        }
        #endregion 

        public static SqlParameter MakeParameter(string parameter,SqlDbType dbType, object value)
        {
            if (parameter.IndexOf("@")!=0) {
                parameter = string.Concat("@", parameter);
            }
            var p =  new SqlParameter(parameter, dbType, 0);
            p.Value = value;
            return p;
        }

        public static SqlParameter MakeParameterInt(string parameter, int value)
        {
            return MakeParameter(parameter, SqlDbType.Int, value);
        }
        public static SqlParameter MakeParameterVarChar(string parameter, string value)
        {
            return MakeParameter(parameter, SqlDbType.VarChar, value);
        }
        public static SqlParameter MakeParameterDecimal(string parameter, float value)
        {
            return MakeParameter(parameter, SqlDbType.Decimal, value);
        }
        
        

    }
}