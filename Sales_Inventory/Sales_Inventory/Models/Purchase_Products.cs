using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sales_Inventory.Models
{
    public class Purchase_Products
    {
        public int Id { get; set; }
        public string Purchase_No { get; set; }
        public string ItemName { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> Price { get; set; }
        public string Total { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}