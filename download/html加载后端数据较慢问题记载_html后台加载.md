





---


### 标题: html加载后端数据较慢问题记载 日期: 2024\-04\-06 22:29:00 标签: \[html, [flask](https://so.csdn.net/so/search?q=flask&spm=1001.2101.3001.7020)] 分类: \[Python, Flask]


网站页面最近加载很慢，不知道为什么，这里记录一下，一步一步查问题的思路。


说下环境
----


python3\.8  
 flask2\.3\.3  
 mysql5\.7


问题
--


[刷新网页](https://so.csdn.net/so/search?q=%E5%88%B7%E6%96%B0%E7%BD%91%E9%A1%B5&spm=1001.2101.3001.7020)<https://lezhifu.cc/admin/qrcode_list_op>时，需要7s多的时间


F12查哪里慢
-------


F12查看了浏览器的Network，看看到底加载什么内容时很慢  
 ![slow](https://i-blog.csdnimg.cn/blog_migrate/04be70b32196f4497e5f479c6207a0e0.png)


点进去，看到`waiting for server response`这里花了7s多，为啥呢？  
 ![timeline](https://i-blog.csdnimg.cn/blog_migrate/f8a54eb5864b88e7866ce48a6b297bab.png)


查接口为什么慢
-------


![flask_interface](https://i-blog.csdnimg.cn/blog_migrate/a7b8e6a5d8b8cb6586362933a6297c17.png)


第1步分页查询，每次查10条，这里应该不慢


第2步根据分页查询结果进行循环，然后根据id查询payment表，这里可能比较慢，是不是可以改成两表关联形式呢，直接一次性查出来。


那改吧，改成join，这里的需求是join、查询指定字段、分页，如下是flask的查询语句：


使用join方法根据payment\_id连接QRCode和Payment表
--------------------------------------


joined\_query\_pagination \= db.session.query(  
 QRCode.user\_id,  
 QRCode.content\_id,  
 QRCode.header,  
 QRCode.payment\_id,  
 QRCode.create\_time,  
 Payment.payment\_amount,  
 QRCode.status  
 ).join(Payment, QRCode.payment\_id \=\= Payment.payment\_id)   
 .paginate(page\=page, per\_page\=per\_page, error\_out\=False)


ok，性能提高了好多，116ms就查出来了。


![after](https://i-blog.csdnimg.cn/blog_migrate/b50e3f9612c6a89ec8b86298764e2f4c.png)


完美。




---


欢迎关注微信公众号，您的资源可变现：【乐知付加密平台】


一起学习，一起进步。  
 


