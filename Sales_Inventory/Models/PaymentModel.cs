using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sales_Inventory.Models
{
    public class PaymentModel
    {
        public int Id { get; set; }
        public string Purchase_No { get; set; }
        public string Payment_To { get; set; }
        public Nullable<System.DateTime> Payment_Date { get; set; }
        public string Payment_Type { get; set; }
        public decimal Total_Payment_Amount { get; set; }
        public string Contact_No { get; set; }
        public decimal Paid_Amount { get; set; }
        public string Bank_Name { get; set; }
        public string Account_No { get; set; }
        public string Account_Holder_Name { get; set; }
        public string UPI_Id { get; set; }
        public string Cheque_No { get; set; }
        public Nullable<System.DateTime> Cheque_Date { get; set; }
        public string Name_On_Cheque { get; set; }
        public decimal Balance { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public List<SelectListItem> PaymentName { get; set; }
        public List<PaymentModel> List { get; set; }
    }
}