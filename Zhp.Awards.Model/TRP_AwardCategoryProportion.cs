//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Zhp.Awards.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class TRP_AwardCategoryProportion
    {
        public int Id { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string Description { get; set; }
        public Nullable<bool> DeleteMark { get; set; }
        public Nullable<bool> Enable { get; set; }
        public Nullable<int> ActivityId { get; set; }
        public Nullable<int> Proportion { get; set; }
        public Nullable<int> AwardCategoryId { get; set; }
    }
}
