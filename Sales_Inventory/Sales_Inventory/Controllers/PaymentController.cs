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
                        Payment_To = item.Payment_To,
                        Payment_By = item.Payment_By,
                        Payment_Date = item.Payment_Date,
                        Payment_Against = item.Payment_Against,
                        Payment_Type = item.Payment_Type,
                        Payment_Amount = item.Payment_Amount,
                        Balance = item.Balance
                    });
                }
            }
            return PaymentList;
        }
        #endregion

        #region Create Payment
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(PaymentViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Payment payment = new Payment();
                    payment.Payment_To = model.Payment_To;
                    payment.Payment_By = model.Payment_By;
                    payment.Payment_Date = model.Payment_Date;
                    payment.Payment_Against = model.Payment_Against;
                    payment.Payment_Type = model.Payment_Type;
                    payment.Payment_Account_No = model.Payment_Account_No;
                    payment.Payment_Account_Name = model.Payment_Account_Name;
                    payment.Payment_Amount = model.Payment_Amount;
                    payment.Payment_Bank = model.Payment_Bank;
                    payment.IFSC_Code = model.IFSC_Code;
                    payment.Payment_Receiver_Phone = model.Payment_Receiver_Phone;
                    payment.Payment_Sender_Phone = model.Payment_Sender_Phone;
                    //payment.Balance = 
                    payment.CreatedBy = (int)System.Web.HttpContext.Current.Session["UserId"];
                    payment.CreatedDate = DateTime.Now.Date;
                    worker.PaymentEntity.Insert(payment);
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
                model.Payment_By = payment.Payment_By;
                model.Payment_Date = payment.Payment_Date;
                model.Payment_Against = payment.Payment_Against;
                model.Payment_Type = payment.Payment_Type;
                model.Payment_Account_No = payment.Payment_Account_No;
                model.Payment_Account_Name = payment.Payment_Account_Name;
                model.Payment_Amount = model.Payment_Amount;
                model.Payment_Bank = model.Payment_Bank;
                model.IFSC_Code = model.IFSC_Code;
                model.Payment_Receiver_Phone = model.Payment_Receiver_Phone;
                model.Payment_Sender_Phone = payment.Payment_Sender_Phone;
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
                    payment.Payment_By = model.Payment_By;
                    payment.Payment_Date = model.Payment_Date;
                    payment.Payment_Against = model.Payment_Against;
                    payment.Payment_Type = model.Payment_Type;
                    payment.Payment_Account_No = model.Payment_Account_No;
                    payment.Payment_Account_Name = model.Payment_Account_Name;
                    payment.Payment_Amount = model.Payment_Amount;
                    payment.Payment_Bank = model.Payment_Bank;
                    payment.IFSC_Code = model.IFSC_Code;
                    payment.Payment_Receiver_Phone = model.Payment_Receiver_Phone;
                    payment.Payment_Sender_Phone = model.Payment_Sender_Phone;
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