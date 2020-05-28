# WebAPIDemo
 RayData WebAPI服务端示例

当前实例格式为所有参数都启用后的格式，后台按照这个格式进行有效性认证。
WebAPI01请求格式：
~/api/test01?nonce= &timestamp= &sign= 
方法名：test01
功能：对a和b数据进行相加操作

WebAPI02Token请求格式：
~/api/token?appid= &timestamp= &sign=
方法名：token
功能：签名获取Token

WebAPI02请求格式：
~/api/test02?nonce= &timestamp= &token= &sign=
方法名：test02
功能：对a和b数据进行相加操作

实际情况，方法名，参数可以灵活配置。
