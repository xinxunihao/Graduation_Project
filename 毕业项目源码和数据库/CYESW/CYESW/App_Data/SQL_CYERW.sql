
--create database CYESW
--drop database CYESW
--go
--��Ʒ����������¾ɳ̶ȣ�int���Ͱɣ���Ϊ1ȫ��,2-95�£�3-9���£�����--ԭ��--��ϸ��ַ���������ʱ�䣬����
--���վ���ƹ��վ���ƹ��

use CYESW
go
if exists(select * from sysobjects where name='UserInfo')
drop table UserInfo;
go
--�û���
create table UserInfo(
  UserId int primary key identity(1,1),
  UserName nvarchar(10) not null,--�û�����
  UserEmile nvarchar(100) not null ,--�û�����
  UserPwd nvarchar(100) not null ,--�û�����
  Age int , --����
  moneys money,--�û����
  Info nvarchar(100),--���˼��
  Images nvarchar(100),--ͷ��
  IsDelete int ,--�Ƿ�ɾ��
  AddTime datetime,--ע��ʱ��
  EndTime datetime,--���ʱ��
  IsManage int default(0),--�Ƿ�Ϊ����Ա��1Ϊ��
)
go
--select * from userinfo
insert into UserInfo values('admin','123456@qq.com','123456',20,200.2,'����˧�磬���䣬����˵��','man.jpg',0,GETDATE(),GETDATE(),0)
insert into UserInfo values('admin2','1234567@qq.com','123456',20,200.2,'������Ů��hi��һ��play��','women.jpg',0,GETDATE(),GETDATE(),1)

go
if exists(select * from sysobjects where name='Addres')
drop table Addres;
go
--��ַ��
create table Addres(
  AddresId int primary key identity(1,1),
  Name nvarchar(20) not null ,--�ջ�������
  UserId int foreign key references UserInfo(UserId),--�û���
  Addresss1 nvarchar(100) not null ,--���µ�ַ��ʡ���У��أ�
  Addresss2 nvarchar(100) not null ,--��ϸ��ַ
  Phone nvarchar(20),--�ֻ�����
  IsDelete int ,--�Ƿ�ɾ��
)
go
--select * from Addres
insert into Addres values('������',1,'����ʡ����ɳ�У���ɳ��','�����ֵ�100��','15855698562',0)


go
if exists(select * from sysobjects where name='RealName')
drop table RealName;
go
--ʵ����֤��
create table RealName(
  RealNameId int primary key identity(1,1),
  UserId int foreign key references UserInfo(UserId),--�û���
  IdCard nvarchar(20) not null ,--���֤
  AddTime datetime,--��֤ʱ��
  IsDelete int ,--�Ƿ�ɾ��
)
go
--select * from RealName
insert into RealName values(1,'430223200005052145',GETDATE(),0)


go
if exists(select * from sysobjects where name='GoodsType')
drop table GoodsType;
go
--��Ʒ���ͣ��ֻ������ԣ�ͼ�飬��װ��
create table GoodsType(
  GoodsTypeId int primary key identity(1,1),
  TypeName nvarchar(20) not null ,--��������
  GoodsTypeBId int foreign key references GoodsType(GoodsTypeId),--����Լ����������������ͣ���������
)
go
--select * from GoodsType
insert into GoodsType values('����',null),('�ֻ�ƽ��',null),('�칫��Ʒ',null),('�����Ʒ',null),('������Ʒ',null),('ͼ��',null),('�������',null),('����',null)
go
insert into GoodsType values('�ʼǱ�',1),('�������',1),('�����Ʒ',1),('̨ʽ��',1),('��������',1)
go
insert into GoodsType values('ƻ���ֻ�',2),('С���ֻ�',2),('��Ϊ�ֻ�',2),('OPPO�ֻ�',2),('vivo�ֻ�',2),('�����ֻ�',2),('�����ֻ�',2),('ŵ�����ֻ�',2),('�����ֻ�',2),('�����ֻ�',2),('�����ֻ�',2),
('ƻ��ƽ��',2),('��Ϊƽ��',2),('С��ƽ��',2),('΢��ƽ��',2),('����ƽ��',2),('�ȸ�ƽ��',2),('����ƽ��',2),('����ƽ��',2),('�����ֻ�ƽ��',2)
insert into GoodsType values('��ӡ��',3),('ͶӰ��',3),('�����豸',3),('�㳮��',3),('ɨ����',3),('������ɼ���',3),('��ֽ��',3),('�Ž��豸',3),('������',3),('д�ְ�',3),('�����칫��Ʒ',3)
insert into GoodsType values('����',4),('����',4),('��Ϸ��',4),('���',4),('�����豸',4),('������',4),('���������Ʒ',4)
insert into GoodsType values('��װ',5),('Ůװ',5),('Ь',5),('����',5),('����',5),('�ҵ�',5),('����������Ʒ',5)
insert into GoodsType values('��ѧС˵',6),('������־',6),('��ͯ����',6),('�������',6),('�Ƽ�����',6),('��������',6),('�̲ĸ���',6),('����ԭ��',6),('��ƬӰƬ',6),('����ͼ��',6)
insert into GoodsType values('ƻ���ʼǱ�',9),('����ʼǱ�',9),('�����ʼǱ�',9),('���ձʼǱ�',9),('ThinkPad�ʼǱ�',9),('���ǱʼǱ�',9),('��֥�ʼǱ�',9),('��Ϊ�ʼǱ�',9),('С�ױʼǱ�',9),('��˶�ʼǱ�',9),('΢��ʼǱ�',9),('���۱ʼǱ�',9),('����ʼǱ�',9),('����ʼǱ�',9),('�����ʼǱ�',9),('�����ʼǱ�',9)
insert into GoodsType values('�ڴ�',10),('Ӳ��',10),('����',10),('��Դ',10),('����',10),('��¼��',10),('·����',10),('װ�����',10),('��ʾ��',10),('CPU������',10),('�����������',10)
insert into GoodsType values('���',11),('����',11),('U��',11),('��д��',11),('Ӳ�̺�',11),('����ͷ',11),('���������Ʒ',11)
insert into GoodsType values('ƻ��̨ʽ��',12),('����̨ʽ��',12),('����̨ʽ��',12),('����̨ʽ��',12),('ThinkPad̨ʽ��',12),('����̨ʽ��',12),('��̨֥ʽ��',12),('��Ϊ̨ʽ��',12),('С��̨ʽ��',12),('��˶̨ʽ��',12),('΢��̨ʽ��',12),('����̨ʽ��',12),('����̨ʽ��',12),('����̨ʽ��',12),('����̨ʽ��',12),('����̨ʽ��',12)

go
if exists(select * from sysobjects where name='Goodsaddress')
drop table Goodsaddress;
go
--��Ʒ�������У��أ�(�ֶ���)
create table Goodsaddress(
  GoodsaddressId int primary key identity(1,1),
  TypeName nvarchar(20) not null ,--��������
  GoodsaddressBId int foreign key references Goodsaddress(GoodsaddressId),--����Լ����������������ͣ���������
)
go
--select * from Goodsaddress
insert into Goodsaddress values('��������',null),('���ϵ���',null),('���е���',null),('��������',null),('��������',null),('���ϵ���',null),('��������',null)
go
insert into Goodsaddress values('����',1),('�㽭',1),('����',1),('ɽ��',1),('����',1),('����',1)
insert into Goodsaddress values('�㶫',2),('����',2),('����',2)
insert into Goodsaddress values('����',3),('����',3),('����',3)
insert into Goodsaddress values('���ɹ�',4),('�ӱ�',4),('ɽ��',4)
insert into Goodsaddress values('����',5),('����',5),('������',5)
insert into Goodsaddress values('�Ĵ�',6),('����',6),('����',6),('����',6)
insert into Goodsaddress values('����',7),('�½�',7),('�ຣ',7),('����',7),('����',7)
go
insert into Goodsaddress values('�Ͼ�',8),('����',8),('����',8),('����',8),('��ɽ',8),('���Ƹ�',8),('��ͨ',8),('����',8),('̫��',8),('̩��',8),('����',8),('��Ǩ',8),('����',8),('�γ�',8),('����',8),('�żҸ�',8),('��',8)
insert into Goodsaddress values('����',9),('����',9),('����',9),('��',9),('��ˮ',9),('����',9),('����',9),('����',9),('̨��',9),('����',9),('��ɽ',9)
insert into Goodsaddress values('����',10),('����',10),('��ƽ',10),('����',10),('����',10),('Ȫ��',10),('����',10),('����',10),('����',10)
insert into Goodsaddress values('����',11),('����',11),('����',11),('��Ӫ',11),('����',11),('����',11),('����',11),('�ĳ�',11),('��ϫ',11),('�ൺ',11),('����',11),('̩��',11),('����',11),('Ϋ��',11),('��̨',11),('��ׯ',11),('�Ͳ�',11)
insert into Goodsaddress values('�ϲ�',12),('����',12),('����',12),('����',12),('������',12),('�Ž�',12),('Ƽ��',12),('����',12),('����',12),('�˴�',12),('ӥ̶',12)
insert into Goodsaddress values('�Ϸ�',13),('����',13),('����',13),('����',13),('����',13),('����',13),('����',13),('����',13),('����',13),('����',13),('��ɽ',13),('����',13),('��ɽ',13),('ͭ��',13),('�ߺ�',13),('����',13),('����',13)

insert into Goodsaddress values('����',14),('����',14),('��ݸ',14),('��ɽ',14),('��Դ',14),('����',14),('����',14),('����',14),('ï��',14),('÷��',14),('��Զ',14),('��ͷ',14),('��β',14),('�ع�',14),('����',14),('����',14),('�Ƹ�',14),('տ��',14),('����',14),('��ɽ',14),('�麣',14)
insert into Goodsaddress values('����',15),('��ɳ',15),('��ͤ',15),('����',15),('����',15),('����',15),('����',15),('����',15),('�ֶ�',15),('�ٸ�',15),('��ˮ',15),('��',15),('����',15),('��ɳ',15),('����',15),('�Ͳ�',15),('����',15),('�Ĳ�',15),('��ָɽ',15)
insert into Goodsaddress values('����',16),('��ɫ',16),('����',16),('����',16),('���Ǹ�',16),('���',16),('����',16),('�ӳ�',16),('����',16),('����',16),('����',16),('����',16),('����',16),('����',16)

insert into Goodsaddress values('�人',17),('����',17),('��ʩ',17),('�Ƹ�',17),('����',17),('Ǳ��',17),('��ũ��',17),('ʮ��',17),('����',17),('����',17),('����',17),('����',17),('����',17),('Т��',17),('�˲�',17)
insert into Goodsaddress values('��ɳ',18),('����',18),('����',18),('����',18),('����',18),('¦��',18),('����',18),('��̶',18),('����',18),('����',18),('����',18),('����',18),('�żҽ�',18),('����',18)
insert into Goodsaddress values('֣��',19),('����',19),('�ױ�',19),('��Դ',19),('����',19),('����',19),('����',19),('���',19),('����',19),('ƽ��ɽ',19),('���',19),('����Ͽ',19),('����',19),('����',19),('����',19),('���',19),('�ܿ�',19),('פ���',19)

insert into Goodsaddress values('���ͺ���',20),('������',20),('����׿��',20),('��ͷ',20),('���',20),('������˹',20),('���ױ���',20),('ͨ��',20),('�ں�',20),('�����첼',20),('���ֹ���',20),('�˰�',20)
insert into Goodsaddress values('ʯ��ׯ',21),('����',21),('����',21),('�е�',21),('����',21),('��ˮ',21),('�ȷ�',21),('�ػʵ�',21),('��ɽ',21),('��̨',21),('�żҿ�',21)
insert into Goodsaddress values('̫ԭ',22),('��ͬ',22),('����',22),('�ٷ�',22),('����',22),('˷��',22),('����',22),('�˳�',22),('����',22)

insert into Goodsaddress values('����',23),('��ɽ',23),('��Ϫ',23),('����',23),('����',23),('����',23),('��˳',23),('����',23),('��«��',23),('����',23),('����',23),('�̽�',23),('����',23),('Ӫ��',23)
insert into Goodsaddress values('����',24),('�׳�',24),('��ɽ',24),('����',24),('��Դ',24),('��ƽ',24),('��ԭ',24),('ͨ��',24),('�ӱ�',24)
insert into Goodsaddress values('������',25),('����',25),('���˰���',25),('�׸�',25),('�ں�',25),('����',25),('��ľ˹',25),('ĵ����',25),('��̨��',25),('�������',25),('˫Ѽɽ',25),('�绯',25),('����',25)


insert into Goodsaddress values('�ɶ�',26),('����',26),('����',26),('����',26),('����',26),('����',26),('�㰲',26),('��Ԫ',26),('��ɽ',26),('��ɽ',26),('����',26),('ü��',26),('����',26),('����',26),('�ϳ�',26),('�ڽ�',26),('��֦��',26),('����',26),('�Ű�',26),('�˱�',26),('����',26),('�Թ�',26)
insert into Goodsaddress values('����',27),('����',27),('����',27),('��֥',27),('����',27),('�տ���',27),('ɽ��',27)
insert into Goodsaddress values('����',28),('��ɽ',28),('����',28),('����',28),('�º�',28),('����',28),('���',28),('����',28),('�ٲ�',28),('ŭ��',28),('�ն�',28),('����',28),('��ɽ',28),('��˫����',28),('��Ϫ',28),('��ͨ',28)
insert into Goodsaddress values('����',29),('��˳',29),('�Ͻ�',29),('����ˮ',29),('ǭ����',29),('ͭ��',29),('����',29)

insert into Goodsaddress values('����',30),('����',30),('����',30),('����',30),('����',30),('ͭ��',30),('μ��',30),('����',30),('�Ӱ�',30),('����',30)
insert into Goodsaddress values('��³ľ��',31),('������',31),('������',31),('����̩',31),('��������',31),('��������',31),('����',31),('����',31),('����',31),('��ʲ',31),('��������',31),('��������',31),('ʯ����',31),('����',31),('ͼľ���',31),('��³��',31),('�����',31),('����',31)
insert into Goodsaddress values('����',32),('����',32),('����',32),('����',32),('����',32),('����',32),('����',32),('����',32)
insert into Goodsaddress values('����',33),('��ԭ',33),('ʯ��ɽ',33),('����',33),('����',33)
insert into Goodsaddress values('����',34),('����',34),('����',34),('����',34),('������',34),('���',34),('��Ȫ',34),('����',34),('¤��',34),('ƽ��',34),('����',34),('��ˮ',34),('����',34),('��Ҵ',34)


go
if exists(select * from sysobjects where name='Goods')
drop table Goods;
go
--��Ʒ
create table Goods(
  GoodsId int primary key identity(1,1),
  UserId int foreign key references UserInfo(UserId),--�û���
  GoodsTypeId int foreign key references GoodsType(GoodsTypeId),--���ͱ�
  GoodsaddressId int foreign key references Goodsaddress(GoodsaddressId),--������--����

  IsNew int,--�Ƿ�ȫ��
  Name nvarchar(100) not null ,--��Ʒ����,����
  Info nvarchar(200) not null ,--��Ʒ��Ϣ
  Price float,--�۸�
  IsState int,--��Ʒ״̬��1�ϼܣ�2�¼ܣ�3������4ɾ��
  CreateTime datetime,--����ʱ��
  UpdateTime datetime ,--����ʱ�䣨����������
)
go
--select * from Goods

insert into Goods values(1,14,18,1,'��������һ','ȫ�µ�������ԣ����˳���12312312',8880.5,1,getdate(),getdate())
insert into Goods values(1,13,18,1,'�������ݶ�','ȫ�µ�ƻ�����ԣ��������Ǵ򷢷�',9980.5,1,getdate(),getdate())
insert into Goods values(1,13,18,1,'��������������˦��','��ɳ��ɳ������û�н���',1280.25,0,getdate(),getdate())
insert into Goods values(1,13,18,1,'���������ģ�������','������ˮ����ˮ����������Ǯ��˼һ��',0,1,getdate(),getdate())
insert into Goods values(1,14,18,1,'���������壬������','ȫ�µ�ƻ�����ԣ�����������Ǯ��˼һ��',9280.5,2,getdate(),getdate())

insert into Goods values(1,14,18,1,'����������','ȫ�µ�������ԣ����˳���',180.5,2,getdate(),getdate())
insert into Goods values(1,13,18,1,'����������','ȫ�µ�ƻ�����ԣ�����������Ǯ��˼һ��',2660.5,2,getdate(),getdate())
insert into Goods values(1,13,18,1,'�������ݰˣ���˦��','û�н���',2666.25,0,getdate(),getdate())
insert into Goods values(1,13,18,1,'�������ݾţ�������','5555������������Ǯ��˼һ��',188,2,getdate(),getdate())
insert into Goods values(1,14,18,1,'��������ʮ��������','��Ū�ˣ�����������Ǯ��˼һ��',18758.5,2,getdate(),getdate())

insert into Goods values(1,14,18,1,'��������ʮһ','ȫ��ǮǮ222�����˳���',1280.5,2,getdate(),getdate())
insert into Goods values(1,13,18,1,'��������ʮ��','ȫ�µ�ƻ�����ԣ�����������Ǯ��˼һ��',22.5,2,getdate(),getdate())
insert into Goods values(1,13,18,1,'��������ʮ������˦��','û�н���',280.25,0,getdate(),getdate())
insert into Goods values(1,13,18,1,'��������ʮ�ģ�������','ȫ�µ�ƻ�����ԣ�����������Ǯ��˼һ��',888,2,getdate(),getdate())
insert into Goods values(1,14,18,1,'��������ʮ�壬������������������','ȫ�µ�ƻ�����ԣ�����������Ǯ��˼һ��',10.5,2,getdate(),getdate())

insert into Goods values(1,14,18,1,'��������ʮ��','ȫ�µ�������ԣ����˳���',1280.5,2,getdate(),getdate())
insert into Goods values(1,13,18,1,'��������ʮ��','ȫ�µ�ƻ�����ԣ�����������Ǯ��˼һ��',280.5,2,getdate(),getdate())
insert into Goods values(1,13,18,1,'��������ʮ�ˣ���˦��','û�н���',280.25,0,getdate(),getdate())
insert into Goods values(1,13,18,1,'��������ʮ�ţ�������','ȫ�µ�ƻ�����ԣ�����������Ǯ��˼һ��',858,2,getdate(),getdate())
insert into Goods values(1,14,18,1,'�������ݶ�ʮ��������','ȫ�µ�ƻ�����ԣ�����������Ǯ��˼һ��',980.5,2,getdate(),getdate())
go
if exists(select * from sysobjects where name='Goodsimage')
drop table Goodsimage;
go
--��ƷͼƬ��
create table Goodsimage(
  GoodsimageId int primary key identity(1,1),
  images nvarchar(100) not null ,--ͼƬ
  GoodsId int foreign key references Goods(GoodsId),--��Ʒ��
)
go
--select * from Goodsimage
insert into Goodsimage values('�������ͼƬ1.jpg',1),
('�������ͼƬ2.jpg',1),
('�������ͼƬ3.jpg',5),
('�������ͼƬ4.jpg',15),
('ƻ������ͼƬ1.jpg',2),
('ƻ������ͼƬ2.jpg',3),
('ƻ������ͼƬ3.jpg',4)
insert into Goodsimage values('�������ͼƬ1.jpg',2),
('�������ͼƬ2.jpg',2),
('�������ͼƬ3.jpg',3),
('�������ͼƬ4.jpg',4),
('ƻ������ͼƬ1.jpg',6),
('ƻ������ͼƬ2.jpg',7),
('ƻ������ͼƬ3.jpg',8)
insert into Goodsimage values('�������ͼƬ1.jpg',9),
('�������ͼƬ2.jpg',10),
('�������ͼƬ3.jpg',11),
('�������ͼƬ4.jpg',12),
('ƻ������ͼƬ1.jpg',15),
('ƻ������ͼƬ2.jpg',13),
('ƻ������ͼƬ3.jpg',14)
insert into Goodsimage values('�������ͼƬ1.jpg',11),
('�������ͼƬ2.jpg',20),
('�������ͼƬ3.jpg',19),
('�������ͼƬ4.jpg',14),
('ƻ������ͼƬ1.jpg',16),
('ƻ������ͼƬ2.jpg',17),
('ƻ������ͼƬ3.jpg',18)

go
if exists(select * from sysobjects where name='Orders')
drop table Orders;
go
--������
create table Orders(
  OrdersId int primary key identity(1,1),
  UserId1 int foreign key references UserInfo(UserId),--�û���-����
  UserId2 int foreign key references UserInfo(UserId),--�û���-����
  GoodsId int foreign key references Goods(GoodsId),--��Ʒ��
  IsState int ,--����״̬��1-����Ѹ��2-�����ѷ�����3-������ջ���4-����ɣ�δ���ۣ�5-����ɣ������ۣ�
  Wuliu nvarchar(50),--��������
  BuyTime1 datetime,--����ʱ��
  BuyTime2 datetime,--����ʱ��
  BuyTime3 datetime,--�ջ�ʱ��
  BuyTime4 datetime,--���ʱ��
  BuyTime5 datetime,--����ʱ��
)
go
--select * from Orders
insert into Orders values(1,2,4,1,'s120254845154',getdate(),getdate(),getdate(),getdate(),getdate())



go
if exists(select * from sysobjects where name='Love')
drop table Love;
go
--�ղر�
create table Love(
  LoveId int primary key identity(1,1),
  UserId int foreign key references UserInfo(UserId),--�û���
  GoodsId int foreign key references Goods(GoodsId),--��Ʒ��
  addTiem datetime,--�ղ�ʱ��
)
go
--select * from Love
insert into Love values(2,1,getdate()),(2,2,getdate()),(2,3,getdate()),(2,5,getdate())



go
if exists(select * from sysobjects where name='Friends')
drop table Friends;
go
--���ѱ�--ͬʱ����1������ѯʱ��������һ��
create table Friends(
  FriendsId int primary key identity(1,1),
  UserId1 int foreign key references UserInfo(UserId),--�û���(��)
  UserId2 int foreign key references UserInfo(UserId),--�û���(��)
  addTiem datetime,--ʱ��
)
go
--select * from Friends
insert into Friends values(1,2,getdate())



go
if exists(select * from sysobjects where name='Texts')
drop table Texts;
go
--���ֱ��û�ÿ����һ������һ������
create table Texts(
  TextsId int primary key identity(1,1),
  UserId int foreign key references UserInfo(UserId),--�û���(��)
  Textbody nvarchar(100),--�ı�
  addTiem datetime,--ʱ��
)
go
--select * from Texts
insert into Texts values(1,'��ð������ǹ���Աһ��',getdate())
insert into Texts values(1,'��ð�����ӭ����������',getdate())
insert into Texts values(1,'��ð����ȴ����Ǹ��¸��๦�ܰɣ�',getdate())
insert into Texts values(2,'���������ȫ�µ���',getdate())
insert into Texts values(2,'������',getdate())
insert into Texts values(2,'���ʶ���ܵ�',getdate())
insert into Texts values(2,'����������',getdate())
insert into Texts values(2,'���ڱ���һ����',getdate())

go
if exists(select * from sysobjects where name='HuDong_texts')
drop table HuDong_texts;
go
--�������������
create table HuDong_texts(
  HuDong_textsId int primary key identity(1,1),
  TextsId int foreign key references Texts(TextsId),--����(��)
  GoodsId int foreign key references Goods(GoodsId),--��Ʒ��
  States int,--״̬��0-δ����1-�Ѷ�����Ʒ�����ˣ�
)
go
--select * from HuDong_texts
insert into HuDong_texts values(4,1,0),(5,1,0),(6,2,0),(7,3,0),(8,5,0)

go
if exists(select * from sysobjects where name='Pingjia_texts')
drop table Pingjia_texts;
go
--�������������
create table Pingjia_texts(
  Pingjia_textsId int primary key identity(1,1),
  TextsId int foreign key references Texts(TextsId),--����(��)
  GoodsId int foreign key references Goods(GoodsId),--��Ʒ��
  States int,--״̬��0-δ����1-�Ѷ�����Ʒ�����ˣ�
)
go
--select * from Pingjia_texts
insert into Pingjia_texts values(1,4,0)

go
if exists(select * from sysobjects where name='Friends_texts')
drop table Friends_texts;
go
--վ��˽�����������
create table Friends_texts(
  Friends_textsId int primary key identity(1,1),
  TextsId int foreign key references Texts(TextsId),--����(��)
  UserId int foreign key references UserInfo(UserId),--�û���(������Ϣ��)
  States int,--״̬��0-δ����1-�Ѷ���������Ϣ�ߣ�
)
go
--select * from Friends_texts
insert into Friends_texts values(2,2,0),(3,2,0),(4,2,0)


go
if exists(select * from sysobjects where name='JuBao')
drop table JuBao;
go
--�ٱ��û�
create table JuBao(
  JuBaoId int primary key identity(1,1),
  UserId int foreign key references UserInfo(UserId),--�û���(���оٱ���)
  UserId2 int foreign key references UserInfo(UserId),--�û���(���ٱ���)
  JuBao_body nvarchar(500),--�ٱ�����
  addTiem datetime,--ʱ��
  States int,--״̬��0-���ύ��1-�Ѵ���
  HuiFu nvarchar(100),--�Ѵ����Ĵ�
  CLUserId int foreign key references UserInfo(UserId),--�������Ա(�ͷ���)
)
go
--select * from JuBao
insert into JuBao values(1,2,'���ٻ������ƭ�ӣ���������',getdate(),0,null,null)




