using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sales_Inventory.Models
{
    public class SalesModel
    {
        public int Id { get; set; }
        public string Sale_No { get; set; }
        public string Sale_To { get; set; }
        public string Sale_To_Phone { get; set; }
        public Nullable<System.DateTime> Sale_Date { get; set; }
        public decimal GrossTotal { get; set; }
        public decimal Balance { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public List<Sale_Products> sale_Products { get; set; }
        public List<SelectListItem> ProductList { get; set; }
        public List<SelectListItem> SaleTo { get; set; }
        public List<SalesModel> List { get; set; }
    }

    public class Sale_Products
    {
        public int Id { get; set; }
        public string Sale_No { get; set; }
        public string Item { get; set; }
        public decimal Quantity { get; set; }
        public decimal Damaged { get; set; }
        public decimal FinalQty { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}