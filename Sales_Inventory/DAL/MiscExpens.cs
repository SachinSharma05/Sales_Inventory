//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sales_Inventory.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class MiscExpens
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> ExpenseAmt { get; set; }
        public Nullable<System.DateTime> ExpenseDate { get; set; }
        public string ExpenseReason { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}