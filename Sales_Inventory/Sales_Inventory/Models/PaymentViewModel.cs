using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sales_Inventory.Models
{
    public class PaymentViewModel
    {
        public int Id { get; set; }
        public string Payment_To { get; set; }
        public string Payment_By { get; set; }
        public System.DateTime Payment_Date { get; set; }
        public string Payment_Against { get; set; }
        public string Payment_Type { get; set; }
        public string Payment_Account_No { get; set; }
        public string Payment_Account_Name { get; set; }
        public int Payment_Amount { get; set; }
        public string Payment_Bank { get; set; }
        public string IFSC_Code { get; set; }
        public string Payment_Receiver_Phone { get; set; }
        public string Payment_Sender_Phone { get; set; }
        public Nullable<int> Balance { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}