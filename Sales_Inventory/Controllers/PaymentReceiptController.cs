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
            PaymentReceiptModel model = new PaymentReceiptModel();
            model.PaymentReceiptName = GetPaymentReceiptName();
            model.List = GetPaymentReceiptList();
            return View(model);
        }
        public List<PaymentReceiptModel> GetPaymentReceiptList()
        {
            List<PaymentReceiptModel> PaymentReceiptList = new List<PaymentReceiptModel>();
            var list = worker.PaymentReceiptEntity.Get().ToList();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    PaymentReceiptList.Add(new PaymentReceiptModel
                    {
                        Id = item.Id,
                        ReceiptNo = item.ReceiptNo,
                        ReceivedDate = item.ReceivedDate,
                        ReceivedFrom = item.ReceivedFrom,
                        TotalAmount = (decimal)item.TotalAmount,
                        PaidAmount = (decimal)item.PaidAmount,
                        Balance = (decimal)item.Balance,
                        PaymentAgainst = item.PaymentAgainst
                    });
                }
            }
            ViewBag.SalesListModel = worker.SaleEntity.Get(x => x.Balance > 0).ToList();
            return PaymentReceiptList;
        }
        public List<SelectListItem> GetPaymentReceiptName()
        {
            var query = worker.PaymentReceiptEntity.Get().ToList();

            var list = new List<SelectListItem> { new SelectListItem { Value = null, Text = "" } };
            list.AddRange(query.ToList().Select(C => new SelectListItem
            {
                Value = C.Id.ToString(),
                Text = C.ReceivedFrom
            }));

            return list;
        }
        #endregion

        #region Create Payment Receipt
        public ActionResult Create(int Id)
        {
            PaymentReceiptModel model = new PaymentReceiptModel();
            model.Id = Id;
            var data = worker.SaleEntity.GetByID(model.Id);
            model.ReceivedFrom = data.Sale_To;
            model.TotalAmount = (decimal)(data.Balance != null ? data.Balance : data.GrossTotal);
            model.ReceiptNo = data.Sale_No;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(PaymentReceiptModel model)
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

                    Sale sale = worker.SaleEntity.Get(x => x.Sale_No == model.ReceiptNo).FirstOrDefault();
                    sale.Balance = model.Balance;
                    worker.SaleEntity.Update(sale);
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
                PaymentReceiptModel model = new PaymentReceiptModel();
                var paymentReceipt = worker.PaymentReceiptEntity.GetByID(Id);
                model.ReceiptNo = paymentReceipt.ReceiptNo;
                model.ReceivedDate = paymentReceipt.ReceivedDate;
                model.ReceivedFrom = paymentReceipt.ReceivedFrom;
                model.TotalAmount = (decimal)paymentReceipt.TotalAmount;
                model.PaidAmount = (decimal)paymentReceipt.PaidAmount;
                model.Balance = (decimal)paymentReceipt.Balance;
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
        public ActionResult Edit(PaymentReceiptModel model)
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

        #region Search List
        public ActionResult SearchList(string PurchaseName, string StartDate, string EndDate)
        {
            try
            {
                List<PaymentReceiptModel> model = new List<PaymentReceiptModel>();
                var SDate = StartDate != "" ? Convert.ToDateTime(StartDate).Date : DateTime.Now;
                var EDate = EndDate != "" ? Convert.ToDateTime(EndDate).Date : DateTime.Now;

                if (PurchaseName != "" && StartDate != "" && EndDate != "")
                {
                    var list = worker.PaymentReceiptEntity.Get(x => x.ReceivedFrom == PurchaseName && x.ReceivedDate >= SDate && x.ReceivedDate <= EDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new PaymentReceiptModel
                        {
                            Id = item.Id,
                            ReceiptNo = item.ReceiptNo,
                            ReceivedDate = item.ReceivedDate,
                            ReceivedFrom = item.ReceivedFrom,
                            TotalAmount = (decimal)item.TotalAmount,
                            PaidAmount = (decimal)item.PaidAmount,
                            Balance = (decimal)item.Balance,
                            PaymentAgainst = item.PaymentAgainst
                        });
                    }
                }
                else if (PurchaseName != "" && StartDate != "" && EndDate == "")
                {
                    var list = worker.PaymentReceiptEntity.Get(x => x.ReceivedFrom == PurchaseName && x.ReceivedDate == SDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new PaymentReceiptModel
                        {
                            Id = item.Id,
                            ReceiptNo = item.ReceiptNo,
                            ReceivedDate = item.ReceivedDate,
                            ReceivedFrom = item.ReceivedFrom,
                            TotalAmount = (decimal)item.TotalAmount,
                            PaidAmount = (decimal)item.PaidAmount,
                            Balance = (decimal)item.Balance,
                            PaymentAgainst = item.PaymentAgainst
                        });
                    }
                }
                else if (PurchaseName != "" && EndDate != "" && StartDate == "")
                {
                    var list = worker.PaymentReceiptEntity.Get(x => x.ReceivedFrom == PurchaseName && x.ReceivedDate == EDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new PaymentReceiptModel
                        {
                            Id = item.Id,
                            ReceiptNo = item.ReceiptNo,
                            ReceivedDate = item.ReceivedDate,
                            ReceivedFrom = item.ReceivedFrom,
                            TotalAmount = (decimal)item.TotalAmount,
                            PaidAmount = (decimal)item.PaidAmount,
                            Balance = (decimal)item.Balance,
                            PaymentAgainst = item.PaymentAgainst
                        });
                    }
                }
                else if (StartDate != "" && EndDate != "" && PurchaseName == "")
                {
                    var list = worker.PaymentReceiptEntity.Get(x => x.ReceivedDate >= SDate && x.ReceivedDate <= EDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new PaymentReceiptModel
                        {
                            Id = item.Id,
                            ReceiptNo = item.ReceiptNo,
                            ReceivedDate = item.ReceivedDate,
                            ReceivedFrom = item.ReceivedFrom,
                            TotalAmount = (decimal)item.TotalAmount,
                            PaidAmount = (decimal)item.PaidAmount,
                            Balance = (decimal)item.Balance,
                            PaymentAgainst = item.PaymentAgainst
                        });
                    }
                }
                else if (PurchaseName != null && StartDate == "" && EndDate == "")
                {
                    var list = worker.PaymentReceiptEntity.Get(x => x.ReceivedFrom == PurchaseName).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new PaymentReceiptModel
                        {
                            Id = item.Id,
                            ReceiptNo = item.ReceiptNo,
                            ReceivedDate = item.ReceivedDate,
                            ReceivedFrom = item.ReceivedFrom,
                            TotalAmount = (decimal)item.TotalAmount,
                            PaidAmount = (decimal)item.PaidAmount,
                            Balance = (decimal)item.Balance,
                            PaymentAgainst = item.PaymentAgainst
                        });
                    }
                }
                else if (StartDate != "" && PurchaseName == "" && EndDate == "")
                {
                    var list = worker.PaymentReceiptEntity.Get(x => x.ReceivedDate == SDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new PaymentReceiptModel
                        {
                            Id = item.Id,
                            ReceiptNo = item.ReceiptNo,
                            ReceivedDate = item.ReceivedDate,
                            ReceivedFrom = item.ReceivedFrom,
                            TotalAmount = (decimal)item.TotalAmount,
                            PaidAmount = (decimal)item.PaidAmount,
                            Balance = (decimal)item.Balance,
                            PaymentAgainst = item.PaymentAgainst
                        });
                    }
                }
                else if (EndDate != "" && StartDate == "" && PurchaseName == "")
                {
                    var list = worker.PaymentReceiptEntity.Get(x => x.ReceivedDate <= EDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new PaymentReceiptModel
                        {
                            Id = item.Id,
                            ReceiptNo = item.ReceiptNo,
                            ReceivedDate = item.ReceivedDate,
                            ReceivedFrom = item.ReceivedFrom,
                            TotalAmount = (decimal)item.TotalAmount,
                            PaidAmount = (decimal)item.PaidAmount,
                            Balance = (decimal)item.Balance,
                            PaymentAgainst = item.PaymentAgainst
                        });
                    }
                }

                return PartialView("_SearchList", model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}