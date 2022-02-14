using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sales_Inventory.Models
{
    public class StockViewModel
    {
        public int Id { get; set; }
        public string Product { get; set; }
        public Nullable<int> TotalQuantity { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}