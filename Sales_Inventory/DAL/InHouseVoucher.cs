
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
    
public partial class InHouseVoucher
{

    public int Id { get; set; }

    public string TransactionNo { get; set; }

    public string PaymentFrom { get; set; }

    public Nullable<System.DateTime> PaymentDate { get; set; }

    public Nullable<decimal> PaymentAmt { get; set; }

    public string TransactionType { get; set; }

    public string PaymentGivenTo { get; set; }

    public Nullable<int> CreatedBy { get; set; }

    public Nullable<System.DateTime> CreatedDate { get; set; }

}

}
