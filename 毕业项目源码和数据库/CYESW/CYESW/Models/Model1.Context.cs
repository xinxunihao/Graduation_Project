﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace CYESW.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CYESWEntities : DbContext
    {
        public CYESWEntities()
            : base("name=CYESWEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Addres> Addres { get; set; }
        public virtual DbSet<Friends> Friends { get; set; }
        public virtual DbSet<Friends_texts> Friends_texts { get; set; }
        public virtual DbSet<Goods> Goods { get; set; }
        public virtual DbSet<Goodsaddress> Goodsaddress { get; set; }
        public virtual DbSet<Goodsimage> Goodsimage { get; set; }
        public virtual DbSet<GoodsType> GoodsType { get; set; }
        public virtual DbSet<HuDong_texts> HuDong_texts { get; set; }
        public virtual DbSet<JuBao> JuBao { get; set; }
        public virtual DbSet<Love> Love { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Pingjia_texts> Pingjia_texts { get; set; }
        public virtual DbSet<RealName> RealName { get; set; }
        public virtual DbSet<Texts> Texts { get; set; }
        public virtual DbSet<UserInfo> UserInfo { get; set; }
    }
}
