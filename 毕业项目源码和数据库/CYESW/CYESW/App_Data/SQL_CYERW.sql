
--create database CYESW
--drop database CYESW
--go
--商品浏览次数，新旧程度，int类型吧，分为1全新,2-95新，3-9成新，，，--原价--详细地址。添加两个时间，擦亮
--添加站内推广表，站外推广表

use CYESW
go
if exists(select * from sysobjects where name='UserInfo')
drop table UserInfo;
go
--用户表
create table UserInfo(
  UserId int primary key identity(1,1),
  UserName nvarchar(10) not null,--用户名称
  UserEmile nvarchar(100) not null ,--用户邮箱
  UserPwd nvarchar(100) not null ,--用户密码
  Age int , --年龄
  moneys money,--用户存款
  Info nvarchar(100),--个人简介
  Images nvarchar(100),--头像
  IsDelete int ,--是否删除
  AddTime datetime,--注册时间
  EndTime datetime,--最后时间
  IsManage int default(0),--是否为管理员，1为是
)
go
--select * from userinfo
insert into UserInfo values('admin','123456@qq.com','123456',20,200.2,'我是帅哥，高冷，不想说话','man.jpg',0,GETDATE(),GETDATE(),0)
insert into UserInfo values('admin2','1234567@qq.com','123456',20,200.2,'我是美女，hi，一起play啊','women.jpg',0,GETDATE(),GETDATE(),1)

go
if exists(select * from sysobjects where name='Addres')
drop table Addres;
go
--地址表
create table Addres(
  AddresId int primary key identity(1,1),
  Name nvarchar(20) not null ,--收获人姓名
  UserId int foreign key references UserInfo(UserId),--用户表
  Addresss1 nvarchar(100) not null ,--大致地址（省，市，县）
  Addresss2 nvarchar(100) not null ,--详细地址
  Phone nvarchar(20),--手机号码
  IsDelete int ,--是否删除
)
go
--select * from Addres
insert into Addres values('陈能武',1,'湖南省，长沙市，长沙县','湘龙街道100号','15855698562',0)


go
if exists(select * from sysobjects where name='RealName')
drop table RealName;
go
--实名认证表
create table RealName(
  RealNameId int primary key identity(1,1),
  UserId int foreign key references UserInfo(UserId),--用户表
  IdCard nvarchar(20) not null ,--身份证
  AddTime datetime,--认证时间
  IsDelete int ,--是否删除
)
go
--select * from RealName
insert into RealName values(1,'430223200005052145',GETDATE(),0)


go
if exists(select * from sysobjects where name='GoodsType')
drop table GoodsType;
go
--商品类型（手机，电脑，图书，服装）
create table GoodsType(
  GoodsTypeId int primary key identity(1,1),
  TypeName nvarchar(20) not null ,--类型名称
  GoodsTypeBId int foreign key references GoodsType(GoodsTypeId),--外键自己的主键，二级类型，三级类型
)
go
--select * from GoodsType
insert into GoodsType values('电脑',null),('手机平板',null),('办公用品',null),('数码产品',null),('生活用品',null),('图书',null),('虚拟道具',null),('其它',null)
go
insert into GoodsType values('笔记本',1),('电脑组件',1),('外设产品',1),('台式机',1),('其它电脑',1)
go
insert into GoodsType values('苹果手机',2),('小米手机',2),('华为手机',2),('OPPO手机',2),('vivo手机',2),('三星手机',2),('魅族手机',2),('诺基亚手机',2),('锤子手机',2),('联想手机',2),('酷派手机',2),
('苹果平板',2),('华为平板',2),('小米平板',2),('微软平板',2),('三星平板',2),('谷歌平板',2),('戴尔平板',2),('联想平板',2),('其它手机平板',2)
insert into GoodsType values('打印机',3),('投影机',3),('传真设备',3),('点钞机',3),('扫描仪',3),('条形码采集机',3),('碎纸机',3),('门禁设备',3),('收银机',3),('写字板',3),('其他办公用品',3)
insert into GoodsType values('耳机',4),('音箱',4),('游戏机',4),('相机',4),('穿戴设备',4),('随身听',4),('其他数码产品',4)
insert into GoodsType values('男装',5),('女装',5),('鞋',5),('包包',5),('厨具',5),('家电',5),('其他生活用品',5)
insert into GoodsType values('文学小说',6),('经管励志',6),('儿童教育',6),('人文社科',6),('科技科普',6),('生活艺术',6),('教材辅助',6),('外文原版',6),('唱片影片',6),('其他图书',6)
insert into GoodsType values('苹果笔记本',9),('联想笔记本',9),('戴尔笔记本',9),('惠普笔记本',9),('ThinkPad笔记本',9),('三星笔记本',9),('东芝笔记本',9),('华为笔记本',9),('小米笔记本',9),('华硕笔记本',9),('微软笔记本',9),('神舟笔记本',9),('雷神笔记本',9),('索尼笔记本',9),('炫龙笔记本',9),('其它笔记本',9)
insert into GoodsType values('内存',10),('硬盘',10),('机箱',10),('电源',10),('声卡',10),('刻录机',10),('路由器',10),('装机配件',10),('显示器',10),('CPU处理器',10),('其他电脑组件',10)
insert into GoodsType values('鼠标',11),('键盘',11),('U盘',11),('手写板',11),('硬盘盒',11),('摄像头',11),('其他外设产品',11)
insert into GoodsType values('苹果台式机',12),('联想台式机',12),('戴尔台式机',12),('惠普台式机',12),('ThinkPad台式机',12),('三星台式机',12),('东芝台式机',12),('华为台式机',12),('小米台式机',12),('华硕台式机',12),('微软台式机',12),('神舟台式机',12),('雷神台式机',12),('索尼台式机',12),('炫龙台式机',12),('其它台式机',12)

go
if exists(select * from sysobjects where name='Goodsaddress')
drop table Goodsaddress;
go
--商品地区（市，县）(分二级)
create table Goodsaddress(
  GoodsaddressId int primary key identity(1,1),
  TypeName nvarchar(20) not null ,--类型名称
  GoodsaddressBId int foreign key references Goodsaddress(GoodsaddressId),--外键自己的主键，二级类型，三级类型
)
go
--select * from Goodsaddress
insert into Goodsaddress values('华东地区',null),('华南地区',null),('华中地区',null),('华北地区',null),('东北地区',null),('西南地区',null),('西北地区',null)
go
insert into Goodsaddress values('江苏',1),('浙江',1),('福建',1),('山东',1),('江西',1),('安徽',1)
insert into Goodsaddress values('广东',2),('海南',2),('广西',2)
insert into Goodsaddress values('湖北',3),('湖南',3),('河南',3)
insert into Goodsaddress values('内蒙古',4),('河北',4),('山西',4)
insert into Goodsaddress values('辽宁',5),('吉林',5),('黑龙江',5)
insert into Goodsaddress values('四川',6),('西藏',6),('云南',6),('贵州',6)
insert into Goodsaddress values('陕西',7),('新疆',7),('青海',7),('宁夏',7),('甘肃',7)
go
insert into Goodsaddress values('南京',8),('常熟',8),('常州',8),('淮安',8),('昆山',8),('连云港',8),('南通',8),('苏州',8),('太仓',8),('泰州',8),('无锡',8),('宿迁',8),('徐州',8),('盐城',8),('扬州',8),('张家港',8),('镇江',8)
insert into Goodsaddress values('杭州',9),('潮州',9),('嘉兴',9),('金华',9),('丽水',9),('宁波',9),('衢州',9),('绍兴',9),('台州',9),('温州',9),('舟山',9)
insert into Goodsaddress values('福州',10),('龙岩',10),('南平',10),('宁德',10),('莆田',10),('泉州',10),('三明',10),('厦门',10),('漳州',10)
insert into Goodsaddress values('济南',11),('滨州',11),('德州',11),('东营',11),('菏泽',11),('济宁',11),('莱芜',11),('聊城',11),('临汐',11),('青岛',11),('日照',11),('泰安',11),('威海',11),('潍坊',11),('烟台',11),('枣庄',11),('淄博',11)
insert into Goodsaddress values('南昌',12),('抚州',12),('赣州',12),('吉安',12),('景德镇',12),('九江',12),('萍乡',12),('上饶',12),('新余',12),('宜春',12),('鹰潭',12)
insert into Goodsaddress values('合肥',13),('安庆',13),('蚌埠',13),('毫州',13),('巢湖',13),('池州',13),('滁州',13),('阜阳',13),('淮北',13),('淮南',13),('黄山',13),('六安',13),('马鞍山',13),('铜陵',13),('芜湖',13),('宿州',13),('宣城',13)

insert into Goodsaddress values('广州',14),('潮州',14),('东莞',14),('佛山',14),('河源',14),('惠州',14),('江门',14),('揭阳',14),('茂名',14),('梅州',14),('清远',14),('汕头',14),('汕尾',14),('韶关',14),('深圳',14),('阳江',14),('云浮',14),('湛江',14),('肇庆',14),('中山',14),('珠海',14)
insert into Goodsaddress values('海口',15),('白沙',15),('保亭',15),('昌江',15),('澄迈',15),('儋州',15),('安定',15),('东方',15),('乐东',15),('临高',15),('陵水',15),('琼海',15),('琼中',15),('三沙',15),('三亚',15),('屯昌',15),('万宁',15),('文昌',15),('五指山',15)
insert into Goodsaddress values('南宁',16),('百色',16),('北海',16),('崇左',16),('防城港',16),('贵港',16),('桂林',16),('河池',16),('贺州',16),('来宾',16),('柳州',16),('钦州',16),('梧州',16),('玉林',16)

insert into Goodsaddress values('武汉',17),('鄂州',17),('恩施',17),('黄冈',17),('荆门',17),('潜江',17),('神农架',17),('十堰',17),('随州',17),('天门',17),('仙桃',17),('咸宁',17),('襄阳',17),('孝感',17),('宜昌',17)
insert into Goodsaddress values('长沙',18),('常德',18),('郴州',18),('衡阳',18),('怀化',18),('娄底',18),('邵阳',18),('湘潭',18),('湘西',18),('益阳',18),('永州',18),('岳阳',18),('张家界',18),('株洲',18)
insert into Goodsaddress values('郑州',19),('安阳',19),('鹤壁',19),('济源',19),('焦作',19),('开封',19),('洛阳',19),('漯河',19),('南阳',19),('平顶山',19),('濮阳',19),('三门峡',19),('商丘',19),('新乡',19),('信阳',19),('许昌',19),('周口',19),('驻马店',19)

insert into Goodsaddress values('呼和浩特',20),('阿拉善',20),('巴彦卓尔',20),('包头',20),('赤峰',20),('鄂尔多斯',20),('呼伦贝尔',20),('通辽',20),('乌海',20),('乌兰察布',20),('锡林郭勒',20),('兴安',20)
insert into Goodsaddress values('石家庄',21),('保定',21),('沧州',21),('承德',21),('邯郸',21),('衡水',21),('廊坊',21),('秦皇岛',21),('唐山',21),('邢台',21),('张家口',21)
insert into Goodsaddress values('太原',22),('大同',22),('晋城',22),('临汾',22),('吕梁',22),('朔州',22),('忻州',22),('运城',22),('长治',22)

insert into Goodsaddress values('沈阳',23),('鞍山',23),('本溪',23),('朝阳',23),('大连',23),('丹东',23),('抚顺',23),('阜新',23),('葫芦岛',23),('锦州',23),('辽阳',23),('盘锦',23),('铁岭',23),('营口',23)
insert into Goodsaddress values('长春',24),('白城',24),('白山',24),('吉林',24),('辽源',24),('四平',24),('松原',24),('通化',24),('延边',24)
insert into Goodsaddress values('哈尔滨',25),('大庆',25),('大兴安岭',25),('鹤岗',25),('黑河',25),('鸡西',25),('佳木斯',25),('牡丹江',25),('七台河',25),('齐齐哈尔',25),('双鸭山',25),('绥化',25),('伊春',25)


insert into Goodsaddress values('成都',26),('阿坝',26),('巴中',26),('达州',26),('德阳',26),('甘孜',26),('广安',26),('广元',26),('乐山',26),('凉山',26),('泸州',26),('眉州',26),('阿坝',26),('绵阳',26),('南充',26),('内江',26),('攀枝花',26),('遂宁',26),('雅安',26),('宜宾',26),('资阳',26),('自贡',26)
insert into Goodsaddress values('拉萨',27),('阿里',27),('昌都',27),('林芝',27),('那曲',27),('日喀则',27),('山南',27)
insert into Goodsaddress values('昆明',28),('保山',28),('楚雄',28),('大理',28),('德宏',28),('迪庆',28),('红河',28),('丽江',28),('临沧',28),('怒江',28),('普洱',28),('曲靖',28),('文山',28),('西双百纳',28),('玉溪',28),('昭通',28)
insert into Goodsaddress values('贵阳',29),('安顺',29),('毕节',29),('六盘水',29),('黔东南',29),('铜仁',29),('遵义',29)

insert into Goodsaddress values('西安',30),('安康',30),('宝鸡',30),('汉中',30),('商洛',30),('铜川',30),('渭南',30),('咸阳',30),('延安',30),('榆林',30)
insert into Goodsaddress values('乌鲁木齐',31),('阿克苏',31),('阿拉尔',31),('阿勒泰',31),('巴音郭楞',31),('博尔塔拉',31),('昌吉',31),('哈密',31),('和田',31),('喀什',31),('克拉玛依',31),('克孜勒苏',31),('石河子',31),('塔城',31),('图木舒克',31),('吐鲁番',31),('五家渠',31),('伊犁',31)
insert into Goodsaddress values('西宁',32),('果洛',32),('海北',32),('海东',32),('海南',32),('海西',32),('黄南',32),('玉树',32)
insert into Goodsaddress values('银川',33),('固原',33),('石嘴山',33),('吴忠',33),('中卫',33)
insert into Goodsaddress values('兰州',34),('白银',34),('定西',34),('甘南',34),('嘉峪关',34),('金昌',34),('酒泉',34),('临夏',34),('陇南',34),('平凉',34),('庆阳',34),('天水',34),('武威',34),('张掖',34)


go
if exists(select * from sysobjects where name='Goods')
drop table Goods;
go
--商品
create table Goods(
  GoodsId int primary key identity(1,1),
  UserId int foreign key references UserInfo(UserId),--用户表
  GoodsTypeId int foreign key references GoodsType(GoodsTypeId),--类型表
  GoodsaddressId int foreign key references Goodsaddress(GoodsaddressId),--地区表--城市

  IsNew int,--是否全新
  Name nvarchar(100) not null ,--商品名称,标题
  Info nvarchar(200) not null ,--商品信息
  Price float,--价格
  IsState int,--商品状态，1上架，2下架，3卖出，4删除
  CreateTime datetime,--发布时间
  UpdateTime datetime ,--更新时间（点亮宝贝）
)
go
--select * from Goods

insert into Goods values(1,14,18,1,'测试数据一','全新的联想电脑，便宜出了12312312',8880.5,1,getdate(),getdate())
insert into Goods values(1,13,18,1,'测试数据二','全新的苹果电脑，随便出，是打发法',9980.5,1,getdate(),getdate())
insert into Goods values(1,13,18,1,'测试数据三，大甩卖','啊沙发沙发打赏没有介绍',1280.25,0,getdate(),getdate())
insert into Goods values(1,13,18,1,'测试数据四，快乐送','啊发大水发大水范德萨给点钱意思一下',0,1,getdate(),getdate())
insert into Goods values(1,14,18,1,'测试数据五，便宜送','全新的苹果电脑，随便出，给点钱意思一下',9280.5,2,getdate(),getdate())

insert into Goods values(1,14,18,1,'测试数据六','全新的联想电脑，便宜出了',180.5,2,getdate(),getdate())
insert into Goods values(1,13,18,1,'测试数据七','全新的苹果电脑，随便出，给点钱意思一下',2660.5,2,getdate(),getdate())
insert into Goods values(1,13,18,1,'测试数据八，大甩卖','没有介绍',2666.25,0,getdate(),getdate())
insert into Goods values(1,13,18,1,'测试数据九，快乐送','5555，随便出，给点钱意思一下',188,2,getdate(),getdate())
insert into Goods values(1,14,18,1,'测试数据十，便宜送','给弄了，随便出，给点钱意思一下',18758.5,2,getdate(),getdate())

insert into Goods values(1,14,18,1,'测试数据十一','全新钱钱222，便宜出了',1280.5,2,getdate(),getdate())
insert into Goods values(1,13,18,1,'测试数据十二','全新的苹果电脑，随便出，给点钱意思一下',22.5,2,getdate(),getdate())
insert into Goods values(1,13,18,1,'测试数据十三，大甩卖','没有介绍',280.25,0,getdate(),getdate())
insert into Goods values(1,13,18,1,'测试数据十四，快乐送','全新的苹果电脑，随便出，给点钱意思一下',888,2,getdate(),getdate())
insert into Goods values(1,14,18,1,'测试数据十五，啊撒范德萨发生发生','全新的苹果电脑，随便出，给点钱意思一下',10.5,2,getdate(),getdate())

insert into Goods values(1,14,18,1,'测试数据十六','全新的联想电脑，便宜出了',1280.5,2,getdate(),getdate())
insert into Goods values(1,13,18,1,'测试数据十七','全新的苹果电脑，随便出，给点钱意思一下',280.5,2,getdate(),getdate())
insert into Goods values(1,13,18,1,'测试数据十八，大甩卖','没有介绍',280.25,0,getdate(),getdate())
insert into Goods values(1,13,18,1,'测试数据十九，快乐送','全新的苹果电脑，随便出，给点钱意思一下',858,2,getdate(),getdate())
insert into Goods values(1,14,18,1,'测试数据二十，便宜送','全新的苹果电脑，随便出，给点钱意思一下',980.5,2,getdate(),getdate())
go
if exists(select * from sysobjects where name='Goodsimage')
drop table Goodsimage;
go
--商品图片表
create table Goodsimage(
  GoodsimageId int primary key identity(1,1),
  images nvarchar(100) not null ,--图片
  GoodsId int foreign key references Goods(GoodsId),--商品表
)
go
--select * from Goodsimage
insert into Goodsimage values('联想电脑图片1.jpg',1),
('联想电脑图片2.jpg',1),
('联想电脑图片3.jpg',5),
('联想电脑图片4.jpg',15),
('苹果电脑图片1.jpg',2),
('苹果电脑图片2.jpg',3),
('苹果电脑图片3.jpg',4)
insert into Goodsimage values('联想电脑图片1.jpg',2),
('联想电脑图片2.jpg',2),
('联想电脑图片3.jpg',3),
('联想电脑图片4.jpg',4),
('苹果电脑图片1.jpg',6),
('苹果电脑图片2.jpg',7),
('苹果电脑图片3.jpg',8)
insert into Goodsimage values('联想电脑图片1.jpg',9),
('联想电脑图片2.jpg',10),
('联想电脑图片3.jpg',11),
('联想电脑图片4.jpg',12),
('苹果电脑图片1.jpg',15),
('苹果电脑图片2.jpg',13),
('苹果电脑图片3.jpg',14)
insert into Goodsimage values('联想电脑图片1.jpg',11),
('联想电脑图片2.jpg',20),
('联想电脑图片3.jpg',19),
('联想电脑图片4.jpg',14),
('苹果电脑图片1.jpg',16),
('苹果电脑图片2.jpg',17),
('苹果电脑图片3.jpg',18)

go
if exists(select * from sysobjects where name='Orders')
drop table Orders;
go
--订单表
create table Orders(
  OrdersId int primary key identity(1,1),
  UserId1 int foreign key references UserInfo(UserId),--用户表-发布
  UserId2 int foreign key references UserInfo(UserId),--用户表-购买
  GoodsId int foreign key references Goods(GoodsId),--商品表
  IsState int ,--订单状态（1-买家已付款，2-卖家已发货，3-买家已收货，4-已完成，未评价，5-已完成，已评价）
  Wuliu nvarchar(50),--物流单号
  BuyTime1 datetime,--购买时间
  BuyTime2 datetime,--发货时间
  BuyTime3 datetime,--收货时间
  BuyTime4 datetime,--完成时间
  BuyTime5 datetime,--评价时间
)
go
--select * from Orders
insert into Orders values(1,2,4,1,'s120254845154',getdate(),getdate(),getdate(),getdate(),getdate())



go
if exists(select * from sysobjects where name='Love')
drop table Love;
go
--收藏表
create table Love(
  LoveId int primary key identity(1,1),
  UserId int foreign key references UserInfo(UserId),--用户表
  GoodsId int foreign key references Goods(GoodsId),--商品表
  addTiem datetime,--收藏时间
)
go
--select * from Love
insert into Love values(2,1,getdate()),(2,2,getdate()),(2,3,getdate()),(2,5,getdate())



go
if exists(select * from sysobjects where name='Friends')
drop table Friends;
go
--好友表--同时生成1个表，查询时两个都查一下
create table Friends(
  FriendsId int primary key identity(1,1),
  UserId1 int foreign key references UserInfo(UserId),--用户表(主)
  UserId2 int foreign key references UserInfo(UserId),--用户表(次)
  addTiem datetime,--时间
)
go
--select * from Friends
insert into Friends values(1,2,getdate())



go
if exists(select * from sysobjects where name='Texts')
drop table Texts;
go
--文字表，用户每发布一次生成一行数据
create table Texts(
  TextsId int primary key identity(1,1),
  UserId int foreign key references UserInfo(UserId),--用户表(主)
  Textbody nvarchar(100),--文本
  addTiem datetime,--时间
)
go
--select * from Texts
insert into Texts values(1,'你好啊！我是管理员一号',getdate())
insert into Texts values(1,'你好啊！欢迎来到朝阳网',getdate())
insert into Texts values(1,'你好啊！等待我们更新更多功能吧！',getdate())
insert into Texts values(2,'这个宝贝似全新的吗？',getdate())
insert into Texts values(2,'包邮吗？',getdate())
insert into Texts values(2,'请问多久能到',getdate())
insert into Texts values(2,'可以自提吗？',getdate())
insert into Texts values(2,'能在便宜一点吗？',getdate())

go
if exists(select * from sysobjects where name='HuDong_texts')
drop table HuDong_texts;
go
--互动文字外键表
create table HuDong_texts(
  HuDong_textsId int primary key identity(1,1),
  TextsId int foreign key references Texts(TextsId),--文字(主)
  GoodsId int foreign key references Goods(GoodsId),--商品表
  States int,--状态，0-未读，1-已读（商品表主人）
)
go
--select * from HuDong_texts
insert into HuDong_texts values(4,1,0),(5,1,0),(6,2,0),(7,3,0),(8,5,0)

go
if exists(select * from sysobjects where name='Pingjia_texts')
drop table Pingjia_texts;
go
--评价文字外键表
create table Pingjia_texts(
  Pingjia_textsId int primary key identity(1,1),
  TextsId int foreign key references Texts(TextsId),--文字(主)
  GoodsId int foreign key references Goods(GoodsId),--商品表
  States int,--状态，0-未读，1-已读（商品表主人）
)
go
--select * from Pingjia_texts
insert into Pingjia_texts values(1,4,0)

go
if exists(select * from sysobjects where name='Friends_texts')
drop table Friends_texts;
go
--站内私聊文字外键表
create table Friends_texts(
  Friends_textsId int primary key identity(1,1),
  TextsId int foreign key references Texts(TextsId),--文字(主)
  UserId int foreign key references UserInfo(UserId),--用户表(接受信息者)
  States int,--状态，0-未读，1-已读（接受信息者）
)
go
--select * from Friends_texts
insert into Friends_texts values(2,2,0),(3,2,0),(4,2,0)


go
if exists(select * from sysobjects where name='JuBao')
drop table JuBao;
go
--举报用户
create table JuBao(
  JuBaoId int primary key identity(1,1),
  UserId int foreign key references UserInfo(UserId),--用户表(进行举报者)
  UserId2 int foreign key references UserInfo(UserId),--用户表(被举报者)
  JuBao_body nvarchar(500),--举报内容
  addTiem datetime,--时间
  States int,--状态，0-已提交，1-已处理
  HuiFu nvarchar(100),--已处理后的答复
  CLUserId int foreign key references UserInfo(UserId),--处理管理员(客服？)
)
go
--select * from JuBao
insert into JuBao values(1,2,'卖假货，这个骗子，气死我了',getdate(),0,null,null)




