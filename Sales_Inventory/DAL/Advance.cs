
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
    public partial class Advance
    {
        public int Id { get; set; }
        public string Advance_To { get; set; }
        public Nullable<System.DateTime> Advance_Date { get; set; }
        public string Advance_Amount { get; set; }
        public string Advance_Against { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
