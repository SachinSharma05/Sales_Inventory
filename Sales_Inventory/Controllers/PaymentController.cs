using Sales_Inventory.DAL;
using Sales_Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sales_Inventory.Controllers
{
    public class PaymentController : Controller
    {
        DBWorker worker = new DBWorker();

        // GET: Payment
        #region Payment List
        public ActionResult List()
        {
            return View(GetPaymentList());
        }
        public List<PaymentViewModel> GetPaymentList()
        {
            List<PaymentViewModel> PaymentList = new List<PaymentViewModel>();
            var list = worker.PaymentEntity.Get().ToList();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    PaymentList.Add(new PaymentViewModel
                    {
                        Id = item.Id,
                        Purchase_No = item.Purchase_No,
                        Payment_To = item.Payment_To,
                        Payment_Date = item.Payment_Date,
                        Payment_Type = item.Payment_Type,
                        Paid_Amount = item.Paid_Amount,
                        Balance = item.Balance
                    });
                }
            }
            return PaymentList;
        }
        #endregion

        #region Create Payment
        public ActionResult Create(int Id)
        {
            PaymentViewModel model = new PaymentViewModel();
            model.Id = Id;
            var data = worker.PurchaseEntity.GetByID(model.Id);
            model.Total_Payment_Amount = data.Balance != null ? data.Balance : data.GrossTotal;
            model.Purchase_No = data.Purchase_No;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(PaymentViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Payment payment = new Payment();
                    payment.Purchase_No = model.Purchase_No;
                    payment.Payment_To = model.Payment_To;
                    payment.Payment_Date = model.Payment_Date;
                    payment.Payment_Type = model.Payment_Type;
                    payment.Total_Payment_Amount = model.Total_Payment_Amount;
                    payment.Contact_No = model.Contact_No;
                    payment.Paid_Amount = model.Paid_Amount;
                    payment.Bank_Name = model.Bank_Name;
                    payment.Account_No = model.Account_No;
                    payment.Account_Holder_Name = model.Account_Holder_Name;
                    payment.UPI_Id = model.UPI_Id;
                    payment.Cheque_No = model.Cheque_No;
                    payment.Cheque_Date = model.Cheque_Date;
                    payment.Name_On_Cheque = model.Name_On_Cheque;
                    payment.Balance = model.Balance;
                    payment.CreatedBy = (int)System.Web.HttpContext.Current.Session["UserId"];
                    payment.CreatedDate = DateTime.Now.Date;
                    worker.PaymentEntity.Insert(payment);
                    worker.Save();

                    Purchase purchase = worker.PurchaseEntity.Get(x => x.Purchase_No == model.Purchase_No).FirstOrDefault();
                    purchase.Balance = model.Balance;
                    worker.PurchaseEntity.Update(purchase);
                    worker.Save();
                }
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Edit Payment
        public ActionResult Edit(int Id)
        {
            try
            {
                PaymentViewModel model = new PaymentViewModel();
                var payment = worker.PaymentEntity.GetByID(Id);
                model.Payment_To = payment.Payment_To;
                model.Payment_Date = payment.Payment_Date;
                model.Payment_Type = payment.Payment_Type;
                model.Balance = payment.Balance;
                return View(model);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Edit(PaymentViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Payment payment = worker.PaymentEntity.GetByID(model.Id);
                    payment.Payment_To = model.Payment_To;
                    payment.Payment_Date = model.Payment_Date;
                    payment.Payment_Type = model.Payment_Type;
                    payment.Balance = model.Balance;
                    worker.PaymentEntity.Update(payment);
                    worker.Save();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return RedirectToAction("List");
        }
        #endregion

        #region Delete Payment
        [HttpGet]
        public ActionResult Delete(int id)
        {
            DeletePayment(id);
            return RedirectToAction("List");
        }

        public bool DeletePayment(int id)
        {
            var payment = worker.PaymentEntity.GetByID(id);
            if (payment != null)
            {
                worker.PaymentEntity.Delete(payment);
                worker.Save();
                return true;
            }
            else
                return false;
        }
        #endregion
    }
}