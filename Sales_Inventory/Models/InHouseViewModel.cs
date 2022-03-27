using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sales_Inventory.Models
{
    public class InHouseModel
    {
        public int Id { get; set; }
        public string TransactionNo { get; set; }
        public string PaidFor { get; set; }
        public string PaidBy { get; set; }
        public Nullable<System.DateTime> PaidDate { get; set; }
        public Nullable<decimal> PaidAmt { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public Nullable<decimal> CarryForward { get; set; }
        public string TransactionType { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public List<InHouseModel> List { get; set; }
        public InHouseVoucherModel model { get; set; }
        public InHouseModel ViewModel { get; set; }
        public List<InHouseVoucherModel> VoucherModel { get; set; }
        public List<SelectListItem> PaidByList { get; set; }
    }

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