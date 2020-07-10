//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class UserInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserInfo()
        {
            this.Addres = new HashSet<Addres>();
            this.Friends = new HashSet<Friends>();
            this.Friends1 = new HashSet<Friends>();
            this.Friends_texts = new HashSet<Friends_texts>();
            this.Goods = new HashSet<Goods>();
            this.JuBao = new HashSet<JuBao>();
            this.JuBao1 = new HashSet<JuBao>();
            this.JuBao2 = new HashSet<JuBao>();
            this.Love = new HashSet<Love>();
            this.Orders = new HashSet<Orders>();
            this.Orders1 = new HashSet<Orders>();
            this.RealName = new HashSet<RealName>();
            this.Texts = new HashSet<Texts>();
            this.WeiGui = new HashSet<WeiGui>();
            this.WeiGui1 = new HashSet<WeiGui>();
        }
    
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmile { get; set; }
        public string UserPwd { get; set; }
        public Nullable<int> Age { get; set; }
        public Nullable<decimal> moneys { get; set; }
        public string Info { get; set; }
        public string Images { get; set; }
        public Nullable<int> IsDelete { get; set; }
        public Nullable<System.DateTime> AddTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public Nullable<int> IsManage { get; set; }
        public string Sex { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Addres> Addres { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Friends> Friends { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Friends> Friends1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Friends_texts> Friends_texts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Goods> Goods { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JuBao> JuBao { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JuBao> JuBao1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JuBao> JuBao2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Love> Love { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Orders> Orders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Orders> Orders1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RealName> RealName { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Texts> Texts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WeiGui> WeiGui { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WeiGui> WeiGui1 { get; set; }
    }
}
