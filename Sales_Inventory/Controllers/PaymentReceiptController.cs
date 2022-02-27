using Sales_Inventory.DAL;
using Sales_Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sales_Inventory.Controllers
{
    public class PaymentReceiptController : BaseController
    {
        DBWorker worker = new DBWorker();
        // GET: PaymentReceipt
        #region Payment Receipt List
        public ActionResult List()
        {
            return View(GetPaymentReceiptList());
        }
        public List<PaymentReceiptViewModel> GetPaymentReceiptList()
        {
            List<PaymentReceiptViewModel> PaymentReceiptList = new List<PaymentReceiptViewModel>();
            var list = worker.PaymentReceiptEntity.Get().ToList();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    PaymentReceiptList.Add(new PaymentReceiptViewModel
                    {
                        Id = item.Id,
                        ReceiptNo = item.ReceiptNo,
                        ReceivedDate = item.ReceivedDate,
                        ReceivedFrom = item.ReceivedFrom,
                        TotalAmount = item.TotalAmount,
                        PaidAmount = item.PaidAmount,
                        Balance = item.Balance,
                        PaymentAgainst = item.PaymentAgainst
                    });
                }
            }
            ViewBag.SalesListModel = worker.SaleEntity.Get(x => x.Balance > 0).ToList();
            return PaymentReceiptList;
        }
        #endregion

        #region Create Payment Receipt
        public ActionResult Create(int Id)
        {
            PaymentReceiptViewModel model = new PaymentReceiptViewModel();
            model.Id = Id;
            var data = worker.SaleEntity.GetByID(model.Id);
            model.ReceivedFrom = data.Sale_To;
            model.TotalAmount = data.Balance != null ? data.Balance : data.GrossTotal;
            model.ReceiptNo = data.Sale_No;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(PaymentReceiptViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    PaymentReceipt receipt = new PaymentReceipt();
                    receipt.ReceiptNo = model.ReceiptNo;
                    receipt.ReceivedDate = model.ReceivedDate;
                    receipt.ReceivedFrom = model.ReceivedFrom;
                    receipt.TotalAmount = model.TotalAmount;
                    receipt.PaidAmount = model.PaidAmount;
                    receipt.Balance = model.Balance;
                    receipt.PaymentMode = model.PaymentMode;
                    receipt.PaymentAgainst = model.PaymentAgainst;
                    receipt.CreatedBy = (int)System.Web.HttpContext.Current.Session["UserId"];
                    receipt.CreatedDate = DateTime.Now.Date;
                    worker.PaymentReceiptEntity.Insert(receipt);
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

        #region Edit Payment Receipt
        public ActionResult Edit(int Id)
        {
            try
            {
                PaymentReceiptViewModel model = new PaymentReceiptViewModel();
                var paymentReceipt = worker.PaymentReceiptEntity.GetByID(Id);
                model.ReceiptNo = paymentReceipt.ReceiptNo;
                model.ReceivedDate = paymentReceipt.ReceivedDate;
                model.ReceivedFrom = paymentReceipt.ReceivedFrom;
                model.TotalAmount = paymentReceipt.TotalAmount;
                model.PaidAmount = paymentReceipt.PaidAmount;
                model.Balance = paymentReceipt.Balance;
                model.PaymentMode = paymentReceipt.PaymentMode;
                model.PaymentAgainst = paymentReceipt.PaymentAgainst;
                model.BankName = paymentReceipt.BankName;
                model.AccountNo = paymentReceipt.AccountNo;
                model.AccountHolderName = paymentReceipt.AccountHolderName;
                model.UPI_Id = paymentReceipt.UPI_Id;
                model.ChequeNo = paymentReceipt.ChequeNo;
                model.ChequeDate = paymentReceipt.ChequeDate;
                model.NameOnCheque = paymentReceipt.NameOnCheque;
                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Edit(PaymentReceiptViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    PaymentReceipt paymentReceipt = worker.PaymentReceiptEntity.GetByID(model.Id);
                    paymentReceipt.ReceiptNo = model.ReceiptNo;
                    paymentReceipt.ReceivedDate = model.ReceivedDate;
                    paymentReceipt.ReceivedFrom = model.ReceivedFrom;
                    paymentReceipt.TotalAmount = model.TotalAmount;
                    paymentReceipt.PaidAmount = model.PaidAmount;
                    paymentReceipt.Balance = model.Balance;
                    paymentReceipt.PaymentMode = model.PaymentMode;
                    paymentReceipt.PaymentAgainst = model.PaymentAgainst;
                    paymentReceipt.BankName = model.BankName;
                    paymentReceipt.AccountNo = model.AccountNo;
                    paymentReceipt.AccountHolderName = model.AccountHolderName;
                    paymentReceipt.UPI_Id = model.UPI_Id;
                    paymentReceipt.ChequeNo = model.ChequeNo;
                    paymentReceipt.ChequeDate = model.ChequeDate;
                    paymentReceipt.NameOnCheque = model.NameOnCheque;
                    worker.PaymentReceiptEntity.Update(paymentReceipt);
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

        #region Delete Payment Receipt
        [HttpGet]
        public ActionResult Delete(int id)
        {
            DeletePaymentReceipt(id);
            return RedirectToAction("List");
        }

        public bool DeletePaymentReceipt(int id)
        {
            var paymentReceipt = worker.PaymentReceiptEntity.GetByID(id);
            if (paymentReceipt != null)
            {
                worker.PaymentReceiptEntity.Delete(paymentReceipt);
                worker.Save();
                return true;
            }
            else
                return false;
        }
        #endregion
    }
}