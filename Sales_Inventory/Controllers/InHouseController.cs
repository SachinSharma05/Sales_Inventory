using Sales_Inventory.DAL;
using Sales_Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sales_Inventory.Controllers
{
    public class InHouseController : Controller
    {
        DBWorker worker = new DBWorker();

        #region Employee List
        public ActionResult List()
        {
            var InHouseTransaction = worker.InHouseTransactionEntity.Get().ToList();
            ViewBag.InHouseTransaction = InHouseTransaction;
            return View();
        }
        #endregion

        #region Create InHouse Transaction
        [HttpPost]
        public ActionResult Create(string paidTo, string paidBy, string paidDate, string totalAmount, string paidAmount, string returnAmtReceived, string balanceAmt)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    InHouse transaction = new InHouse();
                    transaction.PaidBy = paidBy;
                    transaction.PaidTo = paidTo;
                    transaction.PaidDate = Convert.ToDateTime(paidDate);
                    transaction.TotalAmount = Convert.ToInt32(totalAmount);
                    transaction.PaidAmount = Convert.ToInt32(paidAmount);
                    transaction.ReturnAmtReceived = Convert.ToInt32(returnAmtReceived);
                    transaction.BalanceAmt = Convert.ToInt32(balanceAmt);
                    transaction.CreatedBy = (int)System.Web.HttpContext.Current.Session["UserId"];
                    transaction.CreatedDate = DateTime.Now.Date;
                    worker.InHouseTransactionEntity.Insert(transaction);
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

        #region Edit InHouse Transaction
        public JsonResult Edit(int Id)
        {
            try
            {
                InHouseViewModel model = new InHouseViewModel();
                var list = worker.InHouseTransactionEntity.GetByID(Id);
                model.Id = list.Id;
                model.PaidBy = list.PaidBy;
                model.PaidTo = list.PaidTo;
                model.PaidDate = list.PaidDate;
                model.TotalAmount = list.TotalAmount;
                model.PaidAmount = list.PaidAmount;
                model.ReturnAmtReceived = list.ReturnAmtReceived;
                model.BalanceAmt = list.BalanceAmt;
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Update(InHouseViewModel model)
        {
            try
            {
                var list = worker.InHouseTransactionEntity.GetByID(model.Id);
                if(list != null)
                {
                    InHouse data = new InHouse();
                    data.ReturnAmtReceived = model.ReturnAmtReceived;
                    data.CreatedBy = (int)System.Web.HttpContext.Current.Session["UserId"];
                    data.CreatedDate = DateTime.Now.Date;
                    worker.InHouseTransactionEntity.Update(data);
                    worker.Save();
                }
                return RedirectToAction("list");
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}