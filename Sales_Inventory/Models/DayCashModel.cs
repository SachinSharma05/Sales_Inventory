using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sales_Inventory.Models
{
    public class DayCashModel
    {
        public string Name { get; set; }
        public decimal PaidAmount { get; set; }
        public Nullable<System.DateTime> PaidDate { get; set; }
        public string Action { get; set; }

    }
}