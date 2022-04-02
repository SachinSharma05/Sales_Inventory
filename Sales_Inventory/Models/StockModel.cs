using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sales_Inventory.Models
{
    public class StockModel
    {
        public int Id { get; set; }
        public string Product { get; set; }
        public Nullable<int> TotalQuantity { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public List<SelectListItem> ProductName { get; set; }
        public List<StockModel> List { get; set; }
    }
}