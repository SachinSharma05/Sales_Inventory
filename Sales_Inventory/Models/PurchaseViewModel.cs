using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sales_Inventory.Models
{
    public class PurchaseModel
    {
        public int Id { get; set; }
        public string Purchase_No { get; set; }
        public string Purchase_From { get; set; }
        public string Purchase_From_Phone { get; set; }
        public Nullable<System.DateTime> Purchase_Date { get; set; }
        public decimal GrossTotal { get; set; }
        public decimal Balance { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public List<Purchase_Products> purchase_Products { get; set; }
        public List<SelectListItem> ProductList { get; set; }
        public List<SelectListItem> PurchaseName { get; set; }
        public List<PurchaseModel> List { get; set; }
    }

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