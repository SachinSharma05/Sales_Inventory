using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sales_Inventory.Models
{
    public class AdvanceViewModel
    {
        public int Id { get; set; }
        public string Advance_To { get; set; }
        public Nullable<System.DateTime> Advance_Date { get; set; }
        public string Advance_Amount { get; set; }
        public string Advance_Against { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}