
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
    
    public partial class InHouse
    {
        public int Id { get; set; }
        public string PaidBy { get; set; }
        public string PaidTo { get; set; }
        public Nullable<System.DateTime> PaidDate { get; set; }
        public Nullable<int> TotalAmount { get; set; }
        public Nullable<int> PaidAmount { get; set; }
        public Nullable<int> ReturnAmtReceived { get; set; }
        public Nullable<int> BalanceAmt { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
