





---


### 标题: 乐知付\-如何制作html文件可双击跳转到指定页面？ 标签: \[乐知付, 乐知付加密, 密码管理] 分类: \[网站,html]


![flower](https://i-blog.csdnimg.cn/blog_migrate/1442d1ca5e2f623e04818c12b0b3ff17.png)


为了便于买家理解使用链接进行付费获取密码；现开发个小工具，将支付链接转为浏览器可识别的文件，双击打开即可跳转到浏览器支付页面。


文件命名：**解压密码\-双击查看.html**  
 内容如下：



```
<html>
<head>
<meta http-equiv="refresh"content="0;url=http://lezhifu.cc/static/html/qrcode.html?p=34">
</head>
</html>
* 1
* 2
* 3
* 4
* 5

```

这段内容是一个简单的浏览器页面，它包含了一个自动刷新的元标记（meta tag）来实现页面的[重定向](https://so.csdn.net/so/search?q=%E9%87%8D%E5%AE%9A%E5%90%91&spm=1001.2101.3001.7020)。  
 商家使用时，**必须将上述链接替换成您本身的链接**，上述链接 [http://lezhifu.cc/static/html/qrcode.html?p\=34](http://lezhifu.cc/static/html/qrcode.html?p=34%22%3E) **为演示所用。**


![html内容](https://i-blog.csdnimg.cn/blog_migrate/2b5378d0c3a93457a7599f0440bad393.png)


**具体解释如下：**



> * ：这是HTML页面的根元素。
> * ：这是文档头部的元素，其中包含了页面的元数据和其他引用信息。
> * ：这是一个元标记，用于在HTML文档中提供元数据。
> * http\-equiv\=“refresh”：这是元素的一个属性，指定了HTTP头部的一个等价字段。
> * content\="0;url\=[http://lezhifu.cc/static/html/qrcode.html?p\=34"](http://lezhifu.cc/static/html/qrcode.html?p=34%22%60)：这是元素的另一个属性，它指定了页面的刷新时间和重定向的目标URL。
> 	+ 0：表示页面在加载后立即刷新。
> 	+ url\=[http://lezhifu.cc/static/html/qrcode.html?p\=34](http://lezhifu.cc/static/html/qrcode.html?p=34%60)：是重定向的目标URL，即在刷新后将页面导航到该URL。
> * ：表示头部元素的结束标签。
> * ：表示HTML页面的结束标签。


因此，这段内容的含义是：当买家双击页面时，即打开了支付页面 [http://lezhifu.cc/static/html/qrcode.html?p\=34](http://lezhifu.cc/static/html/qrcode.html?p=34%60) 这个链接。  
 扫描此链接里的二维码，完成付款，即可看到您的资源密码了。


![pay](https://i-blog.csdnimg.cn/blog_migrate/0ce21882fdc8b511343e86bb62c28c8b.png)


示例文件，点击下载查看：[解压密码\_模板.html](https://lezhifu.cc/help/%E8%A7%A3%E5%8E%8B%E5%AF%86%E7%A0%81_%E6%A8%A1%E6%9D%BF.html)




---


欢迎关注微信公众号，你的资源可变现：【乐知付加密平台】


一起学习，一起进步。  
 


