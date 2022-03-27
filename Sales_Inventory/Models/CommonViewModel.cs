using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sales_Inventory.Models
{
    public class CommonModel
    {
        public List<StockViewModel> stockViewModels { get; set; }
        public List<PurchaseModel> purchaseViewModels { get; set; }
    }

    public class AddNewCredits
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> Amount { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Type { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public List<AddNewCredits> creditList { get; set; }
    }
}