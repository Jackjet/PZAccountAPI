using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Model
{
    public class User
    {
        public int user_id { get; set; }
        public string user_name { get; set; }
        public string user_photo { get; set; }
    }

    public class Category
    {
        public int category_id { get; set; }
        public string category_name { get; set; }
        public Category parent_category { get; set; }
    }

    public class Account
    {
        public Account()
        {
            from_user = new User();
            to_user = new User();
            operate_user = new User();
            category = new Category();
            account_type = AccountType.AccountTypeNone;
        }
        public User from_user { get; set; }
        public User to_user { get; set; }
        public User operate_user { get; set; }
        public float money { get; set; }
        public Category category { get; set; }
        public AccountType account_type { get; set; }
        public string other { get; set; }
    }

    public enum AccountType
    {
        AccountTypeNone = 0,
        /// <summary>
        /// 消费
        /// </summary>
        AccountTypeCost = 1,
        /// <summary>
        /// 余额
        /// </summary>
        AccountTypeLast = 2,
        /// <summary>
        /// 工资
        /// </summary>
        AccountTypeSalary = 3,
        /// <summary>
        /// 转账
        /// </summary>
        AccountTypeTransfer = 4
    }
}