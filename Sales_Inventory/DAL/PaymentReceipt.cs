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
    
    public partial class PaymentReceipt
    {
        public int Id { get; set; }
        public string ReceiptNo { get; set; }
        public Nullable<System.DateTime> ReceivedDate { get; set; }
        public string ReceivedFrom { get; set; }
        public Nullable<int> TotalAmount { get; set; }
        public Nullable<int> PaidAmount { get; set; }
        public Nullable<int> Balance { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentAgainst { get; set; }
        public string BankName { get; set; }
        public string AccountNo { get; set; }
        public string AccountHolderName { get; set; }
        public string UPI_Id { get; set; }
        public string ChequeNo { get; set; }
        public Nullable<System.DateTime> ChequeDate { get; set; }
        public string NameOnCheque { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
