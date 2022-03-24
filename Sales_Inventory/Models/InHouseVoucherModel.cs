using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sales_Inventory.Models
{
    public class InHouseVoucherModel
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