
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
    
public partial class Payment
{

    public int Id { get; set; }

    public string Purchase_No { get; set; }

    public string Payment_To { get; set; }

    public Nullable<System.DateTime> Payment_Date { get; set; }

    public string Payment_Type { get; set; }

    public Nullable<decimal> Total_Payment_Amount { get; set; }

    public string Contact_No { get; set; }

    public Nullable<decimal> Paid_Amount { get; set; }

    public string Bank_Name { get; set; }

    public string Account_No { get; set; }

    public string Account_Holder_Name { get; set; }

    public string UPI_Id { get; set; }

    public string Cheque_No { get; set; }

    public Nullable<System.DateTime> Cheque_Date { get; set; }

    public string Name_On_Cheque { get; set; }

    public Nullable<decimal> Balance { get; set; }

    public Nullable<int> CreatedBy { get; set; }

    public Nullable<System.DateTime> CreatedDate { get; set; }

}

}
