using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sales_Inventory.Models
{
    public class Sale_Products
    {
        public int Id { get; set; }
        public string Sale_No { get; set; }
        public string Item { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string Price { get; set; }
        public string Total { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}