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
    
    public partial class WebIn
    {
        public int WebInId { get; set; }
        public Nullable<int> GoodsId { get; set; }
        public Nullable<System.DateTime> addtime { get; set; }
        public string type_1 { get; set; }
        public Nullable<int> Tpromote { get; set; }
        public Nullable<int> Theat { get; set; }
    
        public virtual Goods Goods { get; set; }
    }
}
