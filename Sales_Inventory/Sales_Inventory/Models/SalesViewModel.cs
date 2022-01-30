using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sales_Inventory.Models
{
    public class SalesViewModel
    {
        public int Id { get; set; }
        public string Sale_No { get; set; }
        public string Sale_To { get; set; }
        public string Sale_By { get; set; }
        public string Sale_To_Phone { get; set; }
        public string Sale_By_Phone { get; set; }
        public Nullable<System.DateTime> Sale_Date { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public List<Sale_Products> sale_Products { get; set; }
    }
}