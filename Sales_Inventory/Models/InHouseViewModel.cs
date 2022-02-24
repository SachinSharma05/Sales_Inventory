using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sales_Inventory.Models
{
    public class InHouseViewModel
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