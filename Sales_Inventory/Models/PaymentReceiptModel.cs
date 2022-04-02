using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sales_Inventory.Models
{
    public class PaymentReceiptModel
    {
        public int Id { get; set; }
        public string ReceiptNo { get; set; }
        public Nullable<System.DateTime> ReceivedDate { get; set; }
        public string ReceivedFrom { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal Balance { get; set; }
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
        public List<SelectListItem> PaymentReceiptName { get; set; }
        public List<PaymentReceiptModel> List { get; set; }
    }
}