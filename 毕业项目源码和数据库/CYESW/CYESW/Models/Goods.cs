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
    
    public partial class Goods
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Goods()
        {
            this.Goodsimage = new HashSet<Goodsimage>();
            this.HuDong_texts = new HashSet<HuDong_texts>();
            this.Love = new HashSet<Love>();
            this.Orders = new HashSet<Orders>();
            this.WebIn = new HashSet<WebIn>();
        }
    
        public int GoodsId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> GoodsTypeId { get; set; }
        public Nullable<int> GoodsaddressId { get; set; }
        public Nullable<int> IsNew { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<double> Price_Yuan { get; set; }
        public Nullable<int> munber { get; set; }
        public Nullable<int> IsState { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
        public Nullable<int> Peisong { get; set; }
        public string GoodsaddressInfo { get; set; }
        public string Ipone { get; set; }
    
        public virtual Goodsaddress Goodsaddress { get; set; }
        public virtual GoodsType GoodsType { get; set; }
        public virtual UserInfo UserInfo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Goodsimage> Goodsimage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HuDong_texts> HuDong_texts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Love> Love { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Orders> Orders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WebIn> WebIn { get; set; }
    }
}
