using Sales_Inventory.DAL;
using Sales_Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sales_Inventory.Controllers
{
    public class MiscExpensesController : BaseController
    {
        DBWorker worker = new DBWorker();

        // GET: MiscExpenses
        #region Expenses List
        public ActionResult List()
        {
            MiscExpensesViewModel model = new MiscExpensesViewModel();
            model.ExpensesList = GetExpensesList();
            return View(model);
        }
        public List<MiscExpensesViewModel> GetExpensesList()
        {
            List<MiscExpensesViewModel> ExpensesList = new List<MiscExpensesViewModel>();
            var list = worker.MiscExpensesEntity.Get().ToList();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    ExpensesList.Add(new MiscExpensesViewModel
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

        #region Delete Expenses
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var model = DeleteExpenses(id);
            return RedirectToAction("List"); ;
        }

        public bool DeleteExpenses(int id)
        {

            var result = worker.MiscExpensesEntity.GetByID(id);
            if (result != null)
            {
                worker.MiscExpensesEntity.Delete(result);
                worker.Save();
                return true;
            }
            else
                return false;
        }
        #endregion
    }
}