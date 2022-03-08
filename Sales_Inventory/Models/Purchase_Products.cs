using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sales_Inventory.Models
{
    public class Purchase_Products
    {
        public int Id { get; set; }
        public string Purchase_No { get; set; }
        public string ItemName { get; set; }
        public decimal Quantity { get; set; }
        public decimal Damaged { get; set; }
        public decimal FinalQty { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public List<SelectListItem> ProductName { get; set; }
        public List<Purchase_Products> List { get; set; }
    }
}