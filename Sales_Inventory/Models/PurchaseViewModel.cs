using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sales_Inventory.Models
{
    public class PurchaseViewModel
    {
        public int Id { get; set; }
        public string Purchase_No { get; set; }
        public string Purchase_From { get; set; }
        public string Purchase_From_Phone { get; set; }
        public Nullable<System.DateTime> Purchase_Date { get; set; }
        public Nullable<int> GrossTotal { get; set; }
        public Nullable<int> Balance { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public List<Purchase_Products> purchase_Products { get; set; }
        public List<SelectListItem> ProductList { get; set; }
        public List<SelectListItem> PurchaseName { get; set; }
        public List<PurchaseViewModel> List { get; set; }
    }
}