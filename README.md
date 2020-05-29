# WebAPIDemo
 RayData WebAPI服务端示例

当前实例格式为所有参数都启用后的格式，后台按照这个格式进行有效性认证。
注：参数按照字母排序后进行校验的计算；
    本示例使用HMACSHA1计算校验。
    
## WebAPI01 请求格式（post）：
### ~/api/test01?nonce= &timestamp= &sign= 
方法名：test01
参数：随机数、时间戳（必须）、校验
功能：对a和b数据进行相加操作


## WebAPI02 Token请求格式（get）：
### ~/api/token?appid= &timestamp= &sign=
方法名：token
参数：appid、时间戳（必须）、校验
功能：签名获取Token

## WebAPI02 请求格式（post）：
### ~/api/test02?nonce= &timestamp= &token= &sign=
方法名：test02
参数：随机数、时间戳（必须）、token、校验
功能：对a和b数据进行相加操作

实际情况，方法名，参数可以灵活配置。


# 接口开发标准
简单接口请求节点(方式一)WebAPI01
1.	URL 如：https://localhost/test/data01
2.	随机数 字段名：nonce=0422860342
3.	时间戳 字段名：timestamp=1554365738
4.	校验值 字段名：sign=8cwZxjASO
GET方式请求：
https://apis.map.qq.com/v1?nonce=0422860342&timestamp=1554365738&sign=8cwZxjASO
Post方式请求：
https://apis.map.qq.com/v1?nonce=0422860342&timestamp=1554365738&sign=8cwZxjASO
数据：{"a":"123","b":"321"}

授权接口请求节点(方式二)WebAPI02
1）第一次请求token授权
1.	获取TokenURL 如：https://localhost/test/token
2.	客户端标识 字段名：AppId=APPID
3.	时间戳 字段名：timestamp=1554365738
4.	校验值 字段名：sign=8cwZxjASO
服务端根据需求自由选用以上字段做相关处理。
Token授权接口服务端需要返回字段：
1.	响应码 字段名code 当code=0时表示返回token正常
验证失败， code返回非0值，并且返回错误信息，字段名 error
2.	Token 字段名 access_token
3.	有效时间 字段名 expires_in （单位秒）
### 返回数据规定为json数据格式，如：
{
"code":"0",
"access_token":"token",
"expires_in":"3600",
}
### 授权失败返回json，如：
{
"code":"-1",
"error":"appid validation failed",
}

2）第二次请求数据
1.	URL 如：https://localhost/test/data02
2.	随机数 字段名：nonce=0422860342
3.	时间戳 字段名：timestamp=1554365738
4.	校验值 字段名：sign=8cwZxjASO
5.	客户端标识 字段名：AppId=APPID
6.	Token 字段名：token= XE9NZDy0t8W3TcNekr
以上字段根据需求自由选用，做token验证处理返回数据，无类型限制。
