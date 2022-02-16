using Sales_Inventory.DAL;
using Sales_Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sales_Inventory.Controllers
{
    public class MiscExpensesController : Controller
    {
        DBWorker worker = new DBWorker();

        // GET: MiscExpenses
        #region Expenses List
        public ActionResult List()
        {
            return View(GetExpenseList());
        }
        public CommonViewModel GetExpenseList()
        {
            CommonViewModel ExpensesList = new CommonViewModel();
            var list = worker.MiscExpensesEntity.Get().ToList();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    ExpensesList.miscExpensesViewModels.Add(new MiscExpensesViewModel
                    {
                        Id = item.Id,
                        Name = item.Name,
                        ExpenseAmt = item.ExpenseAmt,
                        ExpenseDate = item.ExpenseDate,
                        ExpenseReason = item.ExpenseReason
                    });
                }
            }
            return ExpensesList;
        }
        #endregion

        #region Create Expense
        [HttpPost]
        public ActionResult Create(string expenseName, int expenseAmt, string expenseDate, string expenseReason)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    MiscExpens miscExpense = new MiscExpens();
                    miscExpense.Name = expenseName;
                    miscExpense.ExpenseAmt = expenseAmt;
                    miscExpense.ExpenseDate = Convert.ToDateTime(expenseDate);
                    miscExpense.ExpenseReason = expenseReason;
                    miscExpense.CreatedBy = (int)System.Web.HttpContext.Current.Session["UserId"];
                    miscExpense.CreatedDate = DateTime.Now.Date;
                    worker.MiscExpensesEntity.Insert(miscExpense);
                    worker.Save();
                }
                return RedirectToAction("List");
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}