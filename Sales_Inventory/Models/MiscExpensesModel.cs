using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sales_Inventory.Models
{
    public class MiscExpensesModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> ExpenseAmt { get; set; }
        public Nullable<System.DateTime> ExpenseDate { get; set; }
        public string ExpenseReason { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public List<MiscExpensesModel> ExpensesList { get; set; }
    }
}