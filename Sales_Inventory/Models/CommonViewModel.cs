using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sales_Inventory.Models
{
    public class CommonViewModel
    {
        public List<StockViewModel> stockViewModels { get; set; }
        public List<PurchaseViewModel> purchaseViewModels { get; set; }
    }
}