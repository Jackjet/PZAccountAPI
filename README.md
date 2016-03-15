PZAccountAPI说明
域名：http://imfyp.com
默认参数：token（测试：123456），否则无法请求结果

	URL
/api/account.ashx?op=add
	接口说明
添加用户数据
	请求方式
POST
	参数列表
参数名称	类型	必须
from_user	int	false
to_user	int	false
operate_user	int	true
type	int	true
category	int	true
money	decimal	true
other	string	false
	返回结果
{
 "result": 0,
 "code": "ok",
 "msg": "",
 "data": null
}
	参数说明
type: 1.消费 2.余额 3.工资 4.转账
from_user:转账人 type为4时必传
to_user:被转账人 type为4时必传
operate_user:操作人id
category:分类id
money:金额
other:备注
---------------------------------------------------------------------------

	URL
/api/account.ashx?op=user_summary
	接口说明
获取用户的汇总数据
	请求方式
POST
	参数列表
参数名称	类型	必须
operate_user	int	true
	返回结果
{
    "result": 0,
    "code": "ok",
    "msg": null,
    "data": {
        "user": {
            "user_id": 1,
            "user_name": " 张三",
            "user_photo": "http: //qlogo2.store.qq.com/qzone/123456789/123456789/100?1448534077"
        },
        "detail": {
            "cost_money": 26116.11,
            "cost_last": 1,
            "cost_salary": 8500,
            "trans_out": 0,
            "trans_in": 16900
        }
    }
}
	参数说明
operate_user 用户id
---------------------------------------------------------------------------

	URL
/api/account.ashx?op=user_cost_list
	接口说明
查询用户的消费列表
	请求方式
GET
	参数列表
参数名称	类型	必须
operate_user	int	true
page_index	int	true
page_size	int	false
	返回结果
{
    "result": 0,
    "code": "ok",
    "msg": null,
    "data": [
        {
            "id": 98,
            "operate_user": 1,
            "username": "张三",
            "userphoto": "http://qlogo2.store.qq.com/qzone/123456789/123456789/100?1448534077",
            "money": 5789.36,
            "category": 3,
            "name": "住",
            "t": 1,
            "tname": "消费",
            "other": "信用卡还款",
            "addtime": "2016-03-11T15:26:01.023"
        },
        {
            "id": 97,
            "operate_user": 3,
            "username": "李四",
            "userphoto": "http://qlogo2.store.qq.com/qzone/123456789/123456789/100?1448534077",
            "money": 30,
            "category": 2,
            "name": "食",
            "t": 1,
            "tname": "消费",
            "other": "午饭晚饭",
            "addtime": "2016-03-11T09:30:13.327"
        }}
	参数说明
operate_user:查询的用户id 传0为所有
page_index:分页索引
page_size:分页每页多少条，默认为20
---------------------------------------------------------------------------

	URL
/api/account.ashx?op=user_salary_list
	接口说明
查询用户的工资记录
	请求方式
GET
	参数列表
参数名称	类型	必须
operate_user	int	true
page_index	int	true
page_size	int	false
	返回结果
{
    "result": 0,
    "code": "ok",
    "msg": null,
    "data": [
        {
            "id": 104,
            "operate_user": 1,
            "username": "张三",
            "userphoto": "http://qlogo2.store.qq.com/qzone/123456789/123456789/100?1448534077",
            "money": 8500,
            "category": 0,
            "name": null,
            "t": 3,
            "tname": "工资",
            "other": "工资",
            "addtime": "2016-02-25T00:00:00"
        }
    ]
}
	参数说明
operate_user:查询的用户id 不能传0
page_index:分页索引
page_size:分页每页多少条，默认为20
---------------------------------------------------------------------------

	URL
/api/account.ashx?op=user_transform
	接口说明
查询用户的转账记录
	请求方式
GET
	参数列表
参数名称	类型	必须
from_user	int	true
to_user	int	true
last_id	int	false
page_size	int	false
	返回结果
{
    "result": 0,
    "code": "ok",
    "msg": null,
    "data": [
        {
            "id": 119,
            "from_user": 2,
            "from_username": "王五",
            "from_userphoto": "http://qlogo2.store.qq.com/qzone/123456789/123456789/100?1448534077",
            "to_user": 1,
            "to_username": "张三",
            "to_userphoto": "http://qlogo2.store.qq.com/qzone/123456789/123456789/100?1448534077",
            "money": 8900,
            "t": 4,
            "tname": "转账",
            "addtime": "2016-03-10T22:20:22"
        }}
	参数说明
from_user: 转账人id
to_user:被转账人id
last_id:上一条



