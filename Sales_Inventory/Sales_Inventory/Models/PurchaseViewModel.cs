using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sales_Inventory.Models
{
    public class PurchaseViewModel
    {
        public int Id { get; set; }
        public string Purchase_No { get; set; }
        public string Purchase_From { get; set; }
        public string Purchase_By { get; set; }
        public string Purchase_From_Phone { get; set; }
        public string Purchase_By_Phone { get; set; }
        public Nullable<System.DateTime> Purchase_Date { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public List<Purchase_Products> purchase_Products { get; set; }
    }
}