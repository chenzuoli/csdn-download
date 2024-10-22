





---


title: cookie技术


date: 2023\-08\-27 21:34:19


tags: \[cookie, 网络, [http](https://so.csdn.net/so/search?q=http&spm=1001.2101.3001.7020)]


categories: 网络




---


我们经常说的`cookie缓存数据`，`允许cookie`是什么意思?


**Cookie也被称作Cookies，它是一种让网站的服务器端可以把少量数据存储在客户端的硬盘或内存中，而后续又可以从客户端中读取该数据的技术。**


cookie的使用
---------


1. 在网站中，http请求是呈无序状态的
	* 无序状态是指协议对于事务处理没有记忆能力，同一个服务器上你新打开的网页和之前打开的网页之间没有任何联系，你的当前请求和上一次请求究竟是不是一个用户发出的，服务器也无从得知；
2. 为了解决这一问题，就出现了Cookie技术
	* 当用户访问服务器并登录成功后，服务器向客户端返回一些数据(Cookie)；
	* 客户端将服务器返回的Cookie数据保存在本地，当用户再次访问服务器时，浏览器自动携带Cookie数据给服务器，服务器便知道访问者的身份信息了；
	* 值得注意的是，浏览器发送的是当前本地保存的所有作用在当前域且在有效期内的cookie；
	* 也就是说由上一次访问当前网站某一板块时服务器发送的旧cookie如果未过期，在访问其他板块的新请求中也会被携带，一同发送出去。这就是为什么有些时候我们抓包获取到了cookie，但用它测试连接时却发现这个cookie可有可无；
	* 单个Cookie数据大小一般规定不超过3KB。


设置cookie
--------


Cookie一般通过Response对象的set\_cookie()方法来设置，其基本语法如下：  
 前两个参数必须设置，后续参数则为可选参数：



```
set_cookie(key,value[,max_age,expires,path,domain,secure,httponly,samesite])

* 1
* 2

```

* set\_cookie()的参数说明如下：




| 参数 | 描述 |
| --- | --- |
| key(或name) | 必需项，规定cookie的名称，字符串 |
| value | 必需项，规定cookie的内容，字符串 |
| max\_age | 可选项，规定cookie的失效时间（单位：秒），与expires同时设置时，其优先级更高 |
| expires | 可选项，规定cookie的有效期，使用具体日期 |
| path | 可选项，规定cookie在当前web下那些目录有效，默认情况下为所有目录"/"有效 |
| domain | 可选项，规定cookie作用的有效域（如：127\.0\.0\.1）或域名（如：baidu.com），默认情况下只作用于设置该cookie的域 |
| secure | 可选项，规定cookie是否只能通过安全的HTTPS连接传输，默认关闭 |
| httponly | 可选项，规定cookie是否只能被HTTP连接访问，默认关闭, 即是否拒绝JavaScript访问Cookie |
| samesite | 可选项，规定cookie是否只能被附加在同一站点（客户端）的请求中。 |


不同的web框架设置cookie的方法不同，这里就不一一赘述了，大家可以自行网上soso。




---


**每日一记。**


我是chenzuoli，欢迎交流。


